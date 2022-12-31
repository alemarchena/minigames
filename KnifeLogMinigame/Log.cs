using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Log : MonoBehaviour
{
    [SerializeField]
    ScreenShake camShake = null;

    [SerializeField]
    float intensidadShakeGarra = 1;

    [SerializeField] AudioClip sonidoCuchilloClavado;
    [SerializeField] private SoundController controladorSonido;

    private KnifeLogMinigame controlador;

    private void Start()
    {
        controlador = FindObjectOfType<KnifeLogMinigame>();
    }

    private void OnTriggerEnter(Collider other)
    {

        controlador.TK.Ticket += 1;
        controlador.TK.Aciertos += 1;

        //KnifeLogMinigame.currentTry -= 1;
        controlador.HitKnife();

        //Debug.Log("Aciertos:" + KnifeLogMinigame.knifeCount);
        SoundController.sonido2 = sonidoCuchilloClavado;
        controladorSonido.ReproducirSonido2();
        ShakeCamera();
    }

    private void ShakeCamera()
    {
        camShake.MinorShake(intensidadShakeGarra);
        
    }
}
