using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDisplacer : MonoBehaviour {

    public enum Eje {X, Y, Z };
    public Eje ejeDeMovimiento = Eje.X;

    public float maxOffset = 4f;
    public float minOffset = -4f;
    public float currentOffset = 0f;
    public bool incrementando = true;
    public float velocidad = 1f;
    public float delayStartTime = 0f;

    private float origen;

	// Use this for initialization
	void Start () {
        origen = 0f;
        switch (ejeDeMovimiento)
        {
            case Eje.X:
                origen = transform.position.x;
                break;
            case Eje.Y:
                origen = transform.position.y;
                break;
            case Eje.Z:
                origen = transform.position.z;
                break;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (delayStartTime > 0f)
        {
            delayStartTime -= Time.deltaTime;
            return;
        }

        if (incrementando)
        {
            currentOffset += velocidad * Time.deltaTime;
            if (currentOffset >= maxOffset)
            {
                currentOffset -= (currentOffset - maxOffset); // si se pasó por 0.20 entonces los retrocede en la otra direccion
                incrementando = false;
            }
        }
        else
        {
            currentOffset -= velocidad * Time.deltaTime;
            if (currentOffset <= minOffset)
            {
                currentOffset -= (currentOffset - minOffset); // si se pasó por 0.20 entonces los retrocede en la otra direccion
                incrementando = true;
            }
        }

        switch (ejeDeMovimiento)
        {
            case Eje.X:
                transform.position = new Vector3(currentOffset + origen, transform.position.y, transform.position.z);
                break;
            case Eje.Y:
                transform.position = new Vector3(transform.position.x, currentOffset + origen, transform.position.z);
                break;
            case Eje.Z:
                transform.position = new Vector3(transform.position.x, transform.position.y, currentOffset + origen);
                break;
        }
    }
}
