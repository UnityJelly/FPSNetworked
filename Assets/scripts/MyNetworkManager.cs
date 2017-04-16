using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MyNetworkManager : NetworkManager {

	public void StartupHost()
	{
		SetPort();
		NetworkManager.singleton.StartHost();
	}

	public void JoinGame()
	{
		SetIPAddress();
		SetPort();
		NetworkManager.singleton.StartClient();
	}

	public  void HostServer()
	{
		//NetworkServer.Listen(7777);
		SetIPAddress();
		SetPort();
		NetworkManager.singleton.StartServer();
	}

	void SetIPAddress()
	{
		string ipAddress = GameObject.Find("InputFieldIPAddress").transform.FindChild("Text").GetComponent<Text>().text;
		NetworkManager.singleton.networkAddress = ipAddress;
	}

	void SetPort()
	{
		NetworkManager.singleton.networkPort = 7777;
	}

	void OnLevelWasLoaded (int level)
	{
		if(level == 0)
		{
            //SetupMenuSceneButtons();
            StartCoroutine(SetupMenuSceneButtons());
		}

		else
		{
			SetupOtherSceneButtons();
		}
	}

	IEnumerator SetupMenuSceneButtons()
	{
        yield return new WaitForSeconds(0.3f);
		GameObject.Find("ButtonStartHost").GetComponent<Button>().onClick.RemoveAllListeners();
		GameObject.Find("ButtonStartHost").GetComponent<Button>().onClick.AddListener(StartupHost);

		GameObject.Find("ButtonJoinGame").GetComponent<Button>().onClick.RemoveAllListeners();
		GameObject.Find("ButtonJoinGame").GetComponent<Button>().onClick.AddListener(JoinGame);

		GameObject.Find("ButtonHostServer").GetComponent<Button>().onClick.RemoveAllListeners();
//		GameObject.Find("ButtonHostServer").GetComponent<Button>().onClick.AddListener(HostServer);
	}

	void SetupOtherSceneButtons()
	{
		GameObject.Find("ButtonDisconnect").GetComponent<Button>().onClick.RemoveAllListeners();
		GameObject.Find("ButtonDisconnect").GetComponent<Button>().onClick.AddListener(NetworkManager.singleton.StopHost);
	}

}

//	public void MyStartHost ()
//	{
//		Debug.Log (Time.timeSinceLevelLoad + " Starting Host.");
//		StartHost ();
//	}
//
//
//	public override void OnStartHost ()
//	{
//		Debug.Log (Time.timeSinceLevelLoad + " Host Started.");
//	}
//
//	public override void OnStartClient(NetworkClient myclient)
//	{
//		Debug.Log (Time.timeSinceLevelLoad + " Client Start Requested.");
//		InvokeRepeating ("PrintDots", 0f, 1f);
//	}
//
//	public override void OnClientConnect (NetworkConnection conn) {
//		Debug.Log (Time.timeSinceLevelLoad + " Client is connected to IP: " + conn.address);
//		CancelInvoke ();
//	}
//
//	void PrintDots () {
//		Debug.Log (".");
//	}
//}
