﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpriteAnimator : MonoBehaviour 
{
	[SerializeField]
	private Transform _TransformToTrack;
	
	private Animator _Animator;
	private NavMeshAgent _NavMeshAgent;

	private void Start()
	{
		_NavMeshAgent = GetComponentInParent<NavMeshAgent>();
		_Animator = GetComponent<Animator>();
	}

	private void Update()
	{
		// Maintaining the rotation of the sprite with in the game object
		transform.localEulerAngles = - _TransformToTrack.eulerAngles;
		int angle = (int)_TransformToTrack.localEulerAngles.y;

		if (angle > 180)
		{
			angle = angle - 360;
		}

		// Updating the the angle 
		_Animator.SetInteger("FacingAngle", angle);
		_Animator.SetFloat("Speed", _NavMeshAgent.velocity.magnitude);
	}
}