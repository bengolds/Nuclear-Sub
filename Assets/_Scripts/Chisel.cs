using UnityEngine;
using System.Collections.Generic;
using VRTK;

public class Chisel : VRTK_InteractableObject {

	public float detachFromBoltDistance;
	public bool detachesWhenUngrabbed = true;
	private FixedJoint boltJoint;
	private SafeBolt attachedBolt;
	private Vector3 handleAttachPosition;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}

	// Update is called once per frame
	protected override void Update () {
		base.Update ();
		if (IsGrabbed () && boltJoint != null) {				
			Vector3 attachPosition = grabbingObject.GetComponent<VRTK_InteractGrab> ().controllerAttachPoint.position;
			float handDistance = Vector3.Distance (rightSnapHandle.position, attachPosition);
			if (handDistance > detachFromBoltDistance) {
				Destroy (boltJoint);
				attachedBolt = null;
			}
		}
	}

	public override void Ungrabbed (GameObject previousGrabbingObject)
	{
		base.Ungrabbed (previousGrabbingObject);
		if (boltJoint != null && detachesWhenUngrabbed) {
			Destroy (boltJoint);
			attachedBolt = null;
		}
	}

	public void LockIntoPlace(Transform snapPoint, SafeBolt bolt) {
		if (IsGrabbed()) {	
			transform.rotation = snapPoint.rotation;
			transform.position = snapPoint.position;
			boltJoint = gameObject.AddComponent<FixedJoint> ();
			attachedBolt = bolt;
		}
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.GetComponentInParent<Hammer> () != null && 
			attachedBolt != null) {
			Vector3 chiselAxis = transform.right;
			float speedAlongAxis = Vector3.Dot (collision.relativeVelocity, chiselAxis);
			attachedBolt.KnockBolt (Mathf.Clamp(speedAlongAxis, 0, float.PositiveInfinity));
		}
	}
}
