using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;

    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - (player1.transform.position+player2.transform.position)/2;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = (player1.transform.position+player2.transform.position)/2 + offset;
    }
}

