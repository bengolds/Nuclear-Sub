using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using VRTK;

public class Dial : VRTK_InteractableObject {
	
	[Header("Dial Settings", order = 5)]
	public Material touchedStateMaterial;
	public Material usedStateMaterial;
	protected Material normalStateMaterial;
	public ushort scrollingHapticsStrength;
	public ushort clickingHapticsStrength;
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
	protected override void Start () {
		base.Start ();
		meshRenderer = GetComponent<MeshRenderer> ();
		normalStateMaterial = meshRenderer.material;
		lastTrackpadPos = null;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
		if (IsUsing ()) {
			meshRenderer.material = usedStateMaterial;
		} else if (IsTouched ()) {
			meshRenderer.material = touchedStateMaterial;
		} else {
			meshRenderer.material = normalStateMaterial;
		}

		transform.rotation = transform.parent.rotation * Quaternion.AngleAxis (-scrollAmount, Vector3.right);
	}

	public override void StartUsing(GameObject usingWith)
	{
		base.StartUsing (usingWith);
		usingWith.GetComponent<VRTK_ControllerEvents> ().TouchpadAxisChanged += TouchpadAxisChanged;
		usingWith.GetComponent<VRTK_ControllerEvents> ().TouchpadTouchEnd += TouchpadTouchEnd;
		usingWith.GetComponent<VRTK_ControllerActions> ().SetControllerOpacity (0.1f);
	}

	public override void StopUsing(GameObject usingWith)
	{
		base.StopUsing (usingWith);
		usingWith.GetComponent<VRTK_ControllerEvents> ().TouchpadAxisChanged -= TouchpadAxisChanged;
		usingWith.GetComponent<VRTK_ControllerEvents> ().TouchpadTouchEnd -= TouchpadTouchEnd;
		usingWith.GetComponent<VRTK_ControllerActions> ().SetControllerOpacity (1.0f);
		SnapDial ();
	}

	protected void TouchpadAxisChanged(object sender, ControllerInteractionEventArgs e) {
		if (IsUsing ()) {
			Vector2 trackpadPos = e.touchpadAxis;
			if (lastTrackpadPos != null) {
				Vector2 trackpadDelta = trackpadPos - (Vector2)lastTrackpadPos;
				var controllerActions = ((VRTK_ControllerEvents)sender).GetComponent<VRTK_ControllerActions> ();
				ScrollDial (trackpadDelta.y, controllerActions);
			}
			lastTrackpadPos = trackpadPos;
		} else {
			Debug.LogWarning ("The touchpadaxischanged event didn't get released. Weird.");
		}
	}

	protected void TouchpadTouchEnd(object sender, ControllerInteractionEventArgs e) {
		if (IsUsing ()) {
			lastTrackpadPos = null;
		} else {
			Debug.LogWarning ("The touchpadtouchend event didn't get released. Weird.");
		}
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

	void ScrollDial(float amount, VRTK_ControllerActions controller) {
		float snapIncrement = 360f / numFaces;
		float currentLocation = scrollAmount / snapIncrement;
		float amountToAdd = -1 * amount * scrollSpeed;
		float newLocation = (scrollAmount + amountToAdd) / snapIncrement;

		if (Mathf.RoundToInt (currentLocation) != Mathf.RoundToInt (newLocation)) {		
			controller.TriggerHapticPulse (clickingHapticsStrength, 1f, 1f);		
		} else {
			controller.TriggerHapticPulse (scrollingHapticsStrength, 0.1f, 0.1f);
		}

		scrollAmount += amountToAdd;
	}
}
