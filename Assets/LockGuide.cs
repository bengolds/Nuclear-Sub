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
	private ConfigurableJoint sliderJoint;
	private ConfigurableJoint turnJoint;
	private FixedJoint fixedJoint;
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

			sliderJoint = SetupSliderJoint (other.attachedRigidbody);
			state = LockState.Sliding;
			MakeKeyTrigger (other.gameObject);
		}
	}


	void OnTriggerStay(Collider other) {
		if (isKey(other)) {
			if (state == LockState.Sliding) {
				float keyTravel = Vector3.Distance (other.transform.position, snapPoint.transform.position);
				if (Mathf.Approximately (keyTravel, lockDepth)) {
					Destroy (sliderJoint);
					turnJoint = SetupTurnJoint (other.attachedRigidbody);
					lockBody.transform.SetParent (other.transform, true);
					state = LockState.Turning;
				}
			} else if (state == LockState.Turning) {
				float keyAngle = Vector3.Angle (other.transform.up, transform.up);
				//CHECK IF KEY IS TURNED HERE
			}
		}
	}

	void OnTriggerExit(Collider other) {
		if (isKey(other) && state == LockState.Sliding) {
			state = LockState.Empty;
			Destroy (sliderJoint);
			MakeKeyCollider (other.gameObject);
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

	ConfigurableJoint SetupTurnJoint(Rigidbody connectedBody) {

		var joint = connectedBody.gameObject.AddComponent<ConfigurableJoint> ();

		joint.angularXMotion = ConfigurableJointMotion.Limited;
		joint.angularYMotion = ConfigurableJointMotion.Locked;
		joint.angularZMotion = ConfigurableJointMotion.Locked;
		joint.xMotion = ConfigurableJointMotion.Locked;
		joint.yMotion = ConfigurableJointMotion.Locked;
		joint.zMotion = ConfigurableJointMotion.Locked;

//		joint.axis = transform.right;
//		joint.secondaryAxis = transform.up;

		var xLimit = new SoftJointLimit ();
		xLimit.limit = -90; 
		joint.lowAngularXLimit = xLimit;

		var xDrive = new JointDrive ();
		xDrive.positionSpring = 1000f;
		xDrive.maximumForce = 200f;
		joint.angularXDrive = xDrive;

		joint.autoConfigureConnectedAnchor = false;
		joint.connectedAnchor = connectedBody.transform.position;
		joint.anchor = Vector3.zero;
//		joint.connectedAnchor = Vector3.zero;
//		joint.anchor = lockBody.transform.InverseTransformPoint (connectedBody.position);

//		joint.connectedBody = connectedBody;

//		joint.enableCollision = false;

		return joint;
	}

	FixedJoint SetupFixedJoint(Rigidbody connectedBody) {
		var joint = lockBody.AddComponent<FixedJoint> ();

		joint.autoConfigureConnectedAnchor = false;
		joint.connectedAnchor = Vector3.zero;
		joint.anchor = lockBody.transform.InverseTransformPoint (connectedBody.position);
		joint.connectedBody = connectedBody;

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
