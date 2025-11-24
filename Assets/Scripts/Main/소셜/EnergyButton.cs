using UnityEngine;

public class EnergyButton : MonoBehaviour
{
    public void OnClickSendEnergy()
    {
        GlobalMessageUI.Instance.ShowMessage("에너지가 전송되었습니다!");
    }
}
