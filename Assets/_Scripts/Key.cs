using UnityEngine;
using System.Collections.Generic;
using VRTK;

public class Key : VRTK_InteractableObject {

	public override void Grabbed (GameObject currentGrabbingObject)
	{
		base.Grabbed (currentGrabbingObject);
		currentGrabbingObject.GetComponent<VRTK_ControllerActions> ().ToggleControllerModel (false, gameObject);
	}

	public override void Ungrabbed (GameObject previousGrabbingObject)
	{
		base.Ungrabbed (previousGrabbingObject);
		previousGrabbingObject.GetComponent<VRTK_ControllerActions> ().ToggleControllerModel (true, gameObject);
	}

	protected override void Start ()
	{
		base.Start ();
	}

	protected override void Update ()
	{
		base.Update ();
	}
}
