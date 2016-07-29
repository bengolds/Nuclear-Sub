﻿using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class LockGuide : MonoBehaviour {
	private enum LockState {
		Empty,
		Sliding,
		Turning
	}

	public float lockDepth = 0.02f;
	public float maxAngle = 90f;
	public GameObject snapPoint;
	public GameObject lockBody;
	public UnityEvent onTurn;

	private ConfigurableJoint sliderJoint;
	private ConfigurableJoint turnJoint;
	private LockState state;
	private bool justTurned;
	private float keyAngle;

	// Use this for initialization
	void Start () {
		keyAngle = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (keyAngle < maxAngle - 25f) {
			justTurned = false;
		}
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
				//TODO I THINK THIS IS WRONG
				keyAngle = Vector3.Angle (other.transform.up, transform.up);
				if (keyAngle >= maxAngle-2f && !justTurned) {
					KeyTurned ();
				}
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

	void KeyTurned() {
		justTurned = true;
		onTurn.Invoke ();
	}

	void SnapKeyToPosition(GameObject key) {
		key.transform.rotation = snapPoint.transform.rotation;
		key.transform.position = snapPoint.transform.position;
	}

	ConfigurableJoint SetupSliderJoint(Rigidbody key) {
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

		joint.connectedBody = key;

		joint.enableCollision = false;

		return joint;
	}

	ConfigurableJoint SetupTurnJoint(Rigidbody key) {

		var joint = key.gameObject.AddComponent<ConfigurableJoint> ();

		joint.angularXMotion = ConfigurableJointMotion.Limited;
		joint.angularYMotion = ConfigurableJointMotion.Locked;
		joint.angularZMotion = ConfigurableJointMotion.Locked;
		joint.xMotion = ConfigurableJointMotion.Locked;
		joint.yMotion = ConfigurableJointMotion.Locked;
		joint.zMotion = ConfigurableJointMotion.Locked;

		var xLimit = new SoftJointLimit ();
		xLimit.limit = -maxAngle; 
		joint.lowAngularXLimit = xLimit;

		var xDrive = new JointDrive ();
		xDrive.positionSpring = 1000f;
		xDrive.maximumForce = 200f;
		joint.angularXDrive = xDrive;

		joint.autoConfigureConnectedAnchor = false;
		joint.connectedAnchor = key.transform.position;
		joint.anchor = Vector3.zero;

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
