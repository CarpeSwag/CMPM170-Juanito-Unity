﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;

[RequireComponent(typeof (AICharacterControl))]
public class AIFollowController : MonoBehaviour {

	public float CollisionRadius;

	public AICharacterControl aiController;

	private Transform escapeLocation;

	// Use this for initialization
	void Start () {
		aiController = GetComponent<AICharacterControl>();

		escapeLocation = new GameObject().transform;
	}
	
	// Update is called once per frame
	void Update () {
		CheckForPlayer();
	}
 
	private void CheckForPlayer()
	{
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, CollisionRadius);

		if(hitColliders.Length != 0)
		{
			foreach(Collider hit in hitColliders)
			{
				if(hit.gameObject.GetComponent<SpiritController>())
				{
					aiController.SetTarget(hit.gameObject.transform);
					return;
				}
				else if(hit.gameObject.GetComponent<ThirdPersonUserControl>())
				{
					FleePlayer(hit.gameObject.transform);
					return;
				}
			}

			aiController.SetTarget(null);
		}
		else
		{
			Debug.Log("No collisions!");
		}

	}

	void FleePlayer(Transform fleeTarget)
	{
		Vector3 directionToPlayer = transform.position - fleeTarget.position;
		Vector3 checkPos = transform.position + directionToPlayer;

		NavMeshHit navHitForward, navHitLeft, navHitRight, navHitToward;

		if(NavMesh.SamplePosition(checkPos, out navHitForward, 1.0f, NavMesh.AllAreas))
		{
			// Debug.Log("Flee Forward: " + navHitForward.position);
		}
		
		if(NavMesh.SamplePosition(FleeLeft(checkPos), out navHitLeft, 1.0f, NavMesh.AllAreas))
		{
			// Debug.Log("Flee Left: " + navHitLeft.position);
		}
		
		if(NavMesh.SamplePosition(FleeRight(checkPos), out navHitRight, 1.0f, NavMesh.AllAreas))
		{
			// Debug.Log("Flee Right: " + navHitRight.position);
		}

		if(NavMesh.SamplePosition(FleeToward(checkPos), out navHitToward, 1.0f, NavMesh.AllAreas))
		{
			// Debug.Log("Flee Right: " + navHitRight.position);
		}

		float scoreForward = Vector3.Distance(navHitForward.position, transform.position);
		float scoreLeft = Vector3.Distance(navHitLeft.position, transform.position);
		float scoreRight = Vector3.Distance(navHitRight.position, transform.position);
		float scoreToward = Vector3.Distance(navHitToward.position, transform.position);


		scoreForward = (scoreForward == Mathf.Infinity ? 0 : scoreForward);
		scoreLeft = (scoreLeft == Mathf.Infinity ? 0 : scoreLeft);
		scoreRight = (scoreRight == Mathf.Infinity ? 0 : scoreRight);
		scoreToward = (scoreToward == Mathf.Infinity ? 0 : scoreToward);

		// Debug.Log("F: " + scoreForward + " L: " + scoreLeft + " R: " + scoreRight + " T: " + scoreToward);

		float maxScore = Mathf.Max(new float[] {scoreForward, scoreLeft, scoreRight, scoreToward});
		
		if(maxScore != 0)
		{
			if(maxScore == scoreForward)
				escapeLocation.position = navHitForward.position;
			else if(maxScore == scoreLeft)
				escapeLocation.position = navHitLeft.position;
			else if(maxScore == scoreRight)
				escapeLocation.position = navHitRight.position;
			else if(maxScore == scoreToward)
				escapeLocation.position = navHitToward.position;
		}

		if(escapeLocation)
			aiController.SetTarget(escapeLocation);
	}

	Vector3 FleeToward(Vector3 targetPos)
	{
		return Quaternion.Euler(0, 50, 0) * targetPos;	
	}

	Vector3 FleeRight(Vector3 targetPos)
	{
		return Quaternion.Euler(0, 44, 0) * targetPos;
	}

	Vector3 FleeLeft(Vector3 targetPos)
	{
		return Quaternion.Euler(0, -46, 0) * targetPos;
	}
}