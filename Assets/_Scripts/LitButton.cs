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
    public bool blinkAtFirst = false;

    private VRButton button;
    private EmissiveLight eLight;

    private float currTargetBrightness = float.NaN;
    private Tween currTween;
    private bool overridden = false;
    private bool used = false;

    void Start()
    {
        button = GetComponentInParent<VRButton>();
        eLight = GetComponent<EmissiveLight>();
    }

    void Update()
    {
        var io = GetComponentInParent<VRTK_InteractableObject>();
        if (!overridden)
        {
            if (button.IsEnabled())
            {
                if (io.IsUsing())
                {
                    ChangeBrightness(pressedBrightness);
                    used = true;
                }
                else if (io.IsTouched())
                {
                    ChangeBrightness(touchedBrightness);
                }
                else
                {
                    if (blinkAtFirst && !used)
                    {
                        BlinkBrightness(normalOnBrightness);
                    }
                    else
                    {
                        ChangeBrightness(normalOnBrightness);
                    }
                }
            }
            else
            {
                ChangeBrightness(offBrightness);
            }
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

    void BlinkBrightness(float targetBrightness)
    {
        if (targetBrightness == currTargetBrightness)
        {
            return;
        }
        if (currTween != null)
        {
            currTween.Kill();
        }

        currTween = DOTween.To(x => eLight.brightness = x, 0, targetBrightness, 1f)
            .SetEase(ease)
            .SetLoops(-1, LoopType.Yoyo);

        currTargetBrightness = targetBrightness;
    }


    public void OverrideBrightness(float targetBrightness, float transitionDuration, Ease ease)
    {
        currTween = DOTween.To(x => eLight.brightness = x, eLight.brightness, targetBrightness, transitionDuration)
            .SetEase(ease);
        currTargetBrightness = targetBrightness;
        overridden = true;
    }
}
