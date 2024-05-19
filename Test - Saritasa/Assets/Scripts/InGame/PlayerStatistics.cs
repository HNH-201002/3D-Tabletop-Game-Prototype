using UnityEngine;

public class PlayerStatistics : MonoBehaviour
{
    public int TurnsTaken { get; private set; }
    public int BonusPoints { get; private set; }
    public int FailPoints { get; private set; }

    public void IncrementTurnsTaken()
    {
        TurnsTaken++;
    }

    public void IncrementBonusPoints()
    {
        BonusPoints++;
    }

    public void IncrementFailPoints()
    {
        FailPoints++;
    }
}
