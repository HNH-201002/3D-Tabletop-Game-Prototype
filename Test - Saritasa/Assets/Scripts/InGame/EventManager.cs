using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static event Action<List<GameObject>> OnPlayersSpawned;
    public static event Action<int,PlayerController> OnPlayerEndRoute;
    public static void PlayersSpawned(List<GameObject> players)
    {
        OnPlayersSpawned?.Invoke(players);
    }

    public static void PlayerEndTurn(int playerIndex,PlayerController playerController)
    {
        OnPlayerEndRoute?.Invoke(playerIndex,playerController);
    }
}
