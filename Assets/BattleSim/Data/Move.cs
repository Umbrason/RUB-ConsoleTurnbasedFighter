
using System;
using UnityEngine;

[Serializable]
public class Move
{
    public string move;
    public string id;
    public int pp;
    public int maxpp;
    public bool disabled;
    public MoveTarget target;
    public enum MoveTarget
    {
        allySide, ally, normal, self
    }
}