using UnityEngine;

public class Mobius_data :MonoBehaviour
{

    /// <summary>
    /// テンポ
    /// </summary>
    public enum Tenpo
    {
        Two = 2,
        Three = 3,
        Four = 4,
    }

    /// <summary>
    /// 現在のテンポ
    /// </summary>
    public Tenpo tempo = Tenpo.Four;

    /// <summary>
    /// 曲を再生する場合曲を追加
    /// </summary>
    [SerializeField] AudioClip music = null;

    /// <summary>
    /// 曲のBPMを設定
    /// 曲がない場合設定は無効化される
    /// </summary>
    [SerializeField] int bpm = 120;

    /// <summary>
    /// 合計小節数
    /// </summary>
    int Maxbeat = 0;

    /// <summary>
    /// ステージの時間
    /// </summary>
    public float StageTime = 90;

    /// <summary>
    /// 現在の時間
    /// </summary>
    private float time = 0;

    /// <summary>
    /// 時間を変化させるか
    /// </summary>
    public bool IsTimeCount = true;

    /// <summary>
    /// 時間の戻る最大倍率
    /// </summary>
    public float MinTimeScale = -10;

    /// <summary>
    /// 現在の時間を知る
    /// </summary>
    public float Tok_time()
    {
        return time;
    }

    /// <summary>
    /// 時間を加算する
    /// </summary>
    /// <param name="n">加算する倍率</param>
    public void Timecount(float n)
    {
        time = Mathf.Clamp(time + Time.deltaTime * n, 0, StageTime);
    }
    
    /// <summary>
    /// UIの位置
    /// </summary>
    public Vector3 Position { get; set; } = Vector3.zero;

    /// <summary>
    /// 半周の移動時間
    /// </summary>
    public float Herftime = 3.0f;

    /// <summary>
    /// UI上のオブジェクトを動かすか
    /// </summary>
    public bool IsMoveWisp = true;

    /// <summary>
    /// 逆再生をしているか
    /// </summary>
    public bool Movewisp { get; set; } = true;

    /// <summary>
    /// UIを動かすか
    /// </summary>
    public bool IsMoveUI = true;

    /// <summary>
    /// UIの横の大きさ
    /// </summary>
    public float UIscalex { get; set; } = 0f;

    /// <summary>
    /// 楕円の状態
    /// </summary>
    public enum EllipseMode
    {
        Normal = 0,
        Cross = 1
    }

    /// <summary>
    /// 現在の楕円の状態
    /// </summary>
    public EllipseMode[] ellipsemodes { get; set; } = null;

    /// <summary>
    /// 1秒間にクリックできる限界の回数
    /// </summary>
    [SerializeField] int MaxClicForSec = 16;

    /// <summary>
    /// 最大でUIを操作することのできる回数
    /// </summary>
    private int MaxChangeCount = 0;

    /// <summary>
    /// UIを操作した回数
    /// </summary>
    private int changecount { get; set; } = 0;

    /// <summary>
    /// ボタンを押したときの情報
    /// </summary>
    private struct Clicdata
    {
        //ボタンが押された時間
        public float time;
        //何番のボタンが押されたか
        public int number;
    }

    /// <summary>
    /// UIを操作した記録
    /// </summary>
    private Clicdata[] buttonclic { get; set; } = null;

    /// <summary>
    /// UIを操作した時間を記録する
    /// </summary>
    public void ChangeCount(int n)
    {
        if (changecount < MaxChangeCount)
        {
            buttonclic[changecount].time = Tok_time();
            buttonclic[changecount].number = n;
            ++changecount;
        }
    }

    /// <summary>
    /// 直前のUIを操作した時間を知る
    /// 直前の時間がない場合-2を返す
    /// </summary>
    public float Tok_clictime()
    {
        if (changecount - 1 < 0)
        {
            return -2;
        }
        else
        {
            return buttonclic[changecount - 1].time;
        }
    }

    /// <summary>
    /// 直前のUIを操作したUIの番号を知る
    /// 直前の操作記録がない場合-2を返す
    /// </summary>
    public int Tok_clicnumber()
    {
        if (changecount - 1 < 0)
        {
            return -2;
        }
        else
        {
            return buttonclic[changecount - 1].number;
        }
    }

    /// <summary>
    /// 直前のUI操作記録がある場合に直前の操作記録を削除する
    /// </summary>
    public void Delete_ClicRecord()
    {
        if (changecount > 0)
        {
            buttonclic[changecount - 1].time = 0;
            buttonclic[changecount - 1].number = 0;
            --changecount;
        }
    }

    /// <summary>
    /// 最大で交点を通る回数
    /// </summary>
    private int MaxMoveCount = 0;

