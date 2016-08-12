using UnityEngine;
using System.Collections.Generic;

public class AkButtonTrigger : AkTriggerBase
{

    // Use this for initialization
    void Start()
    {
        var buttonScript = GetComponent<VRButton>();
        if (buttonScript)
        {
            buttonScript.onPress.AddListener(OnButtonPress);
        }
    }

    void OnButtonPress()
    {
        if (triggerDelegate != null)
        {
            triggerDelegate(null);
        }
    }
}