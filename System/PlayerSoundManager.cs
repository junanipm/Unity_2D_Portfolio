using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    [Header("오디오1")]
    public AudioClip b_a1;
    public AudioClip b_a2;
    public AudioClip b_a3;
    public AudioClip b_s;
    public AudioClip p_a;
    public AudioClip p_s;
    public AudioClip y_a;
    public AudioClip y_s;
    [Header("오디오2")]
    public AudioClip dashClip;
    public AudioClip dieClip;
    
    public AudioClip jumpClip;

    void Play_B_A1()
    {
        SoundManager.instance.SFXPlay("B_A1", b_a1);
    }
    void Play_B_A2()
    {
        SoundManager.instance.SFXPlay("B_A2", b_a2);
    }
    void Play_B_A3()
    {
        SoundManager.instance.SFXPlay("B_A3", b_a3);
    }
    void Play_B_S()
    {
        SoundManager.instance.SFXPlay("B_Skill", b_s);
    }
    void Play_P_A()
    {
        SoundManager.instance.SFXPlay("P_A", p_a);
    }
    void Play_P_S()
    {
        SoundManager.instance.SFXPlay("P_Skill", p_s);
    }
    void Play_Y_A()
    {
        SoundManager.instance.SFXPlay("y_A", y_a);
    }
    void Play_Y_S()
    {
        SoundManager.instance.SFXPlay("y_Skill", y_s);
    }
    void Play_Dash()
    {
        SoundManager.instance.SFXPlay("Dash", dashClip);
    }
    void Play_Die()
    {
        SoundManager.instance.SFXPlay("Die", dieClip);
    }
    void Play_Jump()
    {
        SoundManager.instance.SFXPlay("Jump", jumpClip);
    }
}
