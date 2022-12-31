using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrowBall : MonoBehaviour {


    [Space]
    [Header("Controladores")]
    [Tooltip("Muestra los valores de Tickets, aciertos y desaciertos del minijuego")]
    public UIminijuego uiminigame;
    [Tooltip("Permite manipular los valores  Tickets, aciertos y desaciertos del minijuego")]
    public Ticketera TK;
    [Tooltip("Permite manipular los valores de tiempo del minijuego")]
    public Tiempo tie;
    [Space]
    public Slider slider;
    public Text text;

    public GameObject basketBall_GO;
    public GameObject perfect;
    public GameObject particle;
    public Camera gameCam;

    private float distance;
    private float time;
    private float timeF, timeS;
    public float forceThrow;
    public float force;
    public float highThrow;

    public bool ballInHands;

    private Ray mousePoint;
    private Rigidbody basketBall_RB;

    [SerializeField]private Vector2 StartPoint;
    [SerializeField]private Vector2 FinishedPoint;

    public int score;
    public int maxScore;

    public AudioClip[] aro;
    public AudioClip embocarConAro, embocarSinAro,golpearAlambrado,golpearParquet,Tirar,whoa;

    [HideInInspector] public AudioSource source;
	
	void Start ()
    {
        ballInHands = true;
        source = GetComponent<AudioSource>();
	}
	
	
	void FixedUpdate ()
    {
        if(ballInHands)
            SetForce();
        uiminigame.PublicarTextos(tie.VerTiempo, TK.Ticket, TK.Aciertos, 0);
    }

    private void SetForce()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartPoint = Input.mousePosition;
            //time = Time.time;
        }
        else if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            FinishedPoint = Input.mousePosition;

            if (FinishedPoint.x < 0.3f)
                FinishedPoint.x = 0.3f;

            distance = Vector2.Distance(StartPoint, FinishedPoint);
            //time = Time.time - time;
            text.text = forceThrow.ToString();

            forceThrow = distance;

            if (forceThrow > 800)
                forceThrow = 800;
            
            basketBall_RB = basketBall_GO.GetComponent<Rigidbody>();
            Throw();
            ballInHands = false;
            basketBall_GO = null;
            //StartCoroutine(OtherBall());   
        }

        
    }

    private void Throw()
    {
        source.PlayOneShot(Tirar);
        mousePoint = gameCam.ScreenPointToRay(Input.mousePosition);
        mousePoint.direction = new Vector3(mousePoint.direction.x,mousePoint.direction.y + highThrow,mousePoint.direction.z);
        basketBall_RB.isKinematic = false;
        basketBall_RB.AddForce(mousePoint.direction * forceThrow * force);
    }

    private void OnDrawGizmos()
    {
        if(basketBall_GO != null)
            Debug.DrawRay(basketBall_GO.transform.position, mousePoint.direction * 1000, Color.red);
    }

    IEnumerator OtherBall()
    {
        yield return new WaitForSeconds(0);
        basketBall_GO = null;

    }
    
   
}
