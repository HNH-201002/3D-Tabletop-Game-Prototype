using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathFinder : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public GameObject pointPrefab;
    public GameObject bonusPointPrefab;
    public GameObject failPointPrefab;
    public int totalNumberOfPoints = 50;
    public int numberOfBonusPoints = 5;
    public int numberOfFailPoints = 5;
    public float maxSampleDistance = 1.0f; // Khoảng cách tối đa để sample vị trí trên NavMesh

    private List<Vector3> sampledPoints = new List<Vector3>();
    private HashSet<Vector3> occupiedPositions = new HashSet<Vector3>();

    private void Start()
    {
        if (CalculateNavMeshPath(out NavMeshPath navMeshPath))
        {
            SamplePathPoints(navMeshPath);
            SpawnPoints(pointPrefab, bonusPointPrefab, failPointPrefab, sampledPoints, numberOfBonusPoints, numberOfFailPoints);
            Debug.Log("Path sampling complete.");
        }
        else
        {
            Debug.LogError("Failed to calculate path or path is incomplete.");
        }
    }

    bool CalculateNavMeshPath(out NavMeshPath navMeshPath)
    {
        navMeshPath = new NavMeshPath();

        if (!NavMesh.SamplePosition(startPoint.position, out NavMeshHit startHit, maxSampleDistance, NavMesh.AllAreas))
        {
            Debug.LogError("Start point is not on the NavMesh.");
            return false;
        }
        Vector3 validStartPoint = startHit.position;

        if (!NavMesh.SamplePosition(endPoint.position, out NavMeshHit endHit, maxSampleDistance, NavMesh.AllAreas))
        {
            Debug.LogError("End point is not on the NavMesh.");
            return false;
        }
        Vector3 validEndPoint = endHit.position;

        return NavMesh.CalculatePath(validStartPoint, validEndPoint, NavMesh.AllAreas, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete;
    }

    void SamplePathPoints(NavMeshPath navMeshPath)
    {
        sampledPoints.Clear();
        if (navMeshPath.corners.Length == 0) return;

        float totalPathLength = 0.0f;
        for (int i = 1; i < navMeshPath.corners.Length; i++)
        {
            totalPathLength += Vector3.Distance(navMeshPath.corners[i - 1], navMeshPath.corners[i]);
        }

        float distanceBetweenPoints = totalPathLength / (totalNumberOfPoints - 1);
        float accumulatedDistance = 0.0f;
        sampledPoints.Add(navMeshPath.corners[0]);

        for (int i = 1; i < navMeshPath.corners.Length; i++)
        {
            Vector3 startPoint = navMeshPath.corners[i - 1];
            Vector3 endPoint = navMeshPath.corners[i];
            float segmentLength = Vector3.Distance(startPoint, endPoint);

            while (accumulatedDistance + segmentLength >= distanceBetweenPoints)
            {
                float t = (distanceBetweenPoints - accumulatedDistance) / segmentLength;
                Vector3 sampledPoint = Vector3.Lerp(startPoint, endPoint, t);

                if (NavMesh.SamplePosition(sampledPoint, out NavMeshHit hit, maxSampleDistance, NavMesh.AllAreas))
                {
                    sampledPoint = hit.position;
                }

                sampledPoints.Add(sampledPoint);
                accumulatedDistance = 0.0f;
                startPoint = sampledPoint;
                segmentLength = Vector3.Distance(startPoint, endPoint);
            }

            accumulatedDistance += segmentLength;
        }

        if (sampledPoints.Count < totalNumberOfPoints)
        {
            sampledPoints.Add(navMeshPath.corners[navMeshPath.corners.Length - 1]);
        }
    }

    void SpawnPoints(GameObject pointPrefab, GameObject bonusPointPrefab, GameObject failPointPrefab, List<Vector3> points, int numberOfBonusPoints, int numberOfFailPoints)
    {
        int bonusPointsSpawned = 0;
        int failPointsSpawned = 0;

        List<Vector3> availablePositions = new List<Vector3>(points);
        availablePositions.RemoveAt(0); // Remove start point
        availablePositions.RemoveAt(availablePositions.Count - 1); // Remove end point

        SpawnSpecialPoints(bonusPointPrefab, availablePositions, ref bonusPointsSpawned, numberOfBonusPoints);
        SpawnSpecialPoints(failPointPrefab, availablePositions, ref failPointsSpawned, numberOfFailPoints);

        foreach (Vector3 position in points)
        {
            if (!occupiedPositions.Contains(position))
            {
                Instantiate(pointPrefab, position, Quaternion.identity);
                occupiedPositions.Add(position);
            }
        }
    }

    void SpawnSpecialPoints(GameObject specialPrefab, List<Vector3> availablePositions, ref int pointsSpawned, int numberOfPoints)
    {
        while (pointsSpawned < numberOfPoints && availablePositions.Count > 0)
        {
            int randomIndex = Random.Range(0, availablePositions.Count);
            Vector3 randomPosition = availablePositions[randomIndex];
            Instantiate(specialPrefab, randomPosition, Quaternion.identity);
            occupiedPositions.Add(randomPosition);
            availablePositions.RemoveAt(randomIndex);
            pointsSpawned++;
        }
    }
}
