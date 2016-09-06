using UnityEngine;
using System.Collections;

public class HasWatchableBool : MonoBehaviour, IWatchableBool
{
    public bool boolValue
    {
        get { return m_value; }
        set
        {
            bool oldValue = m_value;
            m_value = value;
            if (OnBoolValueChanged != null && oldValue != value)
            {
                OnBoolValueChanged.Invoke(this, value);
            }
        }
    }

    public event WatchableBoolEventHandler OnBoolValueChanged;

    public void SilentSetBoolValue(bool value)
    {
        m_value = value;
    }

    private bool m_value;
}

public interface IWatchableBool
{
    bool boolValue { get; }
    event WatchableBoolEventHandler OnBoolValueChanged;
}

public delegate void WatchableBoolEventHandler(
    object sender,
    bool value
);