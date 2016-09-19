using UnityEngine;
using System.Collections.Generic;
using VRTK;

public class SafeDoor : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

    }
    public void UnlockDoor()
    {
        Destroy(GetComponent<FixedJoint>());
        var io = gameObject.AddComponent<VRTK_InteractableObject>();
        io.isGrabbable = true;
        io.highlightOnTouch = true;
        io.touchHighlightColor = new Color(225f/255f, 225f / 255f, 225f / 255f);
        io.grabAttachMechanic = VRTK_InteractableObject.GrabAttachType.Track_Object;
    }
}
