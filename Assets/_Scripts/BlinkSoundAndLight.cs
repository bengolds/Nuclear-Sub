using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

[RequireComponent(typeof(AkEvent))]
public class BlinkSoundAndLight : MonoBehaviour {
    public bool on = true;
    public EmissiveLight eLight;
    public float blinkDelay;
    public float blinkDuration;
    
    private AkScriptTrigger trigger;

	// Use this for initialization
	void Start () {
        trigger = GetComponent<AkScriptTrigger>();
        BuzzAndLight();
	}
	
	void BuzzAndLight()
    {
        trigger.triggerDelegate(this.gameObject);

        Sequence tweenSequence = DOTween.Sequence();
        tweenSequence.Append(DOTween.To(x => eLight.brightness = x, 0, 1f, blinkDuration / 2));
        tweenSequence.Append(DOTween.To(x => eLight.brightness = x, 1, 0f, blinkDuration / 2));
        tweenSequence.PrependInterval(blinkDelay);
    }

    public void StopLooping()
    {
        on = false;
    }

    void SoundEndedCallback (object in_info)
    {
        if (on)
        {
            BuzzAndLight();
        }
    }
}
