using UnityEngine;
using UnityEngine.UI;

public class UIButtonSFX : MonoBehaviour
{
    [Header("전용 SFX (없으면 기본 버튼음 재생)")]
    public AudioClip customSFX;

    private Button btn;

    void Awake()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(PlaySound);
    }

    void PlaySound()
    {
        if (AudioManager.Instance == null) return;

        // 전용 SFX가 있으면 해당 SFX 재생
        if (customSFX != null)
        {
            AudioManager.Instance.PlaySFX(customSFX);
        }
        else
        {
            // 없으면 기본 버튼음
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonSFX);
        }
    }
}
