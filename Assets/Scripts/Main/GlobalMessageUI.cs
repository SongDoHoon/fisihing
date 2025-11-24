using System.Collections;
using UnityEngine;
using TMPro;

public class GlobalMessageUI : MonoBehaviour
{
    public static GlobalMessageUI Instance;

    public GameObject panel;
    public TextMeshProUGUI messageText;
    public float showTime = 2f;

    void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    public void ShowMessage(string message)
    {
        StopAllCoroutines();
        StartCoroutine(ShowMessageRoutine(message));
    }

    IEnumerator ShowMessageRoutine(string message)
    {
        messageText.text = message;
        panel.SetActive(true);

        yield return new WaitForSeconds(showTime);

        panel.SetActive(false);
    }
}
