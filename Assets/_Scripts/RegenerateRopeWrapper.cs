using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UltimateRope))]
public class RegenerateRopeWrapper : MonoBehaviour {

    private UltimateRope rope;
	// Use this for initialization
	void Start () {
        rope = GetComponent<UltimateRope>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Regenerate(bool bResetNodePositions)
    {
        rope.Regenerate(bResetNodePositions);
    }
}
