using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject[] playerPrefabs;
    [SerializeField] private Transform spawnPointParent;

    private List<GameObject> players = new List<GameObject>();
    [SerializeField] private PathFinder pathFinder;

    private void Start()
    {
        SpawnPlayers();
    }

    private void SpawnPlayers()
    {
        List<PlayerNameInputMenu> playerNames = GameDataManager.Instance.PlayerProfiles;
        HashSet<string> uniqueNames = new HashSet<string>();
        List<int> usedPrefabs = new List<int>();
        int unknownCount = 1;

        for (int i = 0; i < playerNames.Count; i++)
        {
            string inputName = playerNames[i].GetInputNamePlayer();

            if (string.IsNullOrEmpty(inputName) || uniqueNames.Contains(inputName))
            {
                inputName = "Player" + unknownCount;
                unknownCount++;
            }

            uniqueNames.Add(inputName);
            int prefabIndex = GetUniquePrefabIndex(usedPrefabs);

            GameObject playerObj = Instantiate(playerPrefabs[prefabIndex], GetSpawnPoint(i), Quaternion.Euler(0, 90, 0));
            Player player = playerObj.GetComponent<Player>();

            Color randomColor = Random.ColorHSV();
            player.SetPlayerName(inputName, randomColor);

            players.Add(playerObj);

            playerObj.GetComponent<PlayerController>().spawnedPoints = pathFinder.spawnedPoints;
        }

        EventManager.PlayersSpawned(players);
    }

    private int GetUniquePrefabIndex(List<int> usedPrefabs)
    {
        if (usedPrefabs.Count >= playerPrefabs.Length)
        {
            usedPrefabs.Clear();
        }

        int prefabIndex;
        do
        {
            prefabIndex = Random.Range(0, playerPrefabs.Length);
        } while (usedPrefabs.Contains(prefabIndex));

        usedPrefabs.Add(prefabIndex);
        return prefabIndex;
    }

    private Vector3 GetSpawnPoint(int index)
    {
        if (spawnPointParent != null && spawnPointParent.childCount > index)
        {
            return spawnPointParent.GetChild(index).position;
        }
        return Vector3.zero;
    }
    public List<GameObject> GetPlayers()
    {
        return players;
    }
}
