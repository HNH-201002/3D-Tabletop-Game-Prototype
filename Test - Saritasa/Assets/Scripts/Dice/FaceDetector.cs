using System;
using UnityEngine;

public class FaceDetector : MonoBehaviour
{
    public static Action<int> OnDiceRolled;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            OnDiceRolled?.Invoke(int.Parse(gameObject.name));
        }
    }
}
