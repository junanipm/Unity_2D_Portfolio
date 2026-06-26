<<<<<<< HEAD
using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class PortalBreakDown : MonoBehaviour
{
    public CinemachineCamera cineCam;
    private CinemachineBasicMultiChannelPerlin noise;
    PlayerController playerController;
    Animator anim;
    public AudioClip audioClip;
    [SerializeField]
    private float triggerX;
    bool hasPaused;
    public float amplitude;
    public float frequency;
    public float duration;
    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        noise = cineCam.GetComponent<CinemachineBasicMultiChannelPerlin>();
        anim.enabled = false;
    }

    
    void Update()
    {
        PlayerPositionFounder();
    }
    void PlayerPositionFounder()
    {
        if (playerController.gameObject.transform.position.x >= -21f && !hasPaused)
        {

            hasPaused = true;
            playerController.playerPaused = true;
            playerController.PlayerIdlePlay();
            SoundManager.instance.SFXPlay("breakdown", audioClip);

            StartCoroutine(ShakeCamera());
        }
    }
    IEnumerator ShakeCamera()
    {
        noise.AmplitudeGain = amplitude * 0.5f;
        noise.FrequencyGain = frequency * 0.5f;
        yield return new WaitForSeconds(duration);
        anim.enabled = true;
        noise.AmplitudeGain = amplitude;
        noise.FrequencyGain = frequency;
        yield return new WaitForSeconds(duration);
        StartCoroutine(FadeOutNoise(1));
       
    }
    private IEnumerator FadeOutNoise(float duration)
    {
        float startAmp = noise.AmplitudeGain;
        float startFreq = noise.FrequencyGain;
        float timer = 0f;

        while (timer < duration)
        {
            float t = timer / duration;
            noise.AmplitudeGain = Mathf.Lerp(startAmp, 0f, t);
            noise.FrequencyGain = Mathf.Lerp(startFreq, 0f, t);
            timer += Time.deltaTime;
            yield return null;
        }

        noise.AmplitudeGain = 0f;
        noise.FrequencyGain = 0f;
         playerController.PlayerResume();
    }
}
=======
using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class PortalBreakDown : MonoBehaviour
{
    public CinemachineCamera cineCam;
    private CinemachineBasicMultiChannelPerlin noise;
    PlayerController playerController;
    Animator anim;
    public AudioClip audioClip;
    [SerializeField]
    private float triggerX;
    bool hasPaused;
    public float amplitude;
    public float frequency;
    public float duration;
    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        noise = cineCam.GetComponent<CinemachineBasicMultiChannelPerlin>();
        anim.enabled = false;
    }

    
    void Update()
    {
        PlayerPositionFounder();
    }
    void PlayerPositionFounder()
    {
        if (playerController.gameObject.transform.position.x >= -21f && !hasPaused)
        {

            hasPaused = true;
            playerController.playerPaused = true;
            playerController.PlayerIdlePlay();
            SoundManager.instance.SFXPlay("breakdown", audioClip);

            StartCoroutine(ShakeCamera());
        }
    }
    IEnumerator ShakeCamera()
    {
        noise.AmplitudeGain = amplitude * 0.5f;
        noise.FrequencyGain = frequency * 0.5f;
        yield return new WaitForSeconds(duration);
        anim.enabled = true;
        noise.AmplitudeGain = amplitude;
        noise.FrequencyGain = frequency;
        yield return new WaitForSeconds(duration);
        StartCoroutine(FadeOutNoise(1));
       
    }
    private IEnumerator FadeOutNoise(float duration)
    {
        float startAmp = noise.AmplitudeGain;
        float startFreq = noise.FrequencyGain;
        float timer = 0f;

        while (timer < duration)
        {
            float t = timer / duration;
            noise.AmplitudeGain = Mathf.Lerp(startAmp, 0f, t);
            noise.FrequencyGain = Mathf.Lerp(startFreq, 0f, t);
            timer += Time.deltaTime;
            yield return null;
        }

        noise.AmplitudeGain = 0f;
        noise.FrequencyGain = 0f;
         playerController.PlayerResume();
    }
}
>>>>>>> c1275e0b368542d9fd2997dc6383c3e959a629e3
