using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamSheik : MonoBehaviour {
    public float power;
    public float duration;
    private Transform camera;
    public float slowDownTime;
    public bool shouldShake;
    private bool otherBool;
    public Vector3 startingPos;
    public float initialDuration;
    PlayerBehaviour playerScript;


	// Use this for initialization
	void Start ()
    {
        camera = Camera.main.transform;
        initialDuration = duration;
        startingPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {


        if (shouldShake)
        {
            if (duration > 0)
            {
                camera.localPosition = startingPos + Random.insideUnitSphere * power;
                duration -= Time.deltaTime * slowDownTime;
            }
            else
            {
                
                duration = initialDuration;
                
            }
        }

        if (shouldShake == false)
        {
            camera.localPosition = startingPos;
        }
    }
}
