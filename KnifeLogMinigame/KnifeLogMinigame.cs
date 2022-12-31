///—————————————————————–
///   File: KnifeLogMinigame.cs
///   Author: Luciano Donati
///   me@lucianodonati.com	www.lucianodonati.com
///   Last edit: 23-May-18
///   Description: Knife & Log minigame class. In charge of the general management of the minigame.
///—————————————————————–

using System.Collections;
using UnityEngine;
/// <summary>
/// Defines the <see cref="KnifeLogMinigame" />
/// </summary>
public class KnifeLogMinigame : Minigame
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

    private OnKeyCallAnimation animaTiraCuchillo;

    /// <summary>
    /// Defines the spinner
    /// </summary>
    [SerializeField]
    private LogSpinner spinner = null;

    /// <summary>
    /// Defines the numberOfTries
    /// </summary>
    [SerializeField]
    private byte numberOfTries = 3;

    /// <summary>
    /// Defines the currentTry
    /// </summary>
    public static byte currentTry = 0;

    private void ReiniciaMiniJuego()
    {
        animaTiraCuchillo = FindObjectOfType<OnKeyCallAnimation>();
        animaTiraCuchillo.tieneCuchillos = true;
        currentTry = 0;
    }
    /// <summary>
    /// The BeginStartRoutine: Starts spinning the log and waits for the initial momentum
    /// </summary>
    /// <returns>The <see cref="IEnumerator"/></returns>
    protected override IEnumerator BeginStartRoutine()
    {
        yield return spinner.StartSpinning();
    }

    /// <summary>
    /// Starts with the full speed rotation and "starts" the minigame
    /// </summary>
    protected override void StartMinigame()
    {
        ReiniciaMiniJuego();
        spinner.Spin();
    }

    /// <summary>
    /// Update method (with scaled own delta time)
    /// </summary>
    /// <param name="deltaTime">The deltaTime<see cref="float"/></param>
    public override void UpdateMinigame(float scaledDeltaTime)
    {
        //Debug.Log("Herrado:" + currentTry);
        base.UpdateMinigame(scaledDeltaTime); // Importante (o sino hagan this.deltaTime = deltaTime)  <--- Mala idea.
    }

    /// <summary>
    /// Starts the deaceleration of the log
    /// </summary>
    /// <returns>The <see cref="IEnumerator"/></returns>
    public override IEnumerator TransitionToEnd()
    {
        yield return spinner.StopSpining();
    }

    /// <summary>
    /// Method called everytime we hit a knife (1 less chance to win)
    /// </summary>
    public void HitKnife()
    {
        uiminigame.PublicarTextos("", TK.Ticket, TK.Aciertos, TK.Desaciertos);
        if (currentTry == numberOfTries)
        {
            //Debug.Log("Termino");
            
            animaTiraCuchillo.tieneCuchillos = false;
            EndMinigame();
        }
    }
}
