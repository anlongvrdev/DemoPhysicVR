using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObjectToRightbyClicking : MonoBehaviour
{
    public Transform objectToRotate; // The object to rotate
    public float rotationAngle; // Angle to rotate per click

    // This method will be called when the button is clicked
    public void RotateRight()
    {
        if (objectToRotate != null)
        {
            objectToRotate.Rotate(Vector3.up, rotationAngle);
        }
    }
}
