using UnityEngine;
using System.Collections.Generic;

public class SafeBolt : MonoBehaviour {

	public Transform chiselSnapPoint;
	public GameObject boltBody;
	public float maxTravelDistance;
	public float forceMultiplier;
	private ConfigurableJoint springJoint;
	private Vector3 boltDirection;
	private bool traveling = false;
	private Vector3 restPosition;

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

	}
	
	// Update is called once per frame
	void Update () {
		if (traveling) {
			var rb = boltBody.GetComponent<Rigidbody> ();
			if (Vector3.Distance (rb.position, restPosition) < 0.002f &&
			    rb.velocity.magnitude < 0.002f) {
				traveling = false;
			}
		}
	}

	void OnTriggerEnter(Collider other) {
		var chisel = other.GetComponent<Chisel> ();
		if (chisel != null) {
			chisel.LockIntoPlace (chiselSnapPoint, this);
		}
	}

	public void KnockBolt(float force) {
		if (!traveling) {
			var rb = boltBody.GetComponent<Rigidbody> ();
			rb.AddForce (boltDirection * force * forceMultiplier);
			traveling = true;
		}
	}
}
