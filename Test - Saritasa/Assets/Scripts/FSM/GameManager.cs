using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    [HideInInspector]
    public List<GameObject> players;
    [HideInInspector]
    public int numberOfPlayers { get; private set; }
    private int currentPlayerIndex = 0;
    private GameState currentState;
    private bool playersSpawned = false;

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
        numberOfPlayers = GameDataManager.Instance.PlayerProfiles.Count;
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

        while (!players[currentPlayerIndex].activeSelf)
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % numberOfPlayers;
        }

        SetState(new PlayerTurnState());
    }

    public int GetCurrentPlayerIndex()
    {
        return currentPlayerIndex;
    }

    public void StartTurn(int playerIndex)
    {
        if (!playersSpawned)
        {
            Debug.LogError("Players have not been spawned yet.");
            return;
        }

        playerIndex = playerIndex >= numberOfPlayers ? 0 : playerIndex;
        players[playerIndex].GetComponent<PlayerController>().StartTurn();
    }

    private void HandlePlayersSpawned(List<GameObject> players)
    {
        this.players = players;
        playersSpawned = true;
    }
    private void OnEnable()
    {
        EventManager.OnPlayersSpawned += HandlePlayersSpawned;
    }

    private void OnDisable()
    {
        EventManager.OnPlayersSpawned -= HandlePlayersSpawned;
    }

}
