using UnityEngine;

public class Mobius_con : MonoBehaviour
{
    /// <summary>
    /// テンポ
    /// </summary>
    enum Tenpo
    {
        Two = 2,
        Three = 3,
        Four = 4,
    }

    /// <summary>
    /// 現在のテンポ
    /// </summary>
    [SerializeField] Tenpo m_tempo = Tenpo.Four;

    /// <summary>
    /// ステージの時間
    /// </summary>
    [SerializeField] float StageTime = 90;

    /// <summary>
    /// 現在のステージのタイムアップまでにかかるフレーム数
    /// </summary>
    private int MaxStageFrame = 0;

    /// <summary>
    /// 現在の時間
    /// </summary>
    private float m_stageframe = 0;

    /// <summary>
    /// UIの位置
    /// </summary>
    private Vector3 m_position; 

    /// <summary>
    /// 半周の移動時間
    /// </summary>
    [SerializeField] float Herftime = 3.0f;

    /// <summary>
    /// 生成するUIオブジェクト中
    /// </summary>
    [SerializeField] GameObject UIobject_middle = null;

    /// <summary>
    /// 差し替え用のスプライト
    /// </summary>
    [SerializeField] Sprite[] UIspritemiddle = new Sprite[2];

    /// <summary>
    /// 生成するUIオブジェクト端
    /// </summary>
    [SerializeField] GameObject UIobject_side = null;

    /// <summary>
    /// 生成するUI上を移動するオブジェクト
    /// </summary>
    [SerializeField] GameObject UIobject_wisp = null;

    /// <summary>
    /// 生成するUIのボタン
    /// </summary>
    [SerializeField] GameObject[] UIbutton = new GameObject[3];

    /// <summary>
    /// UIをまとめる親オブジェクト
    /// </summary>
    private GameObject ellipsemother = null;

    /// <summary>
    /// 生成したUI
    /// </summary>
    private GameObject[] m_ellipse = null;

    /// <summary>
    /// UIのSpriteRenderer
    /// </summary>
    private SpriteRenderer[] m_spren = null;

    /// <summary>
    /// 生成したUI上を移動するオブジェクト
    /// </summary>
    private GameObject m_wisp = null;

    /// <summary>
    /// 生成したUIのボタン
    /// </summary>
    private GameObject[] m_uibutton = null;

    /// <summary>
    /// 楕円の半周でかかるフレーム数
    /// 初期地点を含めるため1フレームを追加
    /// </summary>
    private int EllipseHerfFrame = 0;

    /// <summary>
    /// 楕円の状態
    /// </summary>
    private enum EllipseMode
    {
        Normal = 0,
        Cross = 1
    }

    /// <summary>
    /// 現在の楕円の状態
    /// </summary>
    private EllipseMode[] m_ellipsemodes = null;

    /// <summary>
    /// UI上のオブジェクトが移動する位置x
    /// </summary>
    private float[] m_moveposx = null;

    /// <summary>
    /// UI変形時、UI上のオブジェクトが移動する位置y
    /// </summary>
    private float[] m_moveposy = null;

    /// <summary>
    /// プレイヤーが移動できたかの記録
    /// </summary>
    private int[] m_movelist = null;

    /// <summary>
    /// UIを操作した記録
    /// </summary>
    private int[] m_ops = null;

    /// <summary>
    /// UIの縦の大きさをUnityに合わせたもの
    /// </summary>
    const float pixcellforunitysize_y = 1f / 50f * 39;

    /// <summary>
    /// UIの横の大きさをUnityに合わせたもの
    /// </summary>
    const float pixcellforunitysize_x = 1f / 50f * 60;

    /// <summary>
    /// 1/4楕円を移動するのにかかるフレーム数
    /// </summary>
    private int herfellipsesize = 0;

    /// <summary>
    /// プレイヤーを操作する
    /// </summary>
    private Player_con player = null;

