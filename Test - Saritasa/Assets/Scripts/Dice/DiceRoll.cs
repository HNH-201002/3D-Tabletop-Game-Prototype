using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRoll : MonoBehaviour
{
    private Rigidbody rb;
    private bool isRolling = false;

    public float rollForce = 10f; 
    public float rollTorque = 10f;
    int result = 0;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isRolling)
        {
            rb.useGravity = true;
            Debug.Log("is rolling");
            RollDice();
        }

        if (isRolling && rb.IsSleeping())
        {
            isRolling = false;

            Debug.Log("Dice Result: " + result);
        }
    }
    public void RollDice()
    {
        isRolling = true;
        rb.useGravity = true;
        rb.isKinematic = false;

        // Apply random force and torque
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
    private void GetDiceResult(int result)
    {
        this.result = result;
    }
    private void OnEnable()
    {
        FaceDetector.OnDiceRolled += GetDiceResult;
    }
}
