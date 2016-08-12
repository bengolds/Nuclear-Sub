using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class SafeWheel : MonoBehaviour {

	public float maxAngle = 720;
	public float minAngle = 0;
    public bool locked = true;
    public UnityEvent onOpen;

	private float currAngle;
	private ConfigurableJoint joint;
	private Vector3 initialUpAxisWorld;
    private Vector3 initialForwardAxisWorld;
    private Vector3 localUpAxis;
	private bool isOpen;


	// Use this for initialization
	void Start () {
		joint = GetComponent<ConfigurableJoint> ();
		localUpAxis = joint.secondaryAxis;
        initialUpAxisWorld = transform.TransformDirection(localUpAxis);
        initialForwardAxisWorld = transform.TransformDirection(-joint.axis);
	}

	private float mod(float a, float b)
	{
		return a - b * Mathf.Floor(a / b);
	}

    // Update is called once per frame
    void Update() {
        joint.angularXMotion = locked || isOpen ? ConfigurableJointMotion.Locked : ConfigurableJointMotion.Limited;

		if (!isOpen) {
            //TODO: Make this transform/rotation independent.
            Vector3 currUpAxis = transform.TransformDirection(localUpAxis);
            float angleFromVertical = AngleOffAroundAxis(currUpAxis, initialUpAxisWorld, initialForwardAxisWorld);


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
			joint.secondaryAxis = localUpAxis;

			var lowerLimit = joint.lowAngularXLimit;
			lowerLimit.limit = Mathf.Clamp (minAngle - currAngle, -90, 0);
			joint.lowAngularXLimit = lowerLimit;


			var upperLimit = joint.highAngularXLimit;
			upperLimit.limit = Mathf.Clamp (maxAngle - currAngle, 0, 90);
			joint.highAngularXLimit = upperLimit;
            
            if (Mathf.Abs(currAngle - maxAngle) < 0.1) {
				Open ();
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

    public void Unlock()
    {
        locked = false;
    }

	private void Open() {
		isOpen = true;
		joint.angularXMotion = ConfigurableJointMotion.Locked;
		onOpen.Invoke ();
	}
}
