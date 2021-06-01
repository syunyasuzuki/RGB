using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBox : MonoBehaviour
{
    /// <summary>
    /// ワールド番号
    /// </summary>
    private int world { get; set; } = 0;
    /// <summary>
    /// ステージ番号
    /// </summary>
    private int stagenumber { get; set; } = 0;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void deletegameobj()
    {
        Destroy(gameObject);
    }

    public void Set_num(int w,int s)
    {
        world = w;
        stagenumber = s;
    }

    public void Get_num(ref int w, ref int s)
    {
        w = world;
        s = stagenumber;
    }
}
