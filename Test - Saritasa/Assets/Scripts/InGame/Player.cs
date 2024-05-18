using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private TMP_Text playerNameText;

    public void SetPlayerName(string name)
    {
        playerNameText.text = name;
    }
}
