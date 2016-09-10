using UnityEngine;
using System.Collections;

public class Map : MonoBehaviour {

    public GameObject powerBoolObject;
    public GameObject mapUI;
    private IWatchableBool powerBool;

	// Use this for initialization
	void Start () {
        powerBool = powerBoolObject.GetComponent<IWatchableBool>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (powerBool.boolValue)
        {
            mapUI.SetActive(true);
        } else
        {
            mapUI.SetActive(false);
        }
	}
}
