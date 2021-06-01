using UnityEngine;

public class Player_con : MonoBehaviour
{
    /// <summary>
    /// 参照するマップ情報
    /// </summary>
    private Test_map map = null;

    /// <summary>
    /// 生成するプレイヤー
    /// </summary>
    [SerializeField] GameObject Player_prefab = null;

    /// <summary>
    /// 各方向を向いたプレイヤー
    /// </summary>
    [SerializeField] Sprite[] Player_sprite = new Sprite[4];

    /// <summary>
    /// 生成したプレイヤー
    /// </summary>
    private GameObject Player = null;

    /// <summary>
    /// 生成したプレイヤーのSpriteRenderer
    /// </summary>
    private SpriteRenderer Player_spr = null;

    /// <summary>
    /// Int型のベクター2
    /// </summary>
    struct IntVeotor2
    {
        public int x;
        public int y;
    }

    /// <summary>
    /// プレイヤーの現在の位置
    /// </summary>
    private IntVeotor2 playerposition = new IntVeotor2 { x = 0, y = 0 };

    /// <summary>
    /// プレイヤーの向いている方向
    /// </summary>
    private IntVeotor2 playervector = new IntVeotor2 { x = 1, y = 0 };

    /// <summary>
    /// マップから情報をもらってプレイヤーを生成する
    /// </summary>
    void CreatePlayer()
    {
        map = GetComponent<Test_map>();
        map.Tok_first(ref playerposition.x, ref playerposition.y);
        Player = Instantiate(Player_prefab);
        Player.transform.position = map.Tok_pos();
        Player_spr = Player.GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// 各方向に動く
    /// 移動できなかった場合-1を返す
    /// </summary>
    public int Move(int x)
    {
        if (map.Tok_map(playerposition.x + playervector.x * x, playerposition.y + playervector.y * x) != -1)
        {
            playerposition.x += playervector.x * x;
            playerposition.y += playervector.y * x;
            Player.transform.position = map.Tok_pos(playerposition.x, playerposition.y);
            return 1;
        }
        else
        {
            return -1;
        }
    }

    public int Jump(int x)
    {
        if (map.Tok_map(playerposition.x + playervector.x * 2 * x, playerposition.y + playervector.y * 2 * x) != -1)
        {
            playerposition.x += playervector.x * 2 * x;
            playerposition.y += playervector.y * 2 * x;
            Player.transform.position = map.Tok_pos(playerposition.x, playerposition.y);
            return 1;
        }
        else
        {
            return -1;
        }
    }

    /// <summary>
    /// 方向転換する
    /// </summary>
    public void Turn(int x)
    {
        Player_spr.sprite = Player_sprite[x];
        playervector.x = x == 0 ? 1 : x == 2 ? -1 : 0;
        playervector.y = x == 1 ? -1 : x == 3 ? 1 : 0;
    }

    /// <summary>
    /// ある位置に移動する
    /// </summary>
    public void Warmhole(int x,int y)
    {

    }

    /// <summary>
    /// 攻撃？
    /// </summary>
    public void Attack()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        CreatePlayer();
    }
}
