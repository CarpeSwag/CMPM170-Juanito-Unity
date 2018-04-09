﻿using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;

public class Juanito : MonoBehaviour {

	public static Juanito ins;

	public GameObject JuanitoHuman;
	public GameObject JuanitoSpirit;

	public Animator HumanAnim;
	public Animator SpiritAnim;

	public bool butterflyRelic = false;
	public bool statueRelic = false;

	
	private float spirit_time_limit = 50;
	public bool SpiritState = false;
	public bool inButterflyZone = false;

	[HideInInspector]
	public SpiritController SpiritControl;

	private float numberOfButterflies = 0;

	int MAX_SPIRIT_COUNT = 100;

	// Use this for initialization
	void Awake () {
		ins = this;

		Physics.IgnoreCollision(JuanitoHuman.GetComponent<Collider>(), JuanitoSpirit.GetComponent<Collider>());
		SpiritControl = JuanitoSpirit.GetComponent<SpiritController>();
	}
	
	// Update is called once per frame
	void Update () {
		SpiritHandler();
	}

	public float GetSpiritCount()
	{
		return numberOfButterflies;
	}

	public bool AddSpiritCount(float count)
	{
		if(numberOfButterflies < MAX_SPIRIT_COUNT)
		{
			numberOfButterflies += count;
			return true;
		}
		
		return false;
	}

	public void DelSpiritCount(float count)
	{
		numberOfButterflies = Mathf.Max(numberOfButterflies - count, 0); 
	}

	private void EnterSpiritState()
 	{
 		JuanitoHuman.GetComponent<ThirdPersonUserControl>().enabled = false;
 		JuanitoHuman.GetComponent<ThirdPersonCharacter>().enabled = false;
 		FancyCam.ins.player = JuanitoSpirit.transform;
 		JuanitoSpirit.transform.position = JuanitoHuman.transform.position;
 		JuanitoSpirit.SetActive(true);
 		SpiritState = true;

 	}

 	private void SpiritHandler()
 	{	
 		if(inButterflyZone)
 		{
	 		if(Input.GetKeyDown(KeyCode.Q) || CrossPlatformInputManager.GetButtonDown("Toggle-Spirit"))
			{
				if(!SpiritState)
				{
					EnterSpiritState();
				}
				else
				{
					EndSpiritState();
				}
			}
		}
 	}

 	private void EndSpiritState()
 	{
 		SpiritControl.currentFollower = null;
 		JuanitoSpirit.SetActive(false);
 		FancyCam.ins.player = JuanitoHuman.transform;
 		JuanitoHuman.GetComponent<ThirdPersonUserControl>().enabled = true;
 		JuanitoHuman.GetComponent<ThirdPersonCharacter>().enabled = true;
 		SpiritState = false;
 	}

 	public bool CheckFacingObjects(GameObject[] targetObjects, int layerMask = -1)
	{
		if(layerMask == -1) layerMask = LayerMask.GetMask("Default");

		Vector3 fwd = JuanitoHuman.transform.TransformDirection(Vector3.forward);
		Ray ray = new Ray(JuanitoHuman.transform.position, fwd);
		RaycastHit hit;

		Debug.DrawRay(JuanitoHuman.transform.position, fwd, Color.green);

		if(Physics.Raycast(ray, out hit, 1, layerMask))
		{
			//Debug.Log(hit.transform.gameObject.name);
			foreach(GameObject obj in targetObjects)
			{
				if(obj == hit.transform.gameObject)
				{
					//Debug.Log(obj.name);
					return true;
				}
			}
		}

		return false;
	}
}
