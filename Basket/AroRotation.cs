using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AroRotation : MonoBehaviour {

    public Transform aroPos;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = aroPos.position;
	}
}
