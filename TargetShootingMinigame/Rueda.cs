using System.Collections;
using UnityEngine;

public class Rueda : MonoBehaviour {

    [SerializeField]
    private TargetShootingController targetControl= null;
    /// <summary>
    /// Cambia el sentido de rotacion de la rueda
    /// </summary>
    [Tooltip("Cambia el sentido de rotacion de la rueda")]
    public bool sentidoAlaDerecha;

    /// <summary>
    /// Velocidad de Rotacion de la rueda
    /// </summary>
    [Tooltip("Velocidad de Rotacion de la rueda")]
    public float velocidad;
    private int inversor;
    private InfoBlancos infoBlancos;
    private InstanciaDeBlancos instanciaDeBlancos;
    private SoundController controladorSonido;
    private TargetShootingController controlador;

    private void Awake()
    {
        controladorSonido = FindObjectOfType<SoundController>();
        infoBlancos = FindObjectOfType<InfoBlancos>();
        controlador = FindObjectOfType<TargetShootingController>();
    }

    public void MoverRueda()
    {
        StartCoroutine(MueveRuedasUpdateModificado());
    }

    IEnumerator MueveRuedasUpdateModificado()
    {
        while (true) { 
            inversor = (sentidoAlaDerecha == true) ? -1 : 1;
            //transform.Rotate(Vector3.up * inversor, velocidad * deltaTimeModificado);
            transform.Rotate(Vector3.up * inversor, velocidad * targetControl.DeltaTime);
            yield return null;
        }
    }

    public void DetenerRueda()
    {
        StartCoroutine(DetenerRuedaUpdateModificado());
    }

    IEnumerator DetenerRuedaUpdateModificado() {
        StopCoroutine(MueveRuedasUpdateModificado());
        yield return null;

    }

    private void OnTriggerEnter(Collider other)
    {
       
        if (other.tag == infoBlancos.tagMunicion)
        {
            if (infoBlancos.sonidoImpactoRuedas)
            {
                if (!controladorSonido.ASSonido1.isPlaying) { 
                    SoundController.sonido1= infoBlancos.sonidoImpactoRuedas;
                    controladorSonido.ReproducirSonido1();
                    controlador.TK.Desaciertos += 1;
                    controlador.uiminigame.PublicarTextos("", controlador.TK.Ticket, controlador.TK.Aciertos, controlador.TK.Desaciertos);
                }
            }
            other.GetComponent<Municion>().activa = false;
            other.gameObject.SetActive(false);

        }
    }

}
