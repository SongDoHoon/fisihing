using UnityEngine;

public class SettingToggle : MonoBehaviour
{
    public GameObject settingPanel;

    public void OpenSetting()
    {
        settingPanel.SetActive(true); // 설정창을 보이게 함
    }
}
