using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal_ctr : MonoBehaviour
{
    float rot_y;

    float rot_count;

    bool rot_ON;

    // Start is called before the first frame update
    void Start()
    {
        rot_ON = false;
    }

    // Update is called once per frame
    void Update()
    {
        rot_count += 1.0f * Time.deltaTime;
        if(rot_count >= 8.0f)
        {
            rot_ON = true;
        }

        if(rot_ON == true)
        {
            GoalRotation();
        }
    }
    void GoalRotation()
    {
        rot_y += 400.0f * Time.deltaTime;
        if(rot_y >= 360f)
        {
            rot_y = 0.0f;
            rot_count = 0.0f;
            rot_ON = false;
        }

        transform.eulerAngles = new Vector3(0.0f, rot_y, 0.0f);
    }
}
