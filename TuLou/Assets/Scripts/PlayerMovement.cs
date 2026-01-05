using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // 移动速度
    public float jumpForce = 5f; // 跳跃力量
    public float mouseSensitivity = 100f; // 鼠标灵敏度

    public Transform playerBody;
    public Camera playerCamera;

    private Rigidbody rb;
    private bool isJumping = false;
    private float xRotation = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        // 锁定鼠标
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        // 获取键盘输入
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // 计算移动方向
        Vector3 movementDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        // 根据输入移动角色
        transform.Translate(movementDirection * moveSpeed * Time.deltaTime);

        // 处理跳跃
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = true;
        }

        // 处理视野旋转
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 当角色碰撞到地面时
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }
}