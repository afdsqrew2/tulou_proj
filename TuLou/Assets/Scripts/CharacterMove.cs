using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    public CharacterController controller1;
    private Rigidbody rb1;
    private bool isJumping1 = false;
    public float moveSpeed1 = 5f; // 移动速度
    public float jumpForce1 = 5f; // 跳跃力量
    public float mouseSensitivity1 = 100f; // 鼠标灵敏度
    private float xRotation1 = 0f;
    public Camera playerCamera1;
    public Transform playerBody1;
    private Vector3 velocity;
    public float gravity = -9.81f; // 重力

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // 锁定鼠标
        Cursor.visible = false;
    }

    void Update()
    {
        Move();
        Jump();
        Rotation();
    }

    void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // 计算移动方向
        Vector3 dir = playerBody1.transform.right * h + playerBody1.transform.forward * v;
        dir.y = 0; // 防止影响y轴

        // 应用移动
        controller1.Move(dir * moveSpeed1 * Time.deltaTime);

        // 应用重力
        if (controller1.isGrounded)
        {
            velocity.y = -2f; // 确保角色在地面时有一个轻微的向下力
        }
        else
        {
            velocity.y += gravity * Time.deltaTime; // 应用重力
        }

        controller1.Move(velocity * Time.deltaTime);
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && controller1.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce1 * -2f * gravity); // 计算跳跃速度
        }
    }

    void Rotation()
    {
        // 获取鼠标输入
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity1 * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity1 * Time.deltaTime;

        // 处理视角旋转
        xRotation1 -= mouseY;
        xRotation1 = Mathf.Clamp(xRotation1, -90f, 90f);
        playerCamera1.transform.localRotation = Quaternion.Euler(xRotation1, 0f, 0f);

        // 处理身体旋转
        playerBody1.Rotate(Vector3.up * mouseX);
    }
}





