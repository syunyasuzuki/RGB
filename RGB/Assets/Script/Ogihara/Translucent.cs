using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Translucent : MonoBehaviour
{
    float alpha;
    string _tag;

    // Start is called before the first frame update
    void Start()
    {
        //my_name = this.gameObject.tag;       
        alpha = 0.3f;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, alpha);     
        _tag = gameObject.tag;
    }

    float blinkingtime;   
    void Measurement()
    {
        blinkingtime +=Time.deltaTime;
      if(blinkingtime % 4 >0 && blinkingtime % 4 <= 0.01f)
        {
            alpha = 0.3f;
        }
        switch (_tag)
        {
            case "Red":              
                if(blinkingtime%4>= 0.99f && blinkingtime % 4 <= 1.01f)
                {
                    alpha = 1;
                }
                if (blinkingtime % 4 >= 1.99f && blinkingtime % 4 <= 2.01f)
                {
                    alpha = 0.3f;
                }
                ; break;         

            case "Green":          
                if(blinkingtime%4>=1.99f && blinkingtime % 4 <= 2.01f)
                {
                    alpha = 1;
                }
                if (blinkingtime % 4 >= 2.99f && blinkingtime % 4 <= 3.01f)
                {
                    alpha = 0.3f;
                }
                break;

            case "Blue":
                if (blinkingtime % 4>=2.99f  && blinkingtime % 4 <=3.01f )
                {
                    alpha = 1;
                }              
                break;       
        };
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, alpha);

    }
    // Update is called once per frame
    void Update()
        {        
            Measurement();
        }
}

