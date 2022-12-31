using UnityEngine;

public class SoundController : MonoBehaviour {

    public static AudioClip sonido1;
    public static AudioClip sonido2;
    public AudioSource ASSonido1;
    public AudioSource ASSonido2;


    public void ReproducirSonido1()
    {
        if (sonido1)
        {
            if (ASSonido1) {
                ASSonido1.clip = sonido1;
                ASSonido1.Play();
            }
        }
    }
    public void ReproducirSonido2()
    {
        if (sonido2)
        {
            if (ASSonido2)
            {
                ASSonido2.clip = sonido2;
                ASSonido2.Play();
            }
        }
    }
}
