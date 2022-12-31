using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HammerController : MonoBehaviour
{
    [Header("Controladores")]
    [Space]
    [Header("Controladores")]
    [Tooltip("Muestra los valores de Tickets, aciertos y desaciertos del minijuego")]
    public UIminijuego uiminigame;
    [Tooltip("Permite manipular los valores  Tickets, aciertos y desaciertos del minijuego")]
    public Ticketera TK;
    [Tooltip("Permite manipular los valores de tiempo del minijuego")]
    public Tiempo tie;
    [Tooltip("Permite manipular los valores de tiempo del minijuego")]
    [SerializeField] SoundController controladorSonido;
    [Tooltip("Detecta que objeto fue tocado")]
    [SerializeField] UtilizaClic clic;

    [Header("Sonidos")]
    [SerializeField] List<AudioClip> sonidosPuntaje;
    [SerializeField] AudioClip sonidoGolpeFuerte;
    [SerializeField] AudioClip sonidoGolpeSuave;
    [SerializeField] AudioClip sonidoMovimientoMartillo;
    [SerializeField] AudioClip sonidoCampana;
    [SerializeField] AudioClip sonidoAvance;
    [Header("Comando de juego")]
    [SerializeField] KeyCode queToco;
    [Header("Imagenes avance martillo")]
    [SerializeField] Image imaAbajo;
    [SerializeField] Image imaMedio;
    [SerializeField] Image imaTresCuarto;
    [SerializeField] Image imaArriba;
    [Header("Velocidad y Tpo de juego")]
    [SerializeField] float velocidad=1;
    [Tooltip("Tiempo de juego en segundos")]
    [SerializeField] float tiempoDeJuegoSeg = 20;

    [SerializeField] float tiempoMinCambioSlider=1;
    [SerializeField] float tiempoMaxCambioSlider=2;
    private float tiempoRandom;

    
    private float valorTocado = 0;
    private float valorAleatorio = 0;
    [HideInInspector] public bool jugando=true;

    [Header("Vumetro")]
    [SerializeField] float demoraAvance = 0.2f;
    public Image imagen;
    public bool inversor;
    public bool botonTocado=false;
    private bool marcando = false;

    void Start ()
    {
        SoundController.sonido1 = sonidosPuntaje[0];
        tiempoRandom = Random.RandomRange(tiempoMinCambioSlider, tiempoMaxCambioSlider);
        imaAbajo.fillAmount = 0;
        imaMedio.fillAmount = 0;
        imaTresCuarto.fillAmount = 0;
        imaArriba.fillAmount = 0;
    }

    

    // Update is called once per frame
    void Update ()
    {
        
        if (jugando)
        {
            uiminigame.PublicarTextos(tie.VerTiempo, TK.Ticket,0, 0,true,true,false,false);

            if (tie.SuperoTiempoHHMMSS(0, 0, tiempoDeJuegoSeg))
            {
                jugando = false;
            }

            Vumetro();

            if (botonTocado == false) //si aun no golpea con el martillo
            {
                valorTocado = imagen.fillAmount;
                imaAbajo.fillAmount = 0;
                imaMedio.fillAmount = 0;
                imaTresCuarto.fillAmount = 0;
                imaArriba.fillAmount = 0;
            }
            else //ha golpeado con el martillo
            {
                
                if (tie.SuperoDeltaTime(demoraAvance, Time.deltaTime)) //espera un tiempo muy pequeño para dar tiempo a la animacion
                {
                    if (marcando) //Avance de puntos en el martillo
                    {
                        MartilloMarca();//esta mostrando el avance de la marcacion del martillo
                    }
                    else //termino la marcacion del martillo
                    {
                        if (botonTocado)  //va a dejar todo como estaba para poder seguir jugando
                        {
                            RestableceMarcaMartillo();
                        }
                    }
                }
            }

            if (Input.GetKey(queToco))
            {
                clic.VerificaPosicionClicMouse();
                if (clic.objetoAlcanzado) { 
                    if(clic.objetoAlcanzado.tag == "Boton" &&  botonTocado == false)
                    {
                        if (valorTocado > 0.7f)
                        {
                            SoundController.sonido2 = sonidoGolpeFuerte;
                        }
                        else
                        {
                            SoundController.sonido2 = sonidoGolpeSuave;
                        }
                        controladorSonido.ReproducirSonido2();

                        botonTocado = true;
                        marcando = true;
                    }
                }
            }
        }
	}
    public void RestableceMarcaMartillo()
    {
        //Permite seguir jugando
        marcando = true;
        botonTocado = false;
    }
    public void Vumetro()
    {
        if (tie.SuperoDeltaTime(tiempoRandom, Time.deltaTime))
        {
            tiempoRandom = Random.RandomRange(tiempoMinCambioSlider, tiempoMaxCambioSlider);
            valorAleatorio = Random.RandomRange(0f, 1f);
        }

        if (imagen.fillAmount != valorAleatorio)
        {
            if (inversor == true)
            {
                imagen.fillAmount -= Time.deltaTime;

                if (imagen.fillAmount == 0)
                {
                    inversor = false;
                }
            }
            else
            {
                imagen.fillAmount += Time.deltaTime;

                if (imagen.fillAmount >= valorAleatorio)
                {
                    inversor = true;
                }
            }
        }

    }

    private void MartilloMarca()
    {
        SoundController.sonido1 = sonidoAvance;

        if (valorTocado <= 0.25f) 
        {
            IncrementaAbajo();

            if (imaAbajo.fillAmount >= valorTocado)
            {
                imaAbajo.fillAmount = valorTocado;
                SoundController.sonido1 = sonidosPuntaje[0];
                controladorSonido.ReproducirSonido1();
                marcando = false;
            }
        }

        if (valorTocado > 0.25f && valorTocado <= 0.50f) 
        {
            if (imaAbajo.fillAmount < 1f )
            {
                IncrementaAbajo();
            }
            else if (imaMedio.fillAmount < 1f )
            {
                IncrementaMedio();
                if (imaMedio.fillAmount >= valorTocado)
                {
                    imaMedio.fillAmount = valorTocado;
                    TK.Ticket += (int)(valorTocado * 10);

                    SoundController.sonido1 = sonidosPuntaje[1];
                    controladorSonido.ReproducirSonido1();
                    marcando = false;
                }
            }
        }

        if (valorTocado > 0.50f && valorTocado <= 0.75f) 
        {
            if (imaAbajo.fillAmount < 1f)
            {
                IncrementaAbajo();
            }
            else if (imaMedio.fillAmount < 1f)
            {
                IncrementaMedio();
            }
            else if (imaArriba.fillAmount < 1f)
            {
                IncrementaTresCuarto();
                if (imaTresCuarto.fillAmount >= valorTocado)
                {
                    imaTresCuarto.fillAmount = valorTocado;
                    TK.Ticket += (int)(valorTocado * 10);

                    SoundController.sonido1 = sonidosPuntaje[2];
                    controladorSonido.ReproducirSonido1();
                    marcando = false;
                }
            }
        }
        
        if (valorTocado > 0.75f && valorTocado < 0.98f) 
        {
            if (imaAbajo.fillAmount < 1f)
            {
                IncrementaAbajo();
            }
            else if (imaMedio.fillAmount < 1f)
            {
                IncrementaMedio();
            }
            else if (imaTresCuarto.fillAmount < 1f )
            {
                IncrementaTresCuarto();
            }
            else if (imaArriba.fillAmount < 1f )
            {
                IncrementaArriba();
                if (imaArriba.fillAmount >= valorTocado)
                {
                    imaArriba.fillAmount = valorTocado;
                    TK.Ticket += (int)(valorTocado * 10);

                    SoundController.sonido1 = sonidosPuntaje[3];
                    controladorSonido.ReproducirSonido1();
                    marcando = false;
                }
            }
        }

        if (valorTocado >= 0.98f) 
        {
            if (imaAbajo.fillAmount < 1f)
            {
                IncrementaAbajo();
            }
            else if (imaMedio.fillAmount < 1f)
            {
                IncrementaMedio();
            }
            else if (imaTresCuarto.fillAmount < 1f)
            {
                IncrementaTresCuarto();
            }else if (imaArriba.fillAmount < 1f)
            {
                
                IncrementaArriba();
                if (imaArriba.fillAmount >= valorTocado)
                {
                    imaArriba.fillAmount = valorTocado;
                    TK.Ticket += (int) (valorTocado * 10);

                    SoundController.sonido1 = sonidoCampana;
                    controladorSonido.ReproducirSonido1();
                    marcando = false;
                }
            }
        }

        controladorSonido.ReproducirSonido1();
    }

    private void IncrementaAbajo()
    {
        imaAbajo.fillAmount += Time.deltaTime * velocidad;
        uiminigame.PublicarTextos("", 0, (int) (valorTocado * 10), 0, false, false, true, false);
    }
    private void IncrementaMedio()
    {
        imaMedio.fillAmount += Time.deltaTime * velocidad;
        uiminigame.PublicarTextos("", 0, (int)(valorTocado * 10), 0, false, false, true, false);

    }
    private void IncrementaTresCuarto()
    {
        imaTresCuarto.fillAmount += Time.deltaTime * velocidad;
        uiminigame.PublicarTextos("", 0, (int)(valorTocado * 10), 0, false, false, true, false);

    }
    private void IncrementaArriba()
    {
        imaArriba.fillAmount += Time.deltaTime * velocidad;
        uiminigame.PublicarTextos("", 0, (int)(valorTocado * 10), 0, false, false, true, false);

    }
}
