using UnityEngine;

public class Sample_H : MonoBehaviour
{

    [SerializeField] GameObject wisp = null;

    [SerializeField] GameObject side = null;

    [SerializeField] float Herftime = 3.0f;

    GameObject truewisp = null;

    float[] moveposx = null;

    float[] moveposy = null;

    int size = 0;

    [SerializeField] int Strip_num = 1;

    float scalex = 0f;

    const float b = 1f / 50f * 39;

    const float a = 1f / 50f * 60;

    private int onestripsize = 0;

    // Start is called before the first frame update
    void Start()
    {

        if (Strip_num <= 0) return;

        scalex = 1.2f * Strip_num * 2;

        float sx = scalex / 2f;

        for (int s = 0; s < Strip_num; ++s)
        {
            GameObject left = Instantiate(side);
            left.transform.position = new Vector3(-sx + 0.6f + 2.4f * s, 0, 0);
            GameObject right = Instantiate(side);
            right.transform.position = new Vector3(-sx + 1.8f + 2.4f * s, 0, 0);
            right.transform.localScale = new Vector3(-1, 1, 1);
        }

        size = (int)(Herftime * 60 + 1);
        if ((size - 1) % Strip_num != 0) return;

        moveposx = new float[size];
        moveposy = new float[size];

        onestripsize = (size - 1) / Strip_num;
        for (int s = 0; s < Strip_num; ++s)
        {
            float fp = -sx + scalex / Strip_num * s;
            for (int i = 0; i <= onestripsize; ++i)
            {
                moveposx[s * onestripsize + i] = fp + 2.4f / onestripsize * i;
                moveposy[s * onestripsize + i] = Mathf.Sqrt((1 - Mathf.Pow(-1.2f + 2.4f / onestripsize * i, 2) / Mathf.Pow(a, 2)) * Mathf.Pow(b, 2));
            }
        }

        for (int s = 0; s < Strip_num; ++s)
        {
            moveposy[s * onestripsize] = 0;
        }
        //moveposx[0] = -sx;
        moveposx[size - 1] = sx;
        moveposy[size - 1] = 0;

        truewisp = Instantiate(wisp);
        truewisp.transform.position = new Vector3(moveposx[0], moveposy[0], 0);
    }

    float movecount = 0;

    int wasswitch = 0;

    Vector2 movevec = new Vector2(1, 1);

    // Update is called once per frame
    void Update()
    {

        movecount += Time.deltaTime * 60 * (int)movevec.x;

        int p = Mathf.Clamp(Mathf.RoundToInt(movecount), 0, size - 1);

        truewisp.transform.position = new Vector3(moveposx[p], moveposy[p] * movevec.y, 0);

        if (Mathf.Abs(p - wasswitch) > 0)
        {
            for (int i = 1; i < Strip_num; ++i)
            {
                if (p == onestripsize * i)
                {
                    movevec.y *= -1;
                    wasswitch = p;
                }
            }

            if (p == 0 || p == size - 1)
            {
                movevec.y *= -1;
                wasswitch = p;
            }
        }

        if ((movevec.x > 0 && movecount >= size) || (movevec.x < 0 && movecount <= 0))
        {
            movevec.x *= -1;
        }

    }
}
