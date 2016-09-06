using UnityEngine;
using System.Collections;
using DG.Tweening;
using System;

[RequireComponent(typeof(EmissiveLight))]
public class BoolWatchLight : MonoBehaviour
{
    public HasWatchableBool watchedBool;
    public float onDuration;
    public float offDuration;
    public Ease ease;

    private Tween onTween;
    private Tween offTween;
    private EmissiveLight connectedLight;

    void Start()
    {
        watchedBool.OnValueChanged += BoolValChanged;
        connectedLight = GetComponent<EmissiveLight>();
        if (watchedBool.boolValue)
        {
            TurnOn();
        }
    }

    void BoolValChanged(object sender, bool value)
    {
        if (value)
        {
            TurnOn();
        }
        else
        {
            TurnOff();
        }
    }
    
    public void TurnOn()
    {
        onTween = DOTween.To(x => connectedLight.brightness = x, connectedLight.brightness, 1f, onDuration);
        if (offTween != null)
        {
            offTween.Kill();
        }
    }

    public void TurnOff()
    {
        offTween = DOTween.To(x => connectedLight.brightness = x, connectedLight.brightness, 0, offDuration)
            .SetEase(ease);
        if (onTween != null)
        {
            onTween.Kill();
        }
    }

}
