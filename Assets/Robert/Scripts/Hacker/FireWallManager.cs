using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireWallManager : MonoBehaviour {

	public List<FireWall> _fireWalls;
	public int fireWallIndex = 0;

	private TCPMBTesterServer _sender;

	public void SendFireWallUpdate(FireWall firewall)
	{
		_sender.SendFireWallUpdate(firewall);
	}

	public void UpdateFireWallState(CustomCommands.Update.FireWallUpdate update)
	{
		FireWall firewall = GetFireWallByID(update.ID);
		firewall.ChangeState(update.destroyed);
	}

	public void SetSender(TCPMBTesterServer sender)
	{
		_sender = sender;
	}

	void Start()
	{
		_fireWalls = InitGetAllFireWallsInLevel();
	}

	private List<FireWall> InitGetAllFireWallsInLevel()
	{
		List<FireWall> allFireWalls = new List<FireWall>();
		FireWall[] fireWallArray = FindObjectsOfType<FireWall>();
		for (int i = 0; i < fireWallArray.Length; i++)
		{
			fireWallArray[i].ID = fireWallIndex;
			fireWallIndex++;
			allFireWalls.Add(fireWallArray[i]);
			fireWallArray[i].SetManager(this);
		}
		return allFireWalls;
	}

	public FireWall GetFireWallByID(int ID)
	{
		foreach (FireWall f in _fireWalls)
		{
			if (f.ID == ID)
			{
				return f;
			}
		}
		return null;
	}
}
