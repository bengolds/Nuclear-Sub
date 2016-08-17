using UnityEngine;
using System.Collections;

[RequireComponent(typeof(RectTransform))]
public class SetFromDial : MonoBehaviour {
    public enum CoordinateToSet
    {
        X,
        Y
    }
    public DialSet dial;
    public CoordinateToSet connectedCoordinate;
    public float scaleFactor = 1/1000f;
    public int center = 5000;
    private RectTransform rt;

	// Use this for initialization
	void Start () {
        rt = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
        int dialValue = int.Parse(dial.GetValue());
        int diffFromCenter = dialValue - center;
        float scaledValue = 0.5f + scaleFactor * diffFromCenter;
        switch (connectedCoordinate)
        {
            case CoordinateToSet.X:
                rt.anchorMax = new Vector2(scaledValue, rt.anchorMax.y);
                rt.anchorMin = new Vector2(scaledValue, rt.anchorMin.y);
                break;
            case CoordinateToSet.Y:
                rt.anchorMax = new Vector2(rt.anchorMax.x, scaledValue);
                rt.anchorMin = new Vector2(rt.anchorMin.x, scaledValue);
                break;
        }

	}
}
