using UnityEngine;
using System.Collections;

public class BookManager : MonoBehaviour
{
    public static BookManager Instance;

    [Header("도감 전체 패널")]
    public GameObject bookPanel;

    [Header("페이지 슬라이드 컨테이너")]
    public RectTransform pageContainer;  // Page_0~2의 부모
    public RectTransform[] pages;        // Page_0, Page_1, Page_2
    public float pageWidth = 1080f;      // 한 페이지 가로 크기

    [Header("물고기 도감 이미지 (fishID 순서대로 8개)")]
    public GameObject[] fishImages;      // FishImage_0~7

    private int currentPage = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        bookPanel.SetActive(false);
        UpdateFishPages();          // 처음에 저장된 해금 상태 반영
        MoveToPage(0, false);
    }

    // ===== 도감 열기 / 닫기 =====
    public void OpenBook()
    {
        bookPanel.SetActive(true);
        UpdateFishPages();
    }

    public void CloseBook()
    {
        bookPanel.SetActive(false);
    }

    // ===== 페이지 이동 =====
    public void NextPage()
    {
        if (currentPage >= pages.Length - 1) return;
        currentPage++;
        MoveToPage(currentPage, true);
    }

    public void PrevPage()
    {
        if (currentPage <= 0) return;
        currentPage--;
        MoveToPage(currentPage, true);
    }

    private void MoveToPage(int index, bool animate)
    {
        float targetX = -index * pageWidth;

        StopAllCoroutines();
        if (animate)
            StartCoroutine(SmoothMove(pageContainer.anchoredPosition,
                                      new Vector2(targetX, 0), 0.25f));
        else
            pageContainer.anchoredPosition = new Vector2(targetX, 0);
    }

    IEnumerator SmoothMove(Vector2 from, Vector2 to, float time)
    {
        float t = 0;
        while (t < time)
        {
            t += Time.deltaTime;
            pageContainer.anchoredPosition =
                Vector2.Lerp(from, to, t / time);
            yield return null;
        }
        pageContainer.anchoredPosition = to;
    }

    // ===== 물고기 해금 반영 =====
    public void UpdateFishPages()
    {
        for (int i = 0; i < fishImages.Length; i++)
        {
            int unlocked = PlayerPrefs.GetInt("fish_" + i, 0);
            fishImages[i].SetActive(unlocked == 1);
        }
    }
}
