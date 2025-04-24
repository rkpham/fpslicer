using UnityEngine;

public class Boss : Entity
{
    public Stage CurrentStage = Stage.First;
}

public enum Stage
{
    First,
    Second,
    Third
}