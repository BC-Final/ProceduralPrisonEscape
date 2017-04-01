using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keypad : MonoBehaviour, IInteractable
{

    [SerializeField]
    public string keyCode;

    [SerializeField]
    List<ShooterDoor> _doors;

    private bool showWindow;

    public void Interact()
    {
        if (!showWindow)
        {
            showWindow = true;
            KeycodeInputWindow window = Instantiate(ShooterReferenceManager.Instance.KeyCodeInputWindow, Vector3.zero, Quaternion.identity).GetComponent<KeycodeInputWindow>();
            window.SetKeypad(this);
        }
    }

    public void ReleaseWindow()
    {
        showWindow = false;
    }

    public void UseKeycode(string code)
    {
        if (code.ToUpper() == keyCode.ToUpper())
        {
            Debug.Log("SUCCESS!!!!");
            foreach(ShooterDoor d in _doors)
            {
                d.ForceOpen();
            }
        }else
        {
            Debug.Log("FAILURE!!!!");
        }
    }
}
