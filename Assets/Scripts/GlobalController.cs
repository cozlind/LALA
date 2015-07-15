using UnityEngine;
using System.Collections;

public class GlobalController : MonoBehaviour
{
    public static bool isInteract;
    public static bool isPlayBtnActive;

    public GameObject centerCube;
    public static float maxHeight = 3f;
    public float moveX_Speed = 1f;
    public float moveY_Speed = 1f;
    public float moveZ_Speed = 0.2f;
    public enum MOUSE { STATIC, DRAG, CONFIRM };
    public static MOUSE mouse;
    public static bool click;
    //地图信息
    public static int[,] initialMap = new int[3, 3] { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 2 } };
    public static int[,] map=new int[3,3];
    public static int startx = 0;
    public static int starty = 0;
    public static int endx = 2;
    public static int endy = 2;
    public static int[,] vineBlock = new int[3, 3] { { 0,0,1 }, { 0, 0, 0 }, { 0, 0, 0} };//藤蔓方块
    public static int[,] woodenBlock = new int[3, 3] { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };//木块
    public static int[,] blackBlock = new int[3, 3]{ { 0,0,0 }, { 1, 0, 0 }, { 0, 0, 0} };//惩罚黑块
    public static int[,] goldBlock = new int[3, 3] { { 0, 0, 0 }, { 0, 0, 0 }, { 1, 0, 0 } };//奖励金块
    public static int[,] jumpBlock = new int[3, 3] { { 0, 1, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };

    
    public float startTime = 0f;
    private bool isEntering = false;
    private static bool isEnd = false;

    public GameObject normalUI;
    public GameObject failUI;
    public GameObject successUI;
    public static void clearMap()
    {
        for (int i = 0; i < initialMap.GetLength(0); i++)
        {
            for (int j = 0; j < initialMap.GetLength(1); j++)
            {
                map[i, j] = initialMap[i, j];
            }
        }
    }
    void Start()
    {
        clearMap(); 
        isPlayBtnActive = true;
        isInteract = true;
        normalUI.SetActive(false);
        failUI.SetActive(false);
        successUI.SetActive(false);
        isEntering = true;
        Camera.main.GetComponent<UnityStandardAssets.ImageEffects.BlurOptimized>().blurSize = 10;
        Camera.main.GetComponent<UnityStandardAssets.ImageEffects.BlurOptimized>().blurIterations = 4;
        mouse = MOUSE.STATIC;
        click = false;
    }
    void FixedUpdate()
    {
        if (isEntering)
        {
            startTime += Time.fixedDeltaTime * 10;
            Camera.main.GetComponent<UnityStandardAssets.ImageEffects.BlurOptimized>().blurSize = 10 - startTime;
            Camera.main.GetComponent<UnityStandardAssets.ImageEffects.BlurOptimized>().blurIterations = (int)(4 - startTime * 0.4f);
            if (Camera.main.GetComponent<UnityStandardAssets.ImageEffects.BlurOptimized>().blurSize <= 0)
            {
                startTime = 0;
                Camera.main.GetComponent<UnityStandardAssets.ImageEffects.BlurOptimized>().enabled = false;
                isEntering = false;
                normalUI.SetActive(true);
            }
        }
        if (isEnd)
        {
            startTime += Time.fixedDeltaTime * 10;
            Camera.main.GetComponent<UnityStandardAssets.ImageEffects.BlurOptimized>().blurSize = startTime;
            Camera.main.GetComponent<UnityStandardAssets.ImageEffects.BlurOptimized>().blurIterations = (int)(startTime * 0.4f);
            if (Camera.main.GetComponent<UnityStandardAssets.ImageEffects.BlurOptimized>().blurSize>= 9.9f)
            {
                startTime = 0;
                isEnd = false;
            }
        }
    }
    void Update()
    {
       
    }
    public void restart()
    {
        clearMap();
        GameObject.Find("Map").GetComponent<MapController>().clearPath();
        EnergyController.restart();
        Application.LoadLevel(Application.loadedLevelName);
    }
    public void goMainscene()
    {
        clearMap();
        GameObject.Find("Map").GetComponent<MapController>().clearPath();
        EnergyController.gameOver();
        Application.LoadLevel("mainscene");
    }
    public void fail()
    {
        failUI.SetActive(true);
        EnergyController.gameOver();
    }
    public void end()
    {
        clearMap();
        GameObject.Find("Map").GetComponent<MapController>().clearPath();
        normalUI.SetActive(false);
        successUI.SetActive(false);
        failUI.SetActive(false);
        isEnd = true;
        Camera.main.GetComponent<UnityStandardAssets.ImageEffects.BlurOptimized>().enabled = true;
        EnergyController.end();
        isInteract = false;
        if (EnergyController.energyQuantity <= 0)
        {
            fail();
        }
        else
        {
            success();
        }
    }
    public void success()
    {
        successUI.SetActive(true);
    }
    public void nextLevel()
    {
        EnergyController.nextLevel();
        Application.LoadLevel(Application.loadedLevel + 1);
    }
}
