using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class AutoShrinkTextUGUI : MonoBehaviour
{
    public int defaultFontSize = 36;
    public int minFontSize = 14;

    private Text text;
    private RectTransform rectTransform;

    void Awake()
    {
        text = GetComponent<Text>();
        rectTransform = GetComponent<RectTransform>();
    }

    void Start()
    {
        AdjustFontSize();
    }

    void OnValidate()
    {
        if (Application.isPlaying && text != null)
            AdjustFontSize();
    }

    void AdjustFontSize()
    {
        text.fontSize = defaultFontSize;
        text.horizontalOverflow = HorizontalWrapMode.Overflow;

        float maxWidth = rectTransform.rect.width;

        // 如果 preferredWidth 超出最大宽度，尝试逐渐缩小字体
        while (text.preferredWidth > maxWidth && text.fontSize > minFontSize)
        {
            text.fontSize--;
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        }
    }
}
