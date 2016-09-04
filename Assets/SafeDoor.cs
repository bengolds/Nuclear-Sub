using UnityEngine;
using System.Collections.Generic;

public class SafeDoor : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

    }
    public void UnlockDoor()
    {
        Destroy(GetComponent<FixedJoint>());
    }
}
