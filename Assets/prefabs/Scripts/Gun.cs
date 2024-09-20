using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class Gun : MonoBehaviour
{
    public Transform gunBarrel;
    public float bulletSpeed = 10f; // Vận tốc ban đầu
    public float angle; // Góc phóng alpha
    public Button shootButton;
    public float shootDelay = 1.3f;
    public float bulletLifetime = 5f;
    public Vector3 initialPos;
    public Quaternion initialRotation;
    public float gravity = 9.8f; // Gia tốc trọng trường

    public RotateObjectXUp rotateObjectXUp;

    private bool canShoot = true;
    private bool bulletActive = false;
    private float timeElapsed = 0f; // Thời gian đã trôi qua kể từ khi bắn

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
        timeElapsed = 0f; // Đặt lại thời gian bắn
        StartCoroutine(MoveBullet());
    }

    IEnumerator MoveBullet()
    {
        float rotationX = rotateObjectXUp.GetCurrentRotation() - 60;
        Debug.Log("rotationX" + rotationX);
        float radians = rotationX * Mathf.Deg2Rad; // Chuyển đổi góc từ độ sang radian
        Vector3 initialVelocity = new Vector3(0, bulletSpeed * Mathf.Sin(radians), -bulletSpeed * Mathf.Cos(radians)); // Di chuyển về phía trước (Z dương)

        while (timeElapsed < bulletLifetime)
        {
            timeElapsed += Time.deltaTime;

            // Tính toán vị trí mới của viên đạn dựa trên quỹ đạo
            float z = initialVelocity.z * timeElapsed; // Di chuyển theo trục Z (phía trước)
            float y = (initialVelocity.y * timeElapsed) - (0.5f * gravity * timeElapsed * timeElapsed); // Di chuyển theo trục Y (lên/xuống)

            // Dịch chuyển viên đạn về phía trước
            transform.position = initialPos + new Vector3(0, y, z);

            // Nếu viên đạn rơi xuống dưới mức ban đầu (y <= 0), dừng lại
            if (transform.position.y <= initialPos.y)
            {
                break;
            }

            yield return null;
        }

        ResetBullet();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Run");
        if (other.gameObject.CompareTag("Target"))
        {
            Debug.Log("collided");
            Destroy(other.gameObject);
            ResetBullet();
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log("Run");
    //    if(collision.gameObject.CompareTag("Target"))
    //    {
    //        Debug.Log("Collided");
    //        Destroy(collision.gameObject);
    //        ResetBullet();
    //    }        
    //}

    void ResetBullet()
    {
        // Đặt lại viên đạn về vị trí ban đầu
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
