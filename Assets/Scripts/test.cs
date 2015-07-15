using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {

    public Color color1 = Color.red;
    public Color color2 = Color.blue;
    public float duration = 3.0F;
    void Update()
    {
        float t = Mathf.PingPong(Time.time, duration) / duration;
        Camera.main.backgroundColor = Color.Lerp(color1, color2, t);
    }
    //void Example()
    //{
    //    Camera.main.clearFlags = CameraClearFlags.SolidColor;
    //}
}
