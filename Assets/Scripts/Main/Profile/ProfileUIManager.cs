using UnityEngine;
using UnityEngine.UI;

public class ProfileUIManager : MonoBehaviour
{
    public static ProfileUIManager Instance;

    [Header("좌측 상단 프로필 아이콘")]
    public Image currentProfileIcon;       // TopLeftProfile 안의 ProfileIcon

    [Header("프로필 선택 팝업")]
    public GameObject profilePopup;        // ProfilePopup 오브젝트 전체
    public Image previewImage;             // 팝업 안의 미리보기 이미지

    [Header("물고기 아이콘 슬롯들")]
    public Image[] fishIcons;              // 8개 아이콘 이미지(FishIcon_0~7)

    private Sprite selectedSprite;         // 현재 팝업에서 선택된 스프라이트

    void Awake()
    {
        // 싱글톤 셋업
        if (Instance == null)
        {
            Instance = this;
            // 필요하면 다음 줄 주석 해제해서 씬 넘어가도 유지할 수 있음
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // 게임 시작할 때 저장된 해금 정보 불러오고 UI 반영
        LoadUnlockedFish();
    }

    // ===== 팝업 열기 / 선택 / 확정 =====

    // 좌측 상단 프로필 버튼 눌렀을 때 호출
    public void OpenPopup()
    {
        profilePopup.SetActive(true);

        // 기본 선택 상태 = 현재 프로필 이미지
        selectedSprite = currentProfileIcon.sprite;
        previewImage.sprite = currentProfileIcon.sprite;
    }

    // 아이콘 버튼에서 호출: 선택만 담당
    public void SelectIcon(Sprite iconSprite)
    {
        if (iconSprite == null) return;

        selectedSprite = iconSprite;
        previewImage.sprite = iconSprite;
    }

    // 확정 버튼: 좌측 상단 아이콘만 변경 (팝업은 닫지 않음)
    public void ConfirmSelection()
    {
        if (selectedSprite == null) return;

        currentProfileIcon.sprite = selectedSprite;
        // profilePopup.SetActive(false);  // 너는 닫기 원치 않는다 했으니 주석
    }

    // ===== 물고기 해금 관련 =====

    // 낚시하기에서 fishID 뽑았을 때 호출하면 됨
    public void UnlockFish(int fishID)
    {
        if (fishID < 0 || fishID >= fishIcons.Length)
        {
            Debug.LogWarning("잘못된 fishID: " + fishID);
            return;
        }

        // 아이콘 바로 열기
        fishIcons[fishID].enabled = true;

        // 영구 저장
        PlayerPrefs.SetInt("fish_" + fishID, 1);
        PlayerPrefs.Save();
    }

    // 저장된 해금 정보 불러오기
    private void LoadUnlockedFish()
    {
        for (int i = 0; i < fishIcons.Length; i++)
        {
            int unlocked = PlayerPrefs.GetInt("fish_" + i, 0);

            if (unlocked == 1)
            {
                fishIcons[i].enabled = true;   // 이미 잡은 물고기면 아이콘 보이게
            }
            else
            {
                fishIcons[i].enabled = false;  // 안 잡은 건 숨김
            }
        }
    }
}
