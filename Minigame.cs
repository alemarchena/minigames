using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase encargada de definir los estados de un minijuego y la información que ellos devuelven.
/// </summary>
public abstract class Minigame : MonoBehaviour
{
    protected float deltaTime;
    public float DeltaTime
    {
        get { return deltaTime; }
    }

    public static Action minigameEnded = null;

    protected int coins = 0;
    protected int level = 0;
    protected int[] coinsPerLevel;

    /// <summary>
    /// Llamado desde el GameManager para iniciar la transición al START.
    /// </summary>
    public IEnumerator TransitionToStart()
    {
        yield return BeginStartRoutine();
        StartMinigame();
    }

    /// <summary>
    /// En caso de necesitar una rutina de inicio
    /// </summary>
    protected virtual IEnumerator BeginStartRoutine() // #1
    {
        yield return null;
    }

    /// <summary>
    /// Empezar la logica del minijuego aqui.
    /// </summary>
    protected abstract void StartMinigame();// #2

    /// <summary>
    /// Llamado cada frame desde el Game Manager
    /// </summary>
    /// <param name="scaledDeltaTime">El tiempo que tardo el juego en calcular el ultimo frame</param>
    public virtual void UpdateMinigame(float scaledDeltaTime) // #3
    {
        this.deltaTime = scaledDeltaTime;
    }

    /// <summary>
    /// Llamar a este metodo cuando su minijuego haya terminado.
    /// </summary>
    protected void EndMinigame() // #4
    {
        if (null != minigameEnded)
            minigameEnded.Invoke();
    }

    /// <summary>
    /// Llamado desde el GameManager para iniciar la transición al END.
    /// </summary>
    public virtual IEnumerator TransitionToEnd() // #5
    {
        yield return null;
    }
}
