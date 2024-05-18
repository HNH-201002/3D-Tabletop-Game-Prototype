using UnityEngine;

public class SpawnedPoint : MonoBehaviour
{
    public PointType Type;
}

public enum PointType
{
    Normal,
    Bonus,
    Fail
}