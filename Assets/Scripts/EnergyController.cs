using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnergyController : MonoBehaviour {

    public static int energyQuantity = fixedEnergy;
    public static int preLevelConsume;
    public static int nextLevelReward=0;
    public static int rewardValue;
    public static bool haveEnergy = true;
    private const int fixedEnergy = 100;
    void Start()
    {
        energyQuantity =fixedEnergy;
        nextLevelReward = 0;
        DontDestroyOnLoad(gameObject);
        rewardValue = 10;
        preLevelConsume = 0;
    }
    void Update()
    {
        if (GameObject.Find("EnergyValue") != null)
        {
            GameObject.Find("EnergyValue").GetComponent<Text>().text = (energyQuantity-preLevelConsume).ToString();

            GameObject.Find("EnergyProgressBar").GetComponent<Image>().fillAmount = ((float)(energyQuantity - preLevelConsume) / fixedEnergy);
        }
        if (energyQuantity <= 0)
        {
            GameObject.Find("UI").GetComponent<UIController>().fail();
        }
        if (energyQuantity - preLevelConsume > 0)
        {
            haveEnergy = true;
        }
        else
        {
            haveEnergy = false;
        }
    }
    public static void makeValue(int height)
    {
        preLevelConsume += height;
    }
    public static void end()
    {
        energyQuantity -= preLevelConsume;
        preLevelConsume = 0;
    }
    public static void unrecover(int x,int y,int z)
    {
        energyQuantity -= y;
    }
    public static void restart()
    {
        preLevelConsume = 0;
        nextLevelReward = 0;
    }
    public static void gameOver()
    {
        energyQuantity = fixedEnergy;
        preLevelConsume = 0;
        nextLevelReward = 0;
    }
    public static void reward(int x,int y,int z)
    {
        energyQuantity += y * 2;
    }
    public static void nextLevel()
    {
        energyQuantity += nextLevelReward;
        nextLevelReward = 0;
    }
}
