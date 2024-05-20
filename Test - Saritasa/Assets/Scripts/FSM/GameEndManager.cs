using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEndManager : MonoBehaviour
{
    [SerializeField] private GameObject panelStatistic;
    [SerializeField] private GameObject containStatisticGO;
    [SerializeField] private GameObject statisticPrefab;
    [SerializeField] private Button restartButton;
    [SerializeField] private FreeFlyCamera freeFlyCamera;

    private List<bool> playerCompletionStatus;
    private Queue<PlayerController> playerControllers = new Queue<PlayerController>();
    private Vector3 initialScale;
    private const string RestartSceneName = "MainMenu";

    public static event Action OnAllPlayersCompleted;

    private void Start()
    {
        initialScale = panelStatistic.transform.localScale;
        panelStatistic.SetActive(false);
        InitializePlayerCompletionStatus(GameManager.Instance.numberOfPlayers);
        restartButton.onClick.AddListener(RestartGame);
    }

    private void InitializePlayerCompletionStatus(int numberOfPlayers)
    {
        playerCompletionStatus = new List<bool>(new bool[numberOfPlayers]);
    }

    private void HandlePlayerEndRoute(int playerIndex, PlayerController playerData)
    {
        playerCompletionStatus[playerIndex] = true;
        playerControllers.Enqueue(playerData);
        if (CheckAllPlayersCompleted())
        {
            freeFlyCamera.GetComponent<FreeFlyCamera>().enabled = false; 
            Cursor.visible = true;
            Cursor.lockState =  CursorLockMode.None;
            OnAllPlayersCompleted?.Invoke(); 
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
        panelStatistic.transform.localScale = Vector3.zero;
        panelStatistic.transform.DOScale(initialScale, 0.5f).SetEase(Ease.OutBounce);
        int playerIndex = 1;

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

    private void RestartGame()
    {
        SceneManager.LoadScene(RestartSceneName, LoadSceneMode.Single);
    }

    private void OnEnable()
    {
        EventManager.OnPlayerEndRoute += HandlePlayerEndRoute;
    }

    private void OnDisable()
    {
        EventManager.OnPlayerEndRoute -= HandlePlayerEndRoute;
    }
}
