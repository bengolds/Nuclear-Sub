using UnityEngine;
using System.Collections.Generic;

public class LockGuide : MonoBehaviour {
	private enum LockState {
		Empty,
		Sliding,
		Turning
	}

	public float lockDepth = 0.02f;
	public GameObject snapPoint;
	public GameObject lockBody;
	private ConfigurableJoint slideJoint;
	private ConfigurableJoint turnJoint;
	private LockState state;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		if (isKey(other) && state == LockState.Empty) {
			SnapKeyToPosition (other.gameObject);	

			slideJoint = SetupSliderJoint (other.attachedRigidbody);
//			currJoint = SwitchToTurnJoint(other.attachedRigidbody);
			state = LockState.Sliding;
			MakeKeyTrigger (other.gameObject);
		}
	}


	void OnTriggerStay(Collider other) {
		if (isKey(other)) {
			if (state == LockState.Sliding) {
				float keyTravel = Vector3.Distance (other.transform.position, snapPoint.transform.position);
				if (Mathf.Approximately (keyTravel, lockDepth)) {
					Destroy (slideJoint);
					turnJoint = SwitchToTurnJoint (other.attachedRigidbody);
					state = LockState.Turning;
				}
			} else if (state == LockState.Turning) {
				float keyAngle = Vector3.Angle (other.transform.up, transform.up);
			}
		}
	}

	void OnTriggerExit(Collider other) {
		if (isKey(other) && state == LockState.Sliding) {
			state = LockState.Empty;
			//			Destroy (currJoint);
			//			MakeKeyCollider (other.gameObject);
		}
	}


	void SnapKeyToPosition(GameObject key) {
		key.transform.rotation = snapPoint.transform.rotation;
		key.transform.position = snapPoint.transform.position;
	}

	ConfigurableJoint SetupSliderJoint(Rigidbody connectedBody) {
		var joint = lockBody.AddComponent<ConfigurableJoint> ();

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

		return joint;
	}

	ConfigurableJoint SwitchToTurnJoint(Rigidbody connectedBody) {

		var joint = lockBody.AddComponent<ConfigurableJoint> ();

		joint.angularXMotion = ConfigurableJointMotion.Limited;
		joint.angularYMotion = ConfigurableJointMotion.Locked;
		joint.angularZMotion = ConfigurableJointMotion.Locked;
		joint.xMotion = ConfigurableJointMotion.Locked;
		joint.yMotion = ConfigurableJointMotion.Locked;
		joint.zMotion = ConfigurableJointMotion.Locked;

		var xLimit = new SoftJointLimit ();
		xLimit.limit = 90; 
		joint.highAngularXLimit = xLimit;

		var xDrive = new JointDrive ();
		xDrive.positionSpring = 1000f;
		xDrive.maximumForce = 200f;
		joint.angularXDrive = xDrive;

		joint.autoConfigureConnectedAnchor = false;
		joint.connectedAnchor = Vector3.zero;
		joint.anchor = lockBody.transform.InverseTransformPoint (connectedBody.position);

		joint.connectedBody = connectedBody;

		joint.enableCollision = false;

		return joint;
	}
	bool isKey(Collider other) {
		return other.GetComponent<Key> () != null;
	}

	void MakeKeyTrigger(GameObject key) {
		key.GetComponent<Collider> ().isTrigger = true;
	}

	void MakeKeyCollider(GameObject key) {
		key.GetComponent<Collider> ().isTrigger = false;
	}
}
