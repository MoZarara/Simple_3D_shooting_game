using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController m_char;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 3.0f;
    private float jumpHeight = 2.0f;
    private float gravityValue = -22.81f;

    private Animator anim;
    private bool inJump;

    [HideInInspector]
    public bool m_Jump;
    [HideInInspector]
    public float hori_Input;
    [HideInInspector]
    public float ver_Input;

    private Transform m_Cam;
    private Vector3 m_CamForward;
    private Vector3 m_Move;

    


    public Controller controller;


    [Header("")]
    private AudioSource audioSource;

    public bool jumpNow = false;




    public float rangeWeapon = 100;
    public ParticleSystem flashWeapon;

    public GameObject hitEffect;
    public Transform pointShoot;


   

    private void Start()
    {
        if (Camera.main != null)
        {
            m_Cam = Camera.main.transform;
            
        }
        else
        {
            Debug.LogWarning(
                "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
            // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
        }

        

        m_char = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>();
        
    }


    void Update()
    {
        if (!controller.playerStop)
        {


            groundedPlayer = m_char.isGrounded;
            if (groundedPlayer && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;

            }

            float xHorizontal = Input.GetAxis("Horizontal") ;
            float zVertical = Input.GetAxis("Vertical")  ;

            


            if (m_Cam != null)
            {
                // calculate camera relative direction to move:
                transform.rotation = Quaternion.LookRotation(m_Cam.forward, m_Cam.up);
                

                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                //for android
                if (Application.platform == RuntimePlatform.Android)
                {
                    m_Move = ver_Input * m_CamForward + hori_Input * m_Cam.right;
                }
                
                // for wind
                if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
                {
                    m_Move = zVertical * m_CamForward + xHorizontal * m_Cam.right;
                }
                
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                m_Move = ver_Input * Vector3.forward + hori_Input * Vector3.right;
            }



            m_char.Move( m_Move * Time.deltaTime * playerSpeed);


            if (m_Move != Vector3.zero)
            {

                if (!inJump && groundedPlayer)
                {
                    if (!anim.GetCurrentAnimatorStateInfo(0).IsName("shoot"))
                    {
                        anim.Play("walk");

                    }
                   
                
                }
            }
            else
            {
                if (!inJump && groundedPlayer)
                {
                    if (!anim.GetCurrentAnimatorStateInfo(0).IsName("shoot"))
                    {
                        anim.Play("idle");
                    }
                 

                }
            }




            Jump();

            if (Input.GetKeyDown("space"))
            {
                m_Jump = true;
                Jump();
            }



            if (groundedPlayer)
            {
                if (m_Jump)
                {
                    jumpNow = true;
                }
                else
                {
                    jumpNow = false;
                }


                inJump = false;
            }
            else
            {
                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("shoot"))
                {
                    anim.Play("idle");
                }
              
            }





            playerVelocity.y += gravityValue * Time.deltaTime;
            m_char.Move(playerVelocity * Time.deltaTime);

        }
        else
        {
            if (m_char.isGrounded)
            {
                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("shoot"))
                {
                    anim.Play("idle");
                }
                
            }

        }


        //for wind      
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor) {
            if (Input.GetMouseButtonDown(0))
                shootBtn();
        }


        
    }

    private void Jump() {
        if (!jumpNow && m_Jump && groundedPlayer)
        {


            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);

            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("shoot"))
            {
                anim.Play("idle");
            }

         

            inJump = true;

        }
    }

    public void shootBtn() {
        anim.Play("shoot");
        flashWeapon.Play();
        audioSource.Play();
       

        

        RaycastHit hit;

        
        if (Physics.Raycast(m_Cam.transform.position, m_Cam.transform.forward, out hit, rangeWeapon))
        {
           

            if (hit.transform.tag == "Enemy") {
                hit.transform.GetComponent<EnemyAI>().Died();

            }
           
        }
        else {
            return;
        }


    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            controller.Text_Panel("Game Over");
            Time.timeScale = 0;
            controller.playerStop = true;
        }

        if (other.tag == "Finish") {
            controller.Text_Panel("Congratulation");
            Time.timeScale = 0;
            controller.playerStop = true;
        }
    }
    


}
