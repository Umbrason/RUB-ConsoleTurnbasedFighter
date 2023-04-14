public enum PokemonType
{
    Normal,
    Fire,
    Water,
    Electric,
    Grass,
    Ice,
    Fighting,
    Poison,
    Ground,
    Flying,
    Psychic,
    Bug,
    Rock,
    Ghost,
    Dragon,
    Dark,
    Steel,
    Fairy
}

public static class PokemonTypeExtensions
{
    static readonly double[,] TypeTable = new double[,]{
        { 1,1,1,1,1,1,1,1,1,1,1,1,0.5,0,1,1,0.5,1},
        { 1,0.5,0.5,1,2,2,1,1,1,1,1,2,0.5,1,0.5,1,2,1},
        { 1,2,0.5,1,0.5,1,1,1,2,1,1,1,2,1,0.5,1,1,1},
        { 1,1,2,0.5,0.5,1,1,1,0,2,1,1,1,1,0.5,1,1,1},
        { 1,0.5,2,1,0.5,1,1,0.5,2,0.5,1,0.5,2,1,0.5,1,0.5,1},
        { 1,0.5,0.5,1,2,0.5,1,1,2,2,1,1,1,1,2,1,0.5,1},
        { 2,1,1,1,1,2,1,0.5,1,0.5,0.5,0.5,2,0,1,2,2,0.5},
        { 1,1,1,1,2,1,1,0.5,0.5,1,1,1,0.5,0.5,1,1,0,2},
        { 1,2,1,2,0.5,1,1,2,1,0,1,0.5,2,1,1,1,2,1},
        { 1,1,1,0.5,2,1,2,1,1,1,1,2,0.5,1,1,1,0.5,1},
        { 1,1,1,1,1,1,2,2,1,1,0.5,1,1,1,1,0,0.5,1},
        { 1,0.5,1,1,2,1,0.5,0.5,1,0.5,2,1,1,0.5,1,2,0.5,0.5},
        { 1,2,1,1,1,2,0.5,1,0.5,2,1,2,1,1,1,1,0.5,1},
        { 0,1,1,1,1,1,1,1,1,1,2,1,1,2,1,0.5,1,1},
        { 1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,1,0.5,0},
        { 1,1,1,1,1,1,0.5,1,1,1,2,1,1,2,1,0.5,1,0.5},
        { 1,0.5,0.5,0.5,1,2,1,1,1,1,1,1,2,1,1,1,0.5,2},
        { 1,0.5,1,1,1,1,2,0.5,1,1,1,1,1,1,2,2,0.5,1}};
    public static float AttackModifier(this PokemonType attacker, PokemonType defender)
    {
        return (float)TypeTable[(int)attacker, (int)defender];
    }

    public static float DefenseModifier(this PokemonType defender, PokemonType attacker)
    {
        return (float)TypeTable[(int)attacker, (int)defender];
    }
}