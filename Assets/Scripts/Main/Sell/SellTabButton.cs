using UnityEngine;

public class SellTabButton : MonoBehaviour
{
    [Header("열고 싶은 판매 패널")]
    public GameObject sellPanel;

    public void OpenSellPanel()
    {
        if (sellPanel != null)
        {
            sellPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("SellPanel 이 연결되어 있지 않습니다!");
        }
    }
}
