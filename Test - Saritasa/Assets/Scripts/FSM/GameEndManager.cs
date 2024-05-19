using System.Collections.Generic;
using UnityEngine;

public class GameEndManager : MonoBehaviour
{
    [SerializeField] private GameObject panelStatistic;
    [SerializeField] private GameObject containStatisticGO;
    [SerializeField] private GameObject statisticPrefab;
    private List<bool> playerCompletionStatus;
    private Queue<PlayerController> playerControllers = new Queue<PlayerController>();

    private void OnEnable()
    {
        EventManager.OnPlayerEndTurn += HandlePlayerEndTurn;
    }

    private void OnDisable()
    {
        EventManager.OnPlayerEndTurn -= HandlePlayerEndTurn;
    }

    private void Start()
    {
        panelStatistic.SetActive(false);
        InitializePlayerCompletionStatus(GameManager.Instance.numberOfPlayers);
    }

    private void InitializePlayerCompletionStatus(int numberOfPlayers)
    {
        playerCompletionStatus = new List<bool>(new bool[numberOfPlayers]);
    }

    private void HandlePlayerEndTurn(int playerIndex, PlayerController playerData)
    {
        playerCompletionStatus[playerIndex] = true;
        playerControllers.Enqueue(playerData);
        if (CheckAllPlayersCompleted())
        {
            DisplayEndGameStats();
        }
    }

    private bool CheckAllPlayersCompleted()
    {
        foreach (bool status in playerCompletionStatus)
        {
            if (!status)
            {
                return false;
            }
        }
        return true;
    }

    private void DisplayEndGameStats()
    {
        panelStatistic.SetActive(true);
        int playerIndex = 0;

        foreach (var playerController in playerControllers)
        {
            GameObject statInstance = Instantiate(statisticPrefab, containStatisticGO.transform);
            PlayerStatisticsUI statUI = statInstance.GetComponent<PlayerStatisticsUI>();

            if (statUI != null)
            {
                statUI.SetPlayerData(playerIndex, playerController.Statistics);
            }

            playerIndex++;
        }
    }
}
