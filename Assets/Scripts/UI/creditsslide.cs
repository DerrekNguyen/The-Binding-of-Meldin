using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CreditScroller : MonoBehaviour
{
    public RectTransform[] creditPanels; // Assign UI panels or Text objects
    public float slideDuration = 1f;
    public float displayTime = 2f;
    public float offscreenOffset = 1000f; // How far offscreen it starts/ends

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

            // Slide in
            yield return StartCoroutine(Slide(panel, -offscreenOffset, 0));

            // Stay visible
            yield return new WaitForSeconds(displayTime);

            // Slide out
            yield return StartCoroutine(Slide(panel, 0, offscreenOffset));

            // Reset position for reuse
            panel.anchoredPosition = new Vector2(-offscreenOffset, panel.anchoredPosition.y);

            // Move to next panel
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
