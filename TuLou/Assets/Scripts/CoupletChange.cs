using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
//using UnityEditor.Experimental.GraphView;

public class CoupletChange : MonoBehaviour, IPointerClickHandler
{
    public SpriteRenderer spriteRenderer; // 用于显示Sprite的SpriteRenderer
    public Sprite[] sprites; // 存储要切换的Sprite数组
    private int currentSpriteIndex = 0; // 当前显示的Sprite索引

    public GameObject duan;
    public GameObject chang;
    private bool islong;
    string spriteName ;
    void Start()
    {
        // 初始化时显示第一个Sprite
        if (sprites.Length > 0)
        {
            spriteRenderer.sprite = sprites[currentSpriteIndex];
        }
        duan.SetActive(false);
        islong = true;
        spriteName = spriteRenderer.sprite.name;

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // 切换到下一个Sprite
        Debug.Log("点到了对联图片");
        currentSpriteIndex++;
        if (currentSpriteIndex >= sprites.Length)
        {
            currentSpriteIndex = 0; // 如果超出范围，则返回到第一个Sprite
        }
        spriteRenderer.sprite = sprites[currentSpriteIndex]; // 更新Sprite
        spriteName = spriteRenderer.sprite.name;
        if (spriteName == "对联_2"|| spriteName == "对联_3")
        {
            duan.SetActive(true);
            chang.SetActive(false);
            islong = false;
            Debug.Log("islong=" + islong);
        }
       else if (spriteName == "对联_1" || spriteName == "对联_0")
        {
            duan.SetActive(false);
            chang.SetActive(true);
            islong = true;
            Debug.Log("islong=" + islong);
        }
        else if (spriteName == "对联_4" )
        {
            duan.SetActive(false);
            chang.SetActive(true);
            islong = true;
            Debug.Log("islong=" + islong);
        }
        else if (spriteName == "对联_5")
        {
            duan.SetActive(true);
            chang.SetActive(false);
            islong = false;
            Debug.Log("islong=" + islong);
        }
    }
}