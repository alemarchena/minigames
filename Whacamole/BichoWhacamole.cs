using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BichoWhacamole : MonoBehaviour {

    [SerializeField] SoundController controladorSonido;
    [SerializeField] WhacamoleController controladorWhaca;

    private void EmiteSonidoSale()
    {
        if (controladorSonido)
        {
            SoundController.sonido1 = controladorWhaca.sonidoSale[(int)controladorWhaca.pro.DameAleatorio(controladorWhaca.probabilidadEntraSale)];
            controladorSonido.ReproducirSonido1();
        }
    }

    IEnumerator EmiteSonidoEntra()
    {
        yield return new WaitForSeconds(controladorWhaca.esperaSonidoBajada);
        if (controladorSonido)
        {
            if (!controladorSonido.ASSonido2.isPlaying)
            {
                SoundController.sonido2 = controladorWhaca.sonidoEntra[(int)controladorWhaca.pro.DameAleatorio(controladorWhaca.probabilidadEntraSale)];
                controladorSonido.ReproducirSonido2();
            }
        }

    }
}
