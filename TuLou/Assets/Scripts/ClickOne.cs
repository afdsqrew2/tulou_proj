using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickOne : MonoBehaviour
{
    [Header("需要显示/隐藏的UI组")]
    public GameObject Group;

    [Header("UI相机(用于射线检测)")]
    public Camera UICamera;

    private bool isShowing;
    private EventSystem eventSystem;

    private bool justActivatedUI;

    

    void Start()
    {
      
      
        isShowing = false;

        if (Group == null)
        {
            Debug.LogError("错误: Group对象未赋值! 请在Inspector中指定要显示的UI组");
            this.enabled = false;
            return;
        }

        Group.SetActive(false);

        eventSystem = FindObjectOfType<EventSystem>();
        if (eventSystem == null)
        {
            Debug.LogWarning("警告: 场景中未找到EventSystem，UI点击检测可能失效");
        }

        if (UICamera == null)
        {
            Canvas parentCanvas = Group.GetComponentInParent<Canvas>();
            if (parentCanvas != null && parentCanvas.renderMode != RenderMode.ScreenSpaceOverlay)
            {
                UICamera = parentCanvas.worldCamera;
            }
            else
            {
                UICamera = Camera.main;
            }
        }

        Debug.Log("ObjectClick脚本初始化完成，目标UI组: " + Group.name);
    }

    void OnMouseDown()
    {
        this.GetComponent<AudioSource>().Play();
        if (Group == null)
        {
            Debug.LogError("Group 对象未赋值，无法切换显示状态");
            return;
        }

        Debug.Log("物体被点击，当前 isShowing 状态: " + isShowing);

        isShowing = !isShowing;
        Group.SetActive(isShowing);
        justActivatedUI = isShowing;

        if (isShowing)
        {
            Debug.Log("已激活 UI 组: " + Group.name);
        }
        else
        {
            Debug.Log("已隐藏 UI 组: " + Group.name);
        }
    }

    void Update()
    {
        if (!isShowing || Group == null || eventSystem == null)
        {
            justActivatedUI = false;
            return;
        }

        if (justActivatedUI)
        {
            justActivatedUI = false;
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            bool clickedOnUI = IsPointerOverGroupUI(Group);
            Debug.Log("检测到鼠标点击，是否点击在 Group UI 上: " + clickedOnUI);

            if (!clickedOnUI)
            {
                Group.SetActive(false);
                isShowing = false;
                Debug.Log("点击 UI 外部区域，已隐藏 UI");
            }
        }
    }

    /// <summary>
    /// 只检测是否点击在 Group 内部的 UI 元素上
    /// </summary>
    private bool IsPointerOverGroupUI(GameObject group)
    {
        if (eventSystem == null || group == null) return false;

        PointerEventData eventData = new PointerEventData(eventSystem)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.transform.IsChildOf(group.transform))
            {
                return true; // 点击在Group UI内
            }
        }

        return false;
    }
}
