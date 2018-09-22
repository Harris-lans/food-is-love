﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStunnable
{
    void Stun(float stunTime, GameObject attackOwner);

    IEnumerator StunTimer();
}