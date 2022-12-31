using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhacamoleController : Minigame
{
    [SerializeField] KeyCode queToco;

    [Space]
    [Header("Ticketera")]
    public float tiempoDeJuegoSeg = 20f;
    public int valorAcierto = 1;
    public int valorDesAcierto = 1;

    [Space]
    [Header("Controladores")]
    [Tooltip("Muestra los valores de Tickets, aciertos y desaciertos del minijuego")]
    public UIminijuego uiminigame;
    [Tooltip("Permite manipular los valores  Tickets, aciertos y desaciertos del minijuego")]
    public Ticketera TK;
    [Tooltip("Permite manipular los valores de tiempo del minijuego")]
    public Tiempo tie;

    [Space]
    [Header("Sonidos")]
    public List<AudioClip> sonidoSale;
    public List<AudioClip> sonidoEntra;
    [SerializeField] List<AudioClip> sonidoMartillo;
    [SerializeField] AudioClip sonidoBichoGolpeado;
    [SerializeField] AudioClip sonidoAgujeroGolpeado;
    [Space]
    [Header("Valores Posiciones invisibles")]
    [SerializeField] List<Transform> posicionesMartillo;
    [Space]
    [Header("Martillo")]
    [SerializeField] Transform prefabMartillo;
    [Header("Bichos")]
    [SerializeField] List<Transform> personajes;
    public List<Animator> animaciones;

    [SerializeField] List<Transform> prefabsCara;
    public List<MeshRenderer> meshCaras;

    [Header("Bicho Golpeado")]
    [SerializeField] Transform transCaraGolpeado;
    [SerializeField] MeshRenderer meshCaraGolpeado;
    [Space]
    [Header("Valores Personaje")]
    /// <summary>
    /// Velocidad con la que sube el personaje
    /// </summary>
    

    [Tooltip("Cada cuanto minimo tiempo aparece el personaje")]
    public float tiempoLimiteMin;
    [Tooltip("Cada cuanto maximo tiempo aparece el personaje")]
    public float tiempoLimiteMax;
    public float esperaSonidoBajada = 0.1f;
    [SerializeField] float tiempoDesapareceGolpeado = 0.5f;
    [SerializeField] private float[] probabilidadMovimiento = { 0.167f, 0.166f, 0.167f, 0.166f, 0.167f, 0.166f };
    
    //Controladores
    private SoundController controladorSonido;
    public Probabilidad pro;
    private Movimientos mov;
    private UtilizaClic clic;
    

    private float velocidad;
    private float tiempoLimite;
    private int posicionIndice=0;
    private int posicionIndiceAnt=0;
    private float posicion=0;
    private bool jugando = false;
    private bool controlIncremento=true;
    private bool controlDesacierto = true;
    private bool controlInput = true;

    public float[] probabilidadEntraSale = { 0.33f, 0.33f, 0.33f };
    [SerializeField] private float[] probabilidadMartillo = { 0.5f, 0.5f};
    private Martillo martilloMov;

    public GameObject punchParticles;
    ///--------------------------- Contrato del Juego ----------------
    /// <summary>
    /// The BeginStartRoutine: Inicia la creacion de la lista de blancos and waits for the initial momentum
    /// </summary>
    /// <returns>The <see cref="IEnumerator"/></returns>
    protected override IEnumerator BeginStartRoutine()
    {
        

        yield return null;
    }

    /// <summary>
    /// Update method (with scaled own delta time)
    /// </summary>
    /// <param name="deltaTimeModificado">The deltaTime<see cref="float"/></param>
    public override void UpdateMinigame(float deltaTimeModificado)
    {
        base.UpdateMinigame(deltaTimeModificado);// Importante (o sino hagan this.deltaTime = deltaTime)  <--- Mala idea.

        UpdateMinijuego( deltaTimeModificado);


    }

    /// <summary>
    /// ..... ver si lleva transicion
    /// </summary>
    /// <returns>The <see cref="IEnumerator"/></returns>
    public override IEnumerator TransitionToEnd()
    {
       
        yield return null;
    }

    /// <summary>
    /// Comienza el minijuego
    /// </summary>
    protected override void StartMinigame()
    {
        StartMiniJuego();
    }

    //----------------------Metodos del minijuego --------------------------
    private void StartMiniJuego ()
    {
        pro= FindObjectOfType<Probabilidad>();
        mov= FindObjectOfType<Movimientos>();
        clic = FindObjectOfType<UtilizaClic>();

        martilloMov = prefabMartillo.GetComponent<Martillo>();
        controladorSonido = FindObjectOfType<SoundController>(); //sonido 1 : saliendo y martillo - sonido 2 : golpeado y bajando
        InicializaMinijuego();
    }

    private void ObtieneAnimatorsYmesh()
    {
        for (int a = 0; a < personajes.Count; a++)
        {
            animaciones.Add(personajes[a].GetComponent<Animator>());
            meshCaras.Add(prefabsCara[a].GetComponent<MeshRenderer>());
        }
    }

	// Update is called once per frame
	private void UpdateMinijuego(float timeDT)
    {
        if (tie.SuperoTiempoHHMMSS(0f, 0f, tiempoDeJuegoSeg))
        {
            TerminaMinijuego();
        }

        if (jugando)
        {
            if (uiminigame)
            {
                uiminigame.PublicarTextos(tie.VerTiempo, TK.Ticket, TK.Aciertos, TK.Desaciertos);
            }

            if (tie.SuperoDeltaTime(tiempoLimite, Time.deltaTime))
            {
                EligeOrigenRandom();//Elige una posicion e ubica al bicho
            }

            if (Input.GetKeyDown(queToco))
            {
                StartCoroutine(VerificaInput());
            }
        }
    }

    private void InicializaMinijuego()
    {
        ObtieneAnimatorsYmesh();
        TK.Ticket = 0;
        TK.Aciertos= 0;
        TK.Desaciertos= 0;
        jugando = true;
    }

    private void TerminaMinijuego() {

        jugando = false;
    }

   

    private void GolpearBicho()
    {
        if (controladorSonido)
        {
            SoundController.sonido2 = sonidoBichoGolpeado;
            controladorSonido.ReproducirSonido2();

            SoundController.sonido1 = sonidoMartillo[(int)pro.DameAleatorio(probabilidadMartillo)];
            controladorSonido.ReproducirSonido1();
        }
        StartCoroutine( MuestraBichoGolpeado());

        martilloMov.Golpea();
    }

    IEnumerator MuestraBichoGolpeado()
    {
        int posicionSerializada = posicionIndice;
        //deshabilita cara normal y habilita cara golpeado, haciendola hijo del personaje
        meshCaras[posicionIndice].enabled = false;
        transCaraGolpeado.position = prefabsCara[posicionIndice].position;
        transCaraGolpeado.SetParent(prefabsCara[posicionIndice]);
        meshCaraGolpeado.enabled = true;


        yield return new WaitForSeconds(tiempoDesapareceGolpeado);
        //habilita cara normal y deshabilita cara golpeado
        meshCaraGolpeado.enabled = false;
        meshCaras[posicionSerializada].enabled = true;
        transCaraGolpeado.SetParent(null);

    }

    public void GolpeaMesa()
    {
        SoundController.sonido2 = sonidoAgujeroGolpeado;
        controladorSonido.ReproducirSonido2();
    }

    private void EligeOrigenRandom()
    {
        posicion = pro.DameAleatorio(probabilidadMovimiento);
        posicionIndice = (int)posicion;
       
        posicionIndiceAnt = posicionIndice;
        
        //mueve al bicho aleatoriamente
        animaciones[posicionIndice].SetTrigger("mueve");

        prefabMartillo.position = posicionesMartillo[(int)posicion].position;
        if (posicionIndice <= 2)
        {
            martilloMov.derecha = false;
        }
        else
        {
            martilloMov.derecha = true;
        }

        tiempoLimite = Random.Range(tiempoLimiteMin, tiempoLimiteMax);
        martilloMov.EstableceAnguloReposo();
    }

    // ------------------------------------------ Verificadores ------------------------------------
    IEnumerator VerificaInput()
    {
        if (controlInput == true)
        {
            clic.VerificaPosicionClicMouse();

            if (clic.objetoAlcanzado.tag == "Bicho")
            {
                GolpearBicho();
                Vector3 maxPoint = new Vector3(clic.objetoAlcanzado.transform.position.x, 
                    clic.objetoAlcanzado.gameObject.transform.position.y + (clic.objetoAlcanzado.gameObject.GetComponent<CapsuleCollider>().bounds.size.y)/2,
                    clic.objetoAlcanzado.transform.position.z);
                Instantiate(punchParticles, maxPoint, Quaternion.LookRotation(Vector3.up));
                StartCoroutine(SumaTicket());
            }
            else
            {
                GolpeaMesa();
                StartCoroutine(SumaDesacierto());

            }
        }
        controlInput = false;
        yield return new WaitForSeconds(0.5f);
        controlInput = true;
    }

    IEnumerator SumaTicket()
    {
        if (controlIncremento == true)
        {
            TK.Aciertos += 1;
            TK.Ticket = TK.Aciertos * TK.ValorAcierto;
        }
        controlIncremento = false;
        yield return new WaitForSeconds(1f);
        controlIncremento = true;
    }

    IEnumerator SumaDesacierto()
    {
        if (controlDesacierto == true)
        {
            TK.Desaciertos += 1;
            TK.Desaciertos += 1;
        }
        controlDesacierto = false;
        yield return new WaitForSeconds(1f);
        controlDesacierto = true;
    }


    private void ImplementaTicketera()
    {
        TK.ValorAcierto = valorAcierto;
        TK.ValorDesacierto = valorDesAcierto;
    }
}
