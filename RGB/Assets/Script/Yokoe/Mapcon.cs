using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mapcon : MonoBehaviour
{

    /// <summary>
    /// ワールドの数
    /// </summary>
    const int World_num = 1;

    /// <summary>
    /// １つのワールドのステージ数
    /// </summary>
    const int Stage_num = 12;


    /// <summary>
    /// ファイルのパス
    /// </summary>
    string[] FilePath = null;

    /// <summary>
    /// ファイルのパスを指定
    /// </summary>
    void Set_path()
    {
        FilePath = new string[World_num] { "mapdata" };
    }


    /// <summary>
    /// マップサイズ x
    /// </summary>
    const int Mapsize_x = 20;

    /// <summary>
    /// マップサイズ y
    /// </summary>
    const int Mapsize_y = 10;

    /// <summary>
    /// 現在生成されているワールド
    /// </summary>
    private int now_world = 0;

    /// <summary>
    /// 現在生成されているステージ
    /// </summary>
    private int now_stage = 0;

    /// <summary>
    /// 全てのマップデータ
    /// </summary>
    int[,,,] map = new int[World_num, Stage_num, Mapsize_y, Mapsize_x];

    /// <summary>
    /// 一括で全ステージを読み込む
    /// </summary>
    void Read_all_maps()
    {
        for (int w = 0; w < World_num; ++w)
        {
            string text_data = (Resources.Load(FilePath[w], typeof(TextAsset)) as TextAsset).text;
            string[] text_line = text_data.Split('\n');
            for (int s = 0; s < Stage_num; ++s)
            {
                for (int y = 0; y < Mapsize_y; ++y)
                {
                    string[] strsplr = text_line[(Mapsize_y + 1) * s + y + 1].Split(',');
                    for (int x = 0; x < Mapsize_x; ++x)
                    {
                        map[w, s, y, x] = int.Parse(strsplr[x]);
                    }
                }
            }
        }
    }

    /// <summary>
    /// GameMaster取得
    /// </summary>
    GameObject GM;

    //素材
    /// <summary>
    /// ノーマルブロック
    /// </summary>
    [SerializeField] Sprite[] Normal_blocks = new Sprite[2];
    /// <summary>
    /// RGBブロック
    /// </summary>
    [SerializeField] Sprite[] RGB_blocks = new Sprite[3];
    /// <summary>
    /// 各プレハブ
    /// </summary>
    [SerializeField] GameObject[] Map_pre = new GameObject[2];

    /// <summary>
    /// マップチップを入れる親オブジェクト
    /// </summary>
    GameObject Map_mother = null;
    /// <summary>
    /// 1回でもマップを生成したか
    /// </summary>
    bool create_map = false;
    /// <summary>
    /// 現在使っているマップ
    /// </summary>
    int[,] now_map = new int[Mapsize_y, Mapsize_x];

    List<GameObject> red_blocks = new List<GameObject>();
    List<GameObject> green_blocks = new List<GameObject>();
    List<GameObject> blue_blocks = new List<GameObject>();

    [SerializeField] Vector3 Map_pos = new Vector3(0, 0, 0);
    [SerializeField]Vector3 obj_scale = new Vector3(1, 1, 1);
    int sort_layer = 0;

    /// <summary>
    /// マップチップ作成
    /// </summary>
    /// <param name="name">オブジェクトの名前</param>
    /// <param name="sprite">オブジェクトのスプライト</param>
    /// <param name="sortn">オブジェクトのレイヤー</param>
    /// <param name="pos">オブジェクトの座標</param>
    /// <param name="scale">オブジェクトの大きさ</param>
    /// <returns>作成したマップチップ</returns>
    GameObject Create_Chip(string name,ref Sprite sprt,int sortn,Vector3 pos,Vector3 scale)
    {
        GameObject ob = new GameObject(name);
        ob.AddComponent<SpriteRenderer>().sprite = sprt;
        ob.GetComponent<SpriteRenderer>().sortingOrder = sortn;
        ob.AddComponent<BoxCollider2D>();
        ob.transform.position = pos;
        ob.transform.localScale = scale;
        ob.transform.parent = Map_mother.transform;

        return ob;
    }

    /// <summary>
    /// 指定されたマップを生成
    /// </summary>
    /// <param name="w">生成されるワールド</param>
    /// <param name="s">生成されるステージ</param>
    void Create_map(int w,int s)
    {
        Map_mother = new GameObject("map_mother");
        //マップの一番左上の座標を求める
        Vector3 Map_pos2 = new Vector3(Map_pos.x - Mapsize_x * obj_scale.x / 2 + obj_scale.x / 2,
            Map_pos.y + Mapsize_y * obj_scale.y / 2 + obj_scale.y / 2, 0);

        for (int lu = 0; lu < Mapsize_y; ++lu)
        {
            for (int na = 0; na < Mapsize_x; ++na)
            {
                //オブジェクトの座標を求める（）
                Vector3 obj_pos = new Vector3(na * obj_scale.x, -lu * obj_scale.y, 0);
                 switch (now_map[lu, na] = map[w, s, lu, na])
                {
                    case 0:
                        break;
                    case 1:
                        Create_Chip("wh_bl" + na + " - " + lu, ref Normal_blocks[0], sort_layer, Map_pos2 + obj_pos, obj_scale);
                        break;
                    case 2:
                        Create_Chip("black_bl" + na + " - " + lu, ref Normal_blocks[1], sort_layer, Map_pos2 + obj_pos, obj_scale);
                        break;
                    case 3:
                        red_blocks.Add(Create_Chip("red_bl" + na + " - " + lu, ref RGB_blocks[0], sort_layer, Map_pos2 + obj_pos, obj_scale));
                        break;
                    case 4:
                        green_blocks.Add(Create_Chip("green_bl" + na + " - " + lu, ref RGB_blocks[1], sort_layer, Map_pos2 + obj_pos, obj_scale));
                        break;
                    case 5:
                        blue_blocks.Add(Create_Chip("blue_bl" + na + " - " + lu, ref RGB_blocks[2], sort_layer, Map_pos2 + obj_pos, obj_scale));
                        break;
                    case 6:
                        GameObject goal = Instantiate(Map_pre[0]);
                        goal.transform.position = Map_pos2 + obj_pos;
                        goal.transform.localScale = obj_scale;
                        goal.transform.parent = Map_mother.transform;
                        break;
                    case 7:
                        GameObject player = Instantiate(Map_pre[1]);
                        player.transform.position = Map_pos2 + obj_pos;
                        player.transform.localScale = obj_scale;
                        player.GetComponent<Player_ctr>().Setplayer_scale(obj_scale.x);
                        break;

                }
            }
        }
        create_map = true;
    }

    /// <summary>
    /// 現在使っているマップを消す
    /// </summary>s
    void Delete_map()
    {
        GameObject map_mother = GameObject.Find("map_mother");
        Destroy(map_mother.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        Set_path();
        Read_all_maps();
        GameObject createbox = GameObject.Find("CreateBox");
        createbox.GetComponent<CreateBox>().Get_num(ref now_world, ref now_stage);
        Create_map(now_world, now_stage);
        GameObject.Find("GameMaster").GetComponent<Mobius_con3>().SetColorBlockData(red_blocks, green_blocks, blue_blocks);
    }
}
