using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Mixer")]
    public AudioMixer mainMixer;

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    // ?? 추가: 홀드 버튼 전용 SFX 소스
    [Header("Extra SFX Source (Fishing Hold 등)")]
    public AudioSource holdSfxSource;

    [Header("BGM Clips")]
    public AudioClip mainBGM;          // Start / Loading / Main 공용
    public AudioClip fishingGameBGM;

    [Header("SFX Clips")]
    public AudioClip buttonSFX;
    public AudioClip sellSFX;
    public AudioClip nextPageSFX;
    public AudioClip resultOpenSFX;
    public AudioClip catchButtonSFX;

    // ?? 추가: 낚시 전용 SFX
    [Header("Fishing SFX Clips")]
    public AudioClip fishingHoldSFX;      // 홀드 버튼 누르는 동안 나는 소리
    public AudioClip fishingSuccessSFX;   // 게이지 다 찼을 때 효과음

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);   // 씬 넘어가도 유지
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        // 게임 처음 시작하면 메인 BGM부터 재생
        PlayBGM(mainBGM);
    }

    // ===== BGM 재생 =====
    public void PlayBGM(AudioClip clip)
    {
        if (clip == null) return;

        // 같은 BGM이면 굳이 다시 재생 X
        if (bgmSource.clip == clip && bgmSource.isPlaying) return;

        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    // ===== 일반 SFX 재생 (원샷) =====
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;

        sfxSource.PlayOneShot(clip);
    }

    // ===== 홀드 버튼용 SFX (누르는 동안만) =====
    public void PlayFishingHold()
    {
        if (holdSfxSource == null || fishingHoldSFX == null) return;

        // 매번 처음부터 재생
        holdSfxSource.Stop();
        holdSfxSource.clip = fishingHoldSFX;
        holdSfxSource.loop = false;   // 길이가 길다 했으니 반복은 X
        holdSfxSource.time = 0f;
        holdSfxSource.Play();
    }

    public void StopFishingHold()
    {
        if (holdSfxSource == null) return;
        holdSfxSource.Stop();
    }

    // ===== 낚시 성공 SFX =====
    public void PlayFishingSuccess()
    {
        PlaySFX(fishingSuccessSFX);
    }

    // ===== 볼륨 제어 (슬라이더 0~1 값 사용) =====
    public void SetMasterVolume(float value)
    {
        SetVolume("MasterVolume", value);
    }

    public void SetBGMVolume(float value)
    {
        SetVolume("BGMVolume", value);
    }

    public void SetSFXVolume(float value)
    {
        SetVolume("SFXVolume", value);
    }

    private void SetVolume(string parameterName, float value)
    {
        value = Mathf.Clamp01(value); // 안전장치

        if (value <= 0.01f)  // 너무 작은 값도 무음 처리
        {
            mainMixer.SetFloat(parameterName, -80f);
            return;
        }

        float dB = Mathf.Lerp(-25f, 0f, Mathf.Pow(value, 0.8f));  // 더 부드러운 곡선
        mainMixer.SetFloat(parameterName, dB);
    }
}
