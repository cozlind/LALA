using UnityEngine;
using System.Collections;

public class HeadUpBlock : MonoBehaviour {

    public GameObject dragUpBlock;
    public enum BLOCK { STATIC, DRAG, CONFIRM };
    public BLOCK block;
    void OnMouseOver()
    {
    }
    void OnMouseDown()
    {
        if (Input.GetMouseButton(0) && GlobalController.mouse == GlobalController.MOUSE.STATIC)
        {
            switch (block)
            {
                case BLOCK.STATIC:
                    block = BLOCK.DRAG;
                    dragUpBlock.GetComponent<DragUpBlock>().block = DragUpBlock.BLOCK.DRAG;
                    GlobalController.click = true;
                    break;
            }
        }
    }
}
