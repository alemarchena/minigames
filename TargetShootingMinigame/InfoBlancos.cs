using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoBlancos : MonoBehaviour {

    [Space]
    public List<GameObject> prefabBlancos;  // Sprite de los posibles blancos, deben ser prefabs

    [Space]
    public AudioClip sonidoImpactoRuedas;

    [Space]
    [Tooltip("Tags de la municion que pueden destruir al blanco")]
    public string tagMunicion;

    [Tooltip("Tag del blanco que se reconoce como valido al disparar")]
    public string tagBlancoCorrecto;

    [Space]
    public GameObject BlancoQueCae;

    [SerializeField]
    [Tooltip("Velocidad con la que voltea el blanco")]
    private float MaxVelocidadVolteo;

    [SerializeField]
    [Tooltip("Angulo de giro X al caer")]
    private float anguloCaidaX=0;

    [SerializeField]
    [Tooltip("Angulo de giro Y al caer")]
    private float anguloCaidaY = 0;

    [SerializeField]
    [Tooltip("Angulo de giro Z al caer")]
    private float anguloCaidaZ = 0;
    
    [SerializeField]
    [Tooltip("Tiempo en caer el blanco")]
    private float retrasoCaida = 0;

    [SerializeField]
    [Tooltip("Tiempo en desaparecer el blanco caido")]
    private float retrasoDesapareceBlanco = 0;

    private bool voltear;
    private float sentido=10;
    private int sentidoVolteo = 0;
    private Disparador disparador;

    private void Awake()
    {
        disparador = FindObjectOfType<Disparador>();
    }

    public int AleatorioDeBlancos()
    {
        int indiceBlanco = 0;
        indiceBlanco = Random.Range(0, prefabBlancos.Count);
        return indiceBlanco;
    }

    public void AsignaSpriteAlBlanco(SpriteRenderer imagen, int indiceBlanco)
    {
        if (imagen) { imagen.sprite = prefabBlancos[indiceBlanco].GetComponent<SpriteRenderer>().sprite;}
    }

    public void AsignaTagBlanco(Transform transformBlanco, int indiceBlanco)
    {
        transformBlanco.tag = prefabBlancos[indiceBlanco].GetComponent<Transform>().tag; //los prefabs de blancos le pasan el TAG al objeto creado
    }

    public void AsignaSonidoBlanco(MoldeBlanco sonidoBlanco, int indiceBlanco)
    {
        sonidoBlanco.sonidoImpacto = prefabBlancos[indiceBlanco].GetComponent<Blanco>().sonidoImpacto; //asigna el sonido del prefab al nuevo blanco
    }

    public void AsignaMonedasBlanco(MoldeBlanco moldeBlanco, int indiceBlanco)
    {
        moldeBlanco.monedasPorImpacto = prefabBlancos[indiceBlanco].GetComponent<Blanco>().monedasPorImpacto; //asigna el sonido del prefab al nuevo blanco
    }

    public void VoltearBlanco(GameObject blancoAcertado)
    {
        BlancoQueCae.transform.SetPositionAndRotation(blancoAcertado.transform.position, blancoAcertado.transform.rotation);
        BlancoQueCae.transform.GetComponent<SpriteRenderer>().sprite = blancoAcertado.transform.GetComponent<SpriteRenderer>().sprite;
        voltear = true;

        sentidoVolteo = (Random.Range(0, sentido) > 5) ? 1 : -1;
        StartCoroutine(AccionaCaida());
    }

    private void Update()
    {
        if (voltear){ BlancoQueCae.transform.Rotate(Random.Range(anguloCaidaX /2,anguloCaidaX) * Time.deltaTime * MaxVelocidadVolteo * sentidoVolteo, Random.Range(anguloCaidaY / 2, anguloCaidaY) * Time.deltaTime * MaxVelocidadVolteo * sentidoVolteo, Random.Range(anguloCaidaZ / 2, anguloCaidaZ) * Time.deltaTime * MaxVelocidadVolteo * sentidoVolteo);}
    }

    IEnumerator AccionaCaida() {

        disparador.disparoActivo = false;
        yield return new WaitForSeconds(retrasoCaida);
        BlancoQueCae.transform.GetComponent<Rigidbody>().isKinematic = false;
        BlancoQueCae.transform.GetComponent<Rigidbody>().useGravity = true;
        BlancoQueCae.transform.GetComponent<Rigidbody>().AddForce(Vector3.forward, ForceMode.Impulse);
        disparador.disparoActivo = true;
        yield return new WaitForSeconds(retrasoDesapareceBlanco);
        voltear = false;
        BlancoQueCae.transform.GetComponent<SpriteRenderer>().sprite = null;
        BlancoQueCae.transform.GetComponent<Rigidbody>().isKinematic = true;
        BlancoQueCae.transform.GetComponent<Rigidbody>().useGravity = false;
    }
}
