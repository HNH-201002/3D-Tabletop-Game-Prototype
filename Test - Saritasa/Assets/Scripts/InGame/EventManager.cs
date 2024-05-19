using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static event Action<List<GameObject>> OnPlayersSpawned;
    public static event Action<int,PlayerController> OnPlayerEndTurn;
    public static void PlayersSpawned(List<GameObject> players)
    {
        OnPlayersSpawned?.Invoke(players);
    }

    public static void PlayerEndTurn(int playerIndex,PlayerController playerController)
    {
        OnPlayerEndTurn?.Invoke(playerIndex,playerController);
    }
}
