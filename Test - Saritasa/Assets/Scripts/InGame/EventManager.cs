using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static event Action<List<GameObject>> OnPlayersSpawned;

    public static void PlayersSpawned(List<GameObject> players)
    {
        OnPlayersSpawned?.Invoke(players);
    }
}