    /// <summary>
    /// 輪を作成する
    /// </summary>
    private void Create_Strip()
    {

        //ステージのフレーム数を確定
        MaxStageFrame = (int)(60 * StageTime);

        //UIを生成する位置
        m_position = transform.position;

        //もし半周にかかる時間が1秒未満に設定されていた場合1秒に変える
        if (Herftime < 1) Herftime = 1;

        //半周でかかるフレーム数
        EllipseHerfFrame = (int)(Herftime * 60) + 1;

        //適切でない秒数の場合それ以降の処理をはじく
        if ((EllipseHerfFrame - 1) % (int)m_tempo != 0) 
        {
            Debug.Log("指定された秒数が適切ではありません");
            return;
        }

        //配列を確保
        m_moveposx = new float[EllipseHerfFrame];
        m_moveposy = new float[EllipseHerfFrame];
        ellipsemother = new GameObject("strip");
        m_ellipse = new GameObject[(int)m_tempo * 2];
        m_spren = new SpriteRenderer[(int)m_tempo * 2];
        m_ellipsemodes = new EllipseMode[(int)m_tempo * 2];
        m_uibutton = new GameObject[(int)m_tempo - 1];
        m_movelist = new int[MaxStageFrame];
        m_ops = new int[MaxStageFrame];

        //全体の大きさを確定
        float scalex = 2.4f * (int)m_tempo;
        float sx = scalex / 2f - m_position.x;

        //UIを生成
        for(int s = 1; s < (int)m_tempo * 2 - 1; ++s)
        {
            m_ellipse[s] = Instantiate(UIobject_middle);
            m_ellipse[s].transform.position = new Vector3(-sx + 0.6f + (1.2f * s), m_position.y, 0);
            if (s % 2 == 1)
            {
                m_ellipse[s].transform.localScale = new Vector3(-1, 1, 1);
            }
            m_ellipse[s].transform.parent = ellipsemother.transform;
            m_spren[s] = m_ellipse[s].GetComponent<SpriteRenderer>();
        }
        m_ellipse[0] = Instantiate(UIobject_side);
        m_ellipse[0].transform.position = new Vector3(-sx + 0.6f, m_position.y, 0);
        m_ellipse[0].transform.parent = ellipsemother.transform;
        m_ellipse[m_ellipse.Length - 1] = Instantiate(UIobject_side);
        m_ellipse[m_ellipse.Length - 1].transform.position = new Vector3(-sx + 0.6f + (1.2f * ((int)m_tempo * 2 - 1)), m_position.y, 0);
        m_ellipse[m_ellipse.Length - 1].transform.localScale = new Vector3(-1, 1, 1);
        m_ellipse[m_ellipse.Length - 1].transform.parent = ellipsemother.transform;
        for (int s = 0; s < (int)m_tempo - 1; ++s)
        {
            m_uibutton[s] = Instantiate(UIbutton[s]);
            m_uibutton[s].transform.position = new Vector3(-sx + 2.4f * (s + 1), m_position.y, 0);
        }

        //1/4楕円にかかるフレーム数を確定
        herfellipsesize = (EllipseHerfFrame - 1) / (int)m_tempo;

        //移動する位置を確定
        for (int s = 0; s < (int)m_tempo; ++s)
        {
            float fp = -sx + scalex / (int)m_tempo * s;
            for(int i = 0; i <= herfellipsesize; ++i)
            {
                m_moveposx[s * herfellipsesize + i] = fp + 2.4f / herfellipsesize * i;
                m_moveposy[s * herfellipsesize + i] = Mathf.Sqrt((1 - Mathf.Pow(-1.2f + 2.4f / herfellipsesize * i, 2) / Mathf.Pow(pixcellforunitysize_x, 2)) * Mathf.Pow(pixcellforunitysize_y, 2));
            }
            m_moveposy[s * herfellipsesize] = 0;
        }
        m_moveposx[EllipseHerfFrame - 1] = -sx + (1.2f * (int)m_tempo * 2);
        m_moveposy[EllipseHerfFrame - 1] = 0;

        //UIの状態を設定
        m_ellipsemodes[0] = EllipseMode.Cross;
        m_ellipsemodes[m_ellipsemodes.Length - 1] = EllipseMode.Cross;

        //UI上を移動するオブジェクトを生成
        m_wisp = Instantiate(UIobject_wisp);
        m_wisp.transform.position = new Vector3(m_moveposx[0], m_moveposy[0], 0);

        //プレイヤーを参照する
        player = GetComponent<Player_con>();
    }

