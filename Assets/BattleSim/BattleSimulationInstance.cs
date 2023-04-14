using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class BattleSimulationInstance
{
    private readonly Process PokemonShowdownProcess;
    private StreamReader output;
    private StreamWriter input;


    private int activePlayer;
    private List<string> commandLines = new();

    public void MakeMove(int player, Move move)
    {
        UnityEngine.Debug.Log($">p{player} move {move.move}");
        input.WriteLine($">p{player} move {move.move}");
    }

    public void Switch(int player, Pokemon target)
    {
        input.WriteLine($">p{player} switch {target.Name}");
    }

    public void Start(string format, Player p1, Player p2)
    {
        PokemonShowdownProcess.Start();
        input = PokemonShowdownProcess.StandardInput;
        input.WriteLine($">start {{\"formatid\":\"{format}\"}}");
        input.WriteLine($">player p1 {{\"name\":\"{p1.Name}\"}}");
        input.WriteLine($">player p2 {{\"name\":\"{p2.Name}\"}}");
        PokemonShowdownProcess.OutputDataReceived += ConsumeOutput;
        PokemonShowdownProcess.BeginOutputReadLine();
    }

    public void Stop()
    {
        PokemonShowdownProcess.Close();
    }

    public BattleSimulationInstance()
    {

        ProcessStartInfo startInfo = new ProcessStartInfo(Environment.CurrentDirectory + "/pokemon-showdown.exe");
        startInfo.Arguments = "simulate-battle";
        startInfo.UseShellExecute = false;
        startInfo.RedirectStandardInput = true;
        startInfo.RedirectStandardOutput = true;
#if !UNITY_EDITOR
        startInfo.CreateNoWindow = true;
#endif
        PokemonShowdownProcess = new Process() { StartInfo = startInfo };
    }

    private void ConsumeOutput(object o, DataReceivedEventArgs e)
    {
        var line = e.Data;
        if (line == "")
        {
            ParseCommand(commandLines.ToArray());
            commandLines.Clear();
        }
        else commandLines.Add(line);
    }

    private void ParseCommand(string[] lines)
    {
        foreach (var line in lines)
        {
            UnityEngine.Debug.Log(line);
            var match = Regex.Matches(line, @"^p\d$");
            if (match.Count > 0) { activePlayer = int.Parse(match[0].Value.Remove(0, 1)); continue; }
            var targetQueue = activePlayer == 1 ? P1ChoiceRequests : P2ChoiceRequests;
            if (line.StartsWith("|win"))
            {
                //win
                continue;
            }
            if (line.StartsWith("|request"))
            {
                var json = line.Remove(0, "|request|".Length);
                var request = JsonUtility.FromJson<ChoiceRequest>(json);
                if (request.wait) continue;
                latestP1ChoiceRequest = activePlayer == 1 ? request : latestP1ChoiceRequest;
                latestP2ChoiceRequest = activePlayer == 2 ? request : latestP2ChoiceRequest;
                targetQueue.Enqueue(request);
                continue;
            }
            if (line.StartsWith("|faint|"))
            {
                var faintPlayerIDGroup = Regex.Match(line, @"^\|faint\|p(\d)");
                var faintPlayer = faintPlayerIDGroup.Success ? int.Parse(faintPlayerIDGroup.Groups[1].Value) : 0;
                var faintTargetQueue = faintPlayer == 1 ? P1PokemonUpdates : P2PokemonUpdates;
                faintTargetQueue.Enqueue(new ChangeCondition() { condition = "0 fnt" });
            }
            if (Regex.IsMatch(line, @"^\|error"))
            {
                targetQueue.Enqueue(activePlayer == 1 ? latestP1ChoiceRequest : latestP2ChoiceRequest);
                continue;
            }

            if (!Regex.IsMatch(line, @"^\|-")) continue;
            //minor actions
            var playerIDGroup = Regex.Match(line, @"^\|-\w+\|p(\d)");
            var targetPlayer = playerIDGroup.Success ? int.Parse(playerIDGroup.Groups[1].Value) : 0;

            var minorTargetQueue = targetPlayer == 1 ? P1PokemonUpdates : P2PokemonUpdates;
            if (line.StartsWith("|-damage|"))
                minorTargetQueue.Enqueue(new ChangeCondition() { condition = line.Split("|")[3].Trim() });
            if (line.StartsWith("|-heal|"))
                minorTargetQueue.Enqueue(new ChangeCondition() { condition = line.Split("|")[3].Trim() });
            if (line.StartsWith("|-sethp|"))
                minorTargetQueue.Enqueue(new ChangeHealth() { health = line.Split("|")[3] });
            if (line.StartsWith("|-status|"))
                minorTargetQueue.Enqueue(new ChangeStatus() { status = line.Split("|")[3] });
            if (line.StartsWith("|-curestatus|"))
                minorTargetQueue.Enqueue(new ChangeStatus() { status = "" });
/*          if (line.StartsWith("|-boost|")) ;
            if (line.StartsWith("|-unboost|")) ;
            if (line.StartsWith("|-setboost|")) ;
            if (line.StartsWith("|-swapboost|")) ;
            if (line.StartsWith("|-invertboost|")) ;
            if (line.StartsWith("|-clearboost|")) ;
            if (line.StartsWith("|-clearallboost|")) ;
            if (line.StartsWith("|-clearpositiveboost|")) ; */
        }
    }

    //sadly cannot be events due to multithreading issues with Unity
    public readonly Queue<IActivePokemonUpdate> P1PokemonUpdates = new();
    public readonly Queue<IActivePokemonUpdate> P2PokemonUpdates = new();

    public readonly Queue<ChoiceRequest> P1ChoiceRequests = new();
    public readonly Queue<ChoiceRequest> P2ChoiceRequests = new();

    private ChoiceRequest latestP1ChoiceRequest;
    private ChoiceRequest latestP2ChoiceRequest;
}

