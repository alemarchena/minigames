using UnityEngine;
using UnityEngine.UI;

public class UIminijuego : MonoBehaviour {

    [SerializeField] Text textoTiempo;
    [SerializeField] Text textoTicket;
    [SerializeField] Text textoAciertos;
    [SerializeField] Text textoDesaciertos;

    [SerializeField] string stringTextoTiempo;
    [SerializeField] string stringTicket;
    [SerializeField] string stringAciertos;
    [SerializeField] string stringDesaciertos;


    /// <summary>
    /// Publica tiempo transcurrido, tickets ganados, aciertos del minijuego,desaciertos del minijuego
    /// </summary>
    /// <param name="tiempo"></param>
    /// <param name="tickets"></param>
    /// <param name="aciertos"></param>
    /// <param name="desaciertos"></param>
    public void PublicarTextos(string tiempo,int tickets,int aciertos,int desaciertos)
    {
        textoTiempo.text = stringTextoTiempo + " " + tiempo.ToString();
        textoTicket.text = stringTicket + " " + tickets.ToString();
        textoAciertos.text = stringAciertos + " " + aciertos.ToString();
        textoDesaciertos.text = stringDesaciertos + " " + desaciertos.ToString();
    }
    /// <summary>
    /// Muestra el tiempo transcurrido, los tickets acumulados, aciertos y desaciertos. Los valores bool = true validan si se usan los cuatro primeros parametros al mostrar.
    /// </summary>
    /// <param name="tiempo"></param>
    /// <param name="tickets"></param>
    /// <param name="aciertos"></param>
    /// <param name="desaciertos"></param>
    /// <param name="usaTiempo"></param>
    /// <param name="usaTickets"></param>
    /// <param name="usaAciertos"></param>
    /// <param name="usaDesaciertos"></param>
    public void PublicarTextos(string tiempo, int tickets, int aciertos, int desaciertos,bool usaTiempo,bool usaTickets,bool usaAciertos,bool usaDesaciertos)
    {
        if(usaTiempo)
            textoTiempo.text = stringTextoTiempo + " " + tiempo.ToString();
        if (usaTickets)
            textoTicket.text = stringTicket + " " + tickets.ToString();
        if(usaAciertos)
            textoAciertos.text = stringAciertos + " " + aciertos.ToString();
        if(usaDesaciertos)
            textoDesaciertos.text = stringDesaciertos + " " + desaciertos.ToString();
    }
}
