using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ButtonFunctions : MonoBehaviour
{
	//[SerializeField] private Button serverBtn;
	[SerializeField] private Button hostBtn;
	[SerializeField] private Button clientBtn;

	public Relay relay;
	[SerializeField]
	private string joinCode;
	public TMP_InputField inputTextMeshPro;

	private void Awake()
	{
		hostBtn.onClick.AddListener(() =>
		{
			Host();
		});
		clientBtn.onClick.AddListener(() =>
		{
			Client();
		});
		// Add an event listener to capture input changes
		inputTextMeshPro.onValueChanged.AddListener(delegate { ClientInput(); });

	}
	//private TouchScreenKeyboard keyboard;

	//public void ShowKeyboard()
	//{
	//	keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
	//}

	public void ClientInput()
	{
		joinCode = inputTextMeshPro.text;
	}

	public void Host()
	{
		print("Hosted");
		relay.AllocateRelay();
	}
	public void Client()
	{
		print("Joined as Client");
		relay.JoinRelay(joinCode);
	}
}
