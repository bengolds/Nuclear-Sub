using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class Dial : Interactable {

	[HideInInspector]
	public ButtonState state;
	public Material normalMaterial;
	public Material highlightedMaterial;
	public Material pressedMaterial;
	public float scrollSpeed = 60.0f;
	public float snapDuration = 0.5f;
	public int numFaces = 5;
	public int value {
		get { return m_value; }
	}

	private MeshRenderer meshRenderer;
	private Vector2? lastTrackpadPos;
	private float scrollAmount = 0;
	private int m_value = 0;

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

		transform.rotation = transform.parent.rotation * Quaternion.AngleAxis (-scrollAmount, Vector3.right);
	}

	protected override void OnHandEnter(Hand hand) {
		state = ButtonState.Highlighted;
	}

	protected override void OnHandExit(Hand hand) {
		if (state == ButtonState.Pressed) {
			DialingExit (hand);
		}
		state = ButtonState.Normal;
	}

	protected override void OnHandPressDown(Hand hand) {
		state = ButtonState.Pressed;
		DialingEnter (hand);
	}

	protected override void OnHandPressUp(Hand hand) {
		if (state == ButtonState.Pressed) {
			DialingExit (hand);
		}
		state = ButtonState.Highlighted;
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
		
	void DialingEnter(Hand hand) {
		hand.SetVisible (false);
	}

	void DialingExit(Hand hand) {
		hand.SetVisible (true);
		SnapDial ();
	}

	int mod(int x, int m) {
		return (x%m + m)%m;
	}

	void SnapDial() {
		float snapIncrement = 360f / numFaces;
		int selectedFace = Mathf.RoundToInt(scrollAmount / snapIncrement);
		float snapTo = selectedFace * snapIncrement;
		m_value = mod(selectedFace, numFaces) + 2;
		
		DOTween.To (x => scrollAmount = x, scrollAmount, snapTo, snapDuration);
	}

	void ScrollDial(float amount) {
		scrollAmount += -1 * amount * scrollSpeed;
	}
}
