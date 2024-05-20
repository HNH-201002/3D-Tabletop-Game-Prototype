using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIBarController : MonoBehaviour
{
    [SerializeField] private Slider uiBar;
    [SerializeField] private float increaseRate = 2;

    private Image fillImage;

    private Color colorStart = Color.green;
    private Color colorMiddle = Color.yellow;
    private Color colorEnd = Color.red;

    private Coroutine hideBarCoroutine;
    private bool canIncrease = true;

    public static Action<float> OnValueBarChanged;

    private void Start()
    {
        fillImage = uiBar.fillRect.GetComponent<Image>();
        uiBar.gameObject.SetActive(false);
        uiBar.value = 0;
        fillImage.color = colorStart;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!uiBar.gameObject.activeSelf)
            {
                uiBar.gameObject.SetActive(true);
                uiBar.value = 0;
                fillImage.color = colorStart;
                canIncrease = true; 
            }

            if (hideBarCoroutine != null)
            {
                StopCoroutine(hideBarCoroutine);
                hideBarCoroutine = null;
            }
        }

        if (Input.GetKey(KeyCode.Space) && canIncrease)
        {
            uiBar.value += increaseRate;

            if (hideBarCoroutine != null)
            {
                StopCoroutine(hideBarCoroutine);
                hideBarCoroutine = null;
            }
        }
        else if (uiBar.gameObject.activeSelf && hideBarCoroutine == null)
        {
            hideBarCoroutine = StartCoroutine(HideBarAfterDelay(0.25f));
        }

        UpdateBarColor();

        if (uiBar.value >= 100 && hideBarCoroutine == null)
        {
            hideBarCoroutine = StartCoroutine(HideBarAfterDelay(0.25f));
            canIncrease = false; 
        }
    }

    private void UpdateBarColor()
    {
        if (uiBar.value <= 50)
        {
            fillImage.color = colorStart;
        }
        else if (uiBar.value > 50 && uiBar.value < 75)
        {
            fillImage.color = Color.Lerp(colorStart, colorMiddle, (uiBar.value - 50) / 25f);
        }
        else if (uiBar.value >= 75)
        {
            fillImage.color = Color.Lerp(colorMiddle, colorEnd, (uiBar.value - 75) / 25f);
        }
    }

    private IEnumerator HideBarAfterDelay(float delay)
    {
        OnValueBarChanged?.Invoke(uiBar.value);
        yield return new WaitForSeconds(delay);
        UIManager.Instance.TurnOffHoldUi();
        uiBar.gameObject.SetActive(false); 
        canIncrease = false;
    }

    private void OnEnable()
    {
        if (fillImage == null) return;
        fillImage.color = colorStart;
    }

}
