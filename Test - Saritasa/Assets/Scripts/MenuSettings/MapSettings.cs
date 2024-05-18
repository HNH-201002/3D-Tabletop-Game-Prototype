using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MapSettings : MonoBehaviour
{
    [SerializeField] private TMP_InputField numberOfPoints;
    [SerializeField] private TMP_InputField numberOfBonusPoints;
    [SerializeField] private TMP_InputField numberOfFailPoints;

    [SerializeField] private int defaultNumberOfPoints = 10;
    [SerializeField] private int defaultNumberOfBonusPoints = 5;
    [SerializeField] private int defaultNumberOfFailPoints = 3;

    [SerializeField] private TMP_Text errorInform;

    private void Start()
    {
        GameDataManager.Instance.OnStartedGame += HandleGameStarted;

        SetDefaultValues();
        SetInputFieldToAcceptNumbers(numberOfPoints);
        SetInputFieldToAcceptNumbers(numberOfBonusPoints);
        SetInputFieldToAcceptNumbers(numberOfFailPoints);
    }

    private void SetDefaultValues()
    {
        numberOfPoints.text = defaultNumberOfPoints.ToString();
        numberOfBonusPoints.text = defaultNumberOfBonusPoints.ToString();
        numberOfFailPoints.text = defaultNumberOfFailPoints.ToString();
    }

    private void SetInputFieldToAcceptNumbers(TMP_InputField inputField)
    {
        inputField.contentType = TMP_InputField.ContentType.IntegerNumber;
    }

    public void HandleGameStarted()
    {
        if (ValidateInputFields())
        {
            int[] dataMapSetting = {int.Parse(numberOfPoints.text),int.Parse(numberOfBonusPoints.text),int.Parse(numberOfFailPoints.text)};
            GameDataManager.Instance.SetMapSetting(dataMapSetting);
        }
        else
        {
            errorInform.text = "One or more input fields have invalid values. Please ensure all fields contain valid numbers.";
        }
    }

    private bool ValidateInputFields()
    {
        bool isValid = true;

        if (!int.TryParse(numberOfPoints.text, out _))
        {
            isValid = false;
            errorInform.text =  "Invalid number of points." ;
        }

        if (!int.TryParse(numberOfBonusPoints.text, out _))
        {
            isValid = false;
            errorInform.text = "Invalid number of bonus points.";
        }

        if (!int.TryParse(numberOfFailPoints.text, out _))
        {
            isValid = false;
            errorInform.text = "Invalid number of fail points.";
        }

        return isValid;
    }


    private void OnDisable()
    {
        GameDataManager.Instance.OnStartedGame -= HandleGameStarted;
    }
}
