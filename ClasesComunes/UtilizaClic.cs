using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilizaClic : MonoBehaviour {
    [Tooltip("Muestra en la consola datos del objeto alcanzado por el hit")]
    public bool mostrarInfo = false;
    [HideInInspector] public Ray RayoObjetivo;
    public GameObject camaraActual;
    [HideInInspector] public Transform objetoAlcanzado;
    [HideInInspector] public Vector3 objetivo;
    [HideInInspector] public RaycastHit resultado;
    [Tooltip("Objeto mostrable en pantalla adonde apunta el mouse")]
    public GameObject puntoIluminado;

    public void VerificaPosicionClicMouse()
    {
        RayoObjetivo = camaraActual.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

        Physics.Raycast(RayoObjetivo, out resultado, 1500f);
        objetoAlcanzado = resultado.transform;
        objetivo = resultado.point;

        if (puntoIluminado != null)
            puntoIluminado.GetComponent<Transform>().SetPositionAndRotation(objetivo, Quaternion.identity);

        if(mostrarInfo)
            MuestraInfo(resultado);
    }
    private void MuestraInfo(RaycastHit resultado)
    {
        if (resultado.transform)
        {
            Debug.Log(resultado.point);
            Debug.Log(resultado.transform.tag);
            Debug.Log(resultado.transform.gameObject.name);
        }
        else {
            Debug.Log("No le diste a nada");
        }
    }
}
