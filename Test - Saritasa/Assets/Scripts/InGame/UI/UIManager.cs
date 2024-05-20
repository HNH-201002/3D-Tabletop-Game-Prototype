using System.Collections;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance
    {
        get { return _instance; }
        set { _instance = value; }
    }

    [SerializeField] private TMP_Text turnText;
    [SerializeField] private TMP_Text diceResultText;
    [SerializeField] private GameObject holdUi;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }

        turnText.text = "";
        diceResultText.text = "";
        holdUi.SetActive(false);
    }

    public void SetTurnText(string content)
    {
        turnText.text = $"{content}'s turn";
        turnText.transform.localScale = Vector3.zero; 
        turnText.gameObject.SetActive(true); 
        turnText.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce); 
    }

    public void SetDiceResultText(string content)
    {
        diceResultText.text = content;
        diceResultText.transform.localScale = Vector3.zero; 
        diceResultText.gameObject.SetActive(true); 
        diceResultText.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce); 

        StartCoroutine(ResetDiceResultTextAfterDelay(0.75f));
    }

    public void TurnOnHoldUi()
    {
        holdUi.SetActive(true);
        holdUi.transform.localScale = Vector3.zero; 
        holdUi.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce); 
    }

    public void TurnOffHoldUi()
    {
        holdUi.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBounce)
            .OnComplete(() => holdUi.SetActive(false)); 
    }
    public void HandleWhenGameFinished()
    {
        gameObject.SetActive(false);
    }
    private IEnumerator ResetDiceResultTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        diceResultText.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBounce)
            .OnComplete(() => diceResultText.text = ""); 
    }
    private void OnEnable()
    {
        GameEndManager.OnAllPlayersCompleted += HandleWhenGameFinished;
    }

    private void OnDisable()
    {
        GameEndManager.OnAllPlayersCompleted -= HandleWhenGameFinished;
    }

}
