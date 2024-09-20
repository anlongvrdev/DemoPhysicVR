using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.Receiver.Primitives;

public class BulletShooter1 : MonoBehaviour
{
    public Transform gunBarrel;
    public float bulletSpeed; // Vận tốc mặc định của viên đạn
    public Button shootButton;
    public float shootDelay = 1.3f;
    public float bulletLifetime = 10f;
    public float gravity = 9.8f;    
    public Vector3 initialPos;
    public Quaternion initialRotation;

    public RotateObjectXUp rotateObjectXUp;

    private bool canShoot = true;
    private bool bulletActive = false;
    private float timeElapsed = 0f;
    private Vector3 velocity;

    private Vector3 resetPosition;    
    private Quaternion resetRotation; 

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

        // Lấy góc xoay từ script RotateObjectXUp
        float rotationX = rotateObjectXUp.GetCurrentRotation() - 60;
        Debug.Log("rotationX: " + rotationX);

        // Đổi góc từ độ sang radian
        float radians = rotationX * Mathf.Deg2Rad;

        // Tính vận tốc ban đầu theo trục Y và Z
        float initialVelocityZ = bulletSpeed * Mathf.Cos(radians); // Vận tốc theo trục Z
        float initialVelocityY = bulletSpeed * Mathf.Sin(radians); // Vận tốc theo trục Y (lên/xuống)

        // Tạo vector vận tốc với thành phần Y và Z
        velocity = new Vector3(0, initialVelocityY, initialVelocityZ); // X là 0, không di chuyển theo trục X

        // Lưu lại vị trí và góc xoay mới
        resetPosition = transform.position;
        resetRotation = transform.rotation;

        bulletActive = true;
        timeElapsed = 0f; // Đặt lại thời gian bắn
        StartCoroutine(MoveBullet());
    }


    IEnumerator MoveBullet()
    {
        float verticalVelocity = velocity.y; // Vận tốc ban đầu theo trục Y
        float forwardVelocity = velocity.z;  // Vận tốc ban đầu theo trục Z

        while (timeElapsed < bulletLifetime)
        {
            timeElapsed += Time.deltaTime;

            // Trọng lực tác động lên trục Y, làm giảm dần vận tốc theo trục Y
            verticalVelocity -= gravity * Time.deltaTime;

            // Tính toán khoảng cách di chuyển trong mỗi frame theo trục Y và Z
            Vector3 move = new Vector3(0, verticalVelocity * Time.deltaTime, -forwardVelocity * Time.deltaTime);

            //Debug.Log(move);
            // Di chuyển viên đạn theo trục Y và Z
            transform.Translate(move, Space.World);

            yield return null;
        }


        ResetBullet();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Run");
        if (other.gameObject.CompareTag("Target"))
        {
            Debug.Log("Collided");
            Destroy(other.gameObject);
            ResetBullet();
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log("Run");
    //    if (collision.gameObject.CompareTag("Target"))
    //    {
    //        Debug.Log("Collided");
    //        Destroy(collision.gameObject);
    //        ResetBullet();
    //    }
    //}

    void ResetBullet()
    {
        Debug.Log("Reset");
        //transform.position = resetPosition;
        //transform.rotation = resetRotation;
        transform.position = gunBarrel.position;
        transform.rotation = gunBarrel.rotation;
        bulletActive = false;
        canShoot = false;
        StartCoroutine(DelayNextShot());
    }


    IEnumerator DelayNextShot()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootDelay);
        canShoot = true;
    }
}
