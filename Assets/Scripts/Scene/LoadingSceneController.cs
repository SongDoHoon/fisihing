using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneController : MonoBehaviour
{
    public Image fadePanel; // 검은색 Image (Canvas 위)
    public float fadeDuration = 1f; // 페이드 인/아웃 시간
    public float minDisplayTime = 2f; // 최소 로딩 유지 시간

    private void Start()
    {
        StartCoroutine(LoadingRoutine());
    }

    IEnumerator LoadingRoutine()
    {
        yield return StartCoroutine(FadeIn());

        // 병렬로 로딩 & 시간 카운트
        float elapsed = 0f;
        AsyncOperation op = SceneManager.LoadSceneAsync(SceneLoader.NextSceneName);
        op.allowSceneActivation = false;

        while (op.progress < 0.9f || elapsed < minDisplayTime)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        yield return StartCoroutine(FadeOut());
        op.allowSceneActivation = true;
    }

    IEnumerator FadeIn()
    {
        float time = 0f;
        Color color = fadePanel.color;
        color.a = 1f;
        fadePanel.color = color;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, time / fadeDuration);
            fadePanel.color = color;
            yield return null;
        }

        color.a = 0f;
        fadePanel.color = color;
    }

    IEnumerator FadeOut()
    {
        float time = 0f;
        Color color = fadePanel.color;
        color.a = 0f;
        fadePanel.color = color;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, time / fadeDuration);
            fadePanel.color = color;
            yield return null;
        }

        color.a = 1f;
        fadePanel.color = color;
    }
}
