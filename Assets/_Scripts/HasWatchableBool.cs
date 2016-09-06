using UnityEngine;
using System.Collections;

public class HasWatchableBool : MonoBehaviour
{
    public bool boolValue
    {
        get { return m_value; }
        set
        {
            bool oldValue = m_value;
            m_value = value;
            if (OnValueChanged != null && oldValue != value)
            {
                OnValueChanged.Invoke(this, value);
            }
        }
    }

    public delegate void WatchableBoolEventHandler(
        object sender,
        bool value
    );
    public event WatchableBoolEventHandler OnValueChanged;

    public void SilentSetBoolValue(bool value)
    {
        m_value = value;
    }

    private bool m_value;
}
