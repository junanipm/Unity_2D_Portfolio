using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
public class SoundManager : MonoBehaviour
{

    public AudioMixer mixer;
    public AudioSource bgMusic;
    public AudioClip[] bgList;
    public static SoundManager instance;

    public const string PREF_KEY_BG_VOLUME = "BG_Volume";
    public const string PREF_KEY_SFX_VOLUME = "SFX_Volume";
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
            SceneManager.sceneLoaded += OnSceneLoaded;

            float bgVol = PlayerPrefs.GetFloat(PREF_KEY_BG_VOLUME, 1.0f);
            float sfxVol = PlayerPrefs.GetFloat(PREF_KEY_SFX_VOLUME, 1.0f);

            SetVolume("BG", bgVol);
            SetVolume("SFX", sfxVol);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void PlayMusicByName(string clipName)
    {
        for (int i = 0; i < bgList.Length; i++)
        {
            if (bgList[i].name == clipName)
            {
                BGSoundPlay(bgList[i]);
                return; 
            }
        }

       
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        for (int i = 0; i < bgList.Length; i++)
        {
            if (arg0.name == bgList[i].name)
            {
                BGSoundPlay(bgList[i]);
            }

        }

    }
    public void BGVolume(float val)
    {
        SetVolume("BG", val);
    }
    public void SFXVolume(float val)
    {
        SetVolume("SFX", val);
    }

    private void SetVolume(string channel, float val)
    {

        val = Mathf.Clamp(val, 0.0f, 1.0f);


        float dbValue;
        if (val <= 0.0001f) 
        {
            dbValue = -80.0f;
        }
        else
        {
            dbValue = Mathf.Log10(val) * 20;
        }

        mixer.SetFloat(channel, dbValue);

        if (channel == "BG")
        {
            PlayerPrefs.SetFloat(PREF_KEY_BG_VOLUME, val);
        }
        else if (channel == "SFX")
        {
            PlayerPrefs.SetFloat(PREF_KEY_SFX_VOLUME, val);
        }
    }

    public void SFXPlay(string sfxName, AudioClip clip)
    {
        GameObject go = new GameObject(sfxName + "Sound");

        AudioSource audioSource = go.AddComponent<AudioSource>();

        audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
        audioSource.clip = clip;
        audioSource.volume = 0.75f;
        audioSource.Play();
        Destroy(go, clip.length);
    }
    public void BGSoundPlay(AudioClip clip)
    {
        if (bgMusic.clip == clip && bgMusic.isPlaying)
        {
            return;
        }

        bgMusic.outputAudioMixerGroup = mixer.FindMatchingGroups("BG")[0];
        bgMusic.clip = clip;
        bgMusic.loop = true;
        bgMusic.volume = 1.2f;
        bgMusic.Play();
    }
}
