using UnityEngine;
using UnityEngine.UI;

public class ProfileIconButton : MonoBehaviour
{
    public Image fishIcon;     // 이 버튼 자식의 FishIcon 이미지 (스프라이트 + enabled)

    public void OnClickButton()
    {
        // 아직 낚시로 안 뽑아서 안 열린 아이콘이면 무시
        if (fishIcon == null || fishIcon.enabled == false)
        {
            Debug.Log("아직 해금되지 않은 아이콘!");
            return;
        }

        // 열린 아이콘이면 선택
        ProfileUIManager.Instance.SelectIcon(fishIcon.sprite);
    }
}
