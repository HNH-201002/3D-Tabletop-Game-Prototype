using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;
using DG.Tweening; 

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance { get; private set; }

    public List<PlayerNameInputMenu> PlayerProfiles { get; private set; } = new List<PlayerNameInputMenu>();
    public int[] mapSettings = new int[3];

    [SerializeField] private Button startGame;
    [SerializeField] private TMP_Text errorInform;

    public Action OnStartedGame;
    private bool isPrepareForStartGame = true;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void Start()
    {
        AssignReferences();
    }

    private void AssignReferences()
    {
        UIReferences uiReferences = FindObjectOfType<UIReferences>();
        if (uiReferences != null)
        {
            startGame = uiReferences.StartGameButton;
            errorInform = uiReferences.ErrorInformText;

            if (startGame != null)
            {
                startGame.onClick.AddListener(StartGame);
                AnimateStartGameButton();  
            }
            else
            {
                Debug.LogError("StartGame button reference is missing.");
            }

            if (errorInform == null)
            {
                Debug.LogError("ErrorInform text reference is missing.");
            }
        }
        else
        {
            return;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AssignReferences();
    }

    private void StartGame()
    {
        if (isPrepareForStartGame)
        {
            OnStartedGame?.Invoke();

            if (isPrepareForStartGame)
            {
                SceneManager.LoadScene("GamePlayScene");
                errorInform.text = "";
            }
            else
            {
                errorInform.text = "Game isn't ready to start. Please check all settings.";
            }
        }
        else
        {
            errorInform.text = "Game isn't ready to start. Please check all settings.";
            isPrepareForStartGame = true;
        }
    }

    public void SetPlayerProfile(List<PlayerNameInputMenu> profile)
    {
        if (profile == null || profile.Count == 0 || profile.Contains(null))
        {
            isPrepareForStartGame = false;
            errorInform.text = "No player profiles set or contains invalid entries.";
        }
        else
        {
            PlayerProfiles = profile;
        }
    }

    public void SetMapSetting(int[] mapSettings)
    {
        if (mapSettings == null || mapSettings.Length == 0 || mapSettings.Any(setting => setting <= 0))
        {
            isPrepareForStartGame = false;
            errorInform.text = "Map settings not set or contains invalid entries.";
        }
        else
        {
            this.mapSettings = mapSettings;
        }
    }

    public void ClearPlayerProfiles()
    {
        PlayerProfiles.Clear();
    }

    private void AnimateStartGameButton()
    {
        startGame.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.8f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutQuad);
    }
}
