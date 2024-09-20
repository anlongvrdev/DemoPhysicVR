using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Run");
        if (other.CompareTag("Target"))
        {
            Debug.Log("Collided");
            Destroy(other.gameObject);
            //other.gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
