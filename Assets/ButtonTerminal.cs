using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTerminal : MonoBehaviour {

    [SerializeField]
    ShooterDoor _door;

    [Header("Buttons")]
    [SerializeField]
    ShooterButton _leftButton;
    [SerializeField]
    ShooterButton _rightButton;
    
    // Use this for initialization
    void Start () {
        _leftButton.Triggered.OnValueChange += ButtonChanged;
        _rightButton.Triggered.OnValueChange += ButtonChanged;
        ButtonChanged();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void ButtonChanged()
    {   
        if(_leftButton.Triggered.Value && _rightButton.Triggered.Value)
        {
            OnSolved();
        }
    }

    private void OnSolved()
    {
        _leftButton.OnSolved();
        _rightButton.OnSolved();
        _door.ForceOpen();
    }
}
