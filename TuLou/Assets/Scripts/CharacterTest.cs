using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CharacterTest : MonoBehaviour
{

   public CharacterController controller;
    public float moveSpeed = 1f; // 移动速度
    public float mouseSensitivity = 100f; // 鼠标灵敏度
    private float xRotation = 0f;
    public Camera playerCamera;
    public Transform playerBody;
    public float jumpSpeed = 1f;//角色跳跃高度
    public float gravity = 9.8f;//重力
    private float horizontal;
    private float vertical;
    private Vector3 Player_Move;
    private float yVelocity = 0f;
    private bool isJumping = false;



    // Start is called before the first frame update

    void Start()
    {
     
    controller = GetComponent<CharacterController>();
      
        
        // 锁定鼠标
       // Cursor.lockState = CursorLockMode.Locked;
     //   Cursor.visible = false;

    }

    void Update()
    {
        //开发阶段用于同步scene和game视图视角
   //  Camera cameraMain = Camera.main;
     // var sceneView = SceneView.lastActiveSceneView;
      //  if (sceneView != null)
     // {
     //       sceneView.cameraSettings.nearClip = cameraMain.nearClipPlane;
     //       sceneView.cameraSettings.fieldOfView = cameraMain.fieldOfView;
     //     sceneView.pivot = cameraMain.transform.position +
        //       cameraMain.transform.forward * sceneView.cameraDistance;
    //       sceneView.rotation = cameraMain.transform.rotation;
     //  }

        Move();
        //Jump();
        if (Input.GetKey(KeyCode.Mouse1))//右键点击拖动旋转
        {
            Rotation();
        }
    }
    void Move()
    {

        if (controller.isGrounded)
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
            Vector3 moveDirection = (transform.forward * vertical + transform.right * horizontal).normalized;

            // 按下空格时设置初始跳跃速度
            if (Input.GetButtonDown("Jump"))
            {
                yVelocity = jumpSpeed; // 比如 5 或 6
                isJumping = true;
            }
        }

        // 应用重力
        yVelocity -= gravity * Time.deltaTime;

        // 合成移动方向（带垂直速度）
        Vector3 finalMove = (transform.forward * vertical + transform.right * horizontal).normalized * moveSpeed;
        finalMove.y = yVelocity;

        controller.Move(finalMove * Time.deltaTime);

        // 如果落地了，清空跳跃速度
        if (controller.isGrounded && yVelocity < 0)
        {
            yVelocity = -1f; // 让角色稳稳站住，防止掉下去
            isJumping = false;
        }
    

}
     void Rotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
   
    }


}
