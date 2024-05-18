using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField numberOfPlayersInputField;
    [SerializeField] private Button confirmButton;
    [SerializeField] private GameObject playerProfilePrefab;
    [SerializeField] private GameObject nameInputPlayerContainer;
    [SerializeField] private TMP_Text errorInform;

    private List<PlayerNameInputMenu> playerProfileContainGameObjects = new List<PlayerNameInputMenu>();

    void Start()
    {
        GameDataManager.Instance.OnStartedGame += HandleGameStarted;
        confirmButton.onClick.AddListener(OnConfirmButtonClicked);
        SetInputFieldToAcceptNumbers(numberOfPlayersInputField);
    }
    private void SetInputFieldToAcceptNumbers(TMP_InputField inputField)
    {
        inputField.contentType = TMP_InputField.ContentType.IntegerNumber;
    }

    void OnConfirmButtonClicked()
    {
        if (int.TryParse(numberOfPlayersInputField.text, out int playerCount))
        {
            CreateNameInputs(playerCount);
            errorInform.text = "";
        }
        else
        {
            errorInform.text = "Invalid number of players input";
        }
    }

    void CreateNameInputs(int playerCount)
    {
        int currentCount = playerProfileContainGameObjects.Count;

        if (playerCount == currentCount)
            return;

        if (playerCount < currentCount)
        {
            for (int i = 0; i < currentCount - playerCount; i++)
            {
                var playerNameInputMenu = playerProfileContainGameObjects.Last();
                playerProfileContainGameObjects.RemoveAt(playerProfileContainGameObjects.Count - 1);
                Destroy(playerNameInputMenu.gameObject); 
            }
        }
        else
        {
            for (int i = 0; i < playerCount - currentCount; i++)
            {
                GameObject nameInputObj = Instantiate(playerProfilePrefab, nameInputPlayerContainer.transform);
                PlayerNameInputMenu playerNameInputMenu = nameInputObj.GetComponent<PlayerNameInputMenu>();
                playerNameInputMenu.SetNameTextPlayer(playerProfileContainGameObjects.Count + 1);
                playerProfileContainGameObjects.Add(playerNameInputMenu);
            }
        }
    }
    public void HandleGameStarted()
    {
        GameDataManager.Instance.SetPlayerProfile(playerProfileContainGameObjects);
    }
    private void OnDisable()
    {
        GameDataManager.Instance.OnStartedGame -= HandleGameStarted;
    }
}
