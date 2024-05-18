using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private TMP_Text playerNameText;

    public void SetPlayerName(string name, Color color)
    {
        playerNameText.text = name;
        playerNameText.color = color;
    }
}
