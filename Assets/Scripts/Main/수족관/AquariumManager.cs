using UnityEngine;
using UnityEngine.UI;

public class AquariumManager : MonoBehaviour
{
    public static AquariumManager Instance;

    [Header("아쿠아리움 슬롯 (0~2)")]
    public Image[] aquariumSlots;   // 배치되는 3개 슬롯 (Image)

    [Header("수족관 슬롯 배경 (물고기 미배치 상태일 때)")]
    public Sprite emptySlotSprite;

    [Header("수족관 전용 물고기 이미지 (ID 순서대로)")]
    public Sprite[] aquariumFishSprites; // 예: 8개 (ID별 크고 예쁜 이미지)
    [Header("수족관 내부에 배치할 물고기 이미지 (3개)")]
    public Image[] tankFishImages;   // TankFish_0 ~ TankFish_2

    [Header("물고기 선택 팝업")]
    public GameObject fishSelectPopup;
    public Transform contentParent;
    public GameObject fishSelectButtonPrefab;

    private int currentSelectSlot = -1;   // 몇 번 슬롯을 클릭했는지 저장
    private int[] savedFishIDs = new int[3]; // 슬롯 3개 저장용

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        LoadAquariumData();
        ApplySavedSlots();
    }

    // 슬롯 클릭 → 선택 팝업 오픈
    public void OnClickSlot(int slotIndex)
    {
        currentSelectSlot = slotIndex;
        OpenFishSelectPopup();
    }

    // ======== 팝업 열기 ========
    private void OpenFishSelectPopup()
    {
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        // 프로필에서 사용하는 물고기 개수 기준으로 돌기
        int fishCount = ProfileUIManager.Instance.fishIcons.Length;

        for (int i = 0; i < fishCount; i++)
        {
            int unlocked = PlayerPrefs.GetInt("fish_" + i, 0);
            if (unlocked == 0) continue;

            GameObject obj = Instantiate(fishSelectButtonPrefab, contentParent);
            Image img = obj.GetComponent<Image>();

            // 수족관용 스프라이트가 있으면 그걸 쓰고,
            // 없으면 프로필 아이콘 스프라이트를 대신 사용
            Sprite s = null;

            if (i < aquariumFishSprites.Length && aquariumFishSprites[i] != null)
                s = aquariumFishSprites[i];
            else
                s = ProfileUIManager.Instance.fishIcons[i].sprite;

            img.sprite = s;

            int id = i;
            obj.GetComponent<Button>().onClick.AddListener(() => SelectFish(id));
        }

        fishSelectPopup.SetActive(true);
    }


    // ======== 물고기 선택 → 슬롯에 배치 ========
    private void SelectFish(int fishID)
    {
        savedFishIDs[currentSelectSlot] = fishID;

        Sprite s = aquariumFishSprites[fishID];

        // 슬롯에 배치되는 이미지
        aquariumSlots[currentSelectSlot].sprite = s;

        // 수족관 내부에 배치되는 이미지
        tankFishImages[currentSelectSlot].sprite = s;
        tankFishImages[currentSelectSlot].color = Color.white; // 투명 → 나타내기

        SaveAquariumData();
        fishSelectPopup.SetActive(false);
    }


    public void ClosePopup()
    {
        fishSelectPopup.SetActive(false);
    }

    // ======== 저장 ========
    private void SaveAquariumData()
    {
        for (int i = 0; i < 3; i++)
        {
            PlayerPrefs.SetInt("aquarium_slot_" + i, savedFishIDs[i]);
        }
        PlayerPrefs.Save();
    }

    // ======== 불러오기 ========
    private void LoadAquariumData()
    {
        for (int i = 0; i < 3; i++)
        {
            savedFishIDs[i] = PlayerPrefs.GetInt("aquarium_slot_" + i, -1);
        }
    }

    // 불러온 데이터로 UI 적용
    private void ApplySavedSlots()
    {
        for (int i = 0; i < 3; i++)
        {
            int fishID = savedFishIDs[i];

            if (fishID == -1)
            {
                // 슬롯은 빈 배경
                aquariumSlots[i].sprite = emptySlotSprite;

                // 수족관 내부 물고기도 제거
                tankFishImages[i].sprite = null;
                tankFishImages[i].color = new Color(1, 1, 1, 0); // 투명
            }
            else
            {
                Sprite s = aquariumFishSprites[fishID];

                // 슬롯 이미지
                aquariumSlots[i].sprite = s;

                // 수족관 내부 이미지
                tankFishImages[i].sprite = s;
                tankFishImages[i].color = Color.white;
            }
        }
    }


}
