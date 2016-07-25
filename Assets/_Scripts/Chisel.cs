using UnityEngine;
using System.Collections.Generic;
using VRTK;

public class Chisel : VRTK_InteractableObject {

	public float detachFromBoltDistance;
	private FixedJoint boltJoint;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}

	// Update is called once per frame
	protected override void Update () {
		base.Update ();
		if (IsGrabbed () && boltJoint != null) {
			Vector3 attachPosition = grabbingObject.GetComponent<VRTK_InteractGrab> ().controllerAttachPoint.position;
			float handDistance = Vector3.Distance (transform.position, attachPosition);
			Debug.Log ("Grab distance: " + handDistance);
			if (handDistance > detachFromBoltDistance) {
				Destroy (boltJoint);
			}
		}
	}

	public override void Ungrabbed (GameObject previousGrabbingObject)
	{
		base.Ungrabbed (previousGrabbingObject);
		if (boltJoint != null) {
//			Destroy (boltJoint);
		}
	}

	public void LockIntoPlace(Transform snapPoint) {
		if (IsGrabbed()) {
			transform.rotation = snapPoint.rotation;
			transform.position = snapPoint.position;
			boltJoint = gameObject.AddComponent<FixedJoint> ();
		}
	}
}
