using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perfect : MonoBehaviour {
    public GameObject perfect;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DesactivatePerfect()
    {
        perfect.SetActive(false);
    }
}
