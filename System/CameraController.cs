using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public CinemachineCamera cineCam;
    private CinemachineBasicMultiChannelPerlin noise;
    void Start()
    {   
        cineCam = GetComponent<CinemachineCamera>();
        noise = cineCam.GetComponent<CinemachineBasicMultiChannelPerlin>();
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
}
