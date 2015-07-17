using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour
{
    public static GameObject normalUI;
    public static GameObject failUI;
    public static GameObject successUI;

    public static bool isPlayBtnActive;
    void Start()
    {
        isPlayBtnActive = true;

        normalUI = GameObject.Find("normalUI");
        normalUI.SetActive(false);
        failUI = GameObject.Find("failUI");
        failUI.SetActive(false);
        successUI = GameObject.Find("successUI");
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
    public void look()
    {
        GameObject.Find("GlobalController").GetComponent<GlobalController>().look();
    }
}
