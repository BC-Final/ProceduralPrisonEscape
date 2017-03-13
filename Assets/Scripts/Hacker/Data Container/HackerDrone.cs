﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Extensions;

public class HackerDrone {
	private static List<HackerDrone> _drones = new List<HackerDrone>();

	private MinimapDrone _minimapDrone;

	private ObservedValue<int>  _healthPercent = new ObservedValue<int>(0);
	private int _id;

	public int Id {
		get { return _id; }
	}

	public static void CreateDrone (CustomCommands.Creation.DroneCreation pPackage) {
		HackerDrone drone = new HackerDrone();
		drone._id = pPackage.Id;

		MinimapDrone minimapDrone = MinimapManager.Instance.CreateMinimapDrone(pPackage.X, pPackage.Z, pPackage.Rot);
		minimapDrone.InitialTransform(new Vector3(pPackage.X, 0, pPackage.Z) / MinimapManager.Instance.scale, pPackage.Rot);

		drone._healthPercent.OnValueChange += () => minimapDrone.HealthChanged(drone._healthPercent.Value);
		drone._healthPercent.Value = pPackage.HealthPercent;

		minimapDrone.AssociatedDrone = drone;
		drone._minimapDrone = minimapDrone;

		_drones.Add(drone);
	}

    public static void CreateDrone (CustomCommands.Update.DroneUpdate pPackage) {
        HackerDrone drone = new HackerDrone();
        drone._id = pPackage.id;

        MinimapDrone minimapDrone = MinimapManager.Instance.CreateMinimapDrone(pPackage.x, pPackage.z, 0);
        minimapDrone.InitialTransform(new Vector3(pPackage.x, 0, pPackage.z) / MinimapManager.Instance.scale, 0);

        drone._healthPercent.OnValueChange += () => minimapDrone.HealthChanged(drone._healthPercent.Value);
        drone._healthPercent.Value = 100;

        minimapDrone.AssociatedDrone = drone;
        drone._minimapDrone = minimapDrone;

        _drones.Add(drone);
    }

    public static void UpdateDrone (CustomCommands.Update.DroneUpdate pPackage) {
        HackerDrone drone = GetDroneById(pPackage.id);
        if(drone != null)
        {
            drone._minimapDrone.UpdateTransform(new Vector3(pPackage.x, 0, pPackage.z) / MinimapManager.Instance.scale, pPackage.rotation);
        }else{
            CreateDrone(pPackage);
        }

	}

    

	public static HackerDrone GetDroneById (int pId) {
		return _drones.Find(x => x.Id == pId);
	}
}
