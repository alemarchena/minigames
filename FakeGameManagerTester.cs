using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeGameManagerTester : MonoBehaviour
{
    public bool ShouldBePlaying { get; set; }

    [SerializeField]
    Minigame whoToTest = null;

    private void Start()
    {
        Minigame.minigameEnded += MinigameEnded;
        ShouldBePlaying = true;
        StartCoroutine(TestMinigame());
    }

    IEnumerator TestMinigame()
    {
        yield return whoToTest.TransitionToStart();// Intro

        while (ShouldBePlaying)
        {
            whoToTest.UpdateMinigame(Time.deltaTime); // Update
            yield return null;
        }

        // Minigame ended. Start outro.
        yield return whoToTest.TransitionToEnd();
    }

    void Update()
    {
        whoToTest.UpdateMinigame(Time.deltaTime);
    }

    public void MinigameEnded()
    {
        ShouldBePlaying = false;
    }
}
