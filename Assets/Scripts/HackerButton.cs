using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackerButton : ShooterButton, IShooterNetworked{

	private ShooterNetworkId _id = new ShooterNetworkId();
	public ShooterNetworkId Id { get { return _id; } }

	override public void ReceiveDamage(Transform pSource, Vector3 pHitPoint, float pDamage, float pForce)
    {
        
    }

    override public void Interact()
    {
        
    }

    public override void SetColor(Color color)
    {
        if (color != _currentColor)
        {
            _currentColor = color;
            GetComponent<Renderer>().material.color = _currentColor;
            ShooterPackageSender.SendPackage(new NetworkPacket.Update.ButtonFeedback(Id, _currentColor));
        }
    }
    public static void ProccessPacket(NetworkPacket.Update.ButtonPush pPacket)
    {
        HackerButton button = ShooterPackageSender.GetNetworkedObject<HackerButton>(pPacket.Id);
        if (button._canBeTriggered)
        {
            button.TriggerTimer();
        }
    }

    public void Initialize()
    {
        ShooterPackageSender.SendPackage(new NetworkPacket.Create.PushButton(Id, this.transform.position));
        ShooterPackageSender.SendPackage(new NetworkPacket.Update.ButtonFeedback(Id, _currentColor));
    }

    private void Awake()
    {
        ShooterPackageSender.RegisterNetworkObject(this);
    }
}
