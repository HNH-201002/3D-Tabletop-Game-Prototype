using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform spawnPointParent;

    private void Start()
    {
        SpawnPlayers();
    }

    private void SpawnPlayers()
    {
        List<PlayerNameInputMenu> playerNames = GameDataManager.Instance.PlayerProfiles;

        for (int i = 0; i < playerNames.Count; i++)
        {
            GameObject playerObj = Instantiate(playerPrefab, GetSpawnPoint(i), Quaternion.Euler(0,90, 0));
            Player player = playerObj.GetComponent<Player>();
            // player.SetPlayerName(playerNames[i].GetInputNamePlayer());
        }
    }

    private Vector3 GetSpawnPoint(int index)
    {
        if (spawnPointParent != null && spawnPointParent.childCount > index)
        {
            return spawnPointParent.GetChild(index).position;
        }
        return Vector3.zero; 
    }
}
