using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamSheik : MonoBehaviour {
    public float power;
    public float duration;
    public Transform camera;
    public float slowDownTime;
    public bool shouldShake = false;
    Vector3 startingPos;
    public float initialDuration;

	// Use this for initialization
	void Start ()
    {
        camera = Camera.main.transform;

        initialDuration = duration;
	}
	
	// Update is called once per frame
	void Update () {
        if (shouldShake)
        {
            if (duration < 0)
            {
                camera.localPosition = startingPos + Random.insideUnitSphere * power;
                duration -= Time.deltaTime * slowDownTime;
              
            }
            else
            {
                shouldShake = false;
                duration = initialDuration;
                camera.localPosition = startingPos;
            }
        }
	}
}
