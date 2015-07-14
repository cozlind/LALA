using UnityEngine;
using System.Collections;

public class StatueRotate : MonoBehaviour {

	void FixedUpdate () {
	    transform.Rotate(Vector3.up,0.2f);
	}
}
