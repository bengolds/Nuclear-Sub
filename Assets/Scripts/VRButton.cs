using UnityEngine;
using System.Collections;

public enum ButtonState {
	Normal,
	Highlighted,
	Pressed
}

public class VRButton : Interactable {
	[HideInInspector]
	public ButtonState state;
	public Material normalMaterial;
	public Material highlightedMaterial;
	public Material pressedMaterial;
	private MeshRenderer meshRenderer;

	// Use this for initialization
	public override void Start () {
		base.Start ();
		meshRenderer = GetComponent<MeshRenderer> ();
		state = ButtonState.Normal;
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update ();

		switch (state) {
		case ButtonState.Normal:
			meshRenderer.material = normalMaterial;
			break;
		case ButtonState.Highlighted:
			meshRenderer.material = highlightedMaterial;
			break;
		case ButtonState.Pressed:
			meshRenderer.material = pressedMaterial;
			break;
		default:
			break;
		}
	}



	protected override void OnHandEnter(Hand hand) {
		state = ButtonState.Highlighted;
	}

	protected override void OnHandExit(Hand hand) {
		state = ButtonState.Normal;
	}

	protected override void OnHandPressDown(Hand hand) {
		state = ButtonState.Pressed;
	}

	protected override void OnHandPressUp(Hand hand) {
		if (state == ButtonState.Pressed) {
			ButtonPressed ();
		}
		state = ButtonState.Highlighted;
	}

	void ButtonPressed() {
		Debug.Log ("Button pressed");
	}
}
