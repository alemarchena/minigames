///—————————————————————–
///   File: LogSpinner.cs
///   Author: Luciano Donati
///   me@lucianodonati.com	www.lucianodonati.com
///   Last edit: 20-May-18
///   Description: 
///—————————————————————–

using System.Collections;
using UnityEngine;
/// <summary>
/// Defines the <see cref="LogSpinner" />
/// </summary>
public class LogSpinner : MonoBehaviour
{
    /// <summary>
    /// Defines the minigame
    /// </summary>
    [SerializeField]
    private KnifeLogMinigame minigame = null;

    /// <summary>
    /// Defines the accelerationDuration
    /// </summary>
    [SerializeField]
    private float accelerationDuration = 1;

    /// <summary>
    /// Defines the accelerationCurve
    /// </summary>
    [SerializeField]
    private AnimationCurve accelerationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    /// <summary>
    /// Defines the spinSpeed
    /// </summary>
    [SerializeField]
    private float spinSpeed = 1;

    public IEnumerator StartSpinning()
    {
        yield return StartCoroutine(AccelerateTowardsSpeed(true));
    }

    /// <summary>
    /// The Spin
    /// </summary>
    public void Spin()
    {
        StartCoroutine(Spinner());
    }

    /// <summary>
    /// The Spinner
    /// </summary>
    /// <returns>The <see cref="IEnumerator"/></returns>
    private IEnumerator Spinner()
    {
        while (true)
        {
            transform.Rotate(Vector3.forward * (spinSpeed * minigame.DeltaTime));
            yield return null;
        }
    }

    /// <summary>
    /// The AccelerateTowardsSpeed
    /// </summary>
    /// <param name="forward">The <see cref="bool"/></param>
    /// <returns>The <see cref="IEnumerator"/></returns>
    private IEnumerator AccelerateTowardsSpeed(bool forward)
    {
        float timePassed = 0;
        while (timePassed < accelerationDuration)
        {
            float evaluate = accelerationCurve.Evaluate((forward ? timePassed : 1 - timePassed));
            transform.Rotate(Vector3.forward * (evaluate * spinSpeed * Time.smoothDeltaTime));
            yield return null;
            timePassed += minigame.DeltaTime;
        }
    }

    /// <summary>
    /// The StopSpining
    /// </summary>
    public IEnumerator StopSpining()
    {
        StopCoroutine(Spinner());
        yield return StartCoroutine(AccelerateTowardsSpeed(false));
    }
}
