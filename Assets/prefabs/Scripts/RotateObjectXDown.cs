﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateObjectXDown : MonoBehaviour
{
    public Button rotateButton;
    public float rotationAngle = 10f;
    private bool canRotate = true;

    void Start()
    {
        rotateButton.onClick.AddListener(OnRotateButtonPressed);
    }

    public void OnRotateButtonPressed()
    {
        if (canRotate)
        {
            //Debug.Log("Rotate");
            StartCoroutine(RotateWithDelay());
        }
    }

    // Coroutine xoay vật thể và thêm thời gian trễ
    IEnumerator RotateWithDelay()
    {
        canRotate = false; 
        transform.Rotate(-rotationAngle, 0, 0);
        yield return new WaitForSeconds(1f); 
        canRotate = true; 
    }

    public void DisableRotation()
    {
        canRotate = false; 
    }

    public void EnableRotation()
    {
        canRotate = true; 
    }

    public float GetCurrentRotation()
    {
        return transform.eulerAngles.x;
    }
}