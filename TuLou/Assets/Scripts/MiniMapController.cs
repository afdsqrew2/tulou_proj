using UnityEngine;
using UnityEngine.UI;

public class MiniMapController : MonoBehaviour
{
    [Header("核心对象")]
    public Transform player;

    [Header("土楼根节点")]
    public Transform tulouRoot;
    private Vector3 tulouCenter;

    [Header("地图摄像机（唯一，全景俯视）")]
    public Camera miniMapCameraFull;

    [Header("相机参数")]
    public float localMapSize = 30f;     // 小地图正交范围
    public float fullMapSize = 80f;      // 大地图正交范围
    public float localMapHeight = 30f;   // 小地图相机高度
    public float fullMapHeight = 100f;   // 大地图相机高度

    private bool isFullMap = false;

    [Header("UI 组件")]
    public RawImage miniMapLocal;
    public RawImage miniMapFull;
    public RectTransform miniMapLocalArea;
    public RectTransform miniMapFullArea;
    public Button toggleMapButton;
    public Button closeFullMapButton;

    [Header("楼层文字")]
    public Text floorTextSmall;
    public Text floorTextLarge;

    [Header("玩家图标（唯一）")]
    public RectTransform playerIcon;

    [Header("按钮音效")]
    public AudioClip buttonClickClip; // AudioClip 版本

    [Header("楼层定义")]
    public float[] floorHeights = new float[] { 0f, 5.84f, 8.85f, 11.9f };

    void Start()
    {
        // 计算土楼中心
        tulouCenter = Vector3.zero;
        if (tulouRoot != null)
        {
            Renderer[] renderers = tulouRoot.GetComponentsInChildren<Renderer>();
            if (renderers.Length > 0)
            {
                Bounds bounds = renderers[0].bounds;
                for (int i = 1; i < renderers.Length; i++)
                    bounds.Encapsulate(renderers[i].bounds);

                tulouCenter = bounds.center;
            }
        }

        SetMapState(false); // 默认显示小地图
    }

    void Update()
    {
        UpdateFloorText();
        UpdatePlayerIcon();
    }

    /// <summary>
    /// 切换小地图/大地图模式
    /// </summary>
    void SetMapState(bool showFullMap)
    {
        isFullMap = showFullMap;

        // 相机设置：固定在土楼中心
        if (miniMapCameraFull != null)
        {
            miniMapCameraFull.gameObject.SetActive(true);

            Vector3 camPos = tulouCenter;
            camPos.y = isFullMap ? fullMapHeight : localMapHeight;

            miniMapCameraFull.transform.position = camPos;
            miniMapCameraFull.orthographicSize = isFullMap ? fullMapSize : localMapSize;
            miniMapCameraFull.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        }

        // UI 切换
        miniMapLocal.gameObject.SetActive(!isFullMap);
        miniMapFull.gameObject.SetActive(isFullMap);
        floorTextSmall.gameObject.SetActive(!isFullMap);
        floorTextLarge.gameObject.SetActive(isFullMap);
        toggleMapButton.gameObject.SetActive(!isFullMap);
        closeFullMapButton.gameObject.SetActive(isFullMap);

        // 玩家图标缩放与父物体切换
        if (playerIcon != null)
        {
            if (isFullMap && miniMapFullArea != null)
                playerIcon.SetParent(miniMapFullArea, false);
            else if (!isFullMap && miniMapLocalArea != null)
                playerIcon.SetParent(miniMapLocalArea, false);

            // 缩放设置：小地图0.8，大地图3
            playerIcon.localScale = isFullMap ? Vector3.one * 2f : Vector3.one * 0.8f;

            // 锚点和位置重置
            playerIcon.anchorMin = new Vector2(0, 0);
            playerIcon.anchorMax = new Vector2(0, 0);
            playerIcon.anchoredPosition = Vector2.zero;
        }

        UpdatePlayerIcon();
    }

    /// <summary>
    /// 更新楼层文字
    /// </summary>
    void UpdateFloorText()
    {
        if (player == null) return;

        float y = player.position.y;
        int currentFloor = 1;
        for (int i = floorHeights.Length - 1; i >= 0; i--)
        {
            if (y >= floorHeights[i])
            {
                currentFloor = i + 1;
                break;
            }
        }

        string info = $"当前楼层：{currentFloor} 层";
        if (floorTextSmall) floorTextSmall.text = info;
        if (floorTextLarge) floorTextLarge.text = info;
    }

    /// <summary>
    /// 更新玩家图标位置（基于相机视口范围）
    /// </summary>
    void UpdatePlayerIcon()
    {
        if (player == null || playerIcon == null || miniMapCameraFull == null) return;

        Vector3 pos = player.position;
        RectTransform parentArea = isFullMap ? miniMapFullArea : miniMapLocalArea;
        if (parentArea == null) return;

        // 当前相机视口矩形
        float camSize = miniMapCameraFull.orthographicSize;
        float camAspect = miniMapCameraFull.aspect;
        Vector3 camPos = miniMapCameraFull.transform.position;

        float xMin = camPos.x - camSize * camAspect;
        float xMax = camPos.x + camSize * camAspect;
        float zMin = camPos.z - camSize;
        float zMax = camPos.z + camSize;

        // 归一化
        float normX = Mathf.InverseLerp(xMin, xMax, pos.x);
        float normY = Mathf.InverseLerp(zMin, zMax, pos.z);

        // 映射到 UI 区域
        float width = parentArea.rect.width;
        float height = parentArea.rect.height;
        Vector2 anchoredPos = new Vector2(normX * width, normY * height);

        playerIcon.anchoredPosition = anchoredPos;
    }

    // UI 按钮调用
    public void ShowFullMap()
    {
        if (buttonClickClip) AudioSource.PlayClipAtPoint(buttonClickClip, Camera.main.transform.position);
        SetMapState(true);
    }

    public void ShowMiniMap()
    {
        if (buttonClickClip) AudioSource.PlayClipAtPoint(buttonClickClip, Camera.main.transform.position);
        SetMapState(false);
    }
}
