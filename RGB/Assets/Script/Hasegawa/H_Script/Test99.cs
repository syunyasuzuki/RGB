using UnityEngine;

public class Test99 : MonoBehaviour
{
    private void Start()
    {
        float time = GetComponent<AudioSource>().clip.length;
        Destroy(this.gameObject, time);
    }
}
