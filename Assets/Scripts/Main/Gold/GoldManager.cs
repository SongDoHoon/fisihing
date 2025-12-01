using UnityEngine;
using TMPro;

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance;

    [Header("골드 UI")]
    public TextMeshProUGUI goldText;

    [Header("초기 골드 값")]
    public int startGold = 0;

    private int currentGold = 0;
    private const string GoldKey = "Gold";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // 필요하면 다음 줄 풀어서 씬 넘어가도 유지 가능
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        LoadGold();
        UpdateGoldUI();
    }

    // ==========================
    //   골드 로드 / 저장
    // ==========================
    void LoadGold()
    {
        currentGold = PlayerPrefs.GetInt(GoldKey, startGold);
    }

    void SaveGold()
    {
        PlayerPrefs.SetInt(GoldKey, currentGold);
        PlayerPrefs.Save();
    }

    // ==========================
    //   골드 조작 함수
    // ==========================
    public void AddGold(int amount)
    {
        if (amount < 0) return;

        currentGold += amount;
        UpdateGoldUI();
        SaveGold();
    }

    public bool SpendGold(int amount)
    {
        if (amount < 0) return false;
        if (currentGold < amount) return false;

        currentGold -= amount;
        UpdateGoldUI();
        SaveGold();
        return true;
    }

    public int GetGold()
    {
        return currentGold;
    }

    // ==========================
    //   UI 업데이트
    // ==========================
    public void UpdateGoldUI()
    {
        if (goldText != null)
        {
            goldText.text = $"{currentGold}";
        }
    }
}
