using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class ExchangeBlock : MonoBehaviour
{

    //基本结构
    public GameObject shellBlock;
    public int x;
    public int y;
    public int z;
    public string type;
    public enum BLOCK { STATIC, DRAG, CONFIRM };
    public BLOCK block;
    List<GameObject> row;
    //拖拽参数
    public float exchangeOffset = 0.3f;
    private int energyValue1;
    private int energyValue2;
    public float posYBasicOffset;
    public float dragSpeedY = 0.2f;

    public AudioSource asMove;
    private bool isPlayerAs = false;
    private bool isFirstMove = false;
    private float upMoveY = 0.0f;
    void updateMap()
    {
        updateToMap(x, y, z);
        GlobalController.typeMap[x, y, z] = type;
    }
    void updateToMap(int x, int y, int z)
    {
        GlobalController.map[x, y, z] = 1;
    }
    List<GameObject> getRow(int i)
    {
        return GlobalController.getRow(x, y);
    }
    void Start()
    {
        updatePos();
        type = "Exchange";
    }
    public void updatePos()
    {
        x = System.Convert.ToInt32(transform.position.x);
        y = System.Convert.ToInt32(transform.position.y);
        z = System.Convert.ToInt32(transform.position.z);
    }
    public void updatePosY(int i)
    {
        x = System.Convert.ToInt32(transform.position.x);
        y = i;
        z = System.Convert.ToInt32(transform.position.z);
    }
    public void printPos(string head = "")
    {
        Debug.Log(head + "(" + x + "," + y + "," + z + ") ");
    }
    void OnMouseDown()
    {
        //printPos();
    }
    void OnMouseDrag()
    {
        if (Input.GetMouseButton(0) && block == BLOCK.STATIC)
        {
            energyValue1 = y;
            row = getRow(y);
            block = BLOCK.DRAG;
        }
    }
    void OnMouseUp()
    {
        if (Input.GetMouseButtonUp(0) && block == BLOCK.DRAG)
        {
            block = BLOCK.CONFIRM;
        }
    }
    void Update()
    {
        if (GlobalController.isInteract)
        {
            switch (block)
            {
                case BLOCK.DRAG:
                    if (Cursor.visible)
                    {
                        Cursor.visible = false;
                    }
                    float mousY = transform.position.y + Input.GetAxis("Mouse Y") * dragSpeedY;
                    energyValue2 = System.Convert.ToInt32(mousY);
                    if (!isFirstMove)
                    {
                        upMoveY = mousY;
                        isFirstMove = true;
                    }
                    if (Mathf.Abs(upMoveY - mousY) > 0.1f)
                    {
                        if (asMove != null && !asMove.isPlaying && !isPlayerAs)
                        {
                            isPlayerAs = true;
                            asMove.Play();
                        }
                        upMoveY = mousY;
                    }
                    float posY = posYBasicOffset + mousY;
                    if (mousY >= -0.4 && posY < GlobalController.maxy - 0.6 && energyValue2 - energyValue1 < EnergyController.energyQuantity - EnergyController.preLevelConsume)
                    {
                        //移动当前row
                        foreach (var rowItem in row)
                        {
                            rowItem.transform.position = new Vector3(rowItem.transform.position.x, posY, rowItem.transform.position.z);
                        }
                        //上移当前row直至触发上层下移
                        if (posY > y + 1 + exchangeOffset)
                        {
                            GlobalController.exchangeRow(x, y, "Up");
                        }
                        //下移当前row直至触发下层上移
                        else if (posY < y - 1 - exchangeOffset)
                        {
                            GlobalController.exchangeRow(x, y, "Down");
                        }
                    }
                    break;
                case BLOCK.CONFIRM:
                    Cursor.visible = true;

                    mousY = transform.position.y + Input.GetAxis("Mouse Y") * dragSpeedY;
                    energyValue2 = System.Convert.ToInt32(mousY);
                    //Debug.Log(energyValue2);
                    foreach (var rowItem in row)
                    {
                        rowItem.transform.position = new Vector3(System.Convert.ToInt32(rowItem.transform.position.x), y, System.Convert.ToInt32(rowItem.transform.position.z));
                    }
                    //更新地图数据
                    GlobalController.isUpdateMap = true;
                    EnergyController.makeValue(energyValue2 - energyValue1);
                    //更新状态
                    block = BLOCK.STATIC;
                    isPlayerAs = false;
                    isFirstMove = false;
                    break;
            }
        }
    }
}
