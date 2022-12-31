/// <summary>
/// Autor: Ale Marchena
/// Fecha: 25 de Julio de 2018
/// Mendoza - Argentina
/// alemarchena@gmail.com
/// </summary>
using UnityEngine;
/// <summary>
/// Temporiza el valor de tiempo transcurrido en el juego y lo devuelve como:
/// 'Horas, minutos y segundos en formatos de texto', 
/// 'Horas, minutos y segundos en forma individual',
/// 'Horas, minutos y segundos en formato Vector3'
/// </summary>



public class Tiempo : MonoBehaviour {
    [TextArea]
    public string Implementa;
    
    /// <summary>
    /// Tiempo transcurrido en el juego
    /// </summary>
    private float tiempo;
    private float tiempoHora;
    private float tiempoMinuto;
    private float tiempoSegundo;
    /// <summary>
    /// Hora, minuto, segundo transcurrido en el juego
    /// </summary>
    private Vector3 hhmmss;

    /// <summary>
    /// Hora transcurrida en formato texto
    /// </summary>
    private string horas;
    /// <summary>
    /// Minuto transcurrido en formato texto
    /// </summary>
    private string minutos;
    /// <summary>
    /// Segundo transcurrido en formato texto
    /// </summary>
    private string segundos;

    private float contador = 0;


    private void Update()
    {
        
        tiempo += Time.deltaTime;
        tiempoSegundo = tiempo % 60;
        segundos = (tiempo % 60).ToString("00");

        tiempoMinuto = tiempo / 60;
        minutos = Mathf.Floor(tiempo / 60).ToString("00");

        tiempoHora = tiempo / 60;
        horas = Mathf.Floor(tiempoHora / 60).ToString("00");

    }

    /// <summary>
    /// Devuelve un texto con el tiempo transcurrido en horas, minutos y segundos
    /// </summary>
    public string VerTiempo
    {
        get
        {
            return horas + ":" + minutos + ":" + segundos;
        }
    }
    /// <summary>
    /// Devuelve un float con la cantidad de horas transcurridas
    /// </summary>
    public float getHoras
    {
        get
        {
            return tiempoHora;
        }
    }
    /// <summary>
    /// Devuelve un float con la cantidad de horas transcurridas
    /// </summary>
    public float getMinutos
    {
        get
        {
            return tiempoMinuto;
        }
    }
    /// <summary>
    /// Devuelve un float con la cantidad de horas transcurridas
    /// </summary>
    public float getSegundos
    {
        get
        {
            return tiempoSegundo;
        }
    }
    /// <summary>
    /// Devuelve en un vector las horas, minutos, segundos transcurridos
    /// </summary>
    public Vector3 getHHMMSS
    {
        get
        {
            hhmmss = new Vector3(tiempoHora, tiempoMinuto, tiempoSegundo);
            return hhmmss;
        }
    }

    /// <summary>
    /// Temporiza el delta time hasta el límite indicado el acumulado de delta time.
    /// </summary>
    /// <param name="deltaTimeLimite"></param>
    /// <param name="deltaTimeTranscurrido"></param>
    /// <returns></returns>
    public bool SuperoDeltaTime(float deltaTimeLimite,float deltaTimeTranscurrido)
    {
        contador += deltaTimeTranscurrido;

        if (contador >= deltaTimeLimite)
        {
            contador = 0;
            return true;
        }
        return false;
    }
    public bool SuperoTiempoHHMMSS(float limiteHs, float limiteMin, float limiteSeg)
    {
        if (tiempoHora >= limiteHs && tiempoMinuto >= limiteMin && tiempoSegundo > limiteSeg)
        {
            return true;

        }
        else {
            return false;
        }
    }
}
