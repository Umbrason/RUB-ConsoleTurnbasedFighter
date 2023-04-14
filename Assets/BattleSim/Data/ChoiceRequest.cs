using System;
using System.Collections.Generic;

[Serializable]
public class ChoiceRequest
{
    public List<ActivePKMNInfo> active;
    public List<bool> forceSwitch;
    public bool wait;
    public Side side;
}

[Serializable]
public class Side
{
    public string name;
    public string id;
    public List<Pokemon> pokemon;
}

[Serializable]
public class ActivePKMNInfo
{
    public List<Move> moves;
    public bool canDynamax;
}