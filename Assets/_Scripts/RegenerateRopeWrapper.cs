using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(UltimateRope))]
public class RegenerateRopeWrapper : MonoBehaviour {

    private UltimateRope rope;
    private Dictionary<GameObject, Vector3> origPositions;
    private Dictionary<GameObject, Quaternion> origRotations;
    private List<GameObject> resetObjects;

    // Use this for initialization
    void Start () {
        rope = GetComponent<UltimateRope>();
        origPositions = new Dictionary<GameObject, Vector3>();
        origRotations = new Dictionary<GameObject, Quaternion>();
        resetObjects = new List<GameObject>();

        resetObjects.AddRange(FindObjectsOfType<UltimateRopeLink>().Select(link => link.gameObject));
        resetObjects.Add(rope.RopeStart);
        resetObjects.AddRange(rope.RopeNodes.Select(node => node.goNode));
        foreach (var go in resetObjects)
        {
            origPositions[go] = go.transform.position;
            origRotations[go] = go.transform.rotation;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Regenerate(bool bResetNodePositions)
    {
        foreach (var go in resetObjects)
        {
            go.transform.position = origPositions[go];
            go.transform.rotation = origRotations[go];
            var rb = go.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }
}
