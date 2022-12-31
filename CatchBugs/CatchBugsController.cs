using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchBugsController : Minigame
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

    [Space]
    public float limiteXizq = 0;
    public float limiteXder = 0;
    public float limiteZizq = 0;
    public float limiteZder = 0;

    public static int Evolucion = 0;

    [SerializeField] Garra garra;
    public string TagGarra;
    [Space]
    public string TagBicho;
    public static bool activaSonidoBichos = true;
    [Space]
    /// <summary>
    /// Lista de objetos bichos del juego.
    /// </summary>
    [SerializeField] List<GameObject> bichos;

    /// <summary>
    /// Tiempo maximo que tarda en moverse un bicho
    /// </summary>
    [Tooltip("Tiempo minimo que tarda en moverse un bicho")]
    public float tiempoMinMoverBicho = 0;

    /// <summary>
    /// Tiempo maximo que tarda en moverse un bicho
    /// </summary>
    [Tooltip("Tiempo maximo que tarda en moverse un bicho")]
    public float tiempoMaxMoverBicho = 0;

    /// <summary>
    /// Fuerza maxima con la que se mueve un bicho
    /// </summary>
    [Tooltip("Velocidad maxima con la que se mueve un bicho")]

    public float velocidadMaxMovimiento;
    /// <summary>
    /// Fuerza minima con la que se mueve un bicho
    /// </summary>
    [Tooltip("Velocidad minima con la que se mueve un bicho")]
    public float velocidadMinMovimiento;

    /// <summary>
    /// A que distancia se detiene el bicho antes de llegar al objetivo
    /// </summary>
    [Tooltip("A que distancia se detiene el bicho antes de llegar al objetivo")]
    public float distanciaDelObjetivo;

    /// <summary>
    /// Velocidad con la que rota el bicho
    /// </summary>
    [Tooltip("velocidad maxima con la que rota el bicho")]
    public float velocidadRotacion;

    /// <summary>
    /// Tiempo transcurrido desde que se movio el ultimo bicho
    /// </summary>
    private float timerBicho = 0;

    /// <summary>
    /// A que tiempo aleatorio se movera el proximo bicho
    /// </summary>
    private float tiempoProximoSalidaBicho = 0;

    /// <summary>
    /// bicho que le toca moverse segun el orden de la lista de bichos
    /// </summary>
    private int bichoAmover;
    //private bool saltoDeBichos = false;
    public bool jugando = false;
    [SerializeField] private Vector3 vectorSalto;
    

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
        UpdateModificado(deltaTimeModificado);
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
        IniciaJuego();
        RestableceJuego();
    }

    private void UpdateModificado(float deltaTimeModificado)
    {
        if (jugando)
        {
            if (tie.SuperoDeltaTime(tiempoProximoSalidaBicho, deltaTimeModificado))
            {
                timerBicho = 0;
                bichoAmover = Random.Range(0, bichos.Count);
                bichos[bichoAmover].GetComponent<Bicho>().bichoEnMovimiento = true;
                tiempoProximoSalidaBicho = RandomTiempoBicho();
                bichos[bichoAmover].GetComponent<Bicho>().CambiarParametrosMovimiento();
            }

            for (int a = 0; a < bichos.Count; a++)
            {
                bichos[a].GetComponent<Bicho>().UpdateBichos(deltaTimeModificado);
            }
        }
       

        if (Input.GetKey(KeyCode.Mouse0) && jugando)
        {
            garra.GarraYoTeElijo();
            TerminaMiniJuego();
        }
    }

    public void IniciaJuego()
    {
        activaSonidoBichos = true;
        jugando = true;
    }

    public void TerminaMiniJuego()
    {
        
        StartCoroutine(retrasaFinJuego());
    }

    IEnumerator retrasaFinJuego() {
        yield return new WaitForSeconds(1f);

        yield return new WaitForSeconds(10f);
        jugando = false;

    }
    private float RandomTiempoBicho()
    {
        return Random.Range(tiempoMinMoverBicho, tiempoMaxMoverBicho);
    }

    private void RestableceJuego()
    {
        Evolucion = 0;
        TK.Ticket = 0;
        uiminigame.PublicarTextos(tie.VerTiempo, TK.Ticket, TK.Aciertos, 0);
    }
}
