using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Networking;
using System.Collections;


public class Multi : NetworkBehaviour {

	[SerializeField] Camera Eyes;
	[SerializeField] AudioListener audioListener;

	public override void OnStartLocalPlayer()
	{
			GameObject.Find("Scene Camera").SetActive(false);
//			GetComponent<CharacterController>().enabled = true;
			GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
			Eyes.enabled = true;
			audioListener.enabled = true;

			Renderer[] rens = GetComponentsInChildren<Renderer>();
//			foreach(Renderer ren in rens)
//			{
//				ren.enabled = false;
//			}

			GetComponent<NetworkAnimator>().SetParameterAutoSend(0,true);
	}

	public override void PreStartClient ()
	{
		GetComponent<NetworkAnimator>().SetParameterAutoSend(0,true);
	}
}
