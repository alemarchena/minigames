using UnityEngine;

public class MoldeBlanco : MonoBehaviour {

    [HideInInspector] public AudioClip sonidoImpacto;
    private bool tocoMunicion = false;
    private InfoBlancos infoBlancos;
    [Tooltip("monedas obtenidas al impactar un blanco correcto")]
    [HideInInspector] public int monedasPorImpacto = 0;
    /// <summary>
    /// Multiplica el valor del blanco segun la rueda en donde esta el molde
    /// </summary>
    [Tooltip("Multiplica el valor del blanco por este valor")]
    public int multiplicadorMonedas = 1;
    private SpriteRenderer SR;
    private SoundController controladorSonido;
    private TargetShootingController controlador;

    private void Awake()
    {
        controlador = FindObjectOfType<TargetShootingController>();
        controladorSonido = FindObjectOfType<SoundController>();
        infoBlancos = FindObjectOfType<InfoBlancos>();
        SR = GetComponent<SpriteRenderer>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == infoBlancos.tagMunicion) //al colisionar verifica si es un blanco valido
        {
            other.GetComponent<Municion>().activa = false;
            other.gameObject.SetActive(false);
            if (sonidoImpacto)
            {
                SoundController.sonido1 = sonidoImpacto;
                controladorSonido.ReproducirSonido1();
            }
            tocoMunicion = true;
        }
       
        if (tocoMunicion)
        {
            if (transform.tag == infoBlancos.tagBlancoCorrecto)
            {
                controlador.TK.Ticket += (monedasPorImpacto * multiplicadorMonedas); //modifica los puntos al juego
                controlador.TK.Aciertos += 1;
            }
            else {
                controlador.TK.Desaciertos += 1;
            }
            controlador.uiminigame.PublicarTextos("", controlador.TK.Ticket, controlador.TK.Aciertos, controlador.TK.Desaciertos);
            SR.enabled = false;
            infoBlancos.VoltearBlanco(gameObject);
        }
        tocoMunicion = false;
    }
}
