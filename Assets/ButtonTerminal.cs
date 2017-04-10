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

    [Header("Colors")]
    [SerializeField]
    Color offColor;
    [SerializeField]
    Color onColor;
    [SerializeField]
    Color solvedColor;
    
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
            _leftButton.SetColor(solvedColor);
            _rightButton.SetColor(solvedColor);
            OnSolved();
        }
        else if (_leftButton.Triggered.Value)
        {
            _leftButton.SetColor(onColor);
            _rightButton.SetColor(offColor);
        }
        else if(_rightButton.Triggered.Value)
        {
            _rightButton.SetColor(onColor);
            _leftButton.SetColor(offColor);
        }
        else
        {
            _leftButton.SetColor(offColor);
            _rightButton.SetColor(offColor);
        }
    }

    private void OnSolved()
    {
        _door.ForceOpen();
        _leftButton.enabled = false;
        _rightButton.enabled = false;
    }
}
