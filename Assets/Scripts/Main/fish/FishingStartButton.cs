using UnityEngine;

public class FishingStartButton : MonoBehaviour
{
    public void OnClickStartFishing()
    {
        if (SellManager.Instance != null && SellManager.Instance.IsFull())
        {
            GlobalMessageUI.Instance.ShowMessage("판매 슬롯이 가득 찼습니다!");
            return;
        }
        // 에너지 매니저가 없으면 그냥 리턴
        if (EnergyManager.Instance == null)
        {
            Debug.LogWarning("EnergyManager.Instance 가 없습니다!");
            return;
        }

        // 에너지 1 소모 (부족하면 false 리턴 + 자동으로 '에너지가 부족합니다!' 메시지 뜸)
        if (!EnergyManager.Instance.UseEnergy(1))
        {
            // 에너지 부족하면 게임 시작 안 함
            return;
        }

        // 에너지 충분하면 낚시 미니게임 시작
        if (FishingGameManager.Instance != null)
        {
            FishingGameManager.Instance.StartFishingGame();
        }
        else
        {
            Debug.LogWarning("FishingGameManager.Instance 가 없습니다!");
        }
    }
}
