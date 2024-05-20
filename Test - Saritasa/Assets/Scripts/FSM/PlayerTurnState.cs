using UnityEngine;

public class PlayerTurnState : GameState
{
    public override void EnterState(GameManager gameManager)
    {
        int currentPlayerIndex = gameManager.GetCurrentPlayerIndex();
        gameManager.StartTurn(currentPlayerIndex);
    }

    public override void UpdateState(GameManager gameManager)
    {
        
    }

    public override void ExitState(GameManager gameManager)
    {
        
    }
}
