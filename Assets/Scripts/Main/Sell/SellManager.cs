using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class SellManager : MonoBehaviour
{
    public static SellManager Instance;

    [Header("판매 탭 UI")]
    public GameObject sellPanel;
    public TextMeshProUGUI countText;     // 예: "2/8"

    [Header("Slot")]
    public Transform contentParent;       // ScrollView > Viewport > Content
    public GameObject sellSlotPrefab;

    [Header("설정")]
    public int maxSlot = 8;

    [Header("판매 전용 물고기 이미지 (fishID 순서대로)")]
    public Sprite[] fishSellSprites;      // ?? fishID 0~7 용 판매용 스프라이트

    private List<SellSlot> slotList = new List<SellSlot>();

    void Awake()
    {
        Instance = this;
    }

    // =================================
    //   슬롯 추가 (쓰레기 전용 - fishID 없음)
    // =================================
    public bool AddItem(Sprite img, string name, string size, string grade)
    {
        if (slotList.Count >= maxSlot)
        {
            GlobalMessageUI.Instance.ShowMessage("판매 슬롯이 가득 찼습니다!");
            return false;
        }

        GameObject obj = Instantiate(sellSlotPrefab, contentParent);
        SellSlot slot = obj.GetComponent<SellSlot>();

        int price = GetPriceByGrade(grade);

        // ?? 쓰레기는 fishID가 없으므로 -1, 원본 스프라이트 그대로 사용
        slot.Setup(-1, img, name, size, price);

        slotList.Add(slot);
        UpdateUI();

        return true;
    }

    // =================================
    //   슬롯 추가 (물고기 전용 - fishID 사용)
    // =================================
    public bool AddFishItem(int fishID, Sprite originalSprite, string name, string size, string grade)
    {
        if (slotList.Count >= maxSlot)
        {
            GlobalMessageUI.Instance.ShowMessage("판매 슬롯이 가득 찼습니다!");
            return false;
        }

        GameObject obj = Instantiate(sellSlotPrefab, contentParent);
        SellSlot slot = obj.GetComponent<SellSlot>();

        // ?? fishID에 맞는 판매용 이미지 찾기
        Sprite finalSprite = GetSellSpriteByFishID(fishID);

        // 만약 배열이 비어있거나 범위를 벗어났다면, 원래 물고기 스프라이트라도 사용
        if (finalSprite == null)
        {
            finalSprite = originalSprite;
        }

        int price = GetPriceByGrade(grade);
        slot.Setup(fishID, finalSprite, name, size, price);

        slotList.Add(slot);
        UpdateUI();

        return true;
    }

    // =================================
    //   fishID → 판매용 스프라이트
    // =================================
    private Sprite GetSellSpriteByFishID(int fishID)
    {
        if (fishSellSprites == null) return null;
        if (fishID < 0 || fishID >= fishSellSprites.Length) return null;

        return fishSellSprites[fishID];
    }

    // =================================
    //   등급 → 가격 변환
    // =================================
    private int GetPriceByGrade(string grade)
    {
        switch (grade)
        {
            case "Normal": return 500;
            case "Rare": return 1000;
            case "Legendary": return 1500;
        }
        // 혹시 잘못 들어온 경우 기본 500
        return 500;
    }

    // =================================
    //   아이템 판매
    // =================================
    public void SellItem(SellSlot slot)
    {
        // 골드 추가는 GoldManager에게 요청
        if (GoldManager.Instance != null)
        {
            GoldManager.Instance.AddGold(slot.price);
        }

        // 슬롯 제거
        Destroy(slot.gameObject);
        slotList.Remove(slot);

        UpdateUI();
    }

    // =================================
    //   UI 업데이트
    // =================================
    private void UpdateUI()
    {
        if (countText != null)
        {
            countText.text = $"{slotList.Count}/{maxSlot}";
        }
    }

    // =================================
    //   꽉 찼는지 체크 (게임 시작 제한)
    // =================================
    public bool IsFull()
    {
        return slotList.Count >= maxSlot;
    }
}
