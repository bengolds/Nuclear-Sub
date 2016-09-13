using UnityEngine;
using DG.Tweening;
using VRTK;
using System.Collections;

[RequireComponent(typeof(EmissiveLight))]
public class LitButton : MonoBehaviour
{
    public float offBrightness = 0f;
    public float normalOnBrightness = 0.8f;
    public float touchedBrightness = 0.9f;
    public float pressedBrightness = 1f;
    public float transitionDuration = 0.2f;
    public Ease ease = Ease.OutExpo;

    private VRButton button;
    private EmissiveLight eLight;

    private float currTargetBrightness = float.NaN;
    private Tween currTween;

    void Start()
    {
        button = GetComponentInParent<VRButton>();
        eLight = GetComponent<EmissiveLight>();
    }

    void Update()
    {
        var io = GetComponentInParent<VRTK_InteractableObject>();
        if (button.IsEnabled())
        {
            if (io.IsUsing())
            {
                ChangeBrightness(pressedBrightness);
            }
            else if (io.IsTouched())
            {
                ChangeBrightness(touchedBrightness);
            }
            else
            {
                ChangeBrightness(normalOnBrightness);
            }
        } else
        {
            ChangeBrightness(offBrightness);
        }
    }

    void ChangeBrightness(float targetBrightness)
    {
        if (targetBrightness == currTargetBrightness)
        {
            return;
        }
        if (currTween != null)
        {
            currTween.Kill();
        }
        currTween = DOTween.To(x => eLight.brightness = x, eLight.brightness, targetBrightness, transitionDuration)
            .SetEase(ease);

        currTargetBrightness = targetBrightness;
    }

}
