using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStatisticsUI : MonoBehaviour
{
    [SerializeField] private TMP_Text placeText;
    [SerializeField] private TMP_Text playerNameText;
    [SerializeField] private TMP_Text turnsTakenText;
    [SerializeField] private TMP_Text bonusPointsText;
    [SerializeField] private TMP_Text failPointsText;

    public void SetPlayerData(int order,PlayerStatistics statistics)
    {
        placeText.text = order.ToString();
        playerNameText.text = statistics.name;  
        turnsTakenText.text = statistics.TurnsTaken.ToString();
        bonusPointsText.text = statistics.BonusPoints.ToString();
        failPointsText.text = statistics.FailPoints.ToString();
    }
}
