using UnityEngine;

public class PlayerTurnState : GameState
{
    public override void EnterState(GameManager gameManager)
    {
        int currentPlayerIndex = gameManager.GetCurrentPlayerIndex();
        Debug.Log("Player " + (currentPlayerIndex + 1) + "'s turn.");
        gameManager.StartTurn(currentPlayerIndex);
    }

    public override void UpdateState(GameManager gameManager)
    {
        // Update logic for player turn if necessary
    }

    public override void ExitState(GameManager gameManager)
    {
        // Cleanup if necessary
    }
}
