using UnityEngine;

public class Municion : MonoBehaviour {

    public float velocidadDisparo;
    public bool activa = false;

    void Update ()
    {
        if (activa) { 
            transform.Translate(Vector3.forward  * velocidadDisparo);//la municion sale disparada hacia adelante
        }
    }
}
