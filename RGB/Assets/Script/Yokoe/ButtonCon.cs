using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class ButtonCon : MonoBehaviour
{
    [SerializeField] GameObject MenuPanel;

    CreateBox createbox = null;

    float invoke_time = 0.5f;

    void Start()
    {
        MenuPanel.SetActive(false);
        Button button = GameObject.Find("Canvas/Panel/menu/SELECT").GetComponent<Button>();
        button.Select();
        createbox = GameObject.Find("CreateBox").GetComponent<CreateBox>();
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            MenuPanel.SetActive(true);
            Time.timeScale = 0.0f;
        }
    }

    public void select()
    {
        Time.timeScale = 1.0f;
        createbox.deletegameobj();
        Invoke(nameof(selectLoad), invoke_time);
    }

    void selectLoad()
    {
        SceneManager.LoadScene("SelectScene");
    }

    public void title()
    {
        Time.timeScale = 1.0f;
        createbox.deletegameobj();
        Invoke(nameof(titleLoad), invoke_time);
    }

    void titleLoad()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void Back()
    {
        MenuPanel.SetActive(false);
        Time.timeScale = 1.0f;
    }
}
