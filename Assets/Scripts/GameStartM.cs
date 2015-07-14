using UnityEngine;
using System.Collections;

public class GameStartM : MonoBehaviour
{

    public AudioSource[] asList;
    private float mainTime = 0.0f;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (mainTime + Time.deltaTime < 4.0f)
        {
            mainTime += Time.deltaTime;
            asList[0].volume = mainTime / 8.0f;
        }
    }
}
