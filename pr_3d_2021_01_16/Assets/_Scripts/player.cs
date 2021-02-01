using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public Camera cam;
    Rigidbody rb;
    public int dir_grv;
    public float grv_const;
    private Animator anim;
    public float camdist , camxspd, camyspd, camx = 0f, camy = 0f, camymin, camymax,spd,lerpspd;
    public Quaternion camrot,qt_grv;




    // Start is called before the first frame update
    void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        anim = this.GetComponent<Animator>();
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //커서 고정
        Vector3 angles = transform.eulerAngles;
        camx = angles.y;
        camy = angles.x;
    }

    // Update is called once per frame
    void Update()
    {
        camcontrol();
        move();
        grav();
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (dir_grv == 6)
            {
                dir_grv = 1;
            }
            else
            { 
                dir_grv++;
            }
        }
    }

    void move() //카메라 관련해서 참고할 적절한 예시가 안됩니다. 이거 뭔가 꼬임.
    {
        if (rb.velocity.magnitude > -10f & rb.velocity.magnitude< 10f)
        {
            if (Input.GetKey(KeyCode.A))
            {
                Vector3 tmpdir = Quaternion.Euler(0, camx+90f, 0) * new Vector3(-spd, 0, 0);
                rb.velocity += tmpdir;
                this.transform.rotation=Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(0, camx-90, 0), Time.deltaTime*lerpspd);
                anim.SetInteger("state", 1);
            }
            if (Input.GetKey(KeyCode.S))
            {
                Vector3 tmpdir = Quaternion.Euler(0, camx+90f, 0) * new Vector3(0, 0, -spd);
                rb.velocity += tmpdir;
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(this.transform.rotation.x, camx+180, this.transform.rotation.z), Time.deltaTime*lerpspd);
                anim.SetInteger("state", 1);
            }
            if (Input.GetKey(KeyCode.D))
            {
                Vector3 tmpdir = Quaternion.Euler(0, camx+90f, 0) * new Vector3(spd, 0, 0);
                rb.velocity += tmpdir;
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(this.transform.rotation.x, camx+90, this.transform.rotation.z), Time.deltaTime*lerpspd);
                anim.SetInteger("state", 1);
            }
            if (Input.GetKey(KeyCode.W))
            {
                Vector3 tmpdir = Quaternion.Euler(0, camx+90f, 0) * new Vector3(0, 0, spd);
                rb.velocity += tmpdir;
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(this.transform.rotation.x, camx, this.transform.rotation.z), Time.deltaTime*lerpspd);
                anim.SetInteger("state", 1);
            }
            if (!Input.GetKey(KeyCode.W) & !Input.GetKey(KeyCode.A) & !Input.GetKey(KeyCode.S) & !Input.GetKey(KeyCode.D))
            {
                anim.SetInteger("state", 0);
            }
        }
    }
    void grav() // 1:+x 2:-x 3:+y 4:-y 5:+z 6:-z
    {
        if (dir_grv == 1)
        {
            rb.AddForce( Vector3.right*grv_const);
            qt_grv = Quaternion.Euler(0, camx, -90);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, qt_grv, Time.deltaTime * lerpspd);
        }
        else if (dir_grv == 2)
        {
            rb.AddForce( Vector3.left * grv_const);
            qt_grv = Quaternion.Euler(0, camx, 90);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, qt_grv, Time.deltaTime * lerpspd);
        }
        else if (dir_grv == 3)
        {
            rb.AddForce( Vector3.up * grv_const);
            qt_grv = Quaternion.Euler(0, camx, 180);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, qt_grv, Time.deltaTime * lerpspd);
        }
        else if (dir_grv == 4)
        {
            rb.AddForce( Vector3.down * grv_const);
            qt_grv = Quaternion.Euler(0, camx, 0);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, qt_grv, Time.deltaTime * lerpspd);
        }
        else if (dir_grv == 5)
        {
            rb.AddForce( Vector3.forward * grv_const);
            qt_grv = Quaternion.Euler(90, camx, 0);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, qt_grv, Time.deltaTime * lerpspd);
        }
        else if (dir_grv == 6)
        {
            rb.AddForce( Vector3.back * grv_const);
            qt_grv = Quaternion.Euler(-90, camx, 0);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, qt_grv, Time.deltaTime * lerpspd);
        }
    }
    void camcontrol()
    {
        camx += Input.GetAxis("Mouse X") * camxspd * 0.015f;
        camy -= Input.GetAxis("Mouse Y") * camyspd * 0.015f;
        camdist -= Input.GetAxis("Mouse ScrollWheel")*2f ;
        camy = ClampAngle(camy, camymin, camymax);
        camdist = ClampAngle(camdist,4f,20f);
        camrot = Quaternion.Euler(camy, camx+90f, 0);
        Vector3 campos = camrot * new Vector3(0, 0.9f, -camdist) + this.transform.position + new Vector3(0.0f, 0, 0.0f);
        cam.transform.rotation = camrot;
        cam.transform.position = campos;
        
    }
    float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }

   
}

