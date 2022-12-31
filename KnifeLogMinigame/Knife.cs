///—————————————————————–
///   File: Knife.cs
///   Author: Luciano Donati
///   me@lucianodonati.com	www.lucianodonati.com
///   Last edit: 16-Jun-18
///   Description: 
///—————————————————————–

using System.Collections;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]

public class Knife : MonoBehaviour
{
    [SerializeField] AudioClip sonidoCuchillo;
    private SoundController controladorSonido;

    [SerializeField]
    private float gravedadAlcaer = -100;
    private Vector3 gravedadOri;

    [SerializeField]
    private float rotacionAlClavarse = -15f;

    [SerializeField]
    private Vector2 anguloReboteMinMax = new Vector2(30, 90);

    [SerializeField]
    private new Rigidbody rigidbody = null;

    [SerializeField]
    private new Collider collider = null;

    private static KnifeSpawner spawner = null;

    private static float anguloDisparo = 15.0f;

    private static float gravedad = 100f;

    private Vector3 rotacionGolpeCuchillo = new Vector3(0,0,0);

    private bool rotaXgolpe = false;

    private bool cuentaCuchillo = true;

    private KnifeLogMinigame controlador;

    private void Start()
    {
        controlador = FindObjectOfType<KnifeLogMinigame>();

        controladorSonido = FindObjectOfType<SoundController>();
        gravedadOri = Physics.gravity; //guardo la gravedad original
        Physics.gravity = new Vector3(0, gravedadAlcaer, 0); //modifico la gravedad para que los cuchillos caigan mas rapido
    }

    private void Update()
    {
        if (rotaXgolpe)
        {
            transform.Rotate(rotacionGolpeCuchillo);
        }
    }
    public void Throw(Vector3 target)
    {
        collider.enabled = true;
        rigidbody.isKinematic = false;
        StartCoroutine(SimulateProjectile(target));
    }

    private IEnumerator SimulateProjectile(Vector3 target)
    {
        // Short delay added before Projectile is thrown
        //yield return new WaitForSeconds(1.5f);

        // Move projectile to the position of throwing object + add some offset if needed.
        transform.position = transform.position + new Vector3(0, 0.0f, 0);

        // Calculate distance to target
        float target_Distance = Vector3.Distance(transform.position, target);

        // Calculate the velocity needed to throw the object to the target at specified angle.
        float projectile_Velocity = target_Distance  / (Mathf.Sin(2 * anguloDisparo * Mathf.Deg2Rad) / gravedad);

        // Extract the X  Y componenent of the velocity
        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(anguloDisparo * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(anguloDisparo * Mathf.Deg2Rad);

        // Calculate flight time.
        float flightDuration = target_Distance / Vx;

        // Rotate projectile to face the target.
        transform.rotation = Quaternion.LookRotation(target - transform.position);

        float elapse_time = 0;
        while (elapse_time < flightDuration)
        {
            transform.Translate(0, (Vy - (gravedad * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime );
            elapse_time += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator FaceWhereYouGo()
    {
        while (true)
        {
            transform.rotation = Quaternion.LookRotation(rigidbody.velocity.normalized);
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Log"))
        {
            transform.SetParent(other.transform);
            transform.Rotate(Vector3.left, rotacionAlClavarse);
            
            rigidbody.isKinematic = true;
            transform.tag = "Cuchilloclavado";
        }
        else if(other.tag == "Cuchilloclavado" && transform.tag == "Cuchillo")
        {
            //verifica si choco contra otro cuchillo clavado

            rotaXgolpe = true;

            collider.enabled = true;
            collider.isTrigger = false;

            rigidbody.isKinematic = false;
            rigidbody.useGravity = true;

            transform.tag = "Untagged";

            SoundController.sonido2 = sonidoCuchillo;
            controladorSonido.ReproducirSonido2();

            StopCoroutine(RequisitosParaCaer());
            StartCoroutine(RequisitosParaCaer());

            StartCoroutine(IntentoFallido());
        }
    }

    IEnumerator IntentoFallido()
    {
        if(cuentaCuchillo== true)
        {
            KnifeLogMinigame.currentTry += 1; //acumula los intentos fallidos
            controlador.TK.Desaciertos += 1;
        }
        cuentaCuchillo = false;
        controlador.HitKnife();

        yield return null;
    }

    private Vector3 RotacionAleatoria()
    {
        return rotacionGolpeCuchillo = new Vector3( Random.Range(anguloReboteMinMax.x, anguloReboteMinMax.y), Random.Range(anguloReboteMinMax.x, anguloReboteMinMax.y), 0);
    }

    IEnumerator RequisitosParaCaer() {

        yield return new WaitForSeconds(0.5f);
        collider.enabled = false;

        yield return new WaitForSeconds(1f);
        collider.enabled = true;

        yield return new WaitForSeconds(2.5f);
        rotaXgolpe = false;

        yield return new WaitForSeconds(8f);
        Physics.gravity = gravedadOri; //restaura la gravedad
        Destroy(this.gameObject);
    }
}
