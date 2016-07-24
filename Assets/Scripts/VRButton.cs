using UnityEngine;
using System.Collections;

public class VRButton : MonoBehaviour {
	public enum ButtonState {
		Normal,
		Highlighted,
		Pressed
	}

	public ButtonState state;
	public Material normalMaterial;
	public Material highlightedMaterial;
	public Material pressedMaterial;
	private MeshRenderer meshRenderer;

	// Use this for initialization
	void Start () {
		meshRenderer = GetComponent<MeshRenderer> ();
		state = ButtonState.Normal;
	}
	
	// Update is called once per frame
	void Update () {
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

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.GetComponent<Hand> () != null) {
			OnButtonEnter ();
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject.GetComponent<Hand> () != null) {
			OnButtonExit ();
		}
	}

	public void OnButtonEnter() {
		state = ButtonState.Highlighted;
	}

	public void OnButtonExit() {
		state = ButtonState.Normal;
	}

	public void OnButtonPress() {
	
	}
}
