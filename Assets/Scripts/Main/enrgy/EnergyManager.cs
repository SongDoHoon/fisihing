using UnityEngine;
using TMPro;
using System.Collections;

public class EnergyManager : MonoBehaviour
{
    public static EnergyManager Instance;

    [Header("에너지 기본값")]
    public int maxEnergy = 25;
    public int currentEnergy = 25;

    [Header("UI")]
    public TextMeshProUGUI energyText;

    [Header("자동 회복 설정")]
    public float recoverInterval = 180f;   // 3분 = 180초
    private Coroutine recoverRoutine;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        // 시작 시 최대 에너지 설정
        currentEnergy = maxEnergy;
        UpdateEnergyUI();

        // 자동 회복 시작
        recoverRoutine = StartCoroutine(AutoRecoverEnergy());
    }

    // ===== 에너지 소모 =====
    public bool UseEnergy(int amount)
    {
        if (currentEnergy < amount)
        {
            GlobalMessageUI.Instance.ShowMessage("에너지가 부족합니다!");
            return false;
        }

        currentEnergy -= amount;
        UpdateEnergyUI();
        return true;
    }

    // ===== UI 업데이트 =====
    public void UpdateEnergyUI()
    {
        energyText.text = $"{currentEnergy}/{maxEnergy}";
    }

    // ===== 자동 회복 =====
    IEnumerator AutoRecoverEnergy()
    {
        while (true)
        {
            yield return new WaitForSeconds(recoverInterval);

            // 최대치에 도달했으면 회복 X
            if (currentEnergy >= maxEnergy)
                continue;

            currentEnergy += 1;
            UpdateEnergyUI();
        }
    }
}
