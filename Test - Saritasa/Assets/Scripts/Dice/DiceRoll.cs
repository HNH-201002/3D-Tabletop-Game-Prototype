using System.Collections;
using UnityEngine;

public class DiceRoll : MonoBehaviour
{
    private Rigidbody rb;
    private bool isRolling = false;
    private int result = 0;

    public float rollForce = 10f;
    public float rollTorque = 10f;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    public IEnumerator Roll(System.Action<int> callback)
    {
        if (!isRolling)
        {
            isRolling = true; // Đặt cờ isRolling ngay khi bắt đầu lăn
            gameObject.SetActive(true);
            rb.useGravity = true;
            Debug.Log("is rolling");
            HandleRoll();
            yield return StartCoroutine(WaitForRoll(callback));
        }
    }

    private IEnumerator WaitForRoll(System.Action<int> callback)
    {
        float elapsedTime = 0f;
        const float timeout = 2f;

        while (!rb.IsSleeping() && elapsedTime < timeout)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (rb.IsSleeping())
        {
            Debug.Log("Dice Result: " + result);
            callback(result);
        }
        else
        {
            result = Random.Range(1, 7);
            callback(result);
        }

        isRolling = false; 
        ResetDice();
    }

    public void HandleRoll()
    {
        rb.useGravity = true;
        rb.isKinematic = false;

        Vector3 randomForce = new Vector3(
            Random.Range(-1f, 1f),
            1f,
            Random.Range(-1f, 1f)
        ) * rollForce;

        Vector3 randomTorque = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        ) * rollTorque;

        rb.AddForce(randomForce, ForceMode.Impulse);
        rb.AddTorque(randomTorque, ForceMode.Impulse);
    }

    private void ResetDice()
    {
        rb.useGravity = false;
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        gameObject.SetActive(false);
    }

    private void SetDiceResult(int rolledResult)
    {
        result = rolledResult;
    }

    private void OnEnable()
    {
        FaceDetector.OnDiceRolled += SetDiceResult;
    }

    private void OnDisable()
    {
        FaceDetector.OnDiceRolled -= SetDiceResult;
    }
}
