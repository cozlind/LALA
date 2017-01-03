using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;

public class UIController : MonoBehaviour
{
    public static GameObject normalUI;
    public static GameObject failUI;
    public static GameObject successUI;
    public static GameObject mapUI;
    public static GameObject helpUI;
    public static GameObject blockUI;
    public static GameObject pageUI;
    public static Dictionary<String, GameObject> mapUnits;
    private static List<GameObject> destroyList = new List<GameObject>();
    public static List<GameObject> pages;
    public static Text heightValueUI;
    public static Text pageValueUI;
    private static int heightValue;
    private static int pageValue;
    private static int pageCount = 7;
    private static int maxHeight = 1;
    private static int zeroX = -250;
    private static int zeroY = 170;


    public static bool isPlayBtnActive;
    void Start()
    {
        isPlayBtnActive = true;

        normalUI = GameObject.Find("normalUI");
        failUI = GameObject.Find("failUI");
        successUI = GameObject.Find("successUI");
        mapUI = GameObject.Find("mapUI");
        helpUI = GameObject.Find("helpUI");
        blockUI = GameObject.Find("blockUI");
        pageUI = GameObject.Find("pageUI");
        heightValueUI = GameObject.Find("heightValue_text").GetComponent<Text>();
        pageValueUI = GameObject.Find("pageValue_text").GetComponent<Text>();

        mapUnits = new Dictionary<String, GameObject>();
        for (int i = 0; i < blockUI.transform.childCount; i++)
        {
            GameObject child = blockUI.transform.GetChild(i).gameObject;
            String name = child.name.Replace("_ui", "");
            try
            {
                mapUnits.Add(name, child);
            }
            catch (Exception e)
            {
                Debug.Log("Error:"+name);
            }
            child.SetActive(false);
        }
        pages = new List<GameObject>();
        for (int i = 0; i < pageUI.transform.childCount; i++)
        {
            GameObject child = pageUI.transform.GetChild(i).gameObject;
            pages.Add(child);
            child.SetActive(false);
        }
        resetUI2();
        resetUI1();
        pageValue = 0;
        pageValueUI.text = "01";
        heightValue = 0;
        heightValueUI.text = "01";
        zeroX = (-250+200)/2 - (200 + 250) * GlobalController.maxx/2 / 10;
        zeroY = 0 + (175 + 175) * GlobalController.maxz / 2 / 8;
        //Debug.Log(zeroX+","+zeroY);
    }
    #region 一级UI
    public void resetUI1()
    {
        normalUI.SetActive(false);
        failUI.SetActive(false);
        successUI.SetActive(false);
    }
    public void restart()
    {
        GlobalController.resetMap();
        GlobalController.clearPath();
        EnergyController.restart();
        Application.LoadLevel(Application.loadedLevelName);
    }
    public void goMainscene()
    {
        GlobalController.resetMap();
        GlobalController.clearPath();
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
        GlobalController.resetMap();
        GlobalController.clearPath();
        normalUI.SetActive(false);
        successUI.SetActive(false);
        failUI.SetActive(false);
        CameraController.isEnd = true;
        Camera.main.GetComponent<UnityStandardAssets.ImageEffects.BlurOptimized>().enabled = true;
        EnergyController.end();
        GlobalController.isInteract = false;
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
    #endregion
    #region 二级UI
    public void menuOn()
    {
        CameraController.isMenuOn = true;
        Camera.main.GetComponent<UnityStandardAssets.ImageEffects.BlurOptimized>().enabled = true;
        GlobalController.isInteract = false;
    }
    public void menuOff()
    {
        CameraController.isMenuOff = true;
        Camera.main.GetComponent<UnityStandardAssets.ImageEffects.BlurOptimized>().blurSize = 4;
        Camera.main.GetComponent<UnityStandardAssets.ImageEffects.BlurOptimized>().blurIterations = 2;
        GlobalController.isInteract = true;
    }
    public void resetUI2()
    {
        mapUI.SetActive(false);
        helpUI.SetActive(false);
    }
    public void xray()
    {
        GameObject.Find("GlobalController").GetComponent<GlobalController>().xray();
    }
    #region 帮助页面
    public void help()
    {
        if (helpUI.activeSelf)
        {
            resetUI2();
            menuOff();
        }
        else
        {
            menuOn();
            resetUI2();
            helpUI.SetActive(true);
            showPage();
        }
    }
    public void quitHelp()
    {
        resetUI2();
        menuOff();
    }
    public void showPage()
    {
        String pageNum = (pageValue + 1) < 10 ? "0" + (pageValue + 1).ToString() : (pageValue + 1).ToString();
        pageValueUI.text = pageNum;
        String pageName = "Page" + pageNum;
        foreach (var page in pages)
        {
            if (page.name.Equals(pageName))
            {
                page.SetActive(true);
            }
            else
            {
                page.SetActive(false);
            }
        }
    }
    public void pageUp()
    {
        pageValue = (pageValue + 1) % pageCount;
        showPage();
    }
    public void pageDown()
    {
        pageValue = (pageValue + 6) % pageCount;
        showPage();
    }
    #endregion
    #region 地图页面
    public void map()
    {
        if (mapUI.activeSelf)
        {
            resetUI2();
            menuOff();
        }
        else
        {
            menuOn();
            resetUI2();
            mapUI.SetActive(true);
            showMap();
        }
    }
    void showMap()
    {
        foreach (var unit in destroyList)
        {
            Destroy(unit);
        }
        destroyList = new List<GameObject>();
        String str = (heightValue + 1) < 10 ? "0" + (heightValue + 1).ToString() : (heightValue + 1).ToString();
        heightValueUI.text = str;
        for (int k = GlobalController.typeMap.GetLength(2) - 1, row = 0; k >= 0; k--, row++)
        {
            for (int i = 0, col = 0; i < GlobalController.typeMap.GetLength(0); i++, col++)
            {
                String typeStr = GlobalController.typeMap[i, heightValue, k].ToString();
                String unitName = typeStr;
                if (typeStr.Equals("0"))
                {
                    continue;
                }
                if (typeStr.StartsWith("Move"))
                {
                    unitName = "Move" + typeStr.Split(":".ToCharArray())[2].ToString();
                }
                if (typeStr.StartsWith("Arrow"))
                {
                    unitName = "Arrow" + typeStr.Split(":".ToCharArray())[1].ToString();
                }
                try
                {
                    GameObject unit = Instantiate(mapUnits[unitName], Vector3.zero, Quaternion.identity) as GameObject;
                    destroyList.Add(unit);
                    unit.SetActive(true);
                    unit.transform.SetParent(blockUI.transform);
                    unit.GetComponent<RectTransform>().localPosition = new Vector3(zeroX + col * 50, zeroY - row * 50, 0);
                }
                catch (Exception e)
                {
                    Debug.Log("unitName:" + unitName + " typeStr:" + typeStr);
                }
            }
        }
    }
    public void quitMap()
    {
        resetUI2();
        menuOff();
    }
    public void updateMaxHeight()
    {
        for (int j = GlobalController.map.GetLength(1) - 1; j >= 0; j--)
        {
            for (int k = GlobalController.map.GetLength(2) - 1; k >= 0; k--)
            {
                for (int i = 0; i < GlobalController.map.GetLength(0); i++)
                {
                    if (GlobalController.map[i, j, k] != 0)
                    {
                        maxHeight = j;
                        return;
                    }
                }
            }
        }
    }
    public void heightUp()
    {
        updateMaxHeight();
        heightValue = (heightValue + 1) % (maxHeight+1);
        showMap();
    }
    public void heightDown()
    {
        updateMaxHeight();
        heightValue = (heightValue + maxHeight) % (maxHeight + 1);
        showMap();
    }
    #endregion
    #endregion
}
