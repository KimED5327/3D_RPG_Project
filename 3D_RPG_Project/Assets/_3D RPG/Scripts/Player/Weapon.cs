﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int damage;
    public float rate;
    public BoxCollider attackArea;

    public void Use()
    {
        StopCoroutine("Swing");
        StartCoroutine("Swing");
    }

    IEnumerator Swing()
    {
        yield return new WaitForSeconds(0.1f);
        attackArea.enabled = true;

        yield return new WaitForSeconds(0.6f);
        attackArea.enabled = false;
    }
}