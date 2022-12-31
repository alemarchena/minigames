using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementFindTheBug : MonoBehaviour {

    [SerializeField] AudioClip sonidoGarra;
    private SoundController controladorSonido;

    [SerializeField] Animator animaciones;
    public bool moveteP1AP2=false;
    public bool moveteP1AP2IZQ=false;
    public bool moveteP1AP3=false;
    public bool moveteP1AP3IZQ=false;
    public bool moveteP2AP1=false;
    public bool moveteP2AP1DER=false;
    public bool moveteP2AP3=false;
    public bool moveteP3AP1=false;
    public bool moveteP3AP1DER=false;
    public bool moveteP3AP2=false;

    public bool destapar1=false;
    public bool destapar2=false;
    public bool destapar3 = false;
    public bool quieto1=false;
    public bool quieto2=false;
    public bool quieto3=false;

    public Transform bicho = null;


    public Renderer colorTextura;
    [HideInInspector] public Color colorInicial;

    private void Start()
    {
        colorInicial = colorTextura.material.color;
        controladorSonido = FindObjectOfType<SoundController>();
    }
    private void Update()
    {
        //Movimientos de Posicion 1
        if (moveteP1AP2 == true)
        {
            MoverXanimacion("P1AP2");
            moveteP1AP2 = false;
        }
        if (moveteP1AP2IZQ == true)
        {
            MoverXanimacion("P1AP2IZQ");
            moveteP1AP2IZQ = false;
        }
        if (moveteP1AP3 == true)
        {
            MoverXanimacion("P1AP3");
            moveteP1AP3 = false;
        }
        if (moveteP1AP3IZQ == true)
        {
            MoverXanimacion("P1AP3IZQ");
            moveteP1AP3IZQ = false;
        }

        //Movimientos de Posicion 2
        if (moveteP2AP1 == true)
        {
            MoverXanimacion("P2AP1");
            moveteP2AP1 = false;
        }
        if (moveteP2AP1DER == true)
        {
            MoverXanimacion("P2AP1DER");
            moveteP2AP1DER = false;
        }
        if (moveteP2AP3 == true)
        {
            MoverXanimacion("P2AP3");
            moveteP2AP3 = false;
        }


        //Movimientos de Posicion 3
        if (moveteP3AP1 == true)
        {
            MoverXanimacion("P3AP1");
            moveteP3AP1 = false;
        }
        if (moveteP3AP1DER == true)
        {
            MoverXanimacion("P3AP1DER");
            moveteP3AP1DER = false;
        }
        if (moveteP3AP2 == true)
        {
            MoverXanimacion("P3AP2");
            moveteP3AP2 = false;
        }


        if (destapar1) {
            MoverXanimacion("DESTAPA1");
            destapar1 = false;
        }
        if (destapar2)
        {
            MoverXanimacion("DESTAPA2");
            destapar2= false;
        }
        if (destapar3)
        {
            MoverXanimacion("DESTAPA3");
            destapar3= false;
        }
        if (quieto1)
        {
            MoverXanimacion("QUIETO1");
            quieto1 = false;
        }
        if (quieto2)
        {
            MoverXanimacion("QUIETO2");
            quieto2 = false;
        }
        if (quieto3)
        {
            MoverXanimacion("QUIETO3");
            quieto3 = false;
        }

        
    }

    

    public void MoverXanimacion(string triggerName)
    {
        animaciones.SetTrigger(triggerName);
        SoundController.sonido2 = sonidoGarra;
        controladorSonido.ReproducirSonido2();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bicho")
        {
            FindTheBugController.tapar = true;
            other.transform.SetParent(transform);
            bicho = other.transform; //asigna el bicho para saber que garra lo tiene
            StartCoroutine(DesapareceBicho(other.transform));
        }
    }

    public void DesaparecerBicho(Transform objeto) {
        StartCoroutine(DesapareceBicho(objeto));
    }


    IEnumerator DesapareceBicho(Transform other) {

        yield return new WaitForSeconds(other.GetComponent<Bichin>().tiempoDesapareceBicho);
        other.GetComponent<Transform>().gameObject.SetActive(false);
        other.GetComponent<MeshRenderer>().enabled = false;
        MeshRenderer[] comp = other.GetComponentsInChildren<MeshRenderer>();
        for (int a = 0; a < comp.Length; a++)
        {
            comp[a].enabled = false;
        }
        StartCoroutine(EntroElBicho());
    }

    IEnumerator EntroElBicho() {
        yield return new WaitForSeconds(1f);
        FindTheBugController.entroElBicho = true;
    }

    public void PermiteTocar(int garra)
    {

        if (garra == 1)
        {
            FindTheBugController.permitetocar1 = true;
        }
        if (garra == 2)
        {
            FindTheBugController.permitetocar2 = true;
        }
        if (garra == 3)
        {
            FindTheBugController.permitetocar3 = true;
        }
    }
    public void NoPermiteTocar(int garra)
    {

        if (garra == 1)
        {
            FindTheBugController.permitetocar1 = false;
        }
        if (garra == 2)
        {
            FindTheBugController.permitetocar2 = false;
        }
        if (garra == 3)
        {
            FindTheBugController.permitetocar3 = false;
        }
    }
}
