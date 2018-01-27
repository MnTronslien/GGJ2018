using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public Transform otherPlayer;
    [Header("Tweak")]
    public float attraction = 0.1f;
    public float distance;
    [Range(0.004f, 1)] public float attractionMulitplier;
    [Range(1f, 10f)] public float thrust;

    public Rigidbody rb1;
    

    private void Start()
    {
        rb1 = GetComponent<Rigidbody>();
    }

    void Update()
    {
        distance = transform.position.x - otherPlayer.transform.position.x;
        transform.position = Vector3.Lerp(transform.position, otherPlayer.position, attractionMulitplier);

        if (Input.GetButton("Mouse0"))
        {
            GetComponent<Rigidbody>().AddForce(transform.forward * thrust);
        }


    }

}
