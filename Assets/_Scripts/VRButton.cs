using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using VRTK;
using System;

public class VRButton : MonoBehaviour {
	public Material touchedStateMaterial;
	public Material usedStateMaterial;
	public UnityEvent onPress;
	protected Material normalStateMaterial;
	private MeshRenderer meshRenderer;
    private VRTK_InteractableObject io;
    
	// Use this for initialization
	void Start () {
        InitInteractableObject();
		meshRenderer = GetComponent<MeshRenderer> ();
		normalStateMaterial = meshRenderer.material;
	}

    //	
    //	// Update is called once per frame
    void Update () {
		if (io.IsUsing ()) {
			meshRenderer.material = usedStateMaterial;
		} else if (io.IsTouched ()) {
			meshRenderer.material = touchedStateMaterial;
		} else {
			meshRenderer.material = normalStateMaterial;
		}
	}

    private void InitInteractableObject()
    {
        if ((io = GetComponent<VRTK_InteractableObject>()) == null)
        {
            io = gameObject.AddComponent<VRTK_InteractableObject>();
            io.isUsable = true;
            io.isGrabbable = false;
            io.InteractableObjectUnused += ButtonPressed;
        }
    }

    void ButtonPressed(object sender, InteractableObjectEventArgs e) {
		onPress.Invoke ();
	}
}
