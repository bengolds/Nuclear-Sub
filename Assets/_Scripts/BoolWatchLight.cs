using UnityEngine;
using System.Collections;
using DG.Tweening;
using System;

[RequireComponent(typeof(EmissiveLight))]
public class BoolWatchLight : MonoBehaviour
{
    public GameObject watchedBoolObject;
    public float onDuration = 1f;
    public float offDuration = 0.2f;
    public Ease ease = Ease.OutExpo;

    private Tween onTween;
    private Tween offTween;
    protected EmissiveLight connectedLight;
    protected IWatchableBool watchedBool;
    protected bool currentlyOn = false;

    protected virtual void Start()
    {
        watchedBool = watchedBoolObject.GetComponent<IWatchableBool>();
        if (watchedBool == null)
        {
            Debug.LogError("No watchable bool on " + watchedBoolObject.name);
        }
        connectedLight = GetComponent<EmissiveLight>();
    }

    protected virtual void Update()
    {
        if (watchedBool.boolValue != currentlyOn)
        {
            if (watchedBool.boolValue)
            {
                TurnOn();
            } else
            {
                TurnOff();
            }
        }
    }
    
    
    public void TurnOn()
    {
        onTween = DOTween.To(x => connectedLight.brightness = x, connectedLight.brightness, 1f, onDuration);
        if (offTween != null)
        {
            offTween.Kill();
        }
        currentlyOn = true;
    }

    public void TurnOff()
    {
        offTween = DOTween.To(x => connectedLight.brightness = x, connectedLight.brightness, 0, offDuration)
            .SetEase(ease);
        if (onTween != null)
        {
            onTween.Kill();
        }
        currentlyOn = false;
    }

}
