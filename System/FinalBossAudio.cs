using UnityEngine;

public class FinalBossAudio : MonoBehaviour
{
 
    public AudioClip a1;
    public AudioClip a2;
    public AudioClip hor;
    
    public AudioClip ham;
    public AudioClip arrow;
    public AudioClip dash;
    public AudioClip arrowrain;
    public AudioClip intro;
    public AudioClip landing;
    public AudioClip mapbreakdown;
    public AudioClip swordAura;
    

    public void SoundPlay(int sound)
    {


        if (sound == 0)
        {
            SoundManager.instance.SFXPlay("a1", a1);
        }
        else if (sound == 1)
        {
            SoundManager.instance.SFXPlay("a2", a2);
        }
        else if (sound == 2)
        {
            SoundManager.instance.SFXPlay("hor", hor);
        }
        else if (sound == 3)
        {
            SoundManager.instance.SFXPlay("swordAura", swordAura);
        }
        else if (sound == 4)
        {
            SoundManager.instance.SFXPlay("ham", ham);
        }
        else if (sound == 5)
        {
            SoundManager.instance.SFXPlay("arrow", arrow);
        }
        else if (sound == 6)
        {
            SoundManager.instance.SFXPlay("dash", dash);
        }
        else if (sound == 7)
        {
            SoundManager.instance.SFXPlay("arrowrain", arrowrain);
        }
        else if (sound == 8)
        {
            SoundManager.instance.SFXPlay("intro", intro);
        }
        else if (sound == 9)
        {
            SoundManager.instance.SFXPlay("landing", landing);
        }
        else if (sound == 10)
        {
            SoundManager.instance.SFXPlay("mapbreakdown", mapbreakdown);
        }




    }
}
