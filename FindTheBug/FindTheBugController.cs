using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindTheBugController : Minigame
{
    [Header("Controladores")]
    [Space]
    [Header("Controladores")]
    [Tooltip("Muestra los valores de Tickets, aciertos y desaciertos del minijuego")]
    public UIminijuego uiminigame;
    [Tooltip("Permite manipular los valores  Tickets, aciertos y desaciertos del minijuego")]
    public Ticketera TK;
    [Tooltip("Permite manipular los valores de tiempo del minijuego")]
    public Tiempo tie;

    public float tiempoDeJuegoSeg = 20;


    [SerializeField] KeyCode queToco;
    [Tooltip("Detecta adonde se toco la pantalla o clic de mouse")]
    [SerializeField] UtilizaClic clic;
    [Tooltip("Color que aparece cuando se toco la garra correcta")]
    [SerializeField] Color colorGarraOk;
    [Tooltip("Luz que aparece cuando se toco la garra correcta")]
    [SerializeField] GameObject efectoLuz;

    /// <summary>
    /// Bicho del minijuego
    /// </summary>
    [Tooltip("Objeto bicho")]
    [SerializeField] Bichin bichin;

    /// <summary>
    /// Posibles movimientos animados de las garras
    /// </summary>
    [SerializeField] List<Transform> garras;
    [HideInInspector] public List<Transform> AuxGarras;

    /// <summary>
    /// Tiempo entre movimientos
    /// </summary>
    [Tooltip("Tiempo inicial para el movimiento entre garras")]
    public float tiempo=5;

    /// <summary>
    /// Cantidad de movimientos para mezclar
    /// </summary>
    [Tooltip("Cantidad de Movimientos por mezcla")]
    public float movimientos=3;
    [HideInInspector] public float movimientosOri=0;

    

    /// <summary>
    /// Factor de aceleracion de cada jugada
    /// </summary>
    [Tooltip("Factor tiempo que resta en la proxima mezcla")]
    [SerializeField] float factorAceleracion = 0.25f;

    [Tooltip("El menor tiempo permitido para acelerar la velocidad de mezcla")]
    [SerializeField] float limiteMinTiempo = 0.25f;
    
    /// <summary>
    /// Empieza el movimiento de garras si el bicho entro
    /// </summary>
    public static bool entroElBicho = false;
    public static bool tapar = false;
    public static bool jugando = false;

    /// <summary>
    /// Contador interno del tiempo entre movimientos
    /// </summary>
    private float contador=0;
    private float contadorMovimientos=0;
    private float garraElegida1=0;
    private float garraElegida2= 0;
    private float movimientoPreferido= 0;
    [Space]
    [Tooltip("Tiempo de espera para comenzar a jugar")]
    [SerializeField] float tiempoJugar = 0.5f;
    [Tooltip("Tiempo de espera para destapar la proxima garra")]
    [SerializeField] float tiempoDestape = 0.5f;
    [Tooltip("Tiempo de espera para tapar la proxima garra")]
    [SerializeField] float tiempoTapar = 0.5f;
    [Tooltip("Tiempo de espera para empezar a mover el bicho")]
    [SerializeField] float tiempoMoverBicho = 0.5f;
    [Tooltip("Tiempo de espera para empezar la mezcla entre jugadas")]
    [SerializeField] float tiempoResetMovimiento = 2f;
    [Space]
    [SerializeField] private float[] probabilidadFase1 = { 0.33f, 0.33f, 0.33f };
    [SerializeField] private float[] probabilidadFase2 = { 0.5f, 0.5f };
    [SerializeField] private float[] probabilidadMovimiento = { 0.5f, 0.5f };
    private bool sumador = true;
    private bool sumaMon = true;
    private bool toco= true;
    public static bool permitetocar1=false;
    public static bool permitetocar2=false;
    public static bool permitetocar3=false;
    int indiceG = 0;
    private Probabilidad pro;


    ///--------------------------- Contrato del Juego ----------------
    ///

    /// <summary>
    /// The BeginStartRoutine: 
    /// </summary>
    /// <returns>The <see cref="IEnumerator"/></returns>
    protected override IEnumerator BeginStartRoutine()
    {


        yield return null;
    }

    /// <summary>
    /// Update method (with scaled own delta time)
    /// </summary>
    /// <param name="deltaTimeModificado">The deltaTime<see cref="float"/></param>
    public override void UpdateMinigame(float deltaTimeModificado)
    {
        base.UpdateMinigame(deltaTimeModificado);// Importante (o sino hagan this.deltaTime = deltaTime)  <--- Mala idea.
        UpdateModificado(deltaTimeModificado);
    }

    /// <summary>
    /// ..... ver si lleva transicion
    /// </summary>
    /// <returns>The <see cref="IEnumerator"/></returns>
    public override IEnumerator TransitionToEnd()
    {

        yield return null;
    }

    /// <summary>
    /// Comienza el minijuego
    /// </summary>
    protected override void StartMinigame()
    {
        StartMiniJuego();
        ReiniciaJuego();
    }

    //-----------------------------------------------------------------------
    private void StartMiniJuego()
    {
        pro = FindObjectOfType<Probabilidad>();
        
        movimientosOri = movimientos;
        //crea la lista auxiliar para intercambio de posiciones de garras
        for (int a = 0; a < 3; a++)
        {
            AuxGarras.Add(garras[a]);
        }
        StartCoroutine(Jugar());
    }

    /// <summary>
    /// Activa el efecto de luz debajo de la calavera cuando tenia el bicho 
    /// </summary>
    /// <param name="posicion"></param>
    private void ActivaEfecto(Transform posicion)
    {
        Vector3 nuevaPosicion = posicion.position;
        nuevaPosicion.z = nuevaPosicion.z - 2;
        efectoLuz.transform.position = nuevaPosicion;

        efectoLuz.SetActive(true);
    }

    IEnumerator Jugar() {
        yield return new WaitForSeconds(tiempoJugar);
        StartCoroutine(Destapar());
        StartCoroutine(IniciarMovimientoBicho());
        jugando = true;
    }

    private void UpdateModificado(float deltaTimeModificado)
    {
        if (jugando)
        {
            uiminigame.PublicarTextos(tie.VerTiempo, TK.Ticket, TK.Aciertos, 0);

            if (entroElBicho) //si el bicho toco la garra objetivo
            {
                if (tie.SuperoDeltaTime(tiempo, deltaTimeModificado))
                {
                    contador = 0;
                    contadorMovimientos += 1;

                    if (contadorMovimientos <= movimientos)
                    {
                        StartCoroutine(MoverGarras()); //mezcla de garras para esconder al bicho
                    }
                }
               
            }

            if (tapar) //Baja las garras
            {
                StartCoroutine(Tapar(tiempoTapar));
            }

            //permite tocar siempre y cuando hayan terminado todas las animaciones y haya hecho todos los movimientos
            if (Input.GetKey(queToco) && permitetocar1==true && permitetocar2 == true && permitetocar3 == true && contadorMovimientos > movimientos)
            {
                StartCoroutine(TocoGarra());
            }
        }
    }

    

    IEnumerator TocoGarra() {
        if (sumaMon == true)
        {
            StartCoroutine(SumaMovimiento());//incrementa la cantidad de movimientos de las garras en 1

            bool tieneAlbicho = false;
            clic.VerificaPosicionClicMouse();

            if (clic.objetoAlcanzado.tag == "Garra")
            {
                tieneAlbicho = false;
                if (clic.objetoAlcanzado.GetComponent<MovementFindTheBug>().bicho != null)
                {
                    
                    tieneAlbicho = true;
                    ActivaEfecto(clic.objetoAlcanzado);
                    StartCoroutine(SumaTickets());
                }

                if (tieneAlbicho)
                {
                    Transform g = BuscaGarraTocada(clic.objetoAlcanzado);
                    StartCoroutine(RetardoResetearMovimiento(g.GetComponent<MovementFindTheBug>(),indiceG));
                }
                else
                {
                    bichin.GetComponent<Transform>().SetParent(null);
                    bichin.GetComponent<Transform>().tag = "Untagged";
                    StartCoroutine(Destapar());
                    StartCoroutine(MostrarBicho(tiempoDestape));

                    bichin.seEscapa = true;
                    jugando = false;
                }
            }
        }
        sumaMon = false;
        yield return new WaitForSeconds(1f);
        sumaMon = true;
    }

    /// <summary>
    /// busca la garra tocada en el arreglo de garras
    /// </summary>
    /// <returns></returns>
    private Transform BuscaGarraTocada(Transform garraTocada)
    {
        for(int a = 0; a<garras.Count; a++)
        {
            if (garraTocada.name == garras[a].name) {
                indiceG = a;//asigna el orden en el que se encuetra la garra para levantar
                return garras[a];//sabe que garra debe levantarse
            }
        }
        return null;
    }

    IEnumerator RetardoResetearMovimiento(MovementFindTheBug garraTocada,int indice)
    {
        StartCoroutine(DestapaYtapaEncontrado( garraTocada,indice,2f));
        StartCoroutine(MostrarBicho(0f));

        yield return new WaitForSeconds(tiempoResetMovimiento);
        efectoLuz.SetActive(false);
        ReseteaMovimiento();
    }

    IEnumerator DestapaYtapaEncontrado(MovementFindTheBug garraTocada,int indice,float tiempo)
    {
        if (indice == 0)
        {
            garraTocada.destapar1 = true;
            yield return new WaitForSeconds(tiempo);
            garraTocada.quieto1 = true;

        }
        else if (indice == 1)
        {
            garraTocada.destapar2 = true;
            yield return new WaitForSeconds(tiempo);
            garraTocada.quieto2 = true;
        }
        else if (indice == 2)
        {
            garraTocada.destapar3 = true;
            yield return new WaitForSeconds(tiempo);
            garraTocada.quieto3 = true;
        }
        yield return null;
    }

    IEnumerator SumaTickets()
    {
        if (sumaMon == true)
        {
            TK.Aciertos += 1;
            TK.Ticket += 1;
            
        }
        sumaMon = false;
        yield return new WaitForSeconds(1f);
        sumaMon = true;
    }

    IEnumerator SumaMovimiento()
    {
        if (sumador == true) {
            movimientos += 1;
        }
        sumador = false;
        yield return new WaitForSeconds(1f);
        sumador = true;
    }

    IEnumerator MostrarBicho(float tiempo)
    {
        yield return new WaitForSeconds(tiempo);

        bichin.GetComponent<Transform>().gameObject.SetActive(true);
        bichin.GetComponent<Bichin>().enabled = true;

        MeshRenderer[] comp = bichin.GetComponent<Transform>().GetComponentsInChildren<MeshRenderer>();
        for (int a = 0; a < comp.Length; a++)
        {
            comp[a].enabled = true;
        }
    }
  
    public void ReseteaMovimiento()
    {
        
        
        tiempo -= factorAceleracion;
        if (tiempo < limiteMinTiempo) 
        {
            tiempo += factorAceleracion;
        }
        contadorMovimientos = 0;
        //Debug.Log("Tiempo:" + tiempo);
        
    }

    private void ReiniciaJuego() {
       
        contadorMovimientos = 0;
        contador = 0;
        movimientos=movimientosOri;
        TK.Ticket = 0;
        TK.Aciertos = 0;
    }

    /// <summary>
    /// Mueve aleatoriamente las garras
    /// </summary>
    /// <returns></returns>
    IEnumerator MoverGarras()
    {
        
        //Debug.Log("VALOR:" + movimientoPreferido);
        yield return new WaitForSeconds(0.5f);

        //garraElegida1 = AleatorioDeMovimientos(probabilidadFase1);//elige la garra a mover
        garraElegida1 = pro.DameAleatorio(probabilidadFase1);//elige la garra a mover
        //garraElegida2 = AleatorioDeMovimientos(probabilidadFase2);//elige la garra a mover menos la anterior
        garraElegida2 = pro.DameAleatorio(probabilidadFase2);//elige la garra a mover menos la anterior
        //movimientoPreferido = AleatorioDeMovimientos(probabilidadMovimiento);//elige si el movimiento es por derecha o izquierda
        movimientoPreferido = pro.DameAleatorio(probabilidadMovimiento);//elige si el movimiento es por derecha o izquierda
        
        Moviendo(garraElegida1);
    }

    /// <summary>
    /// Destapa las garras
    /// </summary>
    /// <returns></returns>
     IEnumerator Destapar() {

        yield return new WaitForSeconds(tiempoDestape);
        garras[0].GetComponent<MovementFindTheBug>().destapar1 = true;

        yield return new WaitForSeconds(tiempoDestape);
        garras[1].GetComponent<MovementFindTheBug>().destapar2 = true;

        yield return new WaitForSeconds(tiempoDestape);
        garras[2].GetComponent<MovementFindTheBug>().destapar3 = true;
        
    }

    

    /// <summary>
    /// Mueve al bicho hacia una garra
    /// </summary>
    /// <returns></returns>
    IEnumerator IniciarMovimientoBicho()
    {
        yield return new WaitForSeconds(tiempoMoverBicho);
        bichin.moverBicho = true;
    }

    /// <summary>
    /// Baja las garras
    /// </summary>
    /// <returns></returns>
    IEnumerator Tapar(float tiempo)
    {
        if (tapar == true) {
            tapar = false;
            bichin.GetComponent<Transform>().tag = "Bicho";

            yield return new WaitForSeconds(tiempo);
            garras[0].GetComponent<MovementFindTheBug>().quieto1 = true;

            yield return new WaitForSeconds(tiempo);
            garras[1].GetComponent<MovementFindTheBug>().quieto2 = true;

            yield return new WaitForSeconds(tiempo);
            garras[2].GetComponent<MovementFindTheBug>().quieto3 = true;

            tiempo = limiteMinTiempo;
        }
        tapar = false;
    }

    /// <summary>
    /// Controla el movimiento de las garras
    /// </summary>
    /// <param name="garraElegida"></param>
    private void Moviendo(float garraElegida)
    {
        if (garraElegida > 0 && garraElegida < 1) //Garra ubicada arriba
        {
            //Debug.Log("Garra 1 PRINCIPAL");

            garras[0].GetComponent<ParticulasGarraFindTheBug>().particulas.Play();
            if (garraElegida2 == 0 ) //mueve entre garra de arriba y medio
            {
                //Debug.Log("Garra 2");
                if (movimientoPreferido == 0)
                {
                    garras[0].GetComponent<MovementFindTheBug>().moveteP1AP2 = true;//garra de arriba
                }
                else
                {
                    garras[0].GetComponent<MovementFindTheBug>().moveteP1AP2IZQ = true;//garra de arriba

                }

                if (movimientoPreferido == 0)
                {
                    garras[1].GetComponent<MovementFindTheBug>().moveteP2AP1 = true;//garra del medio
                }
                else
                {
                    garras[1].GetComponent<MovementFindTheBug>().moveteP2AP1DER = true;//garra del medio
                }
                //garras[1].GetComponent<ParticulasGarraFindTheBug>().particulas.Play();
                CambioPosicionesGarras(0, 1);
            }
            else //mueve entre garra de arriba y abajo
            {
                //Debug.Log("Garra 3");

                if (movimientoPreferido == 0)
                {
                    garras[0].GetComponent<MovementFindTheBug>().moveteP1AP3 = true;//garra de arriba
                }
                else
                {
                    garras[0].GetComponent<MovementFindTheBug>().moveteP1AP3IZQ = true;//garra de arriba
                }

                if (movimientoPreferido == 0)
                {
                    garras[2].GetComponent<MovementFindTheBug>().moveteP3AP1 = true;//garra de abajo
                }
                else
                {
                    garras[2].GetComponent<MovementFindTheBug>().moveteP3AP1DER = true;//garra de abajo
                }
                //garras[2].GetComponent<ParticulasGarraFindTheBug>().particulas.Play();
                CambioPosicionesGarras(0, 2);
            }
        }
        else if (garraElegida >= 1 && garraElegida < 2) //Garra ubicada el medio
        {

            //Debug.Log("Garra 2 PRINCIPAL");

            garras[1].GetComponent<ParticulasGarraFindTheBug>().particulas.Play();
            if (garraElegida2 == 0) //mueve entre garra del medio y la de arriba
            {
                //Debug.Log("Garra 1");
                if (movimientoPreferido == 0)
                {
                    garras[1].GetComponent<MovementFindTheBug>().moveteP2AP1 = true;//garra del medio
                }
                else
                {
                    garras[1].GetComponent<MovementFindTheBug>().moveteP2AP1DER = true;//garra del medio
                }

                if (movimientoPreferido == 0)
                {
                    garras[0].GetComponent<MovementFindTheBug>().moveteP1AP2 = true;//garra de arriba
                }
                else
                {
                    garras[0].GetComponent<MovementFindTheBug>().moveteP1AP2IZQ = true;//garra de arriba
                }
                //garras[0].GetComponent<ParticulasGarraFindTheBug>().particulas.Play();
                CambioPosicionesGarras(1, 0);
            }
            else //mueve entre garra del medio y abajo
            {
                //Debug.Log("Garra 3");

                garras[1].GetComponent<MovementFindTheBug>().moveteP2AP3 = true;//garra del medio
                garras[2].GetComponent<MovementFindTheBug>().moveteP3AP2 = true;//garra del abajo
                CambioPosicionesGarras(1, 2);
            }
        }
        else //Garra ubicada abajo
        {
            //Debug.Log("Garra 3 PRINCIPAL");

            garras[2].GetComponent<ParticulasGarraFindTheBug>().particulas.Play();
            if (garraElegida2 == 0) //mueve entre garra de abajo y arriba
            {
                //Debug.Log("Garra 1");

                if (movimientoPreferido == 0)
                {
                    garras[2].GetComponent<MovementFindTheBug>().moveteP3AP1 = true;//garra del abajo
                }
                else
                {
                    garras[2].GetComponent<MovementFindTheBug>().moveteP3AP1DER = true;//garra de abajo
                }

                if (movimientoPreferido == 0)
                {
                    garras[0].GetComponent<MovementFindTheBug>().moveteP1AP3 = true;//garra de arriba
                }
                else
                {
                    garras[0].GetComponent<MovementFindTheBug>().moveteP1AP3IZQ = true;//garra de arriba
                }
                //garras[0].GetComponent<ParticulasGarraFindTheBug>().particulas.Play();
                CambioPosicionesGarras(2, 0);
            }
            else //mueve entre garra de abajo y medio
            {
                //Debug.Log("Garra 2");

                garras[2].GetComponent<MovementFindTheBug>().moveteP3AP2 = true;//garra del abajo
                garras[1].GetComponent<MovementFindTheBug>().moveteP2AP3 = true;//garra del medio
                //garras[1].GetComponent<ParticulasGarraFindTheBug>().particulas.Play();
                CambioPosicionesGarras(2, 1);
            }
        }
    }

    /// <summary>
    /// Reubica las posiciones de las garras, arriba, medio, abrajo en el vector que controla los movimientos
    /// </summary>
    /// <param name="origen"></param>
    /// <param name="destino"></param>
    private void CambioPosicionesGarras(int origen,int destino)
    {
        //iguala los vectores para despues consultar las diferencias
        for (int a = 0; a < 3; a++)
        {
            AuxGarras[a] = garras[a];
        }
        //intercambia las posiciones de los valores
        AuxGarras[destino] = garras[origen];
        AuxGarras[origen] = garras[destino];

        //Asigna el auxiliar al vector original
        for(int a = 0;a<3; a++)
        {
            garras[a] = AuxGarras[a];
        }
    }

    
}
