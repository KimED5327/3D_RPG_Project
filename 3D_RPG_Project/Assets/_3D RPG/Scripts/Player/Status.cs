﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Status : MonoBehaviour
{
    [Header("Basic Status")]
    [SerializeField] protected string _name = "이름";
    [SerializeField] protected int _maxHp = 100;
    protected int _curHp = 100;
    [SerializeField] protected int _atk = 0;
    [SerializeField] protected int _def = 0;
    [SerializeField] protected int _level = 1;
    [SerializeField] protected int _giveExp = 50;

    //임시 기초 스탯 (수정 필)
    //[SerializeField] protected int _swordMaxHp = 150;
    //protected int _swordCurHp = 150;
    //[SerializeField] protected int _swordDef = 10;
    ////전사
    //[SerializeField] protected int _mageMaxHp = 130;
    //protected int _mageCurHp = 150;
    //[SerializeField] protected int _mageDef = 2;
    //법사
    protected bool _isDead = false;

    string _skillType = "normal";

    protected void Start()
    {
        Initialized();
    }

    public virtual void Initialized()
    {
        if (_curHp < 0)
            _curHp = _maxHp;

        _isDead = false;
    }

    public virtual void Damage(int num, Vector3 targetPos, string skillType = "normal")
    {
        //if (skillType.Equals("overlap")) return;
        if (_skillType.Equals("overlap")) return;
        _skillType = skillType;
        _curHp -= num;
        Invoke("ChangeSkillType", 0.5f);
        if(_curHp <= 0)
        {
            Dead();
        }
        else
        {
            if (_skillType.Equals("overlap"))
            {
                HurtReaction(targetPos);
            }
        }
    }

    protected abstract void HurtReaction(Vector3 targetPos);

    protected virtual void Dead()
    {
        if (transform.CompareTag(StringManager.enemyTag))
        {
            GetComponent<Enemy>().GetPlayerStatus().IncreaseExp(_giveExp);
        }
        _curHp = 0;
        _isDead = true;
    }


    public string GetName() { return _name; }
    public void SetName(string name) { _name = name; }
    public int GetLevel() { return _level; }
    public void SetLevel(int level) { _level = level; }
    public int GetCurrentHp() { return _curHp; }
    public int SetCurrentHp(int hp) => _curHp = hp;
    public void IncreaseHp(int hp)
    {
        _curHp += hp;
        if (_curHp > _maxHp)
            _curHp = _maxHp;
    }
    public int GetMaxHp() { return _maxHp; }
    public void SetMaxHp(int maxHp) { _maxHp = maxHp; }
    public int GetAtk() { return _atk; }
    public bool IsDead() { return _isDead; }
    public void ChangeSkillType()
    {
        _skillType = "normal";
    }

    //임시 저장 getter setter (수정 필)
    //public int GetSwordCurrentHp() { return _swordCurHp; }
    //public int SetSwordCurrentHp(int hp) => _swordCurHp = hp;
    //public int GetMageCurrentHp() { return _mageCurHp; }
    //public int SetMageCurrentHp(int hp) => _mageCurHp = hp;
    //public int GetSwordMaxHp() { return _swordMaxHp; }
    //public void SetSwordMaxHp(int maxHp) { _swordMaxHp = maxHp; }
    //public int GetMageMaxHp() { return _mageMaxHp; }
    //public void SetMageMaxHp(int maxHp) { _mageMaxHp = maxHp; }
    //public int GetSwordDef() { return _swordDef; }
    //public void SetSwordDef(int def) { _swordDef = def; }
    //public int GetMageDef() { return _mageDef; }
    //public void SetMageDef(int def) { _mageDef = def; }
}
