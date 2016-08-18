using UnityEngine;
using System.Collections;
using DG.Tweening;

public class SimpleOnOffLight : MonoBehaviour {

    public EmissiveLight connectedLight;
    public float onDuration;
    public float offDuration;
    public Ease ease;

    private Tween onTween;
    private Tween offTween;

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
