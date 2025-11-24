using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    // 로딩할 다음 씬 이름 저장용
    public static string NextSceneName;

    // 씬 로딩 시작
    public static void LoadScene(string sceneName)
    {
        NextSceneName = sceneName;
        SceneManager.LoadScene("LoadingScene"); // 항상 로딩씬으로 이동
    }
}