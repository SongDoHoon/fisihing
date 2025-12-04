using UnityEngine;

public class SellTabButton : MonoBehaviour
{
    [Header("열고/닫을 판매 패널")]
    public GameObject sellPanel;

    // 판매 UI 열기
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

    // ?? 새로 추가: 판매 UI 닫기
    public void CloseSellPanel()
    {
        if (sellPanel != null)
        {
            sellPanel.SetActive(false);
            
        }
    }
}
