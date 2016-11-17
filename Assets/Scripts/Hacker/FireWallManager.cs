using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireWallManager : MonoBehaviour {

	public List<Firewall> _fireWalls;
	public int fireWallIndex = 0;

	private ShooterPackageSender _sender;

	public void SendFireWallUpdate(Firewall firewall)
	{
		_sender.SendFireWallUpdate(firewall);
	}

	public void UpdateFireWallState(CustomCommands.Update.FireWallUpdate update)
	{
		Firewall firewall = GetFireWallByID(update.ID);
		firewall.ChangeState(update.destroyed);
	}

	public void SetSender(ShooterPackageSender sender)
	{
		_sender = sender;
	}

	void Start()
	{
		_fireWalls = InitGetAllFireWallsInLevel();
	}

	private List<Firewall> InitGetAllFireWallsInLevel()
	{
		List<Firewall> allFireWalls = new List<Firewall>();
		Firewall[] fireWallArray = FindObjectsOfType<Firewall>();
		for (int i = 0; i < fireWallArray.Length; i++)
		{
			fireWallArray[i].ID = fireWallIndex;
			fireWallIndex++;
			allFireWalls.Add(fireWallArray[i]);
			fireWallArray[i].SetManager(this);
		}
		return allFireWalls;
	}

	public Firewall GetFireWallByID(int ID)
	{
		foreach (Firewall f in _fireWalls)
		{
			if (f.ID == ID)
			{
				return f;
			}
		}
		return null;
	}
}
