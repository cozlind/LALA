using UnityEngine;
using System.Collections;

public class BlockActive : MonoBehaviour
{
    //static Vector3 tPos;
    //public static bool isRunOver = false;
    public Transform grdpos;
    private GameObject tPos;
    private Vector3 zore =new Vector3(0,-2,0);
    // Use this for initialization
    void Start()
    {
        tPos = GameObject.Find("tragetPos");
        //左上跳
        //rigidbody.AddForce(new Vector3(-2.0f, 12.0f, 0.0f), ForceMode.Impulse);
        //左上跳两格
        //rigidbody.AddForce(new Vector3(-1.7f, 15.0f, 0.0f), ForceMode.Impulse);
        //右下跳
        //rigidbody.AddForce(new Vector3(1.7f,9.5f, 0.0f), ForceMode.Impulse);
        //前上跳
        //rigidbody.AddForce(new Vector3(0.0f, 15.0f, 1.6f), ForceMode.Impulse);
        //rigidbody.isKinematic = false;
    }
    // Update is called once per frame
    void Update()
    {
        float v = Vector3.Distance(tPos.transform.position,transform.position);
        if (v < 0.35f) {

            transform.position =Vector3.Slerp(transform.position, tPos.transform.position,0.2f);
            if (!gameObject.GetComponent<Rigidbody>().isKinematic)
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
            if (v < 0.05f) { 
                
                tPos.transform.position = zore;
                MapController.isPlay = true;
            }
        }

    }
    
}
