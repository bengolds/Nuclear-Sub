using UnityEngine;
using System.Collections;

public class Paper : MonoBehaviour {
    public GameObject top;

    public void SetToImage(Texture2D tex)
    {
        top.GetComponent<Renderer>().material.mainTexture = tex;
    }
}
