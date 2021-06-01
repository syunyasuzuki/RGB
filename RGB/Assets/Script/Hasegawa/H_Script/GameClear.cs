using UnityEngine;
using UnityEngine.SceneManagement;

public class GameClear : MonoBehaviour
{
    [SerializeField] Sprite GameClearSprite = null;

    [SerializeField] int SpriteUnitySize = 5;

    [SerializeField] float DisplayTime = 3f;

    [SerializeField] float DisplayUp = 2f;

    [SerializeField] Sprite[] Select = null;

    private enum Mode
    {
        weit = 0,
        movex = 1,
        movey = 2,
        select = 3
    }

    private Mode mode = Mode.weit;

    private Vector2 SpriteSize = Vector2.zero;

    private float time = 0;

    private bool IsGameClear = false;

    private GameObject clearmother = null;

    private GameObject[] clearchild = null;

    private SpriteRenderer[] clearchildsprite = null;

    private GameObject selectmother = null;

    private SpriteRenderer[] selectchildsprite = null;

    private int nowselect = -1;

    // Start is called before the first frame update
    void Start()
    {
        if (GameClearSprite == null) return;

        SpriteSize.x = GameClearSprite.rect.width;
        SpriteSize.y = GameClearSprite.rect.height;

        clearmother = new GameObject("clear");
        clearchild = new GameObject[(int)SpriteSize.y];
        clearchildsprite = new SpriteRenderer[(int)SpriteSize.y];
        for (int y = 0; y < (int)SpriteSize.y; ++y)
        {
            clearchild[y] = new GameObject(y.ToString());
            clearchildsprite[y] = clearchild[y].AddComponent<SpriteRenderer>();
            clearchildsprite[y].sprite = Sprite.Create(GameClearSprite.texture, new Rect(0, y, SpriteSize.x, 1), new Vector2(0.5f, 0.5f), SpriteUnitySize);
            clearchildsprite[y].sortingOrder = 50;
            clearchildsprite[y].color = new Color(1, 1, 1, 0);

            int x = y % 2 == 0 ? -1 : 1;
            clearchild[y].transform.position = new Vector3(SpriteSize.x / SpriteUnitySize * x, 1f / SpriteUnitySize * y, 0);
            clearchild[y].transform.parent = clearmother.transform;
        }
        clearmother.SetActive(false);

        selectmother = new GameObject("select");
        selectchildsprite = new SpriteRenderer[Select.Length];
        for(int s = 0; s < Select.Length; ++s)
        {
            GameObject select = new GameObject(s.ToString());
            selectchildsprite[s] = select.AddComponent<SpriteRenderer>();
            selectchildsprite[s].sprite = Select[s];
            selectchildsprite[s].sortingOrder = 50;
            selectchildsprite[s].color = new Color(1, 1, 1, 1);
            select.transform.position = new Vector3(0, -1.5f + DisplayUp - 1.5f * s, 0);
            select.transform.parent = selectmother.transform;
        }
        selectmother.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsGameClear) return;

        switch (mode)
        {
            case Mode.movex:
                time = Mathf.Clamp(time + Time.deltaTime, 0, DisplayTime);

                float ratio = time / DisplayTime;

                for (int y = 0; y < (int)SpriteSize.y; ++y)
                {
                    int x = y % 2 == 0 ? -1 : 1;
                    clearchild[y].transform.position = new Vector3((SpriteSize.x / SpriteUnitySize * (1 - ratio)) * x, 1f / SpriteUnitySize * y, 0);
                    clearchildsprite[y].color = new Color(1, 1, 1, 1 * ratio);
                }

                if (time >= DisplayTime)
                {
                    mode = Mode.movey;
                }
                break;
            case Mode.movey:
                if (clearmother.transform.position.y <= DisplayUp)
                {
                    clearmother.transform.position += new Vector3(0, 0.025f, 0);
                }
                else
                {
                    mode = Mode.select;
                    selectmother.SetActive(true);
                }
                break;
            case Mode.select:
                if (Input.anyKeyDown)
                {
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        GameObject createbox = GameObject.Find("CreateBox");
                        switch (nowselect)
                        {
                            case 0:
                                //リトライ時
                                //同じシーンを読み込む
                                Invoke(nameof(Retry), 0.5f);
                                break;
                            case 1:
                                //ステージ選択へ
                                createbox.GetComponent<CreateBox>().deletegameobj();
                                Invoke(nameof(LoadSelect), 0.5f);
                                break;
                            case 2:
                                //タイトルへ
                                createbox.GetComponent<CreateBox>().deletegameobj();
                                Invoke(nameof(LoadTitle), 0.5f);
                                break;
                        }
                    }
                    if (Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        if (--nowselect < 0) nowselect = Select.Length - 1;
                    }
                    if (Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        if (++nowselect > Select.Length - 1) nowselect = 0;
                    }
                    if (nowselect != -1)
                    {
                        for(int s = 0; s < Select.Length; ++s)
                        {
                            selectchildsprite[s].color = new Color(1, 1, 1, 1);
                        }
                        selectchildsprite[nowselect].color = new Color(1, 1, 0, 1);
                    }
                }
                break;
        }
    }

    void Retry()
    {
        SceneManager.LoadScene("GameSampleScene");
    }

    void LoadSelect()
    {
        SceneManager.LoadScene("SelectScene");
    }
    void LoadTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && GameClearSprite != null && time <= DisplayTime)
        {
            GameObject ui = GameObject.Find("GameMaster");
            Destroy(ui.GetComponent<Mobius_con3>());
            Destroy(collision.gameObject.GetComponent<Player_ctr>());
            IsGameClear = true;
            clearmother.SetActive(true);
            mode = Mode.movex;
        }
    }
}
