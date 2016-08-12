using UnityEngine;
using System.Collections.Generic;

public class AkScriptTrigger : AkTriggerBase {
    public void TriggerEvent(GameObject obj = null)
    {
        triggerDelegate(obj);
    }
}
