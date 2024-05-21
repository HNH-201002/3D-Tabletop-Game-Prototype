using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class DiceRoll : MonoBehaviour
{
    private Rigidbody rb;
    private bool isRolling = false;
    private int result = 0;

    [SerializeField] private float rollForce = 2;
    [SerializeField] private float rollTorque = 2;
    private float valueBarForce = 1f;

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector3 initialScale;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        initialScale = transform.localScale;

        UIBarController.OnValueBarChanged += HandleValueBarChanged;
    }

    private void OnDestroy()
    {
        UIBarController.OnValueBarChanged -= HandleValueBarChanged;
    }

    private void HandleValueBarChanged(float value)
    {
        valueBarForce = value;
    }

    public IEnumerator Roll(Action<int> callback)
    {
        if (!isRolling)
        {
            isRolling = true;
            gameObject.SetActive(true);
            transform.localScale = Vector3.zero; 
            transform.DOScale(initialScale, 0.5f).SetEase(Ease.OutBounce); 
            yield return new WaitForSeconds(0.5f);
            rb.useGravity = true;
            HandleRoll();
            yield return StartCoroutine(WaitForRoll(callback));
        }
    }

    private IEnumerator WaitForRoll(Action<int> callback)
    {
        float elapsedTime = 0f;
        const float timeout = 3;

        while (!rb.IsSleeping() && elapsedTime < timeout)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (rb.IsSleeping())
        {
            callback(result);
        }
        else
        {
            result = UnityEngine.Random.Range(1, 7);
            callback(result);
        }

        isRolling = false;
        ResetDice();
    }

    public void HandleRoll()
    {
        rb.useGravity = true;
        rb.isKinematic = false;

        float adjustedValueBarForce = Mathf.Max(valueBarForce / 100, 0.1f); 

        Vector3 randomForce = new Vector3(
            UnityEngine.Random.Range(-1f, 1f),
            1f,
            UnityEngine.Random.Range(-1f, 1f)
        ) * rollForce * adjustedValueBarForce;

        Vector3 randomTorque = new Vector3(
            UnityEngine.Random.Range(-1f, 1f),
            UnityEngine.Random.Range(-1f, 1f),
            UnityEngine.Random.Range(-1f, 1f)
        ) * rollTorque * adjustedValueBarForce;

        rb.AddForce(randomForce, ForceMode.Impulse);
        rb.AddTorque(randomTorque, ForceMode.Impulse);
    }

    private void ResetDice()
    {
        rb.useGravity = false;
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBounce).OnComplete(() =>
        {
            transform.position = initialPosition;
            transform.rotation = initialRotation;
            transform.localScale = initialScale; 
            gameObject.SetActive(false);
        });
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
