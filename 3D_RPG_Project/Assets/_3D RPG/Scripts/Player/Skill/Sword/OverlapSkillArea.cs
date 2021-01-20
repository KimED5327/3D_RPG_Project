﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlapSkillArea : MonoBehaviour
{
    PlayerStatus _status;
    PlayerMove _player;

    int skillNum;

    bool isHit;

    float hitCount;

    // Start is called before the first frame update
    void Start()
    {
        _status = FindObjectOfType<PlayerStatus>();
        _player = FindObjectOfType<PlayerMove>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Status targetStatus = other.GetComponent<Status>();
            if (!targetStatus.IsDead())
            {
                switch (skillNum)
                {
                    case 1:
                        targetStatus.Damage((int)(_status.GetStr() * 1.7f), transform.position, "overlap");
                        break;
                    case 2:
                        targetStatus.Damage((int)(_status.GetStr() * 1.9f), transform.position, "overlap");
                        break;
                    case 3:
                        targetStatus.Damage((int)(_status.GetStr() * 2.0f), transform.position, "overlap");
                        break;
                    case 4:
                        targetStatus.Damage((int)(_status.GetStr() * 2.3f), transform.position, "overlap");
                        break;
                }
            }
        }
    }

    public void SetSkillNum(int num) { skillNum = num; }
}
