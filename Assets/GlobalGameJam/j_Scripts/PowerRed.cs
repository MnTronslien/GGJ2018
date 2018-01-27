using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerRed: MonoBehaviour
{

    public PlayerBehaviour _PB;
    public GameObject myPowerBar;

    void Start()
    {
        if (_PB == null)
        {
            _PB = GetComponent<PlayerBehaviour>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        SetPowerToCanvas(_PB._charge);
    }

    void SetPowerToCanvas(float myPower)
    {
        myPowerBar.transform.localScale = new Vector3(Mathf.Clamp(myPower/3.5f,0,1), myPowerBar.transform.localScale.y, myPowerBar.transform.localScale.z);

    }
}