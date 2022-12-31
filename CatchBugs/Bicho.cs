using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bicho : MonoBehaviour
{
    public bool bichoEnMovimiento = false;
    [SerializeField] AudioClip audioBicho;

    [SerializeField] float tiempoPasos;
    [SerializeField] float valorCrecimiento=0.01f;

    private float timerBicho = 0;
    private float tiempoProximoSalidaBicho = 0;
    private float limXder;
    private float limXizq;
    private float limZder;
    private float limZizq;
    private Vector3 vectorNuevaPosicion;
    private float velocidadMaxMovimiento;
    private float velocidadMinMovimiento;

    //valores de rotacion y velocidad originales
    private float velocidadOriMaxMovimiento;
    private float velocidadOriMinMovimiento;
    private float velocidadOriRotacion;

    private float velocidadRotacion;
    private float tiempoMinMoverBicho;
    private float tiempoMaxMoverBicho;
    private float distanciaDelObjetivo;

    private float factorGiro = 0.5f;
    private Quaternion vectorRelativo;
    private int pasosCaminados=0;
    private SoundController sonido;
    private AudioSource AudioSonido;
    private float timerBichoPasos = 0;
    /// <summary>
    /// cantidad de monedas por bicho incrementable segun cuanto a caminado
    /// </summary>
    [HideInInspector] public int valorBicho;

    //Colores
    public int valorBichoColor0 = 1;
    public int valorBichoColor1 = 2;
    public int valorBichoColor2= 3;

    public float variacionMovimiento = 1;
    
    public Color color0;
    [SerializeField] int pasosColor1;
    public Color color1;
    [SerializeField] int pasosColor2;
    public Color color2;
    public Color colorRenace;

    //Escala
    private float escX = 0;
    private float escY = 0;
    private float escZ = 0;

    //Masa del bicho
    private float masaOri = 0f;
    private Rigidbody masaBicho;

    private CatchBugsController controlador;
    private void Start()
    {
        controlador = FindObjectOfType<CatchBugsController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == FindObjectOfType<CatchBugsController>().TagGarra)
        {
            controlador.TK.Aciertos += 1;
            controlador.TK.Ticket +=  valorBicho;
            controlador.uiminigame.PublicarTextos("", controlador.TK.Ticket, controlador.TK.Aciertos, 0);
            GetComponent<Transform>().gameObject.SetActive(false);
        }            
        if (other.tag == FindObjectOfType<CatchBugsController>().TagBicho)
        {
            SonidoCaminaBicho(Time.deltaTime);
        }
    }

    private void Awake()
    {
        PrimerColorBicho();
        CambiarParametrosMovimiento();
        sonido = FindObjectOfType<SoundController>();
        AudioSonido = sonido.ASSonido1;
        timerBichoPasos = tiempoPasos;
        escX = transform.localScale.x;
        escY = transform.localScale.y;
        escZ = transform.localScale.z;

        masaBicho = GetComponent<Rigidbody>();
        masaOri = masaBicho.mass;
    }

    private void PrimerColorBicho()
    {
        transform.GetComponent<Renderer>().material.color = Color.black;
        valorBicho = valorBichoColor0;
    }
    public void CambiarParametrosMovimiento()
    {
        distanciaDelObjetivo = FindObjectOfType<CatchBugsController>().distanciaDelObjetivo;

        velocidadMaxMovimiento = FindObjectOfType<CatchBugsController>().velocidadMaxMovimiento;
        velocidadMinMovimiento = FindObjectOfType<CatchBugsController>().velocidadMinMovimiento;
        velocidadOriMinMovimiento = velocidadMinMovimiento;
        velocidadOriMaxMovimiento = velocidadMaxMovimiento;
        velocidadOriRotacion = velocidadRotacion;

        velocidadRotacion = FindObjectOfType<CatchBugsController>().velocidadRotacion;
        tiempoMinMoverBicho = FindObjectOfType<CatchBugsController>().tiempoMinMoverBicho;
        tiempoMaxMoverBicho = FindObjectOfType<CatchBugsController>().tiempoMaxMoverBicho;
        limXder = FindObjectOfType<CatchBugsController>().limiteXder;
        limXizq = FindObjectOfType<CatchBugsController>().limiteXizq;
        limZder = FindObjectOfType<CatchBugsController>().limiteZder;
        limZizq = FindObjectOfType<CatchBugsController>().limiteZizq;
    }

    public void UpdateBichos(float deltaTimeModificado)
    {
        if (bichoEnMovimiento)
        {
            timerBicho += deltaTimeModificado;

            if (timerBicho >= tiempoProximoSalidaBicho && Vector3.Distance(vectorNuevaPosicion, transform.position) <= distanciaDelObjetivo)
            {
                tiempoProximoSalidaBicho = RandomTiempoBicho();
                timerBicho = 0;
                vectorNuevaPosicion = RandomPosicionBicho();
            }
            MueveBicho( deltaTimeModificado);
            VerificaPasos();
        }
    }

    private void SonidoCaminaBicho(float deltaTimeModificado)
    {
        if (CatchBugsController.activaSonidoBichos)
        { 
            timerBichoPasos += deltaTimeModificado;

            if (timerBichoPasos >= tiempoPasos)
            {
                AudioSonido.Stop();
                AudioSonido.clip = audioBicho;
                timerBichoPasos = 0;
                AudioSonido.Play();
            }
        }
    }

    private void SonidoChocaBicho()
    {
        if (CatchBugsController.activaSonidoBichos)
        {
            AudioSonido.Stop();
            AudioSonido.clip = audioBicho;
            AudioSonido.Play();
        }
    }
    public void VerificaPasos()
    {
        if (pasosCaminados == pasosColor1)
        {
            transform.GetComponent<Renderer>().material.color = color1;
            valorBicho = valorBichoColor1;
            velocidadMaxMovimiento = velocidadOriMaxMovimiento * variacionMovimiento;
            velocidadMinMovimiento = velocidadOriMinMovimiento * variacionMovimiento;

            tiempoMinMoverBicho = tiempoMaxMoverBicho;

            //aumenta la  velocidad de rotación para que el bicho no vuele
            velocidadRotacion = velocidadOriRotacion + velocidadOriRotacion * 10 / 100;

            masaBicho.mass = masaOri - masaOri * 10 / 100;

            StopCoroutine(AchicaBicho(10));
            if (gameObject.activeSelf)
            {
                StartCoroutine(AchicaBicho(10));
            }
        }
        else if (pasosCaminados == pasosColor2)
        {
            transform.GetComponent<Renderer>().material.color = color2;
            valorBicho = valorBichoColor2;
            velocidadMaxMovimiento = velocidadOriMaxMovimiento * (variacionMovimiento * 2);
            velocidadMinMovimiento = velocidadOriMaxMovimiento;

            tiempoMinMoverBicho = tiempoMaxMoverBicho;

            //aumenta la  velocidad de rotación para que el bicho no vuele
            velocidadRotacion = velocidadOriRotacion + velocidadOriRotacion * 20 / 100;

            masaBicho.mass = masaOri - masaOri * 20 / 100;

            StopCoroutine(AchicaBicho(20));
            if (gameObject.activeSelf)
            {
                StartCoroutine(AchicaBicho(20));
            }
        }
        else if (pasosCaminados > pasosColor2  && pasosCaminados < pasosColor2 + pasosColor2 / 2 && transform.GetComponent<Renderer>().material.color == color2)
        {
            transform.GetComponent<Renderer>().material.color = colorRenace;
        }
        else if (pasosCaminados > pasosColor2 + pasosColor2 / 2)
        {
            pasosCaminados = 0;
            valorBicho = valorBichoColor0;
            transform.GetComponent<Renderer>().material.color = Color.black;

            //vuelve la velocidad de rotación al estado inicial
            velocidadRotacion = velocidadOriRotacion;

            masaBicho.mass = masaOri;

            StopCoroutine(AgrandaBicho());
       
            if (gameObject.activeSelf)
            {
                StartCoroutine(AgrandaBicho());
            }
        }
    }

    IEnumerator AchicaBicho(float tamanio)
    {
        
            float cuenta = 0;
            while (escX - (escX * tamanio / 100) < transform.localScale.x )
            {
                cuenta += valorCrecimiento;
                transform.localScale = new Vector3(transform.localScale.x - transform.localScale.x * cuenta / 100,
                            transform.localScale.y - transform.localScale.y * cuenta / 100,
                            transform.localScale.z - transform.localScale.z * cuenta / 100);
            }
        
        yield return null;
    }

    IEnumerator AgrandaBicho()
    {
        float cuenta = 0;
        while (transform.localScale.x < escX )
        {
            cuenta += valorCrecimiento;

            transform.localScale = new Vector3(transform.localScale.x + transform.localScale.x * cuenta / 100,
                        transform.localScale.y + transform.localScale.y * cuenta / 100,
                        transform.localScale.z + transform.localScale.z * cuenta / 100);
            
        }
        yield return null;

    }
    public void MueveBicho( float deltaTimeModificado)
    {
        vectorRelativo = Quaternion.LookRotation(vectorNuevaPosicion - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(vectorNuevaPosicion - transform.position), velocidadRotacion * deltaTimeModificado);

        if (Vector3.Distance(vectorNuevaPosicion, transform.position) >= distanciaDelObjetivo && Vector3.Distance(vectorNuevaPosicion, transform.position) > 0)
        {
            if (Mathf.Round(Mathf.Abs(transform.rotation.y)) == Mathf.Round(Mathf.Abs(vectorRelativo.y)))
            {
                transform.position += transform.forward * Random.Range(velocidadMinMovimiento, velocidadMaxMovimiento) * deltaTimeModificado;
                pasosCaminados += 1;
            }
        }
    }

    private Vector3 RandomPosicionBicho()
    {
        vectorNuevaPosicion = new Vector3(Random.Range(limXizq, limXder), 0, Random.Range(limZizq, limZder))
        {
            y = 0
        };
        return vectorNuevaPosicion;
    }

    private float RandomTiempoBicho()
    {
        return Random.Range(tiempoMinMoverBicho, tiempoMaxMoverBicho);
    }
}
