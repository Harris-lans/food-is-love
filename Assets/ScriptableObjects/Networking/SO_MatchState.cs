﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="FoodIsLove/GameState/MatchState", fileName = "MatchState")]
public class SO_MatchState : ScriptableObject 
{
	#region Member Variables

		[HideInInspector]
		public bool LocalPlayerControllerSpawned;
		[HideInInspector]
		public bool MatchStarted;
		[HideInInspector]
		public int TimeLeft;
		[HideInInspector]
		public SO_Dish[] ExpectedDishes;
		[HideInInspector]
		public bool MatchOver;
		[HideInInspector]
		public string WinnerUsername;
		[HideInInspector]
		public Dictionary<int, CookingPot> PlayerCookingPots;

	#endregion

	#region Member Functions

		public void Initialize(int totalMatchTime, SO_Dish[] expectedDishes)
		{
			LocalPlayerControllerSpawned = false;
			MatchStarted = false;
			MatchOver = false;
		    ExpectedDishes = expectedDishes;
			TimeLeft = totalMatchTime;
			WinnerUsername = "";
			PlayerCookingPots = new Dictionary<int, CookingPot>();
		}

		public void RegisterCookingPot(int playerViewId, CookingPot playersPot)
		{
			PlayerCookingPots[playerViewId] = playersPot;
		}

	#endregion
}