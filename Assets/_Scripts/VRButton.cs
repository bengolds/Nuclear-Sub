using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using VRTK;
using System;
using DG.Tweening;

public class VRButton : MonoBehaviour {
    public GameObject enabledBoolObject;
    public UnityEvent onPress;
    public float depressDistance = 0.01f;
    public Vector3 localDepressAxis = Vector3.back;
    public float depressDuration = 0.2f;
    public Ease depressEase = Ease.Linear;
    
    private IWatchableBool enabledBool;
    private Vector3 origGlobalPosition;
    private VRTK_InteractableObject io;
    private Tween tween;
    
	// Use this for initialization
	void Start () {
        InitInteractableObject();
        if (enabledBoolObject != null)
        {
            enabledBool = enabledBoolObject.GetComponent<IWatchableBool>();
        }
        origGlobalPosition = transform.position;
	}
    
    private void InitInteractableObject()
    {
        if ((io = GetComponent<VRTK_InteractableObject>()) == null)
        {
            io = gameObject.AddComponent<VRTK_InteractableObject>();
            io.isUsable = true;
            io.isGrabbable = false;
            io.InteractableObjectUsed += ButtonPressed;
            io.InteractableObjectUnused += ButtonReleased;
        }
    }
    
    public bool IsEnabled()
    {
        return enabledBool != null ? enabledBool.boolValue : true;
    }

    void ButtonPressed(object sender, InteractableObjectEventArgs e)
    {
        if (IsEnabled())
        {
            if (tween != null)
            {
                tween.Kill();
            }
            var targetPosition = origGlobalPosition + transform.TransformDirection(localDepressAxis.normalized) * depressDistance;
            tween = transform.DOMove(targetPosition, depressDuration).SetEase(depressEase);
        }
    }

    void ButtonReleased(object sender, InteractableObjectEventArgs e) {
        if (IsEnabled())
        {
            onPress.Invoke();

            if (tween != null)
            {
                tween.Kill();
            }
            tween = transform.DOMove(origGlobalPosition, depressDuration).SetEase(depressEase);
        }
    }
}
