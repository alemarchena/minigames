using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disparando : MonoBehaviour {

    Animator anim;
    
    void Start () {
        anim = GetComponent<Animator>();
    }
	
    public void PersonajeDispara()
    {
        anim.SetTrigger("Disparando");
    }
}
