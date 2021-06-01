using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class MapEditor : MonoBehaviour
{
    [SerializeField] GameObject[] bc = new GameObject[8];

    /// <summary>
    /// フォルダパス
    /// </summary>
    string fopath;

    /// <summary>
    /// ファイルパス
    /// </summary>
    string fipath;

    /// <summary>
    /// フォルダとファイルへのパスを設定
    /// </summary>
    void Set_path()
    {
        fopath = Application.dataPath + @"\Resources";
        fipath = Path.Combine(fopath, "mapdata.txt");
    }

    /// <summary>
    /// フォルダーとファイルが存在するか確認、存在しない場合作成する
    /// </summary>
    void Folder_File_check_andCreate()
    {
        //テキストの書き込み
        StreamWriter datafile = null;

        //フォルダが存在するかどうか
        if (!Directory.Exists(fopath))
        {
            Debug.Log("フォルダが存在しません");
            try
            {
                Debug.Log("フォルダを作成します");
                Directory.CreateDirectory(fopath);
                if (!Directory.Exists(fopath))
                {
                    Debug.Log("フォルダを作成しました");
                }
            }
            catch
            {
                Debug.Log("フォルダ作成に失敗しました");
            }
        }
        else
        {
            Debug.Log("フォルダの存在を確認しました");
        }

        //ファイルが存在するか
        if (!File.Exists(fipath))
        {
            Debug.Log("ファイルが存在しません");
            try
            {
                Debug.Log("ファイルを作成します");
                datafile = File.CreateText(fipath);
                if (!File.Exists(fipath))
                {
                    Debug.Log("ファイルを作成しました");
                }
            }
            catch
            {
                Debug.Log("ファイル作成に失敗しました");
            }
            finally
            {
                datafile.Dispose();
            }
        }
        else
        {
            Debug.Log("ファイルの存在を確認しました");
        }
    }

    /// <summary>
    /// メインカメラ
    /// </summary>
    Camera maincam;

    /// <summary>
    /// カメラのpositionのZ座標
    /// </summary>
    float camera_z = -10;

    /// <summary>
    /// カメラを取得
    /// </summary>
    void Set_camera_z()
    {
        maincam = Camera.main;
        camera_z = transform.position.z;
        float camera_x = (Maxsize_x - 1) / 2.0f;
        float camera_y = (Maxsize_y - 1) / 2.0f * -1;
        int min = Mathf.Min(Maxsize_x, Maxsize_y);
        float camera_size = /*min / 2.0f*/7.8f;
        maincam.transform.position = new Vector3(camera_x, camera_y, camera_z);
        maincam.orthographicSize = camera_size;
    }

    /// <summary>
    /// カメラサイズを変更する速度
    /// </summary>
    float Move_camera_size = 1.0f;

    /// <summary>
    /// カメラの移動速度
    /// </summary>
    float Move_camera_xy = 0.05f;

    /// <summary>
    /// カメラの位置を調整
    /// </summary>
    void Set_camera()
    {
        float scr = Input.GetAxisRaw("Mouse ScrollWheel");
        float camsize = Move_camera_size * scr;
        maincam.orthographicSize = maincam.orthographicSize + camsize;
        int x = 0, y = 0;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) { x -= 1; }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) { x += 1; }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) { y += 1; }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) { y -= 1; }
        transform.position = new Vector3(transform.position.x + Move_camera_xy * x, transform.position.y + Move_camera_xy * y, transform.position.z);
    }

    /// <summary>
    /// カーソルの補助
    /// </summary>
    GameObject mouse_point;

    /// <summary>
    /// マウスポインタの作成
    /// </summary>
    void Create_mouse_point()
    {
        mouse_point = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        mouse_point.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
    }

    /// <summary>
    /// 最大で保存できるマップの数
    /// </summary>
    const int Maxmap_num = 12;

    /// <summary>
    /// マップのｘの最大サイズ
    /// </summary>
    const int Maxsize_x = 20;
    /// <summary>
    /// マップのｙの最大サイズ
    /// </summary>
    const int Maxsize_y = 10;

    /// <summary>
    /// 読み込んだファイルデータ
    /// </summary>
    string[] all_data;
    int map_n = 10;
    /// <summary>
    /// 現在読み込み中のマップ番号(x-1=n)
    /// </summary>
    int map_num = 0;

    /// <summary>
    /// 現在選択中のブロック
    /// </summary>
    int now_block = 0;

    /// <summary>
    /// マップの情報を格納
    /// </summary>
    int[,,] map;

    /// <summary>
    /// intの桁数を調べる(何文字入力したか確認)
    /// </summary>
    int Int_length(int n)
    {
        return (int)Mathf.Log10(n) + 1;
    }

    /// <summary>
    /// 配列の確保
    /// </summary>
    void First_setting()
    {
        map = new int[Maxmap_num, Maxsize_y, Maxsize_x];
    }

    /// <summary>
    /// 読み込んだデータから空白を消す
    /// </summary>
    void Clear_reeddata()
    {
        for (int i = 0; i < all_data.Length; ++i)
        {
            while (all_data[i].Substring(0, 1) == " ")
            {
                all_data[i] = all_data[i].Substring(1);
            }
        }

    }

    /// <summary>
    /// マップ本体を配列に変換
    /// </summary>
    void Read_M_line(int n)
    {
        for (int i = 0; i < n; ++i)
        {
            //変換するためのデータを入れる
            string[] str1 = new string[Maxsize_y];
            for (int lu = 0; lu < Maxsize_y; ++lu)
            {
                str1[lu] = all_data[lu + (Maxsize_y + 1) * i + 1];

                //文字列をばらして配列に入れる
                string[] str3 = str1[lu].Split(',');
                //文字列をint型にしてマップに書き込み
                for (int na = 0; na < Maxsize_x; ++na)
                {
                    map[i, lu, na] = int.Parse(str3[na]);
                }
            }
        }
    }

    /// <summary>
    /// ファイルがなかった時の初期設定
    /// </summary>
    void Setup_firstmapdata()
    {
        map_num = 0;
        map_n = Maxmap_num;
    }

    /// <summary>
    /// 読み込んだファイルデータをもとに現在保存されているマップ情報を作成
    /// </summary>
    void Set_mapdata()
    {
        //テキストデータの順序
        //1行目マップの数
        map_n = int.Parse(all_data[0]);
        if (map_n > Maxmap_num) { map_n = Maxmap_num; }
        //2～以降配列データ
        Read_M_line(map_n);
    }

    /// <summary>
    /// マップの範囲が分かるように枠を表示
    /// </summary>
    void Create_Mapgrid()
    {
        float def_hw = 0.4f;
        GameObject mapgr = new GameObject("mapgr");
        int w_size = Maxsize_x;
        float w_pos = (Maxsize_x - 1) / 2.0f;
        GameObject loof = GameObject.CreatePrimitive(PrimitiveType.Cube);
        loof.transform.localScale = new Vector3(w_size, def_hw, 1);
        loof.transform.position = new Vector3(w_pos, 0.7f, 0);
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        floor.transform.localScale = new Vector3(w_size, def_hw, 1);
        floor.transform.position = new Vector3(w_pos, -Maxsize_y + 0.3f, 0);

        int h_size = Maxsize_y;
        float h_pos = (Maxsize_y - 1) / 2.0f;
        GameObject left = GameObject.CreatePrimitive(PrimitiveType.Cube);
        left.transform.localScale = new Vector3(def_hw, h_size, 1);
        left.transform.position = new Vector3(-0.7f, -h_pos, 0);
        GameObject right = GameObject.CreatePrimitive(PrimitiveType.Cube);
        right.transform.localScale = new Vector3(def_hw, h_size, 1);
        right.transform.position = new Vector3(Maxsize_x - 0.3f, -h_pos, 0);

        loof.transform.parent = mapgr.transform;
        floor.transform.parent = mapgr.transform;
        left.transform.parent = mapgr.transform;
        right.transform.parent = mapgr.transform;
    }


    /// <summary>
    /// マップ情報をもとにマップを作成
    /// </summary>
    void Create_Map()
    {
        GameObject mapmother = new GameObject("mapchip");
        for (int i = 0; i < Maxsize_y; ++i)
        {
            for (int lu = 0; lu < Maxsize_x; ++lu)
            {
                if (map[map_num, i, lu] != 0)
                {
                    GameObject ob = Instantiate(bc[map[map_num, i, lu]]);
                    ob.name = "chip_" + i + "_" + lu;
                    ob.transform.position = new Vector3(lu, -i, 0);
                    ob.transform.parent = mapmother.transform;
                }
            }
        }
    }

    /// <summary>
    /// マップを一括削除
    /// </summary>
    void Delete_Map()
    {
        GameObject deathmother = GameObject.Find("mapchip");
        Destroy(deathmother.gameObject);
    }


    /// <summary>
    /// ファイルを一括で読み込む
    /// </summary>
    void Read_data()
    {
        all_data = File.ReadAllLines(fipath, Encoding.GetEncoding("Shift_JIS"));
        if (all_data.Length != 0)
        {
            Debug.Log("読み込みが完了しました");
            Set_mapdata();
            Create_Map();
        }
        else
        {
            Debug.Log("マップデータが存在しません");
            Setup_firstmapdata();
            Create_Map();
        }
        Debug.Log("処理が修了しました");
    }

    /// <summary>
    /// ファイルに書き出す（上書き）
    /// ゲーム開始時にフォルダとファイルがあることは確認済みのため省略
    /// </summary>
    void Write_all_data()
    {
        Debug.Log("ファイルに書き出し");
        //ファイルに書き出す行数を確定
        int Write_num = (Maxsize_y + 1) * map_n + 1;

        string[] write_str = new string[Write_num];

        //マップの数、マップの大きさを確定
        write_str[0] = "" + map_n;

        //マップ情報を確定
        for (int i = 0; i < map_n; ++i)
        {
            int subint = (Maxsize_y + 1) * i + 1;
            for (int lu = 0; lu < Maxsize_y; ++lu)
            {
                string sub_x = "";
                for (int na = 0; na < Maxsize_x; ++na)
                {
                    sub_x = sub_x + map[i, lu, na] + ",";
                }
                sub_x = sub_x.Substring(0, sub_x.Length - 1);
                write_str[subint + lu] = sub_x;
            }
            write_str[subint + Maxsize_y] = ",";
        }

        //テキストに書き出し
        File.WriteAllLines(fipath, write_str);
        Debug.Log("書き出しが完了しました");
    }

    //UIでつかうフォントサイズ
    int Def_fontsize = 15;
    int Max_fontsize = 20;
    //1つ目のボタンの位置
    int px = 400;
    int px_num = 0;
    /// <summary>
    /// 現在選択されているボタン
    /// </summary>
    /// <param name="x">マップの最大数</param>
    void Set_UIpos(int x)
    {
        px_num = x;
    }
    //入力処理
    string text_num = "0";
    //string text_size_x = "0";
    //string text_size_y = "0";
    //各最大入力数
    int max_text_num;
    int max_text_size_x;
    int max_text_size_y;

    /// <summary>
    /// 生成するボタンの数
    /// </summary>
    int max_button = 8;

    /// <summary>
    /// 最大入力数を指定
    /// </summary>
    void GUI_setting()
    {
        max_text_num = Int_length(Maxmap_num);//何文字まで打てるか
    }

    /// <summary>
    /// 入力処理を書き換え
    /// </summary>
    /// <param name="n"></param>
    void Set_text_num(int n)
    {
        text_num = "" + n;
    }

    /// <summary>
    /// 入力処理の確定
    /// </summary>
    void Set_text_num()
    {
        //同じマップを指定した時に大きさを変える
        //違うマップを指定された場合移動するだけ
        int sub_n = 0;
        try
        {
            sub_n = int.Parse(text_num);
        }
        catch
        {
            Debug.LogError("文字列の変換に失敗しました。マップ番号を確認してください");
            return;
        }
        if (map_num != sub_n)
        {
            Delete_Map();
            map_num = sub_n;
            Create_Map();
        }
    }

    //GUI用のテクスチャ
    [SerializeField] Texture2D map_texture = null;
    [SerializeField] Texture2D goal = null;
    [SerializeField] Texture2D player = null;
    Texture2D[] Sliced_texture = new Texture2D[8];
    string[] texture_name = new string[8]
    {
        "空白","","","","","","",""
    };

    /// <summary>
    /// テクスチャを切り抜いて返す
    /// </summary>
    /// <param name="x">x座標のどこから切り抜くか</param>
    /// <param name="y">y座標のどこから切り抜くか</param>
    /// <returns></returns>
    Texture2D Slice_Texture2D(int x, int y)
    {
        Color[] pix = new Color[50 * 50];

        pix = map_texture.GetPixels(x, y, 50, 50);

        Texture2D slice_tex = new Texture2D(50, 50);

        slice_tex.SetPixels(pix);

        slice_tex.Apply();

        return slice_tex;
    }

    /// <summary>
    /// 切り抜き場所を指定
    /// </summary>
    void Set_textures()
    {
        Sliced_texture[0] = default;
        Sliced_texture[1] = Slice_Texture2D(0, 0);
        Sliced_texture[2] = Slice_Texture2D(50, 0);
        Sliced_texture[3] = Slice_Texture2D(200, 0);
        Sliced_texture[4] = Slice_Texture2D(150, 0);
        Sliced_texture[5] = Slice_Texture2D(100, 0);
        Sliced_texture[6] = goal;
        Sliced_texture[7] = player;
    }

    /// <summary>
    /// UIを作成
    /// </summary>
    void OnGUI()
    {
        //画面の比率に応じてサイズが変わるように
        float widthsize = (float)Screen.width / 1280;
        float heightsize = (float)Screen.height / 720;
        GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, new Vector3(widthsize, heightsize, 1));
        //テキストの設定を作成
        GUIStyle Set_text = GUI.skin.label;
        Set_text.fontSize = Def_fontsize;
        //テキストUIを表示
        GUI.Label(new Rect(20, 20, 250, 40), "現在のマップ：" + map_num);
        GUI.Label(new Rect(50, 50, 250, 40), "N");
        GUI.Label(new Rect(px + (50 * px_num), 10, 100, 100), "選択中\nブロック\n ▼", Set_text);
        //ボタンの設定を作成
        GUIStyle Set_button = GUI.skin.button;
        Set_button.fontSize = Def_fontsize;
        Set_button.normal.textColor = Color.white;
        Set_button.normal.background = default;

        //ボタンの表示
        if (GUI.Button(new Rect(200, 70, 40, 40), "決定"))
        {
            Set_text_num();
        }

        if (GUI.Button(new Rect(250, 70, 40, 40), "書出"))
        {
            Write_all_data();
        }
        Set_button.fontSize = Max_fontsize;
        for (int i = 0; i < max_button; ++i) {
            Set_button.normal.background = Sliced_texture[i];
            if (GUI.Button(new Rect(px + (50 * i), 60, 50, 50), texture_name[i]))
            {
                Set_UIpos(i);
                now_block = i;
            }
        }

        //入力テキストの設定を作成
        GUIStyle Set_textfield = GUI.skin.textField;
        Set_textfield.fontSize = Max_fontsize;
        //入力テキストを作成
        text_num = GUI.TextField(new Rect(50, 70, 40, 40), text_num, max_text_num);
    }

    void Awake()
    {
        Set_path();
        Folder_File_check_andCreate();
        First_setting();
        Read_data();
        GUI_setting();
        Set_text_num(map_num);
        Set_camera_z();
        Create_mouse_point();
        Create_Mapgrid();
        Set_textures();
    }

    /// <summary>
    /// マウスの処理
    /// </summary>
    void Mouse_task()
    {
        //マウスの位置を取得
        Vector3 sub3 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(camera_z));
        //ワールド座標に変換
        Vector3 sub31 = Camera.main.ScreenToWorldPoint(sub3);
        //マップ座標に最適化
        Vector3 sub32 = new Vector3(Mathf.FloorToInt(sub31.x), Mathf.FloorToInt(sub31.y), 0);
        mouse_point.transform.position = new Vector3(sub32.x, sub32.y, -1);

        if (Input.GetMouseButton(0))
        {
            //範囲外を除外
            if (sub32.x >= 0 && sub32.x < Maxsize_x && -sub32.y >= 0 && -sub32.y < Maxsize_y)
            {
                //mapに確認
                if (map[map_num, (int)-sub32.y, (int)sub32.x] != now_block)
                {
                    GameObject mother = GameObject.Find("mapchip");

                    switch (now_block)
                    {
                        case 0:
                            GameObject dego = GameObject.Find("chip_" + (int)-sub32.y + "_" + (int)sub32.x);
                            Destroy(dego.gameObject);
                            map[map_num, (int)-sub32.y, (int)sub32.x] = 0;
                            break;

                        default:
                            if (!GameObject.Find("chip_" + (int)-sub32.y + "_" + (int)sub32.x))
                            {
                                GameObject ob = Instantiate(bc[now_block]);
                                ob.name = "chip_" + (int)-sub32.y + "_" + (int)sub32.x;
                                ob.transform.position = new Vector3((int)sub32.x, (int)sub32.y, 0);
                                ob.transform.parent = mother.transform;
                                map[map_num, (int)-sub32.y, (int)sub32.x] = now_block;
                            }
                            break;
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Mouse_task();
        Set_camera();

        {

        }
    }
}
