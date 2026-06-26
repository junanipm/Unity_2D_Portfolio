using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    public CinemachineCamera cineCam;
    private CinemachineBasicMultiChannelPerlin noise;
    public BlueBoss blueBoss;
    public Transform bossArea;
    PlayerCombat playerCombat;
    PlayerController playerController;
    [SerializeField]
    public GameObject bossWall;
    private bool hasPaused = false;
 

    
    void Awake()
    {
 
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        blueBoss = GameObject.FindGameObjectWithTag("Boss").GetComponent<BlueBoss>();
        
        noise = cineCam.GetComponent<CinemachineBasicMultiChannelPerlin>();
        
    }
    void Start()
    {
        if (noise == null)
        {
            Debug.LogWarning("노이즈 컴포넌트가 CinemachineCamera에 없습니다!");
        }
    }
    void Update()
    {
        PlayerPositionFounder();
        
    }
    void PlayerPositionFounder()
    {
        if(playerController.gameObject.transform.position.x >= -18f && !hasPaused)
        {
            hasPaused = true;
            cineCam.Follow = bossArea;
            blueBoss.BossStart();
            bossWall.SetActive(true);
            playerController.PlayerIdlePlay();
        }
    }
    public void ShakeCamera(float amplitude, float frequency)
    {
        if (noise == null) 
        {
            Debug.Log("Noise is null");
            return;
        }
        noise.AmplitudeGain = amplitude;
        noise.FrequencyGain = frequency;

 
        
    }

    public void StopShake()
    {
        if (noise == null) return;

        noise.AmplitudeGain = 0f;
        noise.FrequencyGain = 0f;
    }

    public void SmoothStopShake(float fadeTime)
    {
        if (noise == null) return;
        StartCoroutine(FadeOutNoise(fadeTime));
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
    }
    public void BossClearCam()
    {
        cineCam.Follow = playerController.gameObject.transform;
        bossWall.SetActive(false);
    }
}
