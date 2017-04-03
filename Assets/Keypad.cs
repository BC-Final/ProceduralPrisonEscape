using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keypad : MonoBehaviour, IInteractable
{

    [SerializeField]
    public string keyCode;

    public List<ShooterDoor> Doors;

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
        Debug.Log("Code used: " + code);
        if (code.ToUpper() == keyCode.ToUpper())
        {
            Debug.Log("SUCCESS!!!!");
            FMODUnity.RuntimeManager.CreateInstance("event:/PE_weapon/ep/PE_weapon_ep_reload").start();
            
            foreach (ShooterDoor d in Doors)
            {
                d.ForceOpen();
            }
        }else
        {
            Debug.Log("FAILURE!!!!");
            FMODUnity.RuntimeManager.CreateInstance("event:/PE_hacker/PE_hacker_door_denied").start();      
        }
    }
}
