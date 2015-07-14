using UnityEngine;
using System.Collections;

public class MainsceneUI : MonoBehaviour {

    public void begin()
    {
        Application.LoadLevel(1);
    }
    public void exit()
    {
        Application.Quit();
    }
}
