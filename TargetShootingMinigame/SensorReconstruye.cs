using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorReconstruye : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<MoldeBlanco>())
        {
            other.GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}
