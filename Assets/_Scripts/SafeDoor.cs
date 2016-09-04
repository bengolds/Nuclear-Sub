using UnityEngine;
using System.Collections.Generic;

public class SafeDoor : MonoBehaviour {

    public GameObject safeBody;
	// Use this for initialization
	void Start () {
        var colliders = safeBody.GetComponentsInChildren<Collider>();
        foreach (var coll in colliders)
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), coll);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void UnlockDoor() {
		Destroy (GetComponent<FixedJoint> ());
	}
}
