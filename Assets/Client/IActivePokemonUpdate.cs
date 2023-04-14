
public interface IActivePokemonUpdate
{
    Pokemon Apply(Pokemon pokemon);
}

public struct ChangeCondition : IActivePokemonUpdate
{
    public string condition;
    public Pokemon Apply(Pokemon pokemon)
    {
        pokemon.condition = condition;
        return pokemon;
    }
}

public struct ChangeHealth : IActivePokemonUpdate
{
    public string health;
    public Pokemon Apply(Pokemon pokemon)
    {
        pokemon.condition = $"{health} {pokemon.condition.Split(' ')[1]}";
        return pokemon;
    }
}

public struct ChangeStatus : IActivePokemonUpdate
{
    public string status;
    public Pokemon Apply(Pokemon pokemon)
    {
        pokemon.condition = $"{pokemon.condition.Split(' ')[0]} {status}".Trim();
        return pokemon;
    }
}

public struct ChangeAbility : IActivePokemonUpdate
{
    public string ability;
    public Pokemon Apply(Pokemon pokemon)
    {
        pokemon.ability = ability;
        return pokemon;
    }
}

public struct ChangeStatboost : IActivePokemonUpdate
{
    string stat;
    int stage;
    public Pokemon Apply(Pokemon pokemon)
    {
        throw new System.NotImplementedException();
        return pokemon;
    }
}
