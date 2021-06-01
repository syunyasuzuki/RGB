using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManeger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Return))
        {
            Invoke(nameof(LoadSelect), 0.5f);
        }
    }

    void LoadSelect()
    {
        SceneManager.LoadScene("SelectScene");
    }
}
