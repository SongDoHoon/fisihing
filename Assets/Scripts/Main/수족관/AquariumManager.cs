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

    [Header("팝업에서 보여줄 물고기 이미지 (ID 순서대로)")]
    public Sprite[] popupFishSprites;

    [Header("아쿠아리움 슬롯(버튼)에 보여줄 이미지 (ID 순서대로)")]
    public Sprite[] aquariumSlotSprites;

    [Header("수족관 내부 물고기 이미지 (ID 순서대로)")]
    public Sprite[] tankFishSprites;

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

        int fishCount = ProfileUIManager.Instance.fishIcons.Length;

        for (int i = 0; i < fishCount; i++)
        {
            int unlocked = PlayerPrefs.GetInt("fish_" + i, 0);
            if (unlocked == 0) continue;

            // 슬롯 생성
            GameObject obj = Instantiate(fishSelectButtonPrefab, contentParent);
            FishSelectSlot slot = obj.GetComponent<FishSelectSlot>();

            // 팝업에서 보여줄 전용 이미지
            Sprite popupSprite = popupFishSprites[i];

            // FishData(이름, 사이즈) 가져오기
            var fishData = GetFishDataByID(i);

            // 슬롯 세팅
            slot.Setup(
                i,
                popupSprite,
                fishData.fishName,
                fishData.sizeText
            );
        }

        fishSelectPopup.SetActive(true);
    }

    private FishingGameManager.FishData GetFishDataByID(int id)
    {
        var gm = FishingGameManager.Instance;

        foreach (var f in gm.normalFishList)
            if (f.fishID == id) return f;
        foreach (var f in gm.rareFishList)
            if (f.fishID == id) return f;
        foreach (var f in gm.legendaryFishList)
            if (f.fishID == id) return f;

        Debug.LogWarning($"FishData ID {id} 를 찾지 못했습니다.");
        return null;
    }


    // ======== 물고기 선택 → 슬롯에 배치 ========
    public void SelectFishFromPopup(int fishID)
    {
        savedFishIDs[currentSelectSlot] = fishID;

        // 슬롯 버튼용 전용 이미지
        Sprite slotSprite = aquariumSlotSprites[fishID];

        // Tank 내부 이미지
        Sprite tankSprite = tankFishSprites[fishID];

        // 슬롯에 표시되는 이미지
        aquariumSlots[currentSelectSlot].sprite = slotSprite;

        // Tank 내부 물고기 이미지 적용
        tankFishImages[currentSelectSlot].sprite = tankSprite;
        tankFishImages[currentSelectSlot].color = Color.white;

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

                // Tank 물고기도 제거
                tankFishImages[i].sprite = null;
                tankFishImages[i].color = new Color(1, 1, 1, 0);
            }
            else
            {
                // 슬롯 버튼용 이미지
                Sprite slotSprite = aquariumSlotSprites[fishID];

                // Tank 내부 이미지
                Sprite tankSprite = tankFishSprites[fishID];

                // 슬롯 이미지 적용
                aquariumSlots[i].sprite = slotSprite;

                // Tank 내부 물고기 적용
                tankFishImages[i].sprite = tankSprite;
                tankFishImages[i].color = Color.white;
            }
        }
    }


}
