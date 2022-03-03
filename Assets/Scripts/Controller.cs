using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    public Text gameOver_txt;

    [HideInInspector]
    public bool playerStop;

    [Header("")]
    public GameObject player;
   
    public GameObject[] all_btn;


    private void Awake()
    {
        Time.timeScale = 1;
        //for wind
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }


    }




    // Start is called before the first frame update
    void Start()
    {
        gameOver_txt.enabled = false;
      

        playerStop = false;
       


        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            for (int i = 0; i < all_btn.Length; i++)
            {
                all_btn[i].SetActive(false);
            }
        }


    }

    private void LateUpdate()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }

    public void restartBtn()
    {       
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);      
    }


    public void Text_Panel(string state) {
        gameOver_txt.text = state;
        gameOver_txt.enabled = true;
    }

}
