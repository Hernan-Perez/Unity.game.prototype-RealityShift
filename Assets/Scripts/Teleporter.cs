using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour {

    public GameObject teleporterDestino;
    private bool playerLock = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player" && !playerLock && teleporterDestino != null)
        {
            teleporterDestino.transform.GetChild(0).GetComponent<Teleporter>().RecibirPlayer();
            other.GetComponent<FPP2>().RecalcularGravedad(teleporterDestino.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            playerLock = false;
        }
    }

    public void RecibirPlayer()
    {
        playerLock = true;
    }
}
