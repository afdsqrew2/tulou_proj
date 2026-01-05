using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnTranslate : MonoBehaviour
{
    public GameObject Txt_Chinese;
    public GameObject Txt_English;
    public GameObject Title_Chinese;
    public GameObject Title_English;
    private bool showingEnglish = false; // 当前是否显示英文

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        // 每次弹窗激活时重置为中文
        Txt_Chinese.SetActive(true);
        Title_Chinese.SetActive(true);
        Txt_English.SetActive(false);
        Title_English.SetActive(false);
    }

    // 绑定在按钮上的点击方法
    public void ToggleLanguage()
    {
        showingEnglish = !showingEnglish;
        Txt_Chinese.SetActive(!showingEnglish);
        Txt_English.SetActive(showingEnglish);
        Title_Chinese.SetActive(!showingEnglish);
        Title_English.SetActive(showingEnglish);
    }
}
