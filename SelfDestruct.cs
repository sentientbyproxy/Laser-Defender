using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour {

    public float DestroyedAfter = 3f;

	// Use this for initialization
	void Start () {
        Destroy(gameObject, DestroyedAfter);
	}
}
