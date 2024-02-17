using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;

public class Relay : MonoBehaviour
{
	[SerializeField]
	private short maxPlayers = 4;
	private string joinCode;
	public TMPro.TextMeshProUGUI textMeshProUGUI;

	private async void Start()
	{
		await UnityServices.InitializeAsync();

		AuthenticationService.Instance.SignedIn += () =>
		{
			Debug.Log("Signed In" + AuthenticationService.Instance.PlayerId);
		};
		await AuthenticationService.Instance.SignInAnonymouslyAsync();
	}

	public async void AllocateRelay()
	{
		try
		{
			Debug.Log("Host - Creating an allocation.");

			// Important: Once the allocation is created, you have ten seconds to BIND
			Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxPlayers-1); // takes number of connections allowed as argument. you can add a region argument

			joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
			textMeshProUGUI.text = joinCode;
			Debug.Log(joinCode);

			RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
			NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
			NetworkManager.Singleton.ConnectionApprovalCallback = ConnectionApprovalCallback;
			NetworkManager.Singleton.StartHost();
		} 
		catch (RelayServiceException e)
		{
			Debug.LogError(e.Message);
		}
	}

	private void ConnectionApprovalCallback(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
	{
		print("entrou aqui");
	
		// The client identifier to be authenticated
		var clientId = request.ClientNetworkId;

		// Additional connection data defined by user code
		var connectionData = request.Payload;

		response.PlayerPrefabHash = 397963804;
		// Your approval logic determines the following values
		response.Approved = true;
		response.CreatePlayerObject = true;

		// The Prefab hash value of the NetworkPrefab, if null the default NetworkManager player Prefab is used
		//response.PlayerPrefabHash = null;

		// Position to spawn the player object (if null it uses default of Vector3.zero)
		response.Position = Vector3.zero;

		// Rotation to spawn the player object (if null it uses the default of Quaternion.identity)
		response.Rotation = Quaternion.identity;

		// If response.Approved is false, you can provide a message that explains the reason why via ConnectionApprovalResponse.Reason
		// On the client-side, NetworkManager.DisconnectReason will be populated with this message via DisconnectReasonMessage
		response.Reason = "Some reason for not approving the client";

		// If additional approval steps are needed, set this to true until the additional steps are complete
		// once it transitions from true to false the connection approval response will be processed.
		response.Pending = false;
		// Continue filling out the response
		print("aqui");
	}

	public async void JoinRelay(string joinCode)
	{
		try
		{
			Debug.Log("Joining Relay with " + joinCode);
			JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

			RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
			NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

			NetworkManager.Singleton.StartClient();
		}
		catch (RelayServiceException e)
		{
			Debug.LogError(e.Message);
		}
	}
}
