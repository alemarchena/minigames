using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscenaTiroDespuesBorrar : MonoBehaviour {

    
	// Use this for initialization
	public void AbrirTargetShooting() {
        SceneManager.LoadSceneAsync("TargetShootingMinigame");		
	}

    public void AbrirGarra()
    {
        SceneManager.LoadSceneAsync("CatchBugs");
    }
    public void MenuMain()
    {
        SceneManager.LoadSceneAsync("mainscene");
    }
}
