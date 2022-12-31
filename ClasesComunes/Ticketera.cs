/// <summary>
/// Autor: Ale Marchena
/// Fecha: 25 de Julio de 2018
/// Mendoza - Argentina
/// alemarchena@gmail.com
/// </summary>
using UnityEngine;
public class Ticketera : MonoBehaviour {
    [TextArea]
    public string Implementa;

    [Space]
    [Header("Ticketera")]
    private int tickets = 0;

    private int aciertos = 0;
    private int valorAcierto = 1;
    private int resultadoAcierto;

    private int desAciertos = 0;
    private int valorDesAcierto = 1;
    private int resultadoDesacierto;

    private void Awake()
    {
        aciertos = 0;
        desAciertos = 0;
        tickets = 0;
    }

    /// <summary>
    /// Cantidad de tickets
    /// </summary>
    public int Ticket
    {
        get
        {
            return tickets;
        }
        set
        {
            tickets = value;
        }
    }

    /// <summary>
    /// Cantidad de aciertos
    /// </summary>
    public int Aciertos
    {
        get
        {
            return aciertos;
        }
        set
        {
            aciertos = value;
        }
    }
    /// <summary>
    /// Valor asignado a cada acierto
    /// </summary>
    public int ValorAcierto
    {
        get
        {
            return valorAcierto;
        }
        set
        {
            aciertos = valorAcierto;
        }
    }

    /// <summary>
    /// Valor asignado a cada desacierto
    /// </summary>
    public int ValorDesacierto
    {
        get
        {
            return valorDesAcierto;
        }
        set
        {
            aciertos = valorDesAcierto;
        }
    }
    /// <summary>
    /// Cantidad de desaciertos
    /// </summary>
    public int Desaciertos
    {
        get
        {
            return desAciertos;
        }
        set
        {
            desAciertos = value;
        }
    }

    /// <summary>
    /// Multiplica la cantidad de aciertos por el valor del acierto y devuelve un entero
    /// </summary>
    public int ResultadoAciertos
    {
        get
        {
            return resultadoAcierto = aciertos * valorAcierto;
        }
    }
    /// <summary>
    /// Multiplica la cantidad de aciertos por el valor del acierto y devuelve un entero
    /// </summary>
    public int ResultadoDesaciertos
    {
        get
        {
            return resultadoDesacierto = desAciertos * valorDesAcierto;
        }
    }

}
