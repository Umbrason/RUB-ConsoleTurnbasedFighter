using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyTrainerAI : MonoBehaviour
{
    [SerializeField] private PokemonBattle battle;
    void Update()
    {
        if (battle.p2Request == null) return;
        if (battle.p2Request.active.Count == 0 || (battle.p2Request.forceSwitch != null && battle.p2Request.forceSwitch.Count != 0 && battle.p2Request.forceSwitch[0]))
        {
            //switch
            try
            {
                var bestSwitch = BestSwitch(battle.ActivePokemonP1, battle.p2Request.side.pokemon.ToArray());
                battle.Switch(2, bestSwitch);
            }
            catch { battle.Switch(2, 2); }
            return;
        }
        try
        {
            ChooseTurn(battle.p2Request.active[0].moves.ToArray(), battle.ActivePokemonP2, battle.p2Request.side.pokemon.ToArray(), battle.ActivePokemonP1);
        }
        catch
        {
            battle.MakeMove(2, Random.Range(0, 4) + 1);
        }
    }


    private void ChooseTurn(Move[] moves, Pokemon self, Pokemon[] selfBench, Pokemon enemy)
    {
        var bestMove = ChooseMove(moves, self, enemy, out var movescore);
        if (movescore < 1f)
        {
            var bestSwitch = BestSwitch(enemy, selfBench);
            if (bestSwitch > 1)
            {
                battle.Switch(2, bestSwitch);
                return;
            }
        }
        battle.MakeMove(2, bestMove);
        //if best type bonus is < 1 consider switching
        //   check moves of other pokemon and switch into best fit
        //   check defensive matchup vs all moves of current playermon of other pokemon and switch into best fit        
    }

    private int BestSwitch(Pokemon enemy, Pokemon[] selfBench)
    {
        if (selfBench.Length == 0) return -1;
        var switchChoices = selfBench.Skip(1).Where(pkmn => pkmn.HealthPercent > 0).Select(candidate => new SwitchChoice(candidate, enemy)).OrderByDescending(sc => sc.score).ToList();
        return selfBench.ToList().IndexOf(switchChoices[0].candidate) + 1;
    }

    private int ChooseMove(Move[] moves, Pokemon self, Pokemon enemy, out float moveScore)
    {
        var selfPokemonInfo = PokeDex.Pokemon[enemy.Species.ToLower()];
        var enemyPokemonInfo = PokeDex.Pokemon[enemy.Species.ToLower()];

        var MoveChoices = new List<MoveChoice>();
        for (int i = 0; i < 4; i++)
        {
            if (moves[i].disabled) continue;
            MoveChoices.Add(new MoveChoice(self.moves[i], self, enemy));
        }
        MoveChoices = MoveChoices.OrderByDescending(mc => mc.OffensiveScore).ToList();

        var statusMoves = MoveChoices.Where(mc => mc.moveInfo.moveCategory == MoveDex.MoveInfo.MoveCategory.Status).ToList();
        var attackingMoves = MoveChoices.Where(mc => mc.moveInfo.moveCategory != MoveDex.MoveInfo.MoveCategory.Status).ToList();

        if (Random.Range(0, 4) < statusMoves.Count)
        {
            moveScore = 1f;
            return moves.ToList().FindIndex(m => m.id == statusMoves[Random.Range(0, statusMoves.Count)].move) + 1;
        }

        var move = attackingMoves.First();
        moveScore = move.typeEffectiveness * move.STAB;
        return moves.ToList().FindIndex(m => m.id == attackingMoves[0].move) + 1;
    }

    private struct SwitchChoice
    {
        public float score;
        public Pokemon candidate;
        public SwitchChoice(Pokemon candidate, Pokemon enemy)
        {
            this.candidate = candidate;
            var moves = candidate.moves
                        .Select(m => new MoveChoice(m, candidate, enemy))
                        .OrderByDescending(mc => mc.OffensiveScore);
            score = moves.First().OffensiveScore;
            Debug.Log($"{candidate.Species} has an offensive score of {score} against {enemy.Species} using {moves.First().moveInfo.id}");
        }
    }

    private struct MoveChoice
    {
        public string move;
        public MoveDex.MoveInfo moveInfo;
        public float typeEffectiveness;
        public float STAB;
        public float OffensiveScore => typeEffectiveness * moveInfo.basePower * STAB;

        public MoveChoice(string move, Pokemon self, Pokemon enemy)
        {
            this.move = move;
            this.moveInfo = MoveDex.Moves[move];
            var enemyPokemonInfo = PokeDex.Pokemon[enemy.Species.ToLower()];
            var selfPokemonInfo = PokeDex.Pokemon[self.Species.ToLower()];
            typeEffectiveness = enemyPokemonInfo.GetTypeWeakness(moveInfo.PokemonType);
            STAB = selfPokemonInfo.types.Contains(moveInfo.type) ? 1.5f : 1f;
        }
    }
}