    /// <summary>
    /// 交点を通った回数
    /// </summary>
    public int movecount { get; set; } = 0;

    /// <summary>
    /// プレイヤーのデータ
    /// </summary>
    private struct Playerdata
    {
        //プレイヤーのアクションが起きた時間
        public float time;
        //プレイヤーが移動することができたか
        public int canmove;
        //プレイヤーが起こしたアクション番号
        public int action;
    }

    /// <summary>
    /// プレイヤーのアクション記録
    /// </summary>
    private Playerdata[] playeraction { get; set; } = null;

    /// <summary>
    /// 交点を通った時間を記録する
    /// </summary>
    public void MoveCount(int can,int act)
    {
        if (movecount < MaxMoveCount)
        {
            playeraction[movecount].time = Tok_time();
            playeraction[movecount].canmove = can;
            playeraction[movecount].action = act;
            ++movecount;
        }
    }

    /// <summary>
    /// 直前のUIが交点を通った時間を知る
    /// 直前の情報がない場合-1を返す
    /// </summary>
    public float Tok_action()
    {
        if (movecount - 1 < 0)
        {
            return -1;
        }
        else
        {
            return playeraction[movecount - 1].time;
        }
    }

    /// <summary>
    /// 直前のUIが交点を通りアクションが起こった時にアクションが実行されたかを知る
    /// アクションが実行された場合1を返す
    /// 直前の情報がない場合-2を返す
    /// アクションが実行されなかった場合-1を返す
    /// </summary>
    public int Tok_playermove()
    {
        if (movecount - 1 < 0)
        {
            return -2;
        }
        else
        {
            return playeraction[movecount - 1].canmove;
        }
    }

    /// <summary>
    /// 直前に通った交点の番号を知る
    /// 直前の情報がない場合-2を返す
    /// </summary>
    public int Tok_playeraction()
    {
        if (movecount - 1 < 0)
        {
            return -2;
        }
        else
        {
            return playeraction[movecount - 1].action;
        }
    }

    /// <summary>
    /// 直前の情報がある場合に直前の情報を削除する
    /// </summary>
    public void Delete_ActionRecord()
    {
        if(movecount > 0)
        {
            playeraction[movecount - 1].time = 0;
            playeraction[movecount - 1].canmove = 0;
            playeraction[movecount - 1].action = 0;
            --movecount;
        }
    }

    /// <summary>
    /// UIの縦の大きさをUnityに合わせたもの
    /// </summary>
    public float Pixcellforunitysize_y { get; } = 1f / 50 * 39;

    /// <summary>
    /// UIの横の大きさをUnityに合わせたもの
    /// </summary>
    public float Pixcellforunitysize_x { get; } = 1f / 50 * 60;

    void Awake()
    {
        /*
        if (music)
        {
            StageTime = music.length;
            Maxbeat = Mathf.FloorToInt(StageTime * bpm) / (int)tempo;

        }
        else
        {
        }
        */
        //半周にかかる時間が1秒未満だったら1秒にする
        //if (Herftime < 1) Herftime = 1;

        //ステージの時間が半周にかかる時間より短い場合1周の時間にする
        if (StageTime < Herftime) StageTime = Herftime * 2;

        //UIを生成する位置を確定
        Position = transform.position;

        //UIの横の大きさを確定
        UIscalex = (float)System.Math.Round(Pixcellforunitysize_x * (int)tempo * 2, 1);

        //配列の確保
        ellipsemodes = new EllipseMode[(int)tempo * 2];

        //UIの状態を設定
        ellipsemodes[0] = ellipsemodes[ellipsemodes.Length - 1] = EllipseMode.Cross;

        //UIを操作できる回数を確定する
        MaxChangeCount = Mathf.FloorToInt(MaxClicForSec * StageTime);
        buttonclic = new Clicdata[MaxChangeCount];

        //交点を通る回数を確定する
        MaxMoveCount = Mathf.FloorToInt(StageTime / (Herftime * 2) * ((int)tempo - 1) * 2);
        playeraction = new Playerdata[MaxMoveCount];

        DebugMode();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("movecount:" + movecount + "   changecount:" + changecount);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log(Tok_action());
        }
    }

    void DebugMode()
    {
        Debug.Log("StageTime:" + StageTime + "   HerfTime:" + Herftime + "   Tempo:" + tempo);
        Debug.Log("UI_Position:" + Position + "   UIscalex:" + UIscalex);
        Debug.Log("MaxClicForSec:" + MaxClicForSec + "   MaxChangeCount:" + MaxChangeCount + "   MaxMoveCount:" + MaxMoveCount);

    }
}
