using UnityEngine;
using System.Collections.Generic;

public class SafeBolt : MonoBehaviour {

	public Transform chiselSnapPoint;
	public GameObject boltBody;
	public GameObject victoryArea;
	public float maxTravelDistance;
	public float forceMultiplier;
	public float victoryMax, victoryMin;
	public float boltLength;
	public bool unlocked;


	private ConfigurableJoint springJoint;
	private Vector3 boltDirection;
	private bool traveling = false;
	private Vector3 restPosition;
	private bool lockable = true;
	private Vector3 lastVelocity;

	// Use this for initialization
	void Start () {
		Physics.IgnoreLayerCollision (LayerMask.NameToLayer ("Default"), LayerMask.NameToLayer ("Bolt"));

		restPosition = boltBody.transform.position;

		springJoint = boltBody.GetComponent<ConfigurableJoint> ();
		boltDirection = -boltBody.transform.right;
		springJoint.connectedAnchor = boltBody.transform.TransformPoint (springJoint.anchor) + boltDirection * maxTravelDistance/2; 

		var linearLimit = new SoftJointLimit ();
		linearLimit.limit = maxTravelDistance / 2;
		springJoint.linearLimit = linearLimit;

		boltBody.transform.localScale = new Vector3(boltLength, boltBody.transform.localScale.y, boltBody.transform.localScale.z);
		victoryArea.transform.localScale = new Vector3(victoryMax - victoryMin, victoryArea.transform.localScale.y, victoryArea.transform.localScale.z);

		var victoryPos = victoryArea.transform.localPosition;
		victoryPos.x = victoryMin + (victoryMax - victoryMin) / 2;
		victoryArea.transform.localPosition = -victoryPos;
		}
	
	// Update is called once per frame
	void Update () {
		if (traveling) {
			var rb = boltBody.GetComponent<Rigidbody> ();

			float travelDistance = Vector3.Distance (rb.position, restPosition);

			if (Vector3.Dot (rb.velocity, lastVelocity) <= 0 && 
				travelDistance + boltLength/2 <= victoryMax &&
				travelDistance - boltLength/2 >= victoryMin) {	
				unlocked = true;
				rb.isKinematic = true;
				//Call other unlock event.
			}

			if (travelDistance < 0.002f &&
			    rb.velocity.magnitude < 0.002f) {
				traveling = false;
				lastVelocity = Vector3.zero;
			}
			lastVelocity = rb.velocity;
		}
	}

	void OnTriggerStay(Collider other) {
		var chiselHead = other.GetComponent<ChiselHead> ();
		if (chiselHead != null && lockable) {
			if (Quaternion.Angle (chiselHead.transform.rotation, chiselSnapPoint.rotation) < 15.0f) {
				chiselHead.GetComponentInParent<Chisel>().LockIntoPlace (chiselSnapPoint, this);
				lockable = false;
			}
		}
	}

	void OnTriggerExit(Collider other) {
		var chiselHead = other.GetComponent<ChiselHead> ();
		if (chiselHead != null) {
			lockable = true;	
		}
	}

	public void KnockBolt(float force) {
		if (!traveling && force > 0 ) {
			var rb = boltBody.GetComponent<Rigidbody> ();
			rb.AddForce (boltDirection * force * forceMultiplier);
			traveling = true;
		}
	}
}
