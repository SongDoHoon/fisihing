using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class FishingGameManager : MonoBehaviour
{
    public static FishingGameManager Instance;

    // ====== 로비 UI들 ======
    [Header("로비에서 끌 UI들")]
    public GameObject backgroundUI;
    public GameObject nicknameUI;
    public GameObject settingUI;
    public GameObject socialUI;
    public GameObject sellUI;
    public GameObject fishingUI;
    public GameObject aquariumUI;
    public GameObject bookUI;
    public GameObject fishingImage;
    public GameObject topLeftProfileUI;
    public GameObject energyTextUI;

    // ====== 게임 UI ======
    [Header("낚시 게임 UI")]
    public GameObject fishingGamePanel;
    public GameObject rodImage;
    public GameObject lineImage;

    // ====== 상단 바 ======
    [Header("상단 슬라이더 (물고기/플레이어)")]
    public RectTransform barArea;
    public RectTransform fishRect;
    public RectTransform playerRect;

    [Tooltip("기본 물고기 이동 속도")]
    public float fishMoveSpeed = 150f;

    [Tooltip("플레이어가 왼쪽으로 가는 속도")]
    public float playerMoveSpeed = 250f;

    [Tooltip("플레이어 시작 X")]
    public float playerStartX = 250f;

    [Tooltip("플레이어 목표 X (왼쪽 끝)")]
    public float playerEndX = -250f;

    [Tooltip("겹치는 거리 허용값")]
    public float overlapRange = 40f;

    // ====== 우측 게이지 ======
    [Header("게이지 슬라이더")]
    public Slider progressSlider;

    [Tooltip("초당 게이지 채우는 양")]
    public float fillPerSecond = 0.1f;

    // ====== 결과창 UI ======
    [Header("결과창 UI")]
    public GameObject resultPanel;          // 반드시 FishingGamePanel 밖
    public Image resultTrashImage;
    public Image resultGradeImage;
    public TextMeshProUGUI resultNameText;
    public TextMeshProUGUI resultSizeText;
    public TextMeshProUGUI resultDescText;

    [Header("결과창 배경 (등급별)")]
    public Image resultBackgroundImage;
    public Sprite normalBG;
    public Sprite rareBG;
    public Sprite legendaryBG;

    // ====== 랜덤 물고기 이동 옵션 ======
    [Header("랜덤 물고기 이동 옵션")]
    [Tooltip("방향 전환 확률 (0~1)")]
    public float directionChangeProbability = 0.3f;

    [Tooltip("물고기 속도 최소 배율")]
    public float minSpeedMultiplier = 0.5f;

    [Tooltip("물고기 속도 최대 배율")]
    public float maxSpeedMultiplier = 1.5f;

    [Tooltip("패턴 변경 최소 시간")]
    public float minPatternTime = 0.3f;

    [Tooltip("패턴 변경 최대 시간")]
    public float maxPatternTime = 1.2f;

    // 내부 랜덤 변수
    private float randomTimer = 0f;
    private float nextRandomChange = 1f;
    private float randomSpeedModifier = 1f;

    // ====== 쓰레기 데이터 ======
    [System.Serializable]
    public class TrashData
    {
        public string trashName;
        public Sprite trashSprite;
        public Sprite gradeSprite;
        public string sizeText;
        [TextArea] public string description;
    }

    [Header("쓰레기 데이터")]
    public TrashData[] normalTrashList;
    public TrashData[] rareTrashList;
    public TrashData[] legendaryTrashList;

    // ====== 물고기 데이터 ======
    [System.Serializable]
    public class FishData
    {
        public int fishID;   // TopLeftProfile 연동용 ID
        public string fishName;
        public Sprite fishSprite;
        public Sprite gradeSprite;
        public string sizeText;
        [TextArea] public string description;
    }

    [Header("물고기 데이터")]
    public FishData[] normalFishList;
    public FishData[] rareFishList;
    public FishData[] legendaryFishList;

    // 내부 상태
    private bool isPlaying = false;
    private bool isHoldButton = false;
    private float fishDirection = 1f;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        fishingGamePanel.SetActive(false);
        resultPanel.SetActive(false);
        rodImage.SetActive(false);
        lineImage.SetActive(false);
    }

    void Update()
    {
        if (!isPlaying) return;

        MoveFish();
        MovePlayer();

        if (IsOverlapping())
        {
            progressSlider.value += fillPerSecond * Time.deltaTime;

            if (progressSlider.value >= 1f)
            {
                OnFishingSuccess();
            }
        }
    }

    // =================================
    //   게임 시작
    // =================================
    public void StartFishingGame()
    {
        isHoldButton = false;  // 자동 왼쪽 이동 버그 방지

        SetLobbyUI(false);

        fishingGamePanel.SetActive(true);
        rodImage.SetActive(true);
        lineImage.SetActive(true);

        resultPanel.SetActive(false);
        progressSlider.value = 0f;

        ResetPositions();
        isPlaying = true;
    }

    private void SetLobbyUI(bool value)
    {
        backgroundUI?.SetActive(value);
        nicknameUI?.SetActive(value);
        settingUI?.SetActive(value);
        socialUI?.SetActive(value);
        sellUI?.SetActive(value);
        fishingUI?.SetActive(value);
        aquariumUI?.SetActive(value);
        bookUI?.SetActive(value);
        fishingImage?.SetActive(value);
        topLeftProfileUI?.SetActive(value);
        energyTextUI?.SetActive(value);
    }

    // =================================
    //   물고기 랜덤 이동
    // =================================
    private void MoveFish()
    {
        randomTimer += Time.deltaTime;

        if (randomTimer >= nextRandomChange)
        {
            randomTimer = 0f;

            if (Random.value < directionChangeProbability)
                fishDirection *= -1f;

            randomSpeedModifier = Random.Range(minSpeedMultiplier, maxSpeedMultiplier);
            nextRandomChange = Random.Range(minPatternTime, maxPatternTime);
        }

        Vector2 pos = fishRect.anchoredPosition;
        pos.x += fishMoveSpeed * randomSpeedModifier * fishDirection * Time.deltaTime;

        float half = barArea.rect.width / 2f;

        if (pos.x > half) { pos.x = half; fishDirection = -1f; }
        if (pos.x < -half) { pos.x = -half; fishDirection = 1f; }

        fishRect.anchoredPosition = pos;
    }

    // =================================
    //   플레이어 이동
    // =================================
    private void MovePlayer()
    {
        Vector2 pos = playerRect.anchoredPosition;

        if (isHoldButton)
        {
            pos.x = Mathf.MoveTowards(pos.x, playerEndX, playerMoveSpeed * Time.deltaTime);
        }
        else
        {
            float returnSpeed = playerMoveSpeed * 0.7f;
            pos.x = Mathf.MoveTowards(pos.x, playerStartX, returnSpeed * Time.deltaTime);
        }

        playerRect.anchoredPosition = pos;
    }

    private bool IsOverlapping()
    {
        float dist = Mathf.Abs(fishRect.anchoredPosition.x - playerRect.anchoredPosition.x);
        return dist <= overlapRange;
    }

    public void OnHoldButtonDown() => isHoldButton = true;
    public void OnHoldButtonUp() => isHoldButton = false;

    // =================================
    //   낚시 성공 시
    // =================================
    private void OnFishingSuccess()
    {
        isPlaying = false;

        fishingGamePanel.SetActive(false);
        rodImage.SetActive(false);
        lineImage.SetActive(false);

        float r = Random.value;

        if (r < 0.4f)
            ShowTrashResult();
        else
            ShowFishResult();
    }

    // =================================
    //   쓰레기 뽑기
    // =================================
    private TrashData GetRandomTrash()
    {
        float r = Random.value;

        if (r < 0.6f) return normalTrashList[Random.Range(0, normalTrashList.Length)];
        if (r < 0.95f) return rareTrashList[Random.Range(0, rareTrashList.Length)];
        return legendaryTrashList[Random.Range(0, legendaryTrashList.Length)];
    }

    private void ShowTrashResult()
    {
        var data = GetRandomTrash();

        resultPanel.SetActive(true);

        resultTrashImage.sprite = data.trashSprite;
        resultGradeImage.sprite = data.gradeSprite;
        resultNameText.text = data.trashName;
        resultSizeText.text = data.sizeText;
        resultDescText.text = data.description;

        if (normalTrashList.Contains(data)) resultBackgroundImage.sprite = normalBG;
        else if (rareTrashList.Contains(data)) resultBackgroundImage.sprite = rareBG;
        else resultBackgroundImage.sprite = legendaryBG;

        string grade = "Normal";
        if (rareTrashList.Contains(data)) grade = "Rare";
        else if (legendaryTrashList.Contains(data)) grade = "Legendary";

        SellManager.Instance.AddItem(
            data.trashSprite,
            data.trashName,
            data.sizeText,
            grade
        );
    }

    // =================================
    //   물고기 뽑기
    // =================================
    private FishData GetRandomFish()
    {
        float r = Random.value;

        if (r < 0.65f) return normalFishList[Random.Range(0, normalFishList.Length)];
        if (r < 0.95f) return rareFishList[Random.Range(0, rareFishList.Length)];
        return legendaryFishList[Random.Range(0, legendaryFishList.Length)];
    }

    private void ShowFishResult()
    {
        var data = GetRandomFish();

        resultPanel.SetActive(true);

        resultTrashImage.sprite = data.fishSprite;
        resultGradeImage.sprite = data.gradeSprite;
        resultNameText.text = data.fishName;
        resultSizeText.text = data.sizeText;
        resultDescText.text = data.description;

        if (normalFishList.Contains(data)) resultBackgroundImage.sprite = normalBG;
        else if (rareFishList.Contains(data)) resultBackgroundImage.sprite = rareBG;
        else resultBackgroundImage.sprite = legendaryBG;

        // TopLeftProfile 연동
        ProfileUIManager.Instance.UnlockFish(data.fishID);
        BookManager.Instance.UpdateFishPages();

        string grade = "Normal";
        if (rareFishList.Contains(data)) grade = "Rare";
        else if (legendaryFishList.Contains(data)) grade = "Legendary";

        // ?? 여기만 변경! 물고기 전용 AddFishItem 사용
        SellManager.Instance.AddFishItem(
            data.fishID,
            data.fishSprite,
            data.fishName,
            data.sizeText,
            grade
        );
    }

    // =================================
    //   로비 복귀
    // =================================
    public void OnClickBackButton()
    {
        resultPanel.SetActive(false);
        BackToLobby();
    }

    private void BackToLobby()
    {
        SetLobbyUI(true);

        isPlaying = false;
        fishingGamePanel.SetActive(false);
        rodImage.SetActive(false);
        lineImage.SetActive(false);

        progressSlider.value = 0f;
        ResetPositions();
    }

    private void ResetPositions()
    {
        fishRect.anchoredPosition = new Vector2(0f, fishRect.anchoredPosition.y);
        playerRect.anchoredPosition = new Vector2(playerStartX, playerRect.anchoredPosition.y);

        fishDirection = 1f;
        isHoldButton = false;
    }
}
