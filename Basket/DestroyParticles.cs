using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticles : MonoBehaviour {

    public float time;

	// Use this for initialization
	void Start () {
		
	}
    private void OnEnable()
    {
        StartCoroutine(destruir());
    }

    // Update is called once per frame
    void Update () {
		
	}

    IEnumerator destruir()
    {
        yield return new WaitForSeconds(time);

        gameObject.SetActive(false);
    }
}
