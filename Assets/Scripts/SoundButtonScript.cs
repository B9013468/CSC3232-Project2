using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundButtonScript : MonoBehaviour
{

    [SerializeField] AudioSource clickSound;
    [SerializeField] AudioSource onButtonSound;

    public void PlayClickedSound()
    {
        clickSound.Play();
    }

    public void PlayOnButtonSound()
    {
        onButtonSound.Play();
    }
}
