using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disparador : MonoBehaviour
{
    [SerializeField] AudioClip sonido;
    [Tooltip("Tiempo que tarda en destruirse la munici�n desde que salio disparada")]
    [SerializeField] float tiempoDesactivarMunicion;
    [SerializeField] float tiempoRetardoDisparo;

    [SerializeField]
    [Tooltip("El cuerpo del objeto que dispara")]
    private GameObject disparador;
    [SerializeField] GameObject objetoMunicion;
    [SerializeField] UtilizaClic clic;
    public bool disparoActivo = false;
    [SerializeField] List <GameObject> municionCreada;
    private Municion municion;
    private Disparando disparando;
    private Rifle rifle;
    [HideInInspector] public int disparosDisponibles;
    /// <summary>
    /// Objeto invisible que mueve al personaje con la punta del disparador
    /// </summary>
    [SerializeField] Transform guia;
    private int indiceMunicion=0;
    private SoundController controladorSonido;
    private TargetShootingController targetController;
    

    public void IniciaDisparador()
    {
        disparoActivo = true;
        targetController = FindObjectOfType<TargetShootingController>();
        controladorSonido = FindObjectOfType<SoundController>();
        disparando = FindObjectOfType<Disparando>();
        rifle = FindObjectOfType<Rifle>();
        for (int a = 0; a < municionCreada.Count; a++)
        {
            municionCreada[a].SetActive(false);
            municionCreada[a].GetComponent<Municion>().activa = false;
            municionCreada[a].transform.SetPositionAndRotation(transform.position, transform.rotation);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log("disparooo");
            clic.VerificaPosicionClicMouse();
            Apuntar(clic.objetivo);

            if (disparoActivo && disparosDisponibles > 0) {
                disparosDisponibles -= 1;
                if (disparosDisponibles <= 0)
                {
                    disparoActivo = false;
                    targetController.DetieneDisparos();
                }
                municionCreada[indiceMunicion].SetActive(true);
                municionCreada[indiceMunicion].transform.SetPositionAndRotation(transform.position, transform.rotation);
                municionCreada[indiceMunicion].GetComponent<Municion>().activa = true;

                
                StartCoroutine(DesactivarMunicion(municionCreada[indiceMunicion],indiceMunicion));
                if (sonido)
                {
                    SoundController.sonido2 = sonido;
                    controladorSonido.ReproducirSonido2();
                }
                if (disparando)
                {
                    disparando.PersonajeDispara();//animacion del personaje disparando
                    if (rifle)
                    {
                        rifle.DisparaCulatazo();
                    }
                }
                indiceMunicion = indiceMunicion + 1;
                if (indiceMunicion >= municionCreada.Count) {
                    indiceMunicion = 0;
                }
            }
        }
    }

    IEnumerator DesactivarMunicion(GameObject objeto,int indice)
    {
        yield return new WaitForSeconds(tiempoDesactivarMunicion);
        municionCreada[indice].SetActive(false);
        municionCreada[indice].GetComponent<Municion>().activa = false;
    }

    private void Apuntar(Vector3 puntoTarget)
    {
        disparador.transform.LookAt(puntoTarget);
        guia.LookAt(puntoTarget);
    }
}
