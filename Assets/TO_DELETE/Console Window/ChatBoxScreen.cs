//using System;
//using JetBrains.Annotations;
//using UnityEngine;
//using UnityEngine.Events;
//using UnityEngine.UI;

//public class ChatBoxScreen : MonoBehaviour
//{
 
//    [SerializeField] private InputField _chatInput;

//    [SerializeField] private Text _chatOutput;

//    [SerializeField] private ScrollRect _chatScrollRect;

//    private bool _focusedRequested = false;

//    public string GetChatInput()
//    {
//        return _chatInput.text;
//    }

//    public void SetChatInput(string pInput)
//    {
//        _chatInput.text = pInput;
//        _focusedRequested = true;
//    }

//    public void RegisterChatInputHandler(UnityAction pChatHandler)
//    {
//        _chatInput.onEndEdit.AddListener((value) => pChatHandler());
//    }

//    public void UnregisterChatInputHandlers()
//    {
//        _chatInput.onEndEdit.RemoveAllListeners();
//    }

//    public void AddChatLine(string pChatLine)
//    {
//		_chatOutput.text += pChatLine + "\n";
//		ScrollToBottom();
//	}

//    private void Update()
//    {
//        checkFocus();
//    }

//    private void checkFocus()
//    {
//        if (_focusedRequested)
//        {
//            _chatInput.ActivateInputField();
//            _chatInput.Select();
//            _focusedRequested = false;
//        }
//    }
	
//	//Custom methods
//	public void ClearChat()
//	{
//		_chatOutput.text = "";
//	}
//	bool _scroll;
//	public void ScrollToBottom()
//	{
//		_scroll = true;
//		//_chatScrollRect.verticalNormalizedPosition = 0;
//		//Debug.Break();
//	}
//	void OnGUI() {
//		if (_scroll) {
//			_chatScrollRect.verticalNormalizedPosition = 0;
//			_scroll = false;
//		}
//	}
//}
