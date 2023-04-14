[System.Serializable]
public struct Player
{
    public string Name { get; set; }
    public Player(string name)
    {
        this.Name = name;
    }
}
