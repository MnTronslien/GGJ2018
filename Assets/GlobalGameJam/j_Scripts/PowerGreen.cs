using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerGreen : MonoBehaviour {

    public float maxPower;
    public float MattiasSinInteger = 100;
    public GameObject myPowerBar;
    public  float CurrentPower;

	void Start () {

	}
	
	// Update is called once per frame
	void Update ()
    {
        GeneratePower();
    }

    void GeneratePower()
    {
        SetPowerToCanvas(MattiasSinInteger);
    }

    void SetPowerToCanvas(float myPower)
    {
        myPowerBar.transform.localScale = new Vector3(Mathf.Clamp(MattiasSinInteger,0,1), myPowerBar.transform.localScale.y, myPowerBar.transform.localScale.z);
    }
}
