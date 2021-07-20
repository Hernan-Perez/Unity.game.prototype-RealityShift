using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPP2 : MonoBehaviour
{

    public float sensitivityX = 15F;
    public float sensitivityY = 15F;

    public float fuerzaMov = 10f;
    public float fuerzaMovLateral = 7f;
    public float fuerzaSalto = 20f;
    private float cdSalto = 0f;
    public float fuerzaGravedad = 1f;

    private float rotationX, rotationY;

    public float maxVel = 3f;

    private Vector3 sentidoGravedad;
    private bool pausaGravedad = false;

    private Rigidbody rb;

    //private float tiempoEnElAire = 0f;
    private bool lockControles = false;

    private Vector3[] dirOrtogonales;
    private Vector3 dirOrtogonal;

    private Vector3 vUp, vLeft;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb)
            rb.freezeRotation = true;

        sentidoGravedad = -Vector3.up;
        vUp = Vector3.up;
        vLeft = Vector3.left;
        dirOrtogonales = new Vector3[6];
        dirOrtogonales[0] = new Vector3(1, 0, 0);
        dirOrtogonales[1] = new Vector3(-1, 0, 0);
        dirOrtogonales[2] = new Vector3(0, 1, 0);
        dirOrtogonales[3] = new Vector3(0, -1, 0);
        dirOrtogonales[4] = new Vector3(0, 0, 1);
        dirOrtogonales[5] = new Vector3(0, 0, -1);

        GameObject.FindGameObjectWithTag("RespawnPoint").transform.position = transform.position;
    }

    void Update()
    {
        if (cdSalto > 0f)
        {
            cdSalto -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        /*if (CONFIG.PAUSA)
        {
            return;
        }*/

        if (!pausaGravedad)
        {
            rb.AddForce(sentidoGravedad * fuerzaGravedad);
        }

        if (!lockControles)
        {
            if (true)
            {
                float rotationX = Input.GetAxis("Mouse X") * sensitivityX;

                rotationY = Input.GetAxis("Mouse Y") * sensitivityY;
                //Debug.Log(rotationY);
                //Debug.Log(ang);
                Vector3 vaux = Quaternion.AngleAxis(rotationY, Vector3.Cross(vUp, transform.forward)) * transform.forward;
                float ang = Vector3.Angle(vUp, vaux);
                //Debug.Log(ang + "----" + Vector3.Angle(vUp, vaux));
                if ( (ang < 15f && rotationY > 0f) || (ang > 165f && rotationY < 0f))
                {
                    rotationY = 0;
                }
                transform.Rotate(vUp, rotationX, Space.World);
                transform.Rotate(Vector3.left, rotationY);

            }


            Vector3 dir = new Vector3(transform.forward.x, transform.forward.y, transform.forward.z);

            if (Mathf.Abs(sentidoGravedad.x) >= 0.5f)
            {
                dir.x = 0;
            }
            if (Mathf.Abs(sentidoGravedad.y) >= 0.5f)
            {
                dir.y = 0;
            }
            if (Mathf.Abs(sentidoGravedad.z) >= 0.5f)
            {
                dir.z = 0;
            }
            dir.Normalize();

            if (Input.GetKey(KeyCode.W))
            {
                rb.AddForce(dir * fuerzaMov);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                rb.AddForce(-dir * fuerzaMov);
            }

            if (Input.GetKey(KeyCode.A))
            {
                rb.AddForce((Vector3.Cross(dir, vUp)) * fuerzaMovLateral);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                rb.AddForce(-(Vector3.Cross(dir, vUp)) * fuerzaMovLateral);
            }

            if (Input.GetKeyDown(KeyCode.Space) && IsGrounded() && cdSalto <= 0f)
            {
                rb.AddForce(vUp * fuerzaSalto, ForceMode.Impulse);
                cdSalto = 0.7f;
            }
        }

        //ajusto la vel maxima

        Vector3 vel2d = new Vector3(
            rb.velocity.x * ((Mathf.Abs(sentidoGravedad.x) > 0.5f) ? (0f) : (1f)), 
            rb.velocity.y * ((Mathf.Abs(sentidoGravedad.y) > 0.5f) ? (0f) : (1f)), 
            rb.velocity.z * ((Mathf.Abs(sentidoGravedad.z) > 0.5f) ? (0f) : (1f)));

        if (vel2d.magnitude > maxVel)
        {
            vel2d = vel2d.normalized * 3f;
            //rb.velocity = new Vector3(vel2d.x, rb.velocity.y, vel2d.z);
            rb.velocity = new Vector3(
                ((Mathf.Abs(sentidoGravedad.x) > 0.5f) ? (rb.velocity.x) : (vel2d.x)),
                ((Mathf.Abs(sentidoGravedad.y) > 0.5f) ? (rb.velocity.y) : (vel2d.y)),
                ((Mathf.Abs(sentidoGravedad.z) > 0.5f) ? (rb.velocity.z) : (vel2d.z)) );
        }

        
    }

    public void RecalcularGravedad(Transform t)
    {
        transform.position = t.position;
        transform.rotation = t.rotation;

        sentidoGravedad = -t.transform.up;
        vUp = transform.up;
        vUp = CalcularDirOrtogonalCercana(vUp);

        lockControles = false;

        rb.velocity = new Vector3();
        rb.AddForce(t.forward * fuerzaSalto, ForceMode.Impulse);
    }

    private Vector3 CalcularDirOrtogonalCercana(Vector3 v)
    {
        int index = 0;
        float lastDist = 1000;

        for (int i = 0; i < 6; i++)
        {
            if (Vector3.Angle(v, dirOrtogonales[i]) < lastDist)
            {
                index = i;
                lastDist = Vector3.Angle(this.transform.forward, dirOrtogonales[i]);
            }
        }

        return dirOrtogonales[index];
    }

    private bool IsGrounded()
    {
        float distToGround = 1.5f;
        return Physics.Raycast(transform.position, -vUp, distToGround + 0.1f) && GetVelocidadVerticalAbs() < 2f;
    }

    //usado por IsGrounded
    private float GetVelocidadVerticalAbs()
    {
        if (Mathf.Abs(sentidoGravedad.x) >= 0.5f)
        {
            return Mathf.Abs(rb.velocity.x);
        }
        if (Mathf.Abs(sentidoGravedad.y) >= 0.5f)
        {
            return Mathf.Abs(rb.velocity.y);
        }
        if (Mathf.Abs(sentidoGravedad.z) >= 0.5f)
        {
            return Mathf.Abs(rb.velocity.z);
        }

        return 0f;
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        angle = angle % 360;
        if ((angle >= -360F) && (angle <= 360F))
        {
            if (angle < -360F)
            {
                angle += 360F;
            }
            if (angle > 360F)
            {
                angle -= 360F;
            }
        }
        return Mathf.Clamp(angle, min, max);
    }

    private void OnGUI()
    {
        GUI.color = new Color(1f, 1f, 0.1f, 0.5f);
        //GUI.DrawTexture(new Rect(Screen.width * 0.5f - 8, Screen.height * 0.4f - 8, 16, 16), mira);
        GUI.color = Color.white;
    }
}
