using UnityEngine;
using UnityEngine.UI;

public class CannonControl : MonoBehaviour
{
    public Transform cannonBarrel; // Nòng pháo di chuyển
    public float moveSpeed = 2f; // Tốc độ di chuyển

    public Button moveUpButton; // Nút di chuyển lên
    public Button moveDownButton; // Nút di chuyển xuống

    void Start()
    {
        // Đăng ký sự kiện nhấn nút
        moveUpButton.onClick.AddListener(MoveUp);
        moveDownButton.onClick.AddListener(MoveDown);
    }

    void MoveUp()
    {
        // Di chuyển nòng pháo lên theo trục Y
        Vector3 newPosition = cannonBarrel.position;
        newPosition.y += moveSpeed * Time.deltaTime;
        cannonBarrel.position = newPosition;
    }

    void MoveDown()
    {
        // Di chuyển nòng pháo xuống theo trục Y
        Vector3 newPosition = cannonBarrel.position;
        newPosition.y -= moveSpeed * Time.deltaTime;
        cannonBarrel.position = newPosition;
    }
}
