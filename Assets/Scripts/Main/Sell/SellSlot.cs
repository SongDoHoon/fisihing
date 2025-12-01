using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SellSlot : MonoBehaviour
{
    [Header("UI")]
    public Image backgroundImage;      // 새로 추가된 Background
    public Image itemImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI sizeText;
    public TextMeshProUGUI priceText;

    [HideInInspector] public int price;
    [HideInInspector] public int fishID = -1;   // ?? 이 슬롯에 들어간 물고기의 ID (없으면 -1)

    // 슬롯 세팅
    public void Setup(int fishID, Sprite img, string name, string size, int price)
    {
        this.fishID = fishID;

        itemImage.sprite = img;
        nameText.text = name;
        sizeText.text = size;
        priceText.text = $"{price} G";

        this.price = price;
    }

    // 판매 버튼 클릭 시
    public void OnClickSell()
    {
        SellManager.Instance.SellItem(this);
        GlobalMessageUI.Instance.ShowMessage("판매가 완료되었습니다!");
    }
}
