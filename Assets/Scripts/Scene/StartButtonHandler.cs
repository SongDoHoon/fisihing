using UnityEngine;

public class StartButtonHandler : MonoBehaviour
{
    public void OnClickStart()
    {
        SceneLoader.LoadScene("MainScene");
    }
}
