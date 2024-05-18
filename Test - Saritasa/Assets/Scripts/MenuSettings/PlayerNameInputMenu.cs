using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameInputMenu : MonoBehaviour
{
    [SerializeField] private Text nameTextPlayer;
    [SerializeField] private TMP_InputField inputNamePlayer;

    public void SetNameTextPlayer(int order)
    {
        nameTextPlayer.text = $"Player {order}";
    }
    public string GetInputNamePlayer()
    { 
        return inputNamePlayer.text;
    }
}
