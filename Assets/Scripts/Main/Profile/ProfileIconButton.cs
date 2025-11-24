using UnityEngine;
using UnityEngine.UI;

public class ProfileIconButton : MonoBehaviour
{
    public Image fishIcon;

    public void OnClickButton()
    {
        if (fishIcon.enabled == false)
        {
            // 해당 물고기 안잡아서 비활성화된 경우
            Debug.Log("아직 해금되지 않은 아이콘!");
            return;
        }

        ProfileUIManager.Instance.SelectIcon(fishIcon.sprite);
    }
}
