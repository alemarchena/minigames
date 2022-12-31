///—————————————————————–
///   File: TargetShootingMinigame.cs
///   Author: Alejandro Marchena 
///   Nick name proposed by Luciano Donati : Gallo Viejo
///   alemarchena@gmail.com
///   Last edit: 27-May-18
///   Description: Shot to target class.
///—————————————————————–

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetShootingController : Minigame {

    [Space]
    [Header("Controladores")]
    [Tooltip("Muestra los valores de Tickets, aciertos y desaciertos del minijuego")]
    public UIminijuego uiminigame;
    [Tooltip("Permite manipular los valores  Tickets, aciertos y desaciertos del minijuego")]
    public Ticketera TK;
    [Tooltip("Permite manipular los valores de tiempo del minijuego")]
    public Tiempo tie;


    public static bool juegoTerminado = false;
    private InstanciaDeBlancos instanciaBlancos;
    [SerializeField]
    private List<Rueda> rueda;
    private Disparador disparador;
    public int disparosOriginales;
   
    ///--------------------------- Contrato del Juego ----------------
    /// <summary>
    /// The BeginStartRoutine: Inicia la creacion de la lista de blancos and waits for the initial momentum
    /// </summary>
    /// <returns>The <see cref="IEnumerator"/></returns>
    protected override IEnumerator BeginStartRoutine()
    {
        //instanciaBlancos = FindObjectOfType<InstanciaDeBlancos>();
        
        yield return null;
    }

    /// <summary>
    /// Update method (with scaled own delta time)
    /// </summary>
    /// <param name="deltaTimeModificado">The deltaTime<see cref="float"/></param>
    public override void UpdateMinigame(float deltaTimeModificado)
    {
        base.UpdateMinigame(deltaTimeModificado);// Importante (o sino hagan this.deltaTime = deltaTime)  <--- Mala idea.
        
        
        //instanciaBlancos.InstanciarBlancos();
    }

    /// <summary>
    /// ..... ver si lleva transicion
    /// </summary>
    /// <returns>The <see cref="IEnumerator"/></returns>
    public override IEnumerator TransitionToEnd()
    {
        for (int a = 0; a < rueda.Count; a++)
        {
            rueda[a].DetenerRueda();
        }
        yield return null;
    }

    /// <summary>
    /// Comienza el minijuego
    /// </summary>
    protected override void StartMinigame()
    {
        for (int a = 0; a < rueda.Count; a++)
        {
            rueda[a].MoverRueda();
        }
        
        disparador = FindObjectOfType<Disparador>();
        disparador.IniciaDisparador();
        RestableceMinijuego();
    }
    //------------------------ Metodos del Mini Juego ----------------
    ///Vida del Juego y Nivel llamados desde la clase blanco
    ///
    public void DetieneDisparos() //llama a EndMinigame
    {
        juegoTerminado = true;
        EndMinigame();//invoca el final del mini juego
    }

    private void RestableceMinijuego() {
        disparador.disparosDisponibles = disparosOriginales;
    }
}
