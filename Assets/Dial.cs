using UnityEngine;
using System.Collections.Generic;

public class Dial : Interactable {

	[HideInInspector]
	public ButtonState state;
	public Material normalMaterial;
	public Material highlightedMaterial;
	public Material pressedMaterial;
	public float scrollSpeed = 60.0f;

	private MeshRenderer meshRenderer;
	private Vector2? lastTrackpadPos;
	private float scrollAmount = 0;

	// Use this for initialization
	public override void Start () {
		base.Start ();
		meshRenderer = GetComponent<MeshRenderer> ();
		state = ButtonState.Normal;
		lastTrackpadPos = null;
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

		transform.rotation = Quaternion.AngleAxis (scrollAmount, Vector3.left);
	}

	protected override void OnHandEnter(Hand hand) {
		state = ButtonState.Highlighted;
	}

	protected override void OnHandExit(Hand hand) {
		state = ButtonState.Normal;
		hand.SetVisible (true);
	}

	protected override void OnHandPressDown(Hand hand) {
		state = ButtonState.Pressed;
		hand.SetVisible (false);
	}

	protected override void OnHandPressUp(Hand hand) {
		if (state == ButtonState.Pressed) {
			ButtonPressed ();
		}
		state = ButtonState.Highlighted;
		hand.SetVisible (true);
	}

	protected override void OnHandPress(Hand hand) {
		if (state == ButtonState.Pressed) {
			if (hand.device.GetTouch (SteamVR_Controller.ButtonMask.Touchpad)) {
				Vector2 trackpadPos = hand.device.GetAxis ();
				if (lastTrackpadPos != null) {
					Vector2 trackpadDelta = trackpadPos - (Vector2)lastTrackpadPos;
					ScrollDial (trackpadDelta.y);
				}
				lastTrackpadPos = trackpadPos;
			} else {
				lastTrackpadPos = null;
			}
		}
	}

	void ScrollDial(float amount) {
		Debug.Log ("Scroll amount:" + amount);
		scrollAmount += amount * scrollSpeed;
	}

	void ButtonPressed() {
		Debug.Log ("pressed");
	}
}
