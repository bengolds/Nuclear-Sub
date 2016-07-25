using UnityEngine;
using System.Collections.Generic;

public class LockGuide : MonoBehaviour {
	public float lockDepth = 0.04f;
	public GameObject snapPoint;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		if (other.GetComponent<Key> () != null && GetComponent<ConfigurableJoint>() == null) {	
			other.transform.rotation = transform.rotation;
			Vector3 keyCenterPosition = transform.position;
			keyCenterPosition.x = other.transform.position.x;
			other.transform.position = snapPoint.transform.position;

			var joint = gameObject.AddComponent<ConfigurableJoint> ();
			ConfigureJoint (joint);
			joint.connectedBody = other.attachedRigidbody;
		}
	}

	void ConfigureJoint(ConfigurableJoint joint) {
		joint.angularXMotion = ConfigurableJointMotion.Limited;
		joint.angularYMotion = ConfigurableJointMotion.Limited;
		joint.angularZMotion = ConfigurableJointMotion.Limited;
		joint.xMotion = ConfigurableJointMotion.Limited;
		joint.yMotion = ConfigurableJointMotion.Locked;
		joint.zMotion = ConfigurableJointMotion.Locked;

		var linearLimit = new SoftJointLimit ();
		linearLimit.limit = lockDepth; 
		joint.linearLimit = linearLimit;
	}

	void OnTriggerExit(Collider other) {
		if (other.GetComponent<Key> () != null) {
			Destroy (GetComponent<ConfigurableJoint> ());
		}
	}
}
