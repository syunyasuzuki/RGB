using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeManeger : MonoBehaviour
{
    float alpha;
    public static bool Fade_flag_in, Fade_flag_out,Fade;
    
    // Start is called before the first frame update
    void Start()
    {
        alpha = 1;
        Fade = true;
        Fade_flag_in = true;
        Fade_flag_out = false;
        gameObject.GetComponent<Image>().color = new Color(0, 0, 0, alpha);
    }

    // Update is called once per frame
    void Update()
    {
        Scene gameSc = SceneManager.GetActiveScene();
        if (gameSc.name != "GameSampleScene")
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Fade_flag_out = true;
            }
        }
     
         if(Fade == true)
        {
            if(Fade_flag_in == true)
            {
                FadeIn();
            }
            if(Fade_flag_out == true)
            {
                FadeOut();
            }
        }
    }

    void FadeIn()
    {
        alpha -= 2.5f * Time.deltaTime;
        if (alpha <= 0)
        {
            alpha = 0;
            Fade_flag_in = false;
        }       
        gameObject.GetComponent<Image>().color = new Color(0, 0, 0, alpha);
    }

    void FadeOut()
    {
        alpha += 2.5f * Time.deltaTime;
        if (alpha >= 1)
        {
            alpha = 1;
            Fade_flag_out = false;
        }       
        gameObject.GetComponent<Image>().color = new Color(0, 0, 0, alpha);
    }
}
