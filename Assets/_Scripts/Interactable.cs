using UnityEngine;
using System.Collections.Generic;

public class Interactable : MonoBehaviour {
	protected List<Hand> hands;

	public virtual void Start() {
		hands = new List<Hand> ();
	}

	public virtual void Update() {
		foreach (var hand in hands) {
			var trigger = SteamVR_Controller.ButtonMask.Trigger;
			if (hand.device.GetPressDown (trigger)) {
				OnHandPressDown (hand);
			}
			if (hand.device.GetPressUp (trigger)) {
				OnHandPressUp (hand);
			}
			if (hand.device.GetPress (trigger)) {
				OnHandPress (hand);
			}
		}
	}

	void OnTriggerEnter(Collider other) {
		var hand = other.gameObject.GetComponent<Hand> ();
		if (hand != null && !hands.Contains(hand)) {
			OnHandEnter (hand);
			hands.Add (hand);
		}
	}

	void OnTriggerExit(Collider other) {
		var hand = other.gameObject.GetComponent<Hand> ();
		if (hand != null && hands.Contains(hand)) {
			OnHandExit (hand);
			hands.Remove (hand);
		}
	}

	protected virtual void OnHandEnter(Hand hand) {
	}

	protected virtual void OnHandExit(Hand hand) {
	}

	protected virtual void OnHandPressDown(Hand hand) {
	}

	protected virtual void OnHandPressUp(Hand hand) {
	}

	protected virtual void OnHandPress(Hand hand) {
	}
}

