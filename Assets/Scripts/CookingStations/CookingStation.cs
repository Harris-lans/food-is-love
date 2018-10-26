﻿using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class CookingStation : ANode
{

    #region Member Variables

        public static List<CookingStation> CookingStations;

        [Header("Cooking Station Properties")] 
        public string Name;
        [SerializeField] 
        private float CooldownTime;
        [SerializeField]
        private float CookingTime;
        public SO_Tag CookingStepPerformed;

        [Space, Header("Cooking Station State Details")]
        public CookingStationState State;

        [Space, Header("Local Events")]
        [SerializeField]
        private UnityEvent _StationInUseEvent;
        [SerializeField]
        private UnityEvent _IngredientFinishedCookingEvent;
        public UnityEvent StationInCoolDownEvent;
        [SerializeField]
        private UnityEvent _StationIsAvailableEvent;
        public event IngredientPickedUpAction IngredientPickedUpEvent;

        [Space, Header("Global Events")]
        [SerializeField]
        private SO_GenericEvent _IngredientStartedToCook; 
        [SerializeField]
        private SO_GenericEvent _IngredientCookedEvent;
        [SerializeField]
        private SO_GenericEvent _IngredientPickedUpEvent;

        [Space, Header("Local Inventory Slots")]
        [SerializeField]
        private SO_UIMinionSlot[] _InventorySlots;
        
        [HideInInspector]
        private CookingStationUI _CookingStationUI;
        private SO_Tag _CookedIngredient;
        private Animator _Animator;

    #endregion

    #region Life Cycle

        private void Awake()
        {
            State = (CheckIfTheIngredientsInInventoryAreCompatible()) ? CookingStationState.AVAILABLE : CookingStationState.NOT_VISIBLE_TO_LOCAL_PLAYER;
            _CookingStationUI = GetComponentInChildren<CookingStationUI>();
            _Animator = GetComponent<Animator>();
            _IngredientPickedUpEvent.AddListener(OnPickedUpIngredient);
        }

        private void OnEnable() 
        {
            if (CookingStations == null)
            {
                CookingStations = new List<CookingStation>();
            }
            CookingStations.Add(this);
        }

        private void OnDisable() 
        {
            CookingStations.Remove(this);
        }

    #endregion

    #region Member Functions

        public bool Use(IngredientMinion minion)
        {
            if (State != CookingStationState.AVAILABLE)
            {
                return false;
            }

            State = CookingStationState.COOKING;
            _CookingStationUI.UpdateUI();
            _StationInUseEvent.Invoke();
            _IngredientStartedToCook.Invoke(null);
            StartCoroutine(CookingDelay(CookingTime, minion));
            return true;
        }

        public void PickUpCookedFood(int playerViewID)
        {
            if (_CookedIngredient == null)
            {
                return;
            }
            
            OnPickedUpCookedFood(playerViewID);

            Debug.LogFormat("Person who picked it up: {0}", playerViewID);
            _CookedIngredient = null;
        }

        private void OnPickedUpIngredient(object data)
        {
            Debug.Log(CheckIfTheIngredientsInInventoryAreCompatible());
            if (State == CookingStationState.AVAILABLE || State == CookingStationState.NOT_VISIBLE_TO_LOCAL_PLAYER)
            {
                State = (CheckIfTheIngredientsInInventoryAreCompatible()) ? CookingStationState.AVAILABLE : CookingStationState.NOT_VISIBLE_TO_LOCAL_PLAYER;
            }
        } 

        private bool CheckIfTheIngredientsInInventoryAreCompatible()
        {
            foreach (var inventorySlot in _InventorySlots)
            {
                if (inventorySlot.Ingredient != null && inventorySlot.Ingredient.CheckIfCompatible(CookingStepPerformed))
                {
                    return true;
                }
            }

            return false;
        }

        protected void OnPickedUpCookedFood(int playerWhoPickedUp)
        {
            State = CookingStationState.COOLDOWN;
		    _CookingStationUI.UpdateUI();

            // Invoking Events
            if (IngredientPickedUpEvent != null) { IngredientPickedUpEvent.Invoke(playerWhoPickedUp, _CookedIngredient, CookingStepPerformed); }

            StationInCoolDownEvent.Invoke();
            StartCoroutine(CooldownDelay(CooldownTime));
        }

    #endregion

    #region Coroutines

        protected IEnumerator CookingDelay(float cookingTime, IngredientMinion minion)
	    {
		    yield return new WaitForSeconds(cookingTime);

            _CookedIngredient = minion.Tag;
            _IngredientCookedEvent.Invoke(null);
            _IngredientFinishedCookingEvent.Invoke();

		    State = CookingStationState.COOKED_FOOD_AVAILABLE;
            _CookingStationUI.UpdateUI();
	    }

	    protected IEnumerator CooldownDelay(float cooldownTime)
	    {
		    yield return new WaitForSeconds(cooldownTime);

		    State = (CheckIfTheIngredientsInInventoryAreCompatible()) ? CookingStationState.AVAILABLE : CookingStationState.NOT_VISIBLE_TO_LOCAL_PLAYER;
		    _CookingStationUI.UpdateUI();

	        _StationIsAvailableEvent.Invoke();
	    }

    #endregion

    #region Properties

        public bool IsAvailable
	    {
		    get
		    {
			    return State == CookingStationState.AVAILABLE;
		    }
	    }

        public bool IsCookedAndReady
        {
            get
            {
                return State == CookingStationState.COOKED_FOOD_AVAILABLE;
            }
        }

	    public bool IsOnCooldown
	    {
		    get
		    {
			    return State == CookingStationState.COOLDOWN;
		    }
	    }

        public bool StationInUse
        {
            get
            {
                return State == CookingStationState.UNAVAILABLE;
            }
        }

    #endregion

    #region Enums

        [System.Serializable]
        public enum CookingStationState : byte
        {
            UNAVAILABLE = 0,
            AVAILABLE,
            COOKING,
            COOKED_FOOD_AVAILABLE,
            COOLDOWN,
            NOT_VISIBLE_TO_LOCAL_PLAYER
        }

    #endregion

    public delegate void IngredientPickedUpAction(int playerWhoPickedUp, SO_Tag ingredientTag, SO_Tag cookingMethodTag);
}
