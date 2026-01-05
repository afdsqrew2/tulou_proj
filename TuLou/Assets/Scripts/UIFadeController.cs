using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class UIFadeController : MonoBehaviour
{
    public float fadeDuration = 0.3f;

    private CanvasGroup canvasGroup;
    private Coroutine currentRoutine;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void FadeIn()
    {
        gameObject.SetActive(true);
        if (currentRoutine != null) StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(FadeTo(1, true));
    }

    public void FadeOut(bool disableAfter = true)
    {
        if (currentRoutine != null) StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(FadeTo(0, disableAfter));
    }

    private IEnumerator FadeTo(float targetAlpha, bool disableAfter)
    {
        float startAlpha = canvasGroup.alpha;
        float t = 0;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;

        canvasGroup.interactable = targetAlpha > 0.9f;
        canvasGroup.blocksRaycasts = targetAlpha > 0.9f;

        if (targetAlpha == 0 && disableAfter)
            gameObject.SetActive(false);
    }
}
