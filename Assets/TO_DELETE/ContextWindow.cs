//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Gamelogic.Extensions;

//public class ContextWindow : Singleton<ContextWindow> {
//	private List<NodeContext> _contextWindows;

//	private void Start () {
//		_contextWindows = new List<NodeContext>(GetComponentsInChildren<NodeContext>(true));
//	}

//	public T GetContext<T>() where T :NodeContext {
//		return _contextWindows.Find(x => x is T) as T;
//	}
//}
