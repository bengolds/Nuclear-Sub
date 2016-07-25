using UnityEngine;
using System.Collections.Generic;

public class LockGuide : MonoBehaviour {
	public float lockDepth = 0.02f;
	public GameObject snapPoint;
	public GameObject lockBody;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		if (other.GetComponent<Key> () != null && lockBody.GetComponent<ConfigurableJoint>() == null) {
			SnapKeyToPosition (other.gameObject);	

			var joint = lockBody.AddComponent<ConfigurableJoint> ();
			ConfigureJoint (joint, other.attachedRigidbody);

			MakeKeyTrigger (other.gameObject);
		}
	}

	void SnapKeyToPosition(GameObject key) {
		key.transform.rotation = snapPoint.transform.rotation;
		key.transform.position = snapPoint.transform.position;
	}

	void ConfigureJoint(ConfigurableJoint joint, Rigidbody connectedBody) {
		joint.angularXMotion = ConfigurableJointMotion.Locked;
		joint.angularYMotion = ConfigurableJointMotion.Locked;
		joint.angularZMotion = ConfigurableJointMotion.Locked;
		joint.xMotion = ConfigurableJointMotion.Limited;
		joint.yMotion = ConfigurableJointMotion.Locked;
		joint.zMotion = ConfigurableJointMotion.Locked;

		var linearLimit = new SoftJointLimit ();
		linearLimit.limit = lockDepth; 
		joint.linearLimit = linearLimit;

		joint.connectedBody = connectedBody;

		joint.enableCollision = false;
	}

	void MakeKeyTrigger(GameObject key) {
		key.GetComponent<Collider> ().isTrigger = true;
	}

	void MakeKeyCollider(GameObject key) {
		key.GetComponent<Collider> ().isTrigger = false;
	}

	void OnTriggerExit(Collider other) {
		if (other.GetComponent<Key> () != null) {
			Destroy (lockBody.GetComponent<ConfigurableJoint> ());
			MakeKeyCollider (other.gameObject);
		}
	}
}
