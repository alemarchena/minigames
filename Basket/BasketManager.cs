using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasketManager : MonoBehaviour {

    public static BasketManager Instance = null;
    public UIminijuego uiMiniJuego;

    public float screenTime = 30;
    public Text time_text,dunk__text;

    private float time;
    private int dunkBalls = -1;

    private void Awake()
    {
        Instance = this;
    }

    void Start ()
    {
        Dunk();
	}
	
	
	void Update ()
    {
        if(screenTime > 0)
            TimeRun();
	}

    public void TimeRun()
    {        
        screenTime -= Time.deltaTime;

        time = (int)(screenTime);

        time_text.text = time.ToString();
    }

    public void Dunk()
    {
        dunkBalls++;
        dunk__text.text = dunkBalls.ToString();
    }
}
