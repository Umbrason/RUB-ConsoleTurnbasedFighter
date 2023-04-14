using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class PokeDex
{
    private static Dictionary<string, PokemonInfo> pokemonDict;
    public static Dictionary<string, PokemonInfo> Pokemon
    {
        get
        {
            if (pokemonDict != null) return pokemonDict;
            pokemonDict = new();
            var text = Resources.Load<TextAsset>("dex/mons");
            var monList = JsonUtility.FromJson<PokemonInfoList>(text.text);
            foreach (var mon in monList.mons)
                pokemonDict.Add(mon.name.ToLower(), mon);
            return pokemonDict;
        }
    }

    [System.Serializable]
    public struct PokemonInfoList
    {
        public PokemonInfo[] mons;
    }

    [System.Serializable]
    public struct PokemonInfo
    {
        public string name;
        public string[] types;
        public PokemonType[] PokemonTypes => types.Select(type => Enum.Parse<PokemonType>(type)).ToArray();
        public float GetTypeWeakness(PokemonType attacker)
            => PokemonTypes.Select(type => type.DefenseModifier(attacker)).Aggregate(1f, (x, y) => x * y);
    }

}
