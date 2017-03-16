using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScriptTestScript : MonoBehaviour {

    [SerializeField]
    UnityEvent function;
    [SerializeField]
    UnityEvent function2;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            function.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            function2.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            DoorMapIcon.CreateInstance(new Vector2(0,0),0, true, true);
        }
    }
}
