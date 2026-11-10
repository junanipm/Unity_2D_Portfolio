using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    public Slider bgSlider;
    public Slider sfxSlider;

    public const string PREF_KEY_BG_VOLUME = "BG_Volume";
    public const string PREF_KEY_SFX_VOLUME = "SFX_Volume";
   
    void Awake()
    {

        

    }
    void Start()
    {

  
        if (bgSlider != null)
        {
            bgSlider.value = PlayerPrefs.GetFloat(PREF_KEY_BG_VOLUME, 1.0f);
        }
        if (sfxSlider != null)
        {
            sfxSlider.value = PlayerPrefs.GetFloat(PREF_KEY_SFX_VOLUME, 1.0f);
        }


        if (bgSlider != null)
        {
            bgSlider.onValueChanged.RemoveAllListeners();
        }
        if (sfxSlider != null)
        {
            sfxSlider.onValueChanged.RemoveAllListeners();
        }


        SoundManager soundInstance = SoundManager.instance;

        if (soundInstance != null)
        {

            Debug.Log("SettingsMenu: GameManager.Instance에 연결합니다.");

            if (bgSlider != null)
            {
                bgSlider.onValueChanged.AddListener(soundInstance.BGVolume);
            }
            if (sfxSlider != null)
            {
                sfxSlider.onValueChanged.AddListener(soundInstance.SFXVolume);
            }
        }
        else
        {

            SoundManagerForMainPage mainPageSoundManager = FindAnyObjectByType<SoundManagerForMainPage>();

            if (mainPageSoundManager != null)
            {
                Debug.Log("SettingsMenu: SoundManagerForMainPage에 연결합니다.");

                if (bgSlider != null)
                {
                    bgSlider.onValueChanged.AddListener(mainPageSoundManager.BGVolume);
                }
                if (sfxSlider != null)
                {
                    sfxSlider.onValueChanged.AddListener(mainPageSoundManager.SFXVolume);
                }
            }
            else
            {

                Debug.LogError("SettingsMenu: 어떤 사운드 매니저도 찾을 수 없습니다!");
            }
        }
        
    }

    public void ResumeButton()
    {
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.isPaused)
            {
                GameManager.Instance.ResumeGame();
            }
        }

    }
    public void ExitButton()
    {
        Time.timeScale = 1f;
        GameManager.Instance.ResetGameStats();
        GameManager.Instance.isPaused = false;
        SceneManager.LoadScene("MainPage");
        
        
    }
}