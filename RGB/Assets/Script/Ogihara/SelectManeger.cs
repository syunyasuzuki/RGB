using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectManeger : MonoBehaviour
{
    Button button;
    private int stage { get; set; } = 0;
    private int worldMap { get; set; } = 0;
    CreateBox CreativBox;
    void Start()
    {
        button = GameObject.Find("Canvas/ButtonMain/Button").GetComponent<Button>();
        button.Select();
        GameObject g = new GameObject("CreateBox");
        CreativBox = g.AddComponent<CreateBox>();
    }

        // Start is called before the first frame update
        public void NextSelsect()
        {
        FadeManeger.Fade_flag_in = true;
        Invoke(nameof(LoadSelect), 1.0f);        
        }
    void LoadSelect()
    {
        SceneManager.LoadScene("SelectScene");
    }
    public  void OnClickStart1_1()
    {
        FadeManeger.Fade_flag_in = true;
        Invoke(nameof(LoadStage1_1), 1.0f);       
    }
    void LoadStage1_1()
    {         

        worldMap = 0;
        stage = 0;
        CreativBox.Set_num(worldMap, stage);
        SceneManager.LoadScene("GameSampleScene");
    }
    public void OnClickStart1_2()
    {
        FadeManeger.Fade_flag_in = true;
        Invoke(nameof(LoadStage1_2), 1.0f);
    }
    void LoadStage1_2()
    {      
        //SceneManager.LoadScene("1-2");
        worldMap = 0;
        stage = 1;
        CreativBox.Set_num(worldMap, stage);
        SceneManager.LoadScene("GameSampleScene");

    }
    public void OnClickStart1_3()
    {
        FadeManeger.Fade_flag_in = true;
        Invoke(nameof(LoadStage1_3), 1.0f);
    }
    void LoadStage1_3()
    {
        //SceneManager.LoadScene("1-3");
        worldMap= 0;
        stage = 2;
        CreativBox.Set_num(worldMap, stage);
        SceneManager.LoadScene("GameSampleScene");
    }

    public void OnClickStart1_4()
    {
        FadeManeger.Fade_flag_in = true;
        Invoke(nameof(LoadStage1_4), 1.0f);
    }
    void LoadStage1_4()
    {
        //SceneManager.LoadScene("1-4");
        worldMap = 0;
        stage = 3;
        CreativBox.Set_num(worldMap, stage);
        SceneManager.LoadScene("GameSampleScene");
    }
    public void OnClickStart2_1()
    {
        FadeManeger.Fade_flag_in = true;
        Invoke(nameof(LoadStage2_1), 1.0f);
    }
    void LoadStage2_1()
    {
        //SceneManager.LoadScene("2-1");
        worldMap = 0;
        stage = 4;
        CreativBox.Set_num(worldMap, stage);
        SceneManager.LoadScene("GameSampleScene");
    }
    public void OnClickStart2_2()
    {
        FadeManeger.Fade_flag_in = true;
        Invoke(nameof(LoadStage2_2), 1.0f);
    }
    void LoadStage2_2()
    {
        //SceneManager.LoadScene("2-2");
        worldMap = 0;
        stage = 5;
        CreativBox.Set_num(worldMap, stage);
        SceneManager.LoadScene("GameSampleScene");
    }
    public void OnClickStart2_3()
    {
        FadeManeger.Fade_flag_in = true;
        Invoke(nameof(LoadStage2_3), 1.0f);
    }
    void LoadStage2_3()
    {
        //SceneManager.LoadScene("2-3");
        worldMap = 0;
        stage = 6;
        CreativBox.Set_num(worldMap, stage);
        SceneManager.LoadScene("GameSampleScene");
    }
    public void OnClickStart2_4()
    {
        FadeManeger.Fade_flag_in = true;
        Invoke(nameof(LoadStage2_4), 1.0f);
    }
    void LoadStage2_4()
    {
        //SceneManager.LoadScene("2-4");
        worldMap = 0;
        stage = 7;
        CreativBox.Set_num(worldMap, stage);
        SceneManager.LoadScene("GameSampleScene");
    }
     public void OnClickStart3_1()
    {
        FadeManeger.Fade_flag_in = true;
        Invoke(nameof(LoadStage3_1), 1.0f);
    }
    void LoadStage3_1()
    {
        //SceneManager.LoadScene("3-1");
        worldMap = 0;
        stage = 8;
        CreativBox.Set_num(worldMap, stage);
        SceneManager.LoadScene("GameSampleScene");
    }
     public void OnClickStart3_2()
    {
        FadeManeger.Fade_flag_in = true;
        Invoke(nameof(LoadStage3_2), 1.0f);
    }
    void LoadStage3_2()
    {
        //SceneManager.LoadScene("3-2");
        worldMap = 0;
        stage = 9;
        CreativBox.Set_num(worldMap, stage);
        SceneManager.LoadScene("GameSampleScene");
    }
    public void OnClickStart3_3()
    {
        FadeManeger.Fade_flag_in = true;
        Invoke(nameof(LoadStage3_3), 1.0f);
    }
    void LoadStage3_3()
    {
        //SceneManager.LoadScene("3-3");
        worldMap = 0;
        stage = 10;
        CreativBox.Set_num(worldMap, stage);
        SceneManager.LoadScene("GameSampleScene");
    }
    public void OnClickStart3_4()
    {
        FadeManeger.Fade_flag_in = true;
        Invoke(nameof(LoadStage3_4), 1.0f);
    }
    void LoadStage3_4()
    {
        //SceneManager.LoadScene("3-4");
        worldMap = 0;
        stage = 11;
        CreativBox.Set_num(worldMap, stage);
        SceneManager.LoadScene("GameSampleScene");
    }
    public void NextSelect2()
    {
        FadeManeger.Fade_flag_in = true;
        Invoke(nameof(LoadSelect2), 1.0f);
    }
    void LoadSelect2()
    {
        CreativBox.deletegameobj();
        SceneManager.LoadScene("SelectScene2");
    }
    public void NextSelect3()
    {
        FadeManeger.Fade_flag_in = true;
        Invoke(nameof(LoadSelect3), 1.0f);
    }
    void LoadSelect3()
    {
        CreativBox.deletegameobj();
        SceneManager.LoadScene("SelectScene3");
    }

    void Update()
    {

    }
}

