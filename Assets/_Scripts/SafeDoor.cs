using UnityEngine;
using System.Collections.Generic;

public class SafeDoor : MonoBehaviour {

    public Collider safeBody;
	// Use this for initialization
	void Start () {
        Physics.IgnoreCollision(GetComponent<Collider>(), safeBody);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void UnlockDoor() {
		Destroy (GetComponent<FixedJoint> ());
	}
}
