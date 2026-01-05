using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    CharacterController controller;
    bool isMove=false;
    void Start()
    {
        controller = GetComponent<CharacterController>();

    }

    void Update()
    {
        Move();
      if(isMove == true)
        {
            Debug.Log("在动了！位置是" + transform.position);
        }
        else
        {
            Debug.Log("Shit！一动不动！" );
        }
    }

    void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(-h, 0, -v);
        controller.SimpleMove(dir);
        isMove = true;
    }
}