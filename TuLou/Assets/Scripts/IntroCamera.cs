using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IntroCamera : MonoBehaviour
{
    [Header("模型模块")]
    public GameObject[] introModels;

    [Header("动画控制器")]
    public Animator introAnimator;

    [Header("摄像机")]
    public Camera mainCamera;

    [Header("UI 组件")]
    public Text line1Text;
    public Text line2Text;
    public CanvasGroup bottomTip;        // 底部提示“点击任意位置关闭”
    public CanvasGroup introCanvasGroup; // 整个 IntroCanvas（Overlay）

    [Header("交互点组（按顺序播放 0,1,2）")]
    public GameObject[] interactionGroups;

    [Header("小地图")]
    public GameObject MiniMapCanvas;

    [Header("Intro 播放完成标志")]
    public static bool introFinished = false;

    private bool hasPlayed = false;

    void Start()
    {
        MiniMapCanvas.SetActive(false);
        bottomTip.alpha = 0f;

        // 初始化文字为空
        if (line1Text) line1Text.text = "";
        if (line2Text) line2Text.text = "";

        HideAllInteractionGroups();

        if (!hasPlayed)
        {
            GetComponent<Camera>().cullingMask = LayerMask.GetMask("Intro");
            StartCoroutine(PlayIntroSequence());
            hasPlayed = true;
        }
    }

    IEnumerator PlayIntroSequence()
    {
        mainCamera.gameObject.SetActive(false);
        SetModelsActive(true);
        introFinished = false;

        if (introCanvasGroup != null)
        {
            introCanvasGroup.alpha = 1f;
            introCanvasGroup.blocksRaycasts = true;
            introCanvasGroup.interactable = true;
        }

        // 阶段 1：拆分动画 + 合并动画 + 首组文字
        introAnimator.Play("TulouAnimation", 0, 0f);
        yield return null;
        var splitInfo = introAnimator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(splitInfo.length);

        introAnimator.Play("BackTulouAnimation", 0, 0f);
        yield return null;
        var mergeInfo = introAnimator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(mergeInfo.length);

        SetTextImmediate("漫游中可以点击交互点查看讲解",
                         "During the tour, you can click on the interactive points to view the explanations");
        yield return new WaitForSeconds(1f);

        // 阶段 2：显示第一组交互点 + 文字
        ShowInteractionGroup(0);
        SetTextImmediate("建筑分析", "Building Analysis");
        yield return new WaitForSeconds(1f);

        // 阶段 3：显示第二组交互点 + 文字
        HideAllInteractionGroups();
        ShowInteractionGroup(1);
        SetTextImmediate("功能展示", "Function Display");
        yield return new WaitForSeconds(1f);

        // 阶段 4：显示第三组交互点 + 文字
        HideAllInteractionGroups();
        ShowInteractionGroup(2);
        SetTextImmediate("文化展示", "Cultural Exhibition");
        yield return new WaitForSeconds(1f);

        // 阶段 5：底部提示
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(FadeUI(bottomTip, 0f, 1f, 1f));

        // 等待点击关闭
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        // Intro结束：隐藏所有交互组
        HideAllInteractionGroups();
        if (introCanvasGroup != null)
            introCanvasGroup.gameObject.SetActive(false);

        mainCamera.gameObject.SetActive(true);
        gameObject.SetActive(false);

        introFinished = true;
        MiniMapCanvas.SetActive(true);
    }

    void ShowInteractionGroup(int index)
    {
        if (index < 0 || index >= interactionGroups.Length || interactionGroups[index] == null)
            return;

        interactionGroups[index].SetActive(true);
    }

    void HideAllInteractionGroups()
    {
        foreach (var group in interactionGroups)
        {
            if (group != null)
                group.SetActive(false);
        }
    }

    void SetModelsActive(bool active)
    {
        foreach (var model in introModels)
        {
            if (model != null)
                model.SetActive(active);
        }
    }

    void SetTextImmediate(string t1, string t2)
    {
        if (line1Text) line1Text.text = t1;
        if (line2Text) line2Text.text = t2;
    }

    IEnumerator FadeUI(CanvasGroup group, float from, float to, float duration)
    {
        float timer = 0f;
        group.alpha = from;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            group.alpha = Mathf.Lerp(from, to, timer / duration);
            yield return null;
        }
        group.alpha = to;
    }
}
