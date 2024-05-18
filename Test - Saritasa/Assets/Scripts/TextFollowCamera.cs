using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextFollowCamera : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("No main camera found. Please ensure there is a camera tagged as 'MainCamera'.");
        }
    }

    void Update()
    {
        if (mainCamera != null)
        {
            Vector3 directionToCamera = mainCamera.transform.position - gameObject.transform.position;
            Quaternion rotation = Quaternion.LookRotation(directionToCamera) * Quaternion.Euler(0, 180, 0);

            gameObject.transform.rotation = rotation;
        }
    }
}
