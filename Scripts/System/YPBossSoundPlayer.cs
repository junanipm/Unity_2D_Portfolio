using UnityEngine;

public class YPBossSoundPlayer : MonoBehaviour
{
    public AudioClip fire;
    public AudioClip crystal;
    public AudioClip change;
    public AudioClip meteor;
    public AudioClip sphere;
    public AudioClip spinespawn;

    public AudioClip spinefly;
    public AudioClip thunder;
    public AudioClip teleport;
    public AudioClip clear;

    public void SoundPlay(int sound)
    {


        if (sound == 0)
        {
            SoundManager.instance.SFXPlay("fire", fire);
        }
        else if (sound == 1)
        {
            SoundManager.instance.SFXPlay("crystal", crystal);
        }
        else if (sound == 2)
        {
            SoundManager.instance.SFXPlay("change", change);
        }
        else if (sound == 3)
        {
            SoundManager.instance.SFXPlay("meteor", meteor);
        }
        else if (sound == 4)
        {
            SoundManager.instance.SFXPlay("sphere", sphere);
        }
        else if (sound == 5)
        {
            SoundManager.instance.SFXPlay("spinespawn", spinespawn);
        }
        else if (sound == 6)
        {
            SoundManager.instance.SFXPlay("spinefly", spinefly);
        }
        else if (sound == 7)
        {
            SoundManager.instance.SFXPlay("thunder", thunder);
        }
        else if (sound == 8)
        {
            SoundManager.instance.SFXPlay("teleport", teleport);
        }
        else if (sound == 9)
        {
            SoundManager.instance.SFXPlay("clear", clear);
        }



    }
}
