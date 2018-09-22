﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultMenu : UIScreen
{

    [Space, Header("Screens to switch to")]
    [SerializeField]
    private SO_Tag _FoodWorldScreenTag;

    public void OnClickNext()
    {
        _UIManager.SetScreen(_FoodWorldScreenTag);
    }
}