using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoderController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SecurityCode.RNDAll();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            DecoderNumber.SetSolutionToCurrent();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            SecurityCode.ResetAll();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SecurityCode.RNDAll();
            DecoderNumber.SetSolutionToCurrent();
            SecurityCode.ResetAll();
        }
    }
}
