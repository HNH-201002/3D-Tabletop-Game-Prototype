using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public void StartTurn()
    {
        Debug.Log("Player " + gameObject.name + " turn started.");
        // Trigger dice roll
        StartCoroutine(RollDiceAndMove());
    }

    private IEnumerator RollDiceAndMove()
    {
        // Simulate dice roll
        int diceResult = RollDice();
        Debug.Log("Rolled a " + diceResult);

        // Move character based on dice result
        for (int i = 0; i < diceResult; i++)
        {
            // Move one step
            transform.position += Vector3.forward; // Example movement
            yield return new WaitForSeconds(0.5f);
        }

        // End turn after movement
        GameManager.Instance.EndTurn();
    }

    private int RollDice()
    {
        return Random.Range(1, 7);
    }
}
