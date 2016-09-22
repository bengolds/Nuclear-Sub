using UnityEngine;
using System.Collections;

public class ResetOnLeaveBounds : MonoBehaviour {
    Vector3 origPosition;
    Quaternion origRotation;

	// Use this for initialization
	void Start () {
        origPosition = transform.position;
        origRotation = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.GetComponent<ResetBounds>() != null)
        {
            transform.position = origPosition;
            transform.rotation = origRotation;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
    }
}
