using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    [SerializeField]
    private bool sentidoRuedaALaDerecha = false;
    private InfoBlancos infoBlancos;
    private int indiceBlanco;
    
    private bool cambiaSentidoBlanco = false;
    //private Vector3 vecCorregido;
    private MoldeBlanco molde;
    private void Start()
    {
        infoBlancos = FindObjectOfType<InfoBlancos>();
        //vecCorregido = new Vector3(0.055f, 0f, 0f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<MoldeBlanco>())
        {
            molde = other.GetComponent<MoldeBlanco>();
            indiceBlanco =  infoBlancos.AleatorioDeBlancos();
            infoBlancos.AsignaSpriteAlBlanco(other.GetComponent<SpriteRenderer>(), indiceBlanco);
            infoBlancos.AsignaSonidoBlanco(molde,indiceBlanco);
            infoBlancos.AsignaTagBlanco(other.transform, indiceBlanco);
            infoBlancos.AsignaMonedasBlanco(molde, indiceBlanco);

            if (sentidoRuedaALaDerecha)
            {
                other.transform.Rotate(0f, 180f, 0f);
            }
            else
            {
                other.transform.Rotate(0f, 0f, 0f);
            }
        }
    }
}
