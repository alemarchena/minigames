using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBall : MonoBehaviour {

    public Transform ballPosition;
    public ThrowBall throwBall;

    [SerializeField]private int score;
    public bool under = true;

    [HideInInspector]public AudioSource source;

	void Start () {

        score = throwBall.maxScore;
        source = GetComponent<AudioSource>();
	}
	
	
	void Update () {
		
	}

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "setBall" &&  !throwBall.ballInHands)
        {
            under = true;
            score = throwBall.maxScore;
            throwBall.basketBall_GO = gameObject;
            throwBall.ballInHands = true;
            GetComponent<Rigidbody>().isKinematic = true;
            transform.position = ballPosition.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "aro")
        {
            if(score>1)
                score -= 1;
            source.PlayOneShot(throwBall.aro[Random.Range(0, throwBall.aro.Length)]);
        }

        if(other.tag == "winAro" && under == true)
        {
            BasketManager.Instance.Dunk();
            under = false;
            throwBall.score += score;

            throwBall.TK.Ticket += 1;//linea agregue yo gallo
            throwBall.TK.Aciertos += 1;//linea agregue yo gallo

            Debug.Log(score);
            throwBall.particle.SetActive(true);

            if (score > throwBall.maxScore - 1)
            {
                Camera.main.GetComponent<ScreenShake>().LongShake(0.006f);
                throwBall.perfect.SetActive(false);
                throwBall.perfect.SetActive(true);
                source.PlayOneShot(throwBall.embocarSinAro);
                throwBall.source.PlayOneShot(throwBall.whoa);
            }
            else
            {
                Camera.main.GetComponent<ScreenShake>().MinorShake(0.01f);
                source.PlayOneShot(throwBall.embocarConAro);
            }
            
        }

        if(other.tag == "alambrado")
        {
            source.PlayOneShot(throwBall.golpearAlambrado);

        }
        if(other.tag == "parquet")
        {
            source.PlayOneShot(throwBall.golpearParquet);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "winAro")
        {
            
            //under = true;
        }
    }
}
