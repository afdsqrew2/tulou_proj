using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageChange : MonoBehaviour
{
    public string folderName;      // Resources 文件夹下的子目录名
    public Button leftButton;                 // 左按钮引用
    public Button rightButton;                // 右按钮引用

    private List<Sprite> sprites = new List<Sprite>();
    private int currentIndex = 0;
    private Image displayImage;

    void Awake()
    {
        displayImage = GetComponent<Image>();
        Sprite[] loadedSprites = Resources.LoadAll<Sprite>(folderName);
        if (loadedSprites.Length == 0)
        {
            Debug.LogError("在 Resources/" + folderName + " 中未找到任何图片！");
            return;
        }
        sprites.AddRange(loadedSprites);

        // 初始化按钮点击
        leftButton.onClick.AddListener(OnLeftClick);
        rightButton.onClick.AddListener(OnRightClick);
    }

    void OnEnable()
    {
        // 每次弹窗激活时重置为第一张图
        currentIndex = 0;
        UpdateImage();
    }

    void OnLeftClick()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdateImage();
        }
    }

    void OnRightClick()
    {
        if (currentIndex < sprites.Count - 1)
        {
            currentIndex++;
            UpdateImage();
        }
    }

    void UpdateImage()
    {
        if (sprites.Count > 0)
        {
            displayImage.sprite = sprites[currentIndex];
        }

        leftButton.gameObject.SetActive(currentIndex > 0);
        rightButton.gameObject.SetActive(currentIndex < sprites.Count - 1);
    }
}
