using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Martillo : MonoBehaviour
{

    [SerializeField] private float anguloRebote=10f;
    [SerializeField] private float angulo = 90f;

    [HideInInspector] public bool enderezar = false;
    [HideInInspector] public bool derecha=true;
    private Quaternion qua;

    private void Start()
    {
        qua = transform.rotation;
    }
    private void Update()
    {
        if (enderezar == true) {
            EnderezarMartillo();
        }
    }
    

    public void Golpea()
    {
        if (derecha)
        {
            transform.Rotate(Vector3.forward, angulo);
        }
        else
        {
            transform.Rotate(Vector3.right,180f);
            transform.Rotate(Vector3.forward, angulo);
        }
        enderezar = true;
        StopCoroutine(DetieneEnderezar());
        StartCoroutine(DetieneEnderezar());
    }

    IEnumerator DetieneEnderezar() {
        yield return new WaitForSeconds(2f);
        enderezar = false;
    }

    private void EnderezarMartillo()
    {
        if (derecha)
        {
            transform.Rotate(Vector3.back,anguloRebote );
        }
        else
        {
            transform.Rotate(Vector3.back, anguloRebote);
        }
    }

    public void EstableceAnguloReposo()
    {
        enderezar = false;
        transform.SetPositionAndRotation(transform.position,qua);
    }
  
}
