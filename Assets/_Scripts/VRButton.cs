using UnityEngine;
using System.Collections;using VRTK;


public class VRButton : VRTK_InteractableObject {
	public Material touchedStateMaterial;
	public Material usedStateMaterial;
	protected Material normalStateMaterial;
	private MeshRenderer meshRenderer;

	public override void StopUsing(GameObject usingObject)
	{
		base.StopUsing (usingObject);
		ButtonPressed ();
	}

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		meshRenderer = GetComponent<MeshRenderer> ();
		normalStateMaterial = meshRenderer.material;
	}
//	
//	// Update is called once per frame
	protected override void Update () {
		base.Update ();
		if (IsUsing ()) {
			meshRenderer.material = usedStateMaterial;
		} else if (IsTouched ()) {
			meshRenderer.material = touchedStateMaterial;
		} else {
			meshRenderer.material = normalStateMaterial;
		}
	}

	void ButtonPressed() {
		Debug.Log ("Button pressed");
	}
}
