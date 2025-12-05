using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FishSelectSlot : MonoBehaviour
{
    [Header("UI")]
    public Image backgroundImage;
    public Image fishImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI sizeText;
    public Button selectButton;

    private int fishID;

    // 아쿠아리움 매니저가 슬롯을 생성하면서 호출하는 세팅 함수
    public void Setup(int id, Sprite sprite, string name, string size)
    {
        fishID = id;
        fishImage.sprite = sprite;
        nameText.text = name;
        sizeText.text = size;
    }

    private void Start()
    {
        selectButton.onClick.AddListener(OnClickSelect);
    }

    private void OnClickSelect()
    {
        AquariumManager.Instance.SelectFishFromPopup(fishID);
    }
}
