using UnityEngine;

public class Test_map : MonoBehaviour
{


    



    [System.Serializable]
    public struct Size
    {
        public int x;
        public int y;
    }

    [SerializeField] Size Mapsize = new Size { x = 10, y = 10 };

    [SerializeField] Vector2 Ratio = new Vector2(1, 0.8f);

    [SerializeField] Vector3 BasisPoint = new Vector3(0, 0, 0);

    [SerializeField] GameObject mapchip = null;

    [SerializeField] bool UseGoal = true;

    [SerializeField] Size Goalpos = new Size { x = 5, y = 5 };

    [SerializeField] GameObject Goal = null;

    private GameObject mapmother = null;

    private int[,] map;

    private int[] player = new int[2];

    const float movemapy = 0.4f;

    public int Tok_map(int x,int y)
    {
        if (x < 0 || x >= Mapsize.x || y < 0 || y >= Mapsize.y)
        {
            return -1;
        }

        return map[y, x];
    }

    public Vector3 Tok_pos(int x, int y)
    {
        return new Vector3(BasisPoint.x + Ratio.x * x - Ratio.x * (Mapsize.x - 1) / 2, BasisPoint.y - Ratio.y * y + Ratio.y * (Mapsize.y - 1) / 2 + movemapy, 0);
    }

    public Vector3 Tok_pos()
    {
        return new Vector3(BasisPoint.x + Ratio.x * player[0] - Ratio.x * (Mapsize.x - 1) / 2, BasisPoint.y - Ratio.y * player[1] + Ratio.y * (Mapsize.y - 1) / 2 + movemapy, 0);
    }

    public void Tok_first(ref int x,ref int y)
    {
        x = player[0];
        y = player[1];
    }

    private void Awake()
    {
        map = new int[Mapsize.y, Mapsize.x];

        if (Ratio.x <= 0) Ratio = new Vector2(1, Ratio.y);
        if (Ratio.y <= 0) Ratio = new Vector2(Ratio.x, 1);

        mapmother = new GameObject("mapmother");
        for(int y = 0; y < Mapsize.y; ++y)
        {
            for(int x = 0; x < Mapsize.x; ++x)
            {
                if (map[y, x] == 1)
                {
                    player[0] = x;
                    player[1] = y;
                }

                GameObject chip = Instantiate(mapchip);
                chip.transform.position = new Vector3(BasisPoint.x + Ratio.x * x - Ratio.x * (Mapsize.x - 1) / 2f, BasisPoint.y - Ratio.y * y + Ratio.y * (Mapsize.y - 1) / 2f, 0);
                chip.transform.localScale = new Vector3(Ratio.x, Ratio.y, 1);
                chip.transform.parent = mapmother.transform;
            }
        }

        if (UseGoal)
        {
            if(Goalpos.x<0||Goalpos.x>Mapsize.x - 1) { Goalpos.x = Mapsize.x - 1; }
            if(Goalpos.y<0||Goalpos.y>Mapsize.y - 1) { Goalpos.y = Mapsize.y - 1; }
            GameObject goal = Instantiate(Goal);
            goal.transform.position = new Vector3(BasisPoint.x + Ratio.x * Goalpos.x - Ratio.x * (Mapsize.x - 1) / 2f, BasisPoint.y - Ratio.y * Goalpos.y + Ratio.y * (Mapsize.y - 1) / 2f, 0);
            goal.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
    }








}
