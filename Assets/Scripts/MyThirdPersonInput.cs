using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyThirdPersonInput : MonoBehaviour
{
    public FixedJoystick leftJoystick;
    public FixedButton button;
    public FixedTouchField TouchField;
    protected PlayerMovement control;


    protected float CameraAngle;
    protected float CameraAngleSpeed = 0.2f;

    [Header("")]
    public Controller controller;


    



    // Start is called before the first frame update
    void Start()
    {
        control = GetComponent<PlayerMovement>();     
    }

    // Update is called once per frame
    void Update()
    {
        if (!controller.playerStop)
        {
           
                control.m_Jump = button.Pressed;

            


                control.hori_Input = leftJoystick.Direction.x;
                control.ver_Input = leftJoystick.Direction.y;
           

            //for android          
            if (Application.platform == RuntimePlatform.Android)
            {
                CameraAngle += TouchField.TouchDist.x * CameraAngleSpeed;
            }

            //for wind
            if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
            {
                CameraAngle += (Input.GetAxis("Mouse X") * 30) * CameraAngleSpeed;
            }
            



            Camera.main.transform.position = transform.position + Quaternion.AngleAxis(CameraAngle, Vector3.up) * new Vector3(2.0f, 2.3f, -3.14f);
            Camera.main.transform.rotation = Quaternion.LookRotation(transform.position + Vector3.up * 1.9f - Camera.main.transform.position, Vector3.up);



        }

    }
}
