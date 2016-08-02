using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class SafeWheel : MonoBehaviour {

	public float maxAngle = 720;
	public float minAngle = 0;
	public UnityEvent onUnlock;

	private float currAngle;
	private ConfigurableJoint joint;
	private Vector3 initialUpAxis;
	private bool unlocked;


	// Use this for initialization
	void Start () {
		joint = GetComponent<ConfigurableJoint> ();
		initialUpAxis = joint.secondaryAxis;
	}

	private float mod(float a, float b)
	{
		return a - b * Mathf.Floor(a / b);
	}

	// Update is called once per frame
	void Update () {
		if (!unlocked) {
			//TODO: Make this transform/rotation independent.
            float angleFromVertical = AngleOffAroundAxis (transform.up, initialUpAxis, transform.TransformDirection(-joint.axis));

			int numTurns = Mathf.FloorToInt (currAngle / 360);

			if (angleFromVertical < 0) {
				angleFromVertical += 360;
			}
			var lastAngleFromVertical = mod (currAngle, 360);

			if (lastAngleFromVertical > 270 && angleFromVertical < 90) {
				//If we've wrapped around going forward.
				numTurns++;
			} else if (lastAngleFromVertical < 90 && angleFromVertical > 270) {
				//If we've wrapped around going backwards.
				numTurns--;
			}
			currAngle = numTurns * 360 + angleFromVertical;

			//Setting this resets the anchor angle.
			joint.secondaryAxis = initialUpAxis;

			var lowerLimit = joint.lowAngularXLimit;
			lowerLimit.limit = Mathf.Clamp (minAngle - currAngle, -90, 0);
			joint.lowAngularXLimit = lowerLimit;

			var upperLimit = joint.highAngularXLimit;
			upperLimit.limit = Mathf.Clamp (maxAngle - currAngle, 0, 90);
			joint.highAngularXLimit = upperLimit;

			if (Mathf.Approximately (currAngle, maxAngle)) {
				Unlock ();
			}
		}
	}

	public static float AngleOffAroundAxis(Vector3 v, Vector3 forward, Vector3 axis)
	{
		Vector3 right = Vector3.Cross(axis, forward);
		forward = Vector3.Cross(right, axis);
		var angleInRadians = Mathf.Atan2 (Vector3.Dot (v, right), Vector3.Dot (v, forward));
		return Mathf.Rad2Deg * angleInRadians;
	}

	public void Unlock() {
		unlocked = true;
		joint.angularXMotion = ConfigurableJointMotion.Locked;
		onUnlock.Invoke ();
	}

	public void Hello() {
		Debug.Log ("I'm unlocked. I'm " + name);
	}
}
