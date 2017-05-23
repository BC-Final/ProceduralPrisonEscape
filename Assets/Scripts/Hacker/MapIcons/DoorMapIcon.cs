﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Gamelogic.Extensions;
using System;

public class DoorMapIcon : AbstractMapIcon {

	public static void ProcessPacket(NetworkPacket.Update.Door pPacket) {
		DoorMapIcon icon = HackerPackageSender.GetNetworkedObject<DoorMapIcon>(pPacket.Id);

		if (icon == null) {
			createInstance(pPacket);
		} else {
			icon.updateInstance(pPacket);
		}
	}

	private string _codeSolution;

	public static void AddAddon(NetworkPacket.Create.DecodeAddon pPacket) {
		DoorMapIcon icon = HackerPackageSender.GetNetworkedObject<DoorMapIcon>(pPacket.DoorId);
		Array.Clear(icon.actions, 0, icon.actions.Length);
		AbstractMapIcon.ActionData action = new AbstractMapIcon.ActionData();
		action.DisplayName = "Decode";
		action.HackerPointsCost = 0;
		UnityEvent m_MyEvent = new UnityEvent();
		m_MyEvent.AddListener(icon.Decode);
		action.Action = m_MyEvent;
		icon.actions[0] = action;
		icon._codeSolution = pPacket.CodeString;
        icon._addonSprite.sprite = HackerReferenceManager.Instance.DoorAddonDecoder;
    }

	public static void AddAddon(NetworkPacket.Create.KeyCard pPacket) {
		for (int i = 0; i < pPacket.intArray.Length; i++) {
            DoorMapIcon icon = HackerPackageSender.GetNetworkedObject<DoorMapIcon>(pPacket.intArray[i]);
            icon.SetKeyColor(new Color(1,1,1));
            icon._addonSprite.sprite = HackerReferenceManager.Instance.DoorAddonKeycard;
            icon._addonSprite.color = new Color(pPacket.colorR, pPacket.colorG, pPacket.colorB);
            icon.changeColor(Color.gray);
        }
	}

	public static void AddAddon(NetworkPacket.Create.CodeLockAddon pPacket) {
		DoorMapIcon icon = HackerPackageSender.GetNetworkedObject<DoorMapIcon>(pPacket.DoorId);
		Array.Clear(icon.actions, 0, icon.actions.Length);
		AbstractMapIcon.ActionData action = new AbstractMapIcon.ActionData();
		action.DisplayName = "Input Code";
		action.HackerPointsCost = 0;
		UnityEvent m_MyEvent = new UnityEvent();
		m_MyEvent.AddListener(icon.InputCode);
		action.Action = m_MyEvent;
		icon.actions[0] = action;
		icon._codeSolution = pPacket.CodeString;
		icon._addonId = pPacket.Id;
        icon._addonSprite.sprite = HackerReferenceManager.Instance.DoorAddonCodeinput;
    }

    public static void AddAddon(NetworkPacket.Create.DuoButtonAddon pPacket)
    {
        DoorMapIcon icon = HackerPackageSender.GetNetworkedObject<DoorMapIcon>(pPacket.DoorId);
        icon._addonSprite.sprite = HackerReferenceManager.Instance.DoorAddonDuobutton;
        Array.Clear(icon.actions, 0, icon.actions.Length);
    }

	public static void AddAddon(NetworkPacket.Update.DisableDoor pPacket) {
		DoorMapIcon icon = HackerPackageSender.GetNetworkedObject<DoorMapIcon>(pPacket.Id);
		icon.actions = new ActionData[0];

		icon.changeColor(Color.gray);
	}

	private static void createInstance(NetworkPacket.Update.Door pPacket) {
		DoorMapIcon icon = Instantiate(HackerReferenceManager.Instance.DoorIcon, new Vector3(pPacket.PosX / MinimapManager.scale, pPacket.PosY / MinimapManager.scale, 0), Quaternion.Euler(0, 0, -pPacket.Rot)).GetComponent<DoorMapIcon>();

		icon.Id = pPacket.Id;
		icon._open.Value = pPacket.Open;
		icon._locked.Value = pPacket.Locked;
        icon._addonSprite = icon.transform.GetComponentsInChildren<SpriteRenderer>()[1];

		//TODO Research why onValueChanged is not called
		icon.stateChanged();
	}

	private void updateInstance(NetworkPacket.Update.Door pPacket) {
		_open.Value = pPacket.Open;
		_locked.Value = pPacket.Locked;
	}

	private void Awake() {
		_open.OnValueChange += stateChanged;
		_locked.OnValueChange += stateChanged;
	}

    private SpriteRenderer _addonSprite;
    private int _addonId;
	public bool showWindow = false;
	private bool _keycardLocked = false;
	private Color _keyColor;
	private ObservedValue<bool> _open = new ObservedValue<bool>(false);
	private ObservedValue<bool> _locked = new ObservedValue<bool>(false);
	[SerializeField]
	private Color _lockColor;

	#region Sprites
	[Header("Sprites")]
	[SerializeField]
	private Sprite _openSprite;
	[SerializeField]
	private Sprite _closedSprite;
	#endregion

	public void Toggle() {
		//TODO Send state update but only change when receiving (if it causes problems)
		_open.Value = !_open.Value;

		sendUpdate();
	}

	public void Lock() {
		_locked.Value = true;

		sendUpdate();
	}

	public void Decode() {
		if (!showWindow) {
			showWindow = true;
			DecoderWindow window = Instantiate(HackerReferenceManager.Instance.Decoder, Vector3.zero, Quaternion.identity).GetComponent<DecoderWindow>();
			window.Solution = _codeSolution;
			window.SetDoor(this);
		}
	}

	public void InputCode() {
		if (!showWindow) {
			showWindow = true;
			KeycodeInputWindow window = Instantiate(HackerReferenceManager.Instance.CodeInputWindow, Vector3.zero, Quaternion.identity).GetComponent<KeycodeInputWindow>();
			window.SetDoor(this);
		}
	}

	public void UseKeycode(string codeString) {
		Debug.Log("Code: " + codeString + "/" + "Solution: " + _codeSolution);
		if (codeString.ToUpper() == _codeSolution.ToUpper()) {
			HackerPackageSender.SendPackage(new NetworkPacket.Update.CodeLockCode(_addonId, codeString));
			//FMODUnity.RuntimeManager.CreateInstance("event:/PE_weapon/ep/PE_weapon_ep_reload").start();
		} else {
			//FMODUnity.RuntimeManager.CreateInstance("event:/PE_hacker/PE_hacker_door_denied").start();
		}
	}

	public void SetKeyColor(Color nColor) {
		Array.Clear(actions, 0, actions.Length);
		_keycardLocked = true;
		_keyColor = nColor;
		stateChanged();
	}

	private void sendUpdate() {
		HackerPackageSender.SendPackage(new NetworkPacket.Update.Door(Id, _open.Value, _locked.Value));
	}

	private void stateChanged() {
		if (_open.Value) {
			changeSprite(_openSprite);
            SetAddonSprite(false);
            if (_locked.Value) {
				changeColor(Color.red);
			} else if (_keycardLocked) {
				changeColor(_keyColor);
			}
		} else {
			changeSprite(_closedSprite);
            SetAddonSprite(true);
            if (_locked.Value) {
				changeColor(Color.red);
			} else if (_keycardLocked) {
				changeColor(_keyColor);
			}
		}
	}

    private void SetAddonSprite(bool isActive)
    {
        if (_addonSprite)
        {
            _addonSprite.enabled = isActive;
        }
    }
}
