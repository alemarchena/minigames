using System.Collections.Generic;
using UnityEngine;

public class InstanciaDeBlancos : MonoBehaviour {

    [SerializeField]
    private List<GameObject> MoldesDeblancos;

    [Space]

    public List<Transform> ruedas;
    

    [SerializeField]
    private float radio;

    /// <summary>
    /// Tiempo que tarda en posicionar cada blanco
    /// </summary>
    public float tiempo = 0;

    private InfoBlancos infoBlancos;
    private float tiempoTranscurrido = 0;
    private int orden = 0;
    private int columna = 0;
    private int avanceColumna = 0;
    private float anguloBlancoDerecha = 90;
    private float anguloBlancoIzquierda = -90;

    [HideInInspector] public List<Vector3> posicionInstancia;
    private Vector3 vectorAux;
    private bool cambiaSentidoBlanco = false;
    private float gradosCambioSentido = 0;
    private SpriteRenderer spriteRE;
    private Blanco blancoBL;
    private void Start()
    {
        infoBlancos = FindObjectOfType<InfoBlancos>();
        orden = -1; columna = 0;
        for (int a = 0; a < ruedas.Count; a++)
        {
            //segun el sentido de la rueda, es como establece la primera posicion de aparicion del molde de blancos
            if (!ruedas[a].GetComponent<Rueda>().sentidoAlaDerecha)
            {
                vectorAux = new Vector3(ruedas[a].position.x + radio, ruedas[a].position.y, ruedas[a].position.z);
            }
            else
            {
                vectorAux = new Vector3(ruedas[a].position.x + radio * -1, ruedas[a].position.y, ruedas[a].position.z);
            }
            posicionInstancia[a] = vectorAux;
        }
    }


    public void InstanciarBlancos()
    {
        tiempoTranscurrido += 1;
        if (tiempoTranscurrido >= tiempo)
        {

            tiempoTranscurrido = 0;

            for (int a = 0; a < ruedas.Count; a++)
            {
                orden = orden + 1;

                if (orden >= MoldesDeblancos.Count) { orden = 0; columna = 0; }

                //Segun el sentido de giro de la rueda y la imagen aleatoria del blanco, es como va a girar el molde del blanco
                if (ruedas[a].GetComponent<Rueda>().sentidoAlaDerecha)
                {
                    Aleatorio(columna, false);
                    gradosCambioSentido = (cambiaSentidoBlanco == true) ? 0f : 180f;
                    MoldesDeblancos[columna].transform.SetPositionAndRotation(posicionInstancia[a], Quaternion.Euler(gradosCambioSentido, 0, anguloBlancoDerecha));
                }
                else
                {
                    Aleatorio(columna, true);
                    gradosCambioSentido = (cambiaSentidoBlanco == true) ? 0f : 180f;
                    MoldesDeblancos[columna].transform.SetPositionAndRotation(posicionInstancia[a], Quaternion.Euler(gradosCambioSentido, 0, anguloBlancoIzquierda));

                }
                MoldesDeblancos[columna].transform.SetParent(ruedas[a].transform);
                columna += MoldesDeblancos.Count / ruedas.Count;//calcula que blanco va a reubicar segun la cantidad de blancos total por rueda
            }

            avanceColumna += 1; columna = avanceColumna;
            if (avanceColumna >= MoldesDeblancos.Count / ruedas.Count) { avanceColumna = 0; }
        }
    }
    
    private void Aleatorio(int columna,bool ruedaAlaDerecha)
    {
        SpriteRenderer spriteRE = MoldesDeblancos[columna].GetComponent<SpriteRenderer>();
        if (spriteRE)
            spriteRE.enabled = true;

        int indice = infoBlancos.AleatorioDeBlancos();
        infoBlancos.AsignaSpriteAlBlanco(spriteRE, indice);
        infoBlancos.AsignaTagBlanco(MoldesDeblancos[columna].transform, indice);
        infoBlancos.AsignaSonidoBlanco(MoldesDeblancos[columna].GetComponent<MoldeBlanco>(), indice);

        blancoBL = infoBlancos.prefabBlancos[indice].GetComponent<Blanco>();

        MoldesDeblancos[columna].GetComponent<MoldeBlanco>().monedasPorImpacto = blancoBL.monedasPorImpacto;

        if (blancoBL.sinSentido == false) 
        {
            bool sentidoDerecha = blancoBL.miraAlaDerecha;

            cambiaSentidoBlanco = (ruedaAlaDerecha == sentidoDerecha) ? false : true;
        }
        else {
            cambiaSentidoBlanco = false;
        }
    }
}
