using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private DiceRoll diceRoll;
    private int diceResult;
    public List<SpawnedPoint> spawnedPoints { private get; set; }
    public PlayerStatistics Statistics { get; private set; }

    private int currentPointIndex = -1;
    [SerializeField] private Animator ani;
    private bool valueBarChanged = false;
    private NavMeshAgent navMeshAgent;

    private void Awake()
    {
        Statistics = gameObject.GetComponent<PlayerStatistics>();
        diceRoll = FindObjectOfType<DiceRoll>();
        UIBarController.OnValueBarChanged += HandleValueBarChanged;
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        diceRoll.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        UIBarController.OnValueBarChanged -= HandleValueBarChanged;
    }

    private void HandleValueBarChanged(float value)
    {
        valueBarChanged = true;
    }

    public void StartTurn()
    {
        UIManager.Instance.SetTurnText(gameObject.name);
        UIManager.Instance.TurnOnHoldUi();
        StartCoroutine(WaitForPlayerInput());
    }

    private IEnumerator WaitForPlayerInput()
    {
        Statistics.IncrementTurnsTaken();
        valueBarChanged = false;
        yield return new WaitUntil(() => valueBarChanged);
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
        UIManager.Instance.SetDiceResultText(result.ToString());
        StartCoroutine(MovePlayer());
    }

    private IEnumerator MovePlayer()
    {
        for (int i = 0; i < diceResult; i++)
        {
            currentPointIndex++;

            Vector3 targetPosition = spawnedPoints[currentPointIndex].transform.position;

            yield return StartCoroutine(RotateTowards(targetPosition));

            navMeshAgent.SetDestination(targetPosition);

            while (navMeshAgent.pathPending || navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
            {
                ani.SetBool("Move", true);
                yield return null;
            }
            ani.SetBool("Move", false);
            yield return new WaitForSeconds(0.5f);

            if (currentPointIndex >= spawnedPoints.Count - 1)
            {
                EventManager.PlayerEndTurn(GameManager.Instance.GetCurrentPlayerIndex(), this);
                GameManager.Instance.EndTurn();
                yield return new WaitForEndOfFrame();
                gameObject.SetActive(false);
                yield break;
            }
        }

        if (spawnedPoints[currentPointIndex].Type == PointType.Bonus)
        {
            Statistics.IncrementBonusPoints();
            StartTurn();
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

        yield return StartCoroutine(RotateTowards(targetPosition));

        navMeshAgent.SetDestination(targetPosition);

        while (navMeshAgent.pathPending || navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
        {
            ani.SetBool("Move", true);
            yield return null;
        }
        ani.SetBool("Move", false);
        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.EndTurn();
    }

    private IEnumerator RotateTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        float elapsedTime = 0f;
        float duration = 0.5f;

        while (elapsedTime < duration)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = lookRotation;
    }
}
