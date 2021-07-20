using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            other.GetComponent<Rigidbody>().velocity = new Vector3();
            other.transform.position = GameObject.FindGameObjectWithTag("RespawnPoint").transform.position;
            other.GetComponent<FPP2>().RecalcularGravedad(transform);
        }
    }
}
