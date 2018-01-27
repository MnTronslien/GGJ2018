using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerRed: MonoBehaviour
{

    public float maxPower;
    public float MattiasSinIntegerRed = 100;
    public GameObject myPowerBar;
    public float CurrentPower;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GeneratePower();
    }

    void GeneratePower()
    {
        SetPowerToCanvas(MattiasSinIntegerRed);
    }

    void SetPowerToCanvas(float myPower)
    {
        myPowerBar.transform.localScale = new Vector3(Mathf.Clamp(MattiasSinIntegerRed, 0, 1), myPowerBar.transform.localScale.y, myPowerBar.transform.localScale.z);
    }
}