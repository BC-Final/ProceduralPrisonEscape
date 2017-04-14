using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitHud : MonoBehaviour {

    [SerializeField]
    GameObject HUD;

	// Use this for initialization
	void Start () {
        Instantiate(HUD);
	}
	
}
