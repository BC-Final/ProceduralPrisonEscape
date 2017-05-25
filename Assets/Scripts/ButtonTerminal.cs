using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTerminal : MonoBehaviour, IShooterNetworked{

	private ShooterNetworkId _id = new ShooterNetworkId();
	public ShooterNetworkId Id { get { return _id; } }

	public void Initialize()
	{
		ShooterPackageSender.SendPackage(new NetworkPacket.Create.PushButton(Id, this.transform.position));
		ShooterPackageSender.SendPackage(new NetworkPacket.Update.ButtonFeedback(Id, _hackerButton.CurrentColor));
		ShooterPackageSender.SendPackage(new NetworkPacket.Update.ButtonFeedback(Id, _shooterButton.CurrentColor, 1));
	}

	private void Awake()
	{
		ShooterPackageSender.RegisterNetworkObject(this);
	}

	[SerializeField]
    ShooterDoor _door;

    [Header("Buttons")]
    [SerializeField]
    ShooterButton _shooterButton;
    [SerializeField]
    ShooterButton _hackerButton;

	public static void ProccessPacket(NetworkPacket.Update.ButtonPush pPacket)
	{
		ButtonTerminal terminal = ShooterPackageSender.GetNetworkedObject<ButtonTerminal>(pPacket.Id);
		if (terminal._hackerButton.CanBeTriggered)
		{
			terminal._hackerButton.TriggerTimer();
		}
	}

	void Start () {
        _shooterButton.Triggered.OnValueChange += OnButtonChanged;
		_shooterButton.Terminal = this;
        _hackerButton.Triggered.OnValueChange += OnButtonChanged;
		_hackerButton.Terminal = this;

		OnButtonChanged();
		_door.AddDuoButton();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnButtonChanged()
    {   
        if(_shooterButton.Triggered.Value && _hackerButton.Triggered.Value)
        {
            OnSolved();
        }
	}

	public void OnShooterButtonColorChanged()
	{
		ShooterPackageSender.SendPackage(new NetworkPacket.Update.ButtonFeedback(Id, _shooterButton.CurrentColor, 1));
	}

	public void OnHackerButtonColorChanged()
	{
		ShooterPackageSender.SendPackage(new NetworkPacket.Update.ButtonFeedback(Id, _hackerButton.CurrentColor));
	}

	private void OnSolved()
    {
        _shooterButton.OnSolved();
        _hackerButton.OnSolved();
        _door.ForceOpen();
    }
}
