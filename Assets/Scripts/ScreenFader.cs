using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader Instance;
    public CanvasGroup fadeCanvasGroup; // Siyah panelin opaklığını kontrol eder
    public float fadeDuration = 1f;     // Ne kadar sürsün?

    private void Awake()
    {
        Instance = this;
    }

    // Ekranı Karart (Siyah yap)
    public IEnumerator FadeOut()
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadeCanvasGroup.alpha = timer / fadeDuration; // 0'dan 1'e git
            yield return null;
        }
        fadeCanvasGroup.alpha = 1f; // Tam siyah
    }

    // Ekranı Aç (Görüntü gelsin)
    public IEnumerator FadeIn()
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadeCanvasGroup.alpha = 1f - (timer / fadeDuration); // 1'den 0'a git
            yield return null;
        }
        fadeCanvasGroup.alpha = 0f; // Tam şeffaf
    }
}
