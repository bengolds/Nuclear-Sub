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
    public UnityEvent onUnturn;

	private ConfigurableJoint sliderJoint;
	private ConfigurableJoint turnJoint;
	private LockState state;
	private bool justTurned;
	private float keyAngle;
    private Vector3 initialUp;

	// Use this for initialization
	void Start () {
		keyAngle = 0;
        initialUp = transform.up;
	}
	
	// Update is called once per frame
	void Update () {
		if (keyAngle < maxAngle - 25f && justTurned) {
            KeyUnturned();
		}
	}

	void OnTriggerEnter(Collider otherCollider) {
        var key = getKey(otherCollider);
		if (key != null && state == LockState.Empty) {
			SnapKeyToPosition (key.gameObject);	

			sliderJoint = SetupSliderJoint (otherCollider.attachedRigidbody);
			state = LockState.Sliding;
			MakeKeyTrigger (otherCollider);
		}
	}


	void OnTriggerStay(Collider otherCollider) {
        var key = getKey(otherCollider);
		if (key != null) {
			if (state == LockState.Sliding) {
				float keyTravel = Vector3.Distance (key.transform.position, snapPoint.transform.position);
				if (Mathf.Approximately (keyTravel, lockDepth)) {
					Destroy (sliderJoint);
					turnJoint = SetupTurnJoint (otherCollider.attachedRigidbody);
					lockBody.transform.SetParent (otherCollider.transform, true);
					state = LockState.Turning;
				}
			} else if (state == LockState.Turning) {
				keyAngle = Vector3.Angle (initialUp, transform.up);
				if (keyAngle >= maxAngle-2f && !justTurned) {
					KeyTurned ();
				}
			}
		}
	}

	void OnTriggerExit(Collider other) {
		if (getKey(other) != null && state == LockState.Sliding) {
			state = LockState.Empty;
			Destroy (sliderJoint);
			MakeKeyCollider (other);
		}
	}

	void KeyTurned() {
		justTurned = true;
		onTurn.Invoke ();
    }

    void KeyUnturned()
    {
        Debug.Log("keyangle: " + keyAngle);
        justTurned = false;
        onUnturn.Invoke();
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

		joint.enableCollision = false;

        joint.autoConfigureConnectedAnchor = false;
        joint.anchor = lockBody.transform.InverseTransformPoint(snapPoint.transform.position);
        joint.connectedAnchor = Vector3.zero;

        joint.connectedBody = key;

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

    Key getKey(Collider other)
    {
        return other.GetComponentInParent<Key>();
    }

	void MakeKeyTrigger(Collider keyCollider) {
        keyCollider.isTrigger = true;
	}

	void MakeKeyCollider(Collider keyCollider) {
        keyCollider.isTrigger = false;
	}
}
