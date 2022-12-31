using UnityEngine;

public class Garra : MonoBehaviour
{
    [SerializeField]
    ScreenShake camShake = null;

    [SerializeField]
    float intensidadShakeGarra = 1;

    [SerializeField]
    Animator anim;

    [SerializeField] AudioClip audioGarraSube;
    [SerializeField] AudioClip audioGarraCae;
    [SerializeField] private ParticleSystem particulastierra;

    private SoundController sonido;
    private AudioSource AudioSonido;

    bool atrapar = false;

    private void Start()
    {
        sonido = FindObjectOfType<SoundController>();
        AudioSonido = sonido.ASSonido2;
    }

    public void GarraYoTeElijo()
    {
        anim.SetTrigger("GoGarra");
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (atrapar)
        {
            if (other.tag == FindObjectOfType<CatchBugsController>().TagBicho)
            {
                other.GetComponent<Collider>().isTrigger = true;
            }
        }
    }

    public void ShakeCamera()
    {
        camShake.MinorShake(intensidadShakeGarra);
        

    }

    public void Atrapar()
    {
        atrapar = true;
    }

    public void BastaDeAtrapar()
    {
        atrapar = false;
    }

    public void ReproducirSonidoSube()
    {
        SoundController.sonido2 = audioGarraSube;
        sonido.ReproducirSonido2();
    }
    public void ReproducirSonidoCae()
    {
        SoundController.sonido2 = audioGarraCae;
        sonido.ReproducirSonido2();
    }
    public void VerParticulas()
    {
        if(particulastierra)
            particulastierra.Play();
    }
}
