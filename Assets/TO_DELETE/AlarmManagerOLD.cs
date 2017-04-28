//using system.collections;
//using system.collections.generic;
//using unityengine;

//using gamelogic.extensions;
//using dg.tweening;

//public class alarmmanagerold : singleton<alarmmanager> {
//	[serializefield]
//	private float _minalarmtime;



//	[serializefield]
//	private float _minenemyspawndelay;

//	[serializefield]
//	private float _maxenemyspawndelay;



//	[serializefield]
//	private float _minwavedelay;

//	[serializefield]
//	private float _maxwavedelay;



//	[serializefield]
//	private int _minwaveenemycount;

//	[serializefield]
//	private int _maxwaveenemycount;

//	[serializefield]
//	private gameobject _enemyprefab;



//	private bool _alarmactive;


//	//private timers.timer _wavetimer;
//	//private timers.timer _spawntimer;

//	//private fmod.studio.eventinstance _alarmsound;

//	private void start () {
//		//_wavetimer = timers.createtimer("alarm wave").setcallback(() => spawnwave()).setminmaxtime(_minwavedelay, _maxwavedelay);
//		//_spawntimer = timers.createtimer("alarm spawn").resetonfinish().setcallback(() => spawnenemy()).setminmaxtime(_minenemyspawndelay, _maxenemyspawndelay);

//		//_alarmsound = fmodunity.runtimemanager.createinstance("event:/pe_shooter/pe_shooter_alarm_loop");
//	}


//	private void spawnwave () {
//		//_wavetimer.setloop(-1).start();
//		//debug.log("started wave");
//		//_spawntimer.setloop(random.range(_minwaveenemycount, _maxwaveenemycount + 1)).start();
//	}


//	private void spawnenemy () {

//		vector3 playerpos = findobjectoftype<playermotor>().transform.position;

//		list<dronespawner> dronespawners = dronespawner.dronespawners;

//		dronespawners.sort((x, y) => vector3.distance(x.transform.position, playerpos).compareto(vector3.distance(y.transform.position, playerpos)));

//		//todo make the 3 modifiable
//		int index = random.range(0, mathf.min(3+1, dronespawners.count));

//		gameobject drone = gameobject.instantiate(_enemyprefab, dronespawners[index].transform.position, dronespawners[index].transform.rotation);
//		//drone.getcomponent<droneenemy>().settarget();

//		//debug.log("spawned enemy");
//	}




//	public void activatealarm () {
//		_alarmactive = true;

//		getcomponent<shootergamestatemanager>().triggeralarm();

//		spawnwave();

//		//_alarmsound.start();

//		//todo send min alarm time
//		//todo send update
//	}

//	public void deactivatealarm () {
//		debug.log("disabled alarm");

//		_alarmactive = false;

//		getcomponent<shootergamestatemanager>().disablealarm();

//		//_wavetimer.stop();
//		//_spawntimer.stop();

//		//_alarmsound.stop(fmod.studio.stop_mode.allowfadeout);
//		//todo send update
//	}

//	/*
//	public void setalarm (bool pstate) {
//		_alarmactive = pstate;
//	}
//	*/
//}
