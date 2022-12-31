using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bichin : MonoBehaviour {

    [SerializeField] AudioClip sonidoBicho;
    private SoundController controladorSonido;
    [SerializeField] float tiempoPasos;
    private float timerBichoPasos = 0;

    public Transform spawn;

    [SerializeField] List<Transform> objetivos;
    [SerializeField] private float[] probabilidadObjetivo = { 0.33f, 0.33f, 0.33f };
    private int indice;
    private Quaternion vectorRelativo;
    [HideInInspector] public Vector3 vectorNuevaPosicion;
    [HideInInspector] public bool seEscapa=false;
    [SerializeField] private float distanciaDelObjetivo;
    [SerializeField] private float velocidadMovimiento;
    [SerializeField] private float velocidadRotacion;
    [SerializeField] Color colorBicho;
    public float tiempoDesapareceBicho = 2f;
    public bool moverBicho = false;
    private AudioSource AudioSonido;
    private Probabilidad probabilidad;


    // Use this for initialization
    void Start () {
        //indice = (int) FindObjectOfType<FindTheBugController>().AleatorioDeMovimientos(probabilidadObjetivo);
        probabilidad = FindObjectOfType<Probabilidad>();
        indice = (int)probabilidad.DameAleatorio(probabilidadObjetivo);

        timerBichoPasos = tiempoPasos;
        transform.GetComponent<Renderer>().material.color = colorBicho;
        transform.SetPositionAndRotation(spawn.position, Quaternion.identity);
        controladorSonido = FindObjectOfType<SoundController>();
        AudioSonido = controladorSonido.ASSonido1;
    }
	
	// Update is called once per frame
	void Update () {
        if (moverBicho)
        {
            AndaAlObjetivo();
        }
    }

    public void AndaAlObjetivo()
    {
        if (seEscapa)
        {
            vectorNuevaPosicion = spawn.position;
        }
        else {
            vectorNuevaPosicion = objetivos[indice].transform.position;
        }
        vectorRelativo = Quaternion.LookRotation(vectorNuevaPosicion - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(vectorNuevaPosicion - transform.position), velocidadRotacion * Time.deltaTime);

        if (Vector3.Distance(vectorNuevaPosicion, transform.position) >= distanciaDelObjetivo && Vector3.Distance(vectorNuevaPosicion, transform.position) > 0)
        {
            if (Mathf.Round(Mathf.Abs(transform.rotation.y)) == Mathf.Round(Mathf.Abs(vectorRelativo.y)))
            {
                transform.position += transform.forward * velocidadMovimiento * Time.deltaTime;

                timerBichoPasos += Time.deltaTime;
                
                if (timerBichoPasos >= tiempoPasos)
                {
                    AudioSonido.Stop();
                    AudioSonido.clip = sonidoBicho;
                    timerBichoPasos = 0;
                    AudioSonido.Play();
                }
            }
        }
    }
}
