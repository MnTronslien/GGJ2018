using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam_follow : MonoBehaviour
{

    public GameObject Player1;
    public GameObject Player2;

    [Range(0,5)] public float height;
    [Range(-5, -20)] public float distance;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3((Player1.transform.position.x + Player2.transform.position.x) / 2, height, distance);
        /*distance = middle.x * 2;
        transform.position = middle +  new Vector3(0,0,distance);*/



    }
}
