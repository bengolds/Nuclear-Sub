using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
using VRTK;
using System;

public class Dial : MonoBehaviour {
	
	[Header("Touch Materials")]
	public Material touchedStateMaterial;
	public Material usedStateMaterial;
    protected Material normalStateMaterial;

    public enum DialType {
        IndexedValues, Numbers
    }
    [Header("Dial Faces")]
    public float dialRadius;
    public float dialWidth;
    public Font faceFont;
    public Color faceColor;
    public Vector3 rotationAxis = Vector3.right;
    public DialType dialType;
    public int numFaces = 5;
    public int startingNumber;
    public List<string> values;

    [Header("Scroll Feel")]
	public ushort scrollingHapticsStrength;
	public ushort clickingHapticsStrength;
	public float scrollSpeed = 60.0f;
	public float snapDuration = 0.5f;

    private VRTK_InteractableObject io;
	private MeshRenderer meshRenderer;
	private Vector2? lastTrackpadPos;
	private float scrollAmount = 0;
	private int index = 0;
    private Quaternion baseLocalRotation;

	// Use this for initialization
	void Start ()
    {
        InitInteractableObject();
        InitDialFaces();

        meshRenderer = GetComponent<MeshRenderer>();
        normalStateMaterial = meshRenderer.material;
        lastTrackpadPos = null;
        baseLocalRotation = transform.localRotation;
    }

    private void InitDialFaces()
    {
        GameObject facesCanvas = new GameObject();
        facesCanvas.transform.SetParent(this.transform);
        facesCanvas.transform.position = this.transform.position;
        facesCanvas.transform.rotation = transform.rotation;
        facesCanvas.name = "Faces Canvas";

        float canvasSize = 50;
        var canvas = facesCanvas.AddComponent<Canvas>();
        var rt = facesCanvas.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(canvasSize, canvasSize);
        float canvasScale = dialWidth / canvasSize;
        var currScale = transform.lossyScale;
        rt.localScale = new Vector3(canvasScale/currScale.x, canvasScale/currScale.y, canvasScale/currScale.y);

        var canvasScaler = facesCanvas.AddComponent<CanvasScaler>();
        canvasScaler.dynamicPixelsPerUnit = 5;

        float angleStep = 360 / numFaces;
        float currAngle = 0f;

        rotationAxis.Normalize();

        for (int i = 0; i < numFaces; i++)
        {
            GameObject textFace = new GameObject();
            textFace.transform.SetParent(facesCanvas.transform);
            textFace.transform.localScale = Vector3.one;
            textFace.transform.position = transform.position + dialRadius * GetForwardAxis();
            textFace.transform.rotation = transform.rotation;
            textFace.transform.RotateAround(transform.position, GetGlobalRotationAxis(), -currAngle);

            var text = textFace.AddComponent<Text>();
            text.fontSize = 32;
            text.alignment = TextAnchor.MiddleCenter;
            text.font = faceFont;
            text.color = faceColor;
            textFace.GetComponent<RectTransform>().sizeDelta = new Vector2(canvasSize, canvasSize);
            
            switch (dialType)
            {
                case DialType.IndexedValues:
                    text.text = values[i];
                    break;
                case DialType.Numbers:
                    text.text = (startingNumber + i).ToString();
                    break;
                default:
                    break;
            }

            currAngle += angleStep;
        }
    }

    private void InitInteractableObject()
    {
        if ((io = GetComponent<VRTK_InteractableObject>()) == null)
        {
            io = gameObject.AddComponent<VRTK_InteractableObject>();
        
        }
        io.isUsable = true;
        io.InteractableObjectUsed += StartUsing;
        io.InteractableObjectUnused += StopUsing;
    }

    // Update is called once per frame
    void Update () {
		if (io.IsUsing ()) {
			meshRenderer.material = usedStateMaterial;
		} else if (io.IsTouched ()) {
			meshRenderer.material = touchedStateMaterial;
		} else {
			meshRenderer.material = normalStateMaterial;
		}

        //TODO: Switch to rotationAxis;
		transform.rotation = transform.parent.rotation * baseLocalRotation * Quaternion.AngleAxis (-scrollAmount, Vector3.right);
	}

	void StartUsing(object sender, InteractableObjectEventArgs e)
	{
		e.interactingObject.GetComponent<VRTK_ControllerEvents> ().TouchpadAxisChanged += TouchpadAxisChanged;
        e.interactingObject.GetComponent<VRTK_ControllerEvents> ().TouchpadTouchEnd += TouchpadTouchEnd;
        e.interactingObject.GetComponent<VRTK_ControllerActions> ().SetControllerOpacity (0.1f);
	}

	void StopUsing(object sender, InteractableObjectEventArgs e)
	{
        e.interactingObject.GetComponent<VRTK_ControllerEvents> ().TouchpadAxisChanged -= TouchpadAxisChanged;
        e.interactingObject.GetComponent<VRTK_ControllerEvents> ().TouchpadTouchEnd -= TouchpadTouchEnd;
        e.interactingObject.GetComponent<VRTK_ControllerActions> ().SetControllerOpacity (1.0f);
		SnapDial ();
	}

	void TouchpadAxisChanged(object sender, ControllerInteractionEventArgs e) {
		if (io.IsUsing ()) {
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

	void TouchpadTouchEnd(object sender, ControllerInteractionEventArgs e) {
		if (io.IsUsing ()) {
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
		index = mod(selectedFace, numFaces);
		
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

    public string GetValue()
    {
        switch (dialType)
        {
            case DialType.IndexedValues:
                return values[index];
            case DialType.Numbers:
                return (index + startingNumber).ToString();
            default:
                return string.Empty;
        }
    }

    Vector3 GetGlobalRotationAxis()
    {
        return transform.TransformDirection(rotationAxis).normalized;
    }
    Vector3 GetForwardAxis()
    {
        return Vector3.Cross(GetGlobalRotationAxis(), Vector3.up).normalized;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 axisVector = dialWidth * (GetGlobalRotationAxis());
        Gizmos.DrawRay(transform.position-axisVector/2, axisVector);

        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(transform.position, dialRadius * GetForwardAxis());
    }
}
