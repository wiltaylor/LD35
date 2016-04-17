using UnityEngine;
using System.Collections;

public class SFXPlayer : MonoBehaviour
{

    public AudioSource Audio;
    public float UpperPitch = 1.1f;
    public float LowerPitch = 0.9f;

    public void PlaySFX(AudioClip clip)
    {
        Audio.pitch = Random.Range(LowerPitch, UpperPitch);
        Audio.PlayOneShot(clip);
    }
}
