using UnityEngine;
using UnityEngine.UI;

public class ProfileUIManager : MonoBehaviour
{
    public static ProfileUIManager Instance;

    [Header("좌측 상단 프로필 UI")]
    public Image currentProfileIconUI; // 실제 화면에 보이는 프로필 아이콘

    [Header("프로필 선택 팝업")]
    public GameObject popup;
    public Image popupCurrentIconPreview;

    private Sprite selectedSprite; // 새로 선택된 아이콘

    void Awake()
    {
        Instance = this;
    }

    // 좌측 상단 프로필 클릭 → 팝업 열기
    public void OpenPopup()
    {
        // 미리보기 아이콘을 현재 아이콘으로 초기화
        popupCurrentIconPreview.sprite = currentProfileIconUI.sprite;

        selectedSprite = currentProfileIconUI.sprite;

        popup.SetActive(true);
    }

    // 아이콘 버튼 클릭 시 호출될 함수
    public void SelectIcon(Sprite iconSprite)
    {
        selectedSprite = iconSprite;
        popupCurrentIconPreview.sprite = iconSprite;
    }

    // 확정 버튼 클릭
    public void ConfirmSelection()
    {
        currentProfileIconUI.sprite = selectedSprite;
        //popup.SetActive(false);
    }
}
