
using UnityEngine;

public class Rifle : MonoBehaviour {

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void DisparaCulatazo()
    {
        anim.SetTrigger("Culatazo");
    }
}
