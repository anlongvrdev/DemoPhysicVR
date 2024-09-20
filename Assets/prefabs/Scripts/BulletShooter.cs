using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class BulletShooter : MonoBehaviour
{
    public Transform gunBarrel;
    public float bulletSpeed = 10f;
    public Button shootButton;
    public float shootDelay = 1.3f;
    public float bulletLifetime = 5f;
    public Vector3 initialPos;
    public Quaternion initialRotation;

    private bool canShoot = true;
    private bool bulletActive = false;

    void Start()
    {
        initialPos = transform.position;
        initialRotation = transform.rotation;

        shootButton.onClick.AddListener(TryShootBullet);
    }

    void TryShootBullet()
    {
        if (canShoot && !bulletActive)
        {
            ShootBullet();
            StartCoroutine(DelayNextShot());
        }
    }

    void ShootBullet()
    {
        // Đặt lại vị trí và hướng của viên đạn về vị trí của nòng súng
        transform.position = gunBarrel.position;
        transform.rotation = gunBarrel.rotation;

        bulletActive = true; // Đánh dấu rằng viên đạn hiện đang hoạt động
        StartCoroutine(MoveBullet());
    }

    IEnumerator MoveBullet()
    {
        float elapsedTime = 0f;

        while (elapsedTime < bulletLifetime)
        {
            Debug.Log("Shoot");
            transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        ResetBullet();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Target"))
        {
            Destroy(other.gameObject);
            //gameObject.SetActive(false);
            ResetBullet();
           //gameObject.SetActive(true);
            
        }
    }

    void ResetBullet()
    {
        Debug.Log("Reset");
        transform.position = initialPos;
        transform.rotation = initialRotation;
        bulletActive = false;
    }

    IEnumerator DelayNextShot()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootDelay);
        canShoot = true;
    }
}
