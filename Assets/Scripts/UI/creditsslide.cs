using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// UI text sliding (I dont think this is used anymore)

public class CreditScroller : MonoBehaviour
{
    public RectTransform[] creditPanels;
    public float slideDuration = 1f;
    public float displayTime = 2f;
    public float offscreenOffset = 1000f;

    private int currentIndex = 0;

    void Start()
    {
        StartCoroutine(PlayCredits());
    }

    IEnumerator PlayCredits()
    {
        while (true)
        {
            RectTransform panel = creditPanels[currentIndex];

            yield return StartCoroutine(Slide(panel, -offscreenOffset, 0));

            yield return new WaitForSeconds(displayTime);

            yield return StartCoroutine(Slide(panel, 0, offscreenOffset));

            panel.anchoredPosition = new Vector2(-offscreenOffset, panel.anchoredPosition.y);

            currentIndex = (currentIndex + 1) % creditPanels.Length;
        }
    }

    IEnumerator Slide(RectTransform panel, float startX, float endX)
    {
        float elapsed = 0f;
        Vector2 pos = panel.anchoredPosition;
        pos.x = startX;
        panel.anchoredPosition = pos;

        while (elapsed < slideDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / slideDuration);
            float x = Mathf.Lerp(startX, endX, t);
            panel.anchoredPosition = new Vector2(x, pos.y);
            yield return null;
        }
    }
}
