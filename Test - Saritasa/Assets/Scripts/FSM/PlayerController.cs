using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private DiceRoll diceRoll;
    private int diceResult;
    public List<SpawnedPoint> spawnedPoints { private get; set; }
    public PlayerStatistics Statistics { get; private set; }

    private int currentPointIndex = -1;
    [SerializeField] private Animator ani;
    private void Awake()
    {
        Statistics = gameObject.GetComponent<PlayerStatistics>();
    }
    private void Start()
    {
        diceRoll = FindObjectOfType<DiceRoll>();
    }

    public void StartTurn()
    {
        StartCoroutine(WaitForPlayerInput());
    }

    private IEnumerator WaitForPlayerInput()
    {
        Statistics.IncrementTurnsTaken();   
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        StartCoroutine(RollDiceAndMove());
    }

    private IEnumerator RollDiceAndMove()
    {
        if (diceRoll == null)
        {
            yield break;
        }
        yield return StartCoroutine(diceRoll.Roll(OnDiceResult));
    }

    private void OnDiceResult(int result)
    {
        diceResult = result;
        Debug.Log("Rolled a " + diceResult);
        StartCoroutine(MovePlayer());
    }

    private IEnumerator MovePlayer()
    {
        for (int i = 0; i < diceResult; i++)
        {
            currentPointIndex++;
            if (currentPointIndex >= spawnedPoints.Count)
            {
                EventManager.PlayerEndTurn(GameManager.Instance.GetCurrentPlayerIndex(), this);
                GameManager.Instance.EndTurn();
                yield return new WaitForEndOfFrame(); 
                gameObject.SetActive(false);
                yield break;
            }
            Vector3 targetPosition = spawnedPoints[currentPointIndex].transform.position;
            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                Vector3 direction = (targetPosition - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 0.7f);
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 0.7f); // Speed can be adjusted
                ani.SetBool("Move", true);
                yield return null;
            }
            ani.SetBool("Move", false);
            yield return new WaitForSeconds(0.5f);
        }

        if (spawnedPoints[currentPointIndex].Type == PointType.Bonus)
        {
            Statistics.IncrementBonusPoints();
            StartCoroutine(WaitForPlayerInput());
        }
        else if (spawnedPoints[currentPointIndex].Type == PointType.Fail)
        {
            Statistics.IncrementFailPoints();
            currentPointIndex -= 3;
            if (currentPointIndex < 0)
            {
                currentPointIndex = 0;
            }
            StartCoroutine(MovePlayerToCurrentIndex());
        }
        else
        {
            GameManager.Instance.EndTurn();
        }
    }

    private IEnumerator MovePlayerToCurrentIndex()
    {
        Vector3 targetPosition = spawnedPoints[currentPointIndex].transform.position;

        while (Vector3.Distance(transform.position, targetPosition) > 0.001f)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 0.7f);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 0.7f);
            ani.SetBool("Move", true);
            yield return null;
        }
        ani.SetBool("Move", false);
        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.EndTurn();
    }
}
