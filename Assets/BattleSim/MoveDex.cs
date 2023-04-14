using System;
using System.Collections.Generic;
using UnityEngine;

public static class MoveDex
{
    private static Dictionary<string, MoveInfo> moveDict;
    public static Dictionary<string, MoveInfo> Moves
    {
        get
        {
            if (moveDict != null) return moveDict;
            moveDict = new();
            var text = Resources.Load<TextAsset>("dex/moves");
            var moveList = JsonUtility.FromJson<MoveInfoList>(text.text);
            foreach (var move in moveList.moves)
                moveDict.Add(move.id, move);
            return moveDict;
        }
    }

    public struct MoveInfoList
    {
        public MoveInfo[] moves;
    }

    [System.Serializable]
    public struct MoveInfo
    {
        public string id;
        public string type;
        public string desc;
        public int accuracy;
        public int basePower;
        public string category;
        public PokemonType PokemonType => Enum.Parse<PokemonType>(type);
        public MoveCategory moveCategory => Enum.Parse<MoveCategory>(category);
        public enum MoveCategory
        {
            Status, Special, Physical
        }
    }

}
