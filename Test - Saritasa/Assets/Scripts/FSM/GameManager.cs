using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    public GameObject[] players;
    public int numberOfPlayers = 2;
    private int currentPlayerIndex = 0;
    private GameState currentState;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SetState(new PlayerTurnState());
    }

    public void SetState(GameState newState)
    {
        currentState?.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }

    public void EndTurn()
    {
        currentPlayerIndex = (currentPlayerIndex + 1) % numberOfPlayers;
        SetState(new PlayerTurnState());
    }

    public int GetCurrentPlayerIndex()
    {
        return currentPlayerIndex;
    }

    public void StartTurn(int playerIndex)
    {
        players[playerIndex].GetComponent<PlayerController>().StartTurn();
    }
}
