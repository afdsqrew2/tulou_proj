using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickOneAny : MonoBehaviour
{
    [Header("需要显示/隐藏的UI组")]
    public GameObject Group;

    [Header("UI相机(用于射线检测)")]
    public Camera UICamera;

    private bool isShowing;
    private GraphicRaycaster groupRaycaster;
    private EventSystem eventSystem;

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
        // Group.GetComponent<UIFadeController>().FadeOut();

        groupRaycaster = Group.GetComponentInParent<GraphicRaycaster>();
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

    private bool justActivatedUI;

    void OnMouseDown()
    {
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
            if (Group.transform.childCount == 0)
            {
                Debug.LogWarning("警告: UI 组" + Group.name + "不包含任何子物体，可能导致显示为空");
            }
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
            Debug.Log("检测到鼠标点击，隐藏 UI 组");

            Group.SetActive(false);
            // Group.GetComponent<UIFadeController>().FadeOut();
            isShowing = false;
            Debug.Log("点击后，已隐藏 UI 组（不论是否点击 UI）");
        }
    }

    // 此方法保留，可用于将来恢复“只在 UI 外点击才隐藏”的逻辑
    private bool IsPointerOverUI()
    {
        if (eventSystem == null || Group == null) return false;

        PointerEventData eventData = new PointerEventData(eventSystem);
        eventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.transform.IsChildOf(Group.transform))
            {
                return true;
            }
        }
        return false;
    }
}