    /// <summary>
    /// UI上のオブジェクトを動かすか
    /// </summary>
    [SerializeField] bool IsMoveWisp = true;

    /// <summary>
    /// 現在UIが操作可能か
    /// </summary>
    private bool m_moveuibutton = true;

    /// <summary>
    /// 現在のUI上のオブジェクトの位置
    /// </summary>
    private float movecount = 0;

    /// <summary>
    /// 輪の上にいるか下にいるか
    /// 左右どちらに動いているか
    /// </summary>
    private Vector3 movevec = new Vector3(1, 1, 0);

    /// <summary>
    /// 最期に反転した位置
    /// </summary>
    private int wasswitch = 0;

    private int movebuttonrotatex = 0;

    /// <summary>
    /// UI上のオブジェクトの処理
    /// </summary>
    private void Wisp_task()
    {
        //動かさない場合、wispがない場合は処理をはじく
        if (!IsMoveWisp || !m_wisp) { return; }

        int move = m_moveuibutton ? 1 : -1;

        float dt = Time.deltaTime * 60;

        //時間を加算
        m_stageframe += dt * move;

        m_stageframe = Mathf.Clamp(m_stageframe, 0, MaxStageFrame);

        //カウントを加算
        if (m_stageframe < MaxStageFrame)
        {
            movecount += dt * (int)movevec.x * move;
        }

        //int型に変換、範囲外を除外
        int p = Mathf.Clamp(Mathf.RoundToInt(movecount), 0, EllipseHerfFrame - 1);

        //wipsを移動
        int modepos = Mathf.Clamp(Mathf.FloorToInt(movecount / (herfellipsesize / 2f)), 0, (int)m_tempo * 2 - 1);
        switch (m_ellipsemodes[modepos])
        {
            case EllipseMode.Normal:
                m_wisp.transform.position = new Vector3(m_moveposx[p], m_position.y + 0.8f * movevec.y, 0);
                break;
            case EllipseMode.Cross:
                m_wisp.transform.position = new Vector3(m_moveposx[p], m_position.y + m_moveposy[p] * movevec.y, 0);
                break;
            default:
                break;
        }

        //反転処理
        if (Mathf.Abs(p - wasswitch) > 0)
        {
            for(int i = 1; i < (int)m_tempo; ++i)
            {
                if (p == herfellipsesize * i && m_ellipsemodes[modepos] == EllipseMode.Cross)
                {
                    movevec.y *= -1;
                    wasswitch = p;

                    int x = movebuttonrotatex == 0 ? 1 : movebuttonrotatex == 2 ? -1 : 0;
                    int y = movebuttonrotatex == 1 ? -1 : movebuttonrotatex == 3 ? 1 : 0;
                    x *= move;
                    y *= move;
                    //プレイヤーアクション処理
                    switch (i) {
                        case 1:
                            //移動
                            Debug.Log("move");
                            player.Move(1);
                            break;
                        case 2:
                            //ジャンプ
                            Debug.Log("jump");
                            if (x == 1 || x == -1) x *= 2;
                            if (y == 1 || y == -1) y *= 2;
                            player.Move(1);
                            break;
                        case 3:
                            //方向転換
                            Debug.Log("turn right");
                            movebuttonrotatex -= move;
                            if (movebuttonrotatex < 0) movebuttonrotatex = 3;
                            if (movebuttonrotatex > 3) movebuttonrotatex = 0;
                            m_uibutton[0].transform.localRotation = Quaternion.Euler(0, 0, 90 * movebuttonrotatex);
                            player.Turn(movebuttonrotatex);
                            break;
                    }
                }
            }
            if (p == 0 || p == EllipseHerfFrame - 1)
            {
                movevec.y *= -1;
                wasswitch = p;
            }
        }
        if ((movevec.x * move > 0 && movecount >= EllipseHerfFrame) || (movevec.x * move < 0 && movecount <= 0))
        {

            if (movevec.x * move> 0) { movecount = EllipseHerfFrame - 1; }
            else if (movevec.x  * move< 0) { movecount = 0; }

            movevec.x *= -1;
        }
    }

