using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeycodeInputWindow : MonoBehaviour {

    private Keypad _keypad;
    private DoorMapIcon _door;
    private Text _text;
    private string _stringInput;

	void Awake () {
        _text = GetComponentInChildren<Text>();
	}
	
	void Update () {

        if (_text.text.Length < 6 && Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Backspace) && !Input.GetKeyDown(KeyCode.Return))
        {
            _stringInput += Input.inputString;
            _text.text = _stringInput;
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            _stringInput = _stringInput.Remove(_stringInput.Length - 1);
            _text.text = _stringInput;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (_keypad)
            {
                _keypad.ReleaseWindow();
                _keypad.UseKeycode(_stringInput);
            }
            if (_door)
            {
                _door.showWindow = false;
                _door.UseKeycode(_stringInput);
            }
            
            GameObject.Destroy(this.gameObject);
        }
	}

    public void SetKeypad(Keypad pad)
    {
        _keypad = pad;
    }

    public void SetDoor(DoorMapIcon pad)
    {
        _door = pad;
    }
}