    /// <summary>
    /// UIを動かすか
    /// </summary>
    [SerializeField] bool IsMoveUI = true;

    /// <summary>
    /// 前回UIの操作をしたフレーム
    /// </summary>
    private int waschangeframe = 0;

    /// <summary>
    /// UIをねじれを切り替える
    /// </summary>
    /// <param name="n"></param>
    void ChangeUI(int n)
    {
        if (m_ellipsemodes[n] == EllipseMode.Normal)
        {
            m_spren[n].sprite = m_spren[n + 1].sprite = UIspritemiddle[1];
            m_ellipsemodes[n] = m_ellipsemodes[n + 1] = EllipseMode.Cross;
        }
        else
        {
            m_spren[n].sprite = m_spren[n + 1].sprite = UIspritemiddle[0];
            m_ellipsemodes[n] = m_ellipsemodes[n + 1] = EllipseMode.Normal;
        }
    }

    /// <summary>
    /// 輪の処理
    /// </summary>
    private void Ellipse_task()
    {
        //UIの操作ができない場合処理をはじく
        if (!IsMoveUI) { return; }

        int p = Mathf.Clamp(Mathf.RoundToInt(m_stageframe), 0, MaxStageFrame - 1);

        if (m_moveuibutton)
        {
            //輪をねじる処理
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D[] hit2d = Physics2D.RaycastAll(ray.origin, ray.direction);
                foreach (RaycastHit2D h in hit2d)
                {
                    if (h.collider != null && Mathf.Abs(p - waschangeframe) > 0)
                    {
                        bool hit = false;
                        switch (h.collider.tag)
                        {
                            case "Move":
                                ChangeUI(1);
                                m_ops[p] = 1;
                                waschangeframe = p;
                                hit = true;
                                break;
                            case "Jump":
                                ChangeUI(3);
                                m_ops[p] = 3;
                                waschangeframe = p;
                                hit = true;
                                break;
                            case "Turn":
                                ChangeUI(5);
                                m_ops[p] = 5;
                                waschangeframe = p;
                                hit = true;
                                break;
                        }
                        if (hit) break;
                    }
                }
            }
        }
        else
        {
            //輪を自動でねじる処理
            if (m_ops[p] != 0)
            {
                ChangeUI(m_ops[p]);
                m_ops[p] = 0;
            }
        }
    }

    /// <summary>
    /// 時間を戻す処理
    /// </summary>
    void Time_task()
    {
        if (m_moveuibutton && Input.GetKeyDown(KeyCode.Space))
        {
            wasswitch = -1;
            m_moveuibutton = false;
        }
        if (!m_moveuibutton && Input.GetKeyUp(KeyCode.Space))
        {
            wasswitch = -1;
            m_moveuibutton = true;
        }
    }

    private void OnGUI()
    {
        float x = (float)Screen.width / 1280;
        float y = (float)Screen.height / 720;
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(x, y, 1));

        GUI.Label(new Rect(1100, 100, 200, 50), (int)(m_stageframe / 60) + "秒");
    }

    // Start is called before the first frame update
    void Start()
    {
        Create_Strip();
    }

    // Update is called once per frame
    void Update()
    {
        Wisp_task();
        Ellipse_task();
        Time_task();
    }
}
