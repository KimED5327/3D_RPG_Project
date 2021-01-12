﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Boss : MonoBehaviour
{

    public enum state
    {
        idle, Attack, skill, die
    }

    public state bossState;



    public float attTime;                               //몬스터 공격속도
    public float skillTime;                             //몬스터 스킬속도
    public float dmgApplyTime;                          //실제 데미지 적용 시간.
    public float timer;                                 //공격속도 조절값
    Vector3 startPoint;                                 //최초 생성 값
    Transform player;                                   //공격 목표 (플레이어)
    Transform AttackTransform;                          //공격 장소
    public float speed;                    

    public int maxFindRange;                            //보스 인식 범위
    public int maxAttackRange;                          //보스 공격 범위

    Animator enemyAnimator;                             //몬스터 애니메이터
    BoxCollider myColider;
    Rigidbody myRigid;
    EnemyStatus status;

    public GameObject AttackLocation;                   //공격 위치
    public GameObject AttackEffect;                     //보스 공격 이펙트
    public GameObject SkillEffect;                      //토네이도 리프 이펙트
    public GameObject SkillEffect2;                     //벚꽃 마안 이펙트
    private bool isAttack = false;                      //공격 한번
    private bool isSkillOne = false;                    //토네이도 리프 스킬!
    private bool isSkillTwo = false;                    //벚꽃 마안 스킬!
    GameObject skill;
    private bool sktornado;                             //스킬 토네이도 실행 불값
    private bool skCheerySome = false;                          //스킬 벚꽃 마안 실행 불값
    private bool isDamage;                              //대미지 받는 변수
    private bool skillAttack = false;
    private float skillAttackTime = 0;
    private int skillUpcount = 0;                       //스킬 증가 횟수
    private int skillUseTornadoCount = 0;               //토네이도 리프 사용 횟수
    private int skillUseCherryCount = 0;                //벚꽃 마안 사용 횟수




    public bool getIsDamage() { return isDamage; }


    private void Start()
    {
        bossState = state.idle;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyAnimator = GetComponent<Animator>();
        status = GetComponent<EnemyStatus>();
        myColider = GetComponent<BoxCollider>();
        myRigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (skillAttack)
        {
            SkillAttackUpdate();
        }
        switch (bossState)
        {
            case state.idle:
                IdleUpdate();
                break;
            case state.Attack:
                AttackUpdate();
                break;
            case state.skill:
                SkillUpdate();
                break;
            case state.die:
                DieUpdate();
                break;
        }
        if(status.GetCurrentHp() < 0)
        {
            bossState = state.die;
        }


    }
    private void SkillAttackUpdate()
    {
        skillAttackTime += Time.deltaTime;
        if(skillAttackTime > 1)
        {
            skill.transform.localScale += new Vector3(0.1f, 0, 0.1f);
            skillUpcount++;
            skillAttackTime = 0;
            isDamage = true;
        }
        if(skillUpcount > 20)
        {
            Destroy(skill);
            skillAttack = false;
            isSkillOne = false;
            sktornado = false;
            skillAttackTime = 0;
            skillUpcount = 0;
        }
    }
    private void IdleUpdate()
    {
        if (Vector3.SqrMagnitude(transform.position - player.position) < Mathf.Pow(maxFindRange, 2))
        {
            if(maxFindRange != maxAttackRange)maxFindRange = maxAttackRange;
            timer += Time.deltaTime;

            Vector3 dir = player.transform.position - transform.position;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * speed);

            if (timer > attTime)
            {
                bossState = state.Attack;
                enemyAnimator.SetTrigger("Attack 0");
                isAttack = true;
                timer = 0;
            }
        }
    }

    private void AttackUpdate()
    {

        Vector3 dir = player.transform.position - transform.position;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * speed);

        if (isAttack)
        {
            Vector3 attackDir = player.position - AttackLocation.transform.position;
        
            GameObject attack = Instantiate(AttackEffect, AttackLocation.transform.position, transform.rotation);
            attack.GetComponent<ProjectileMover>().Pushinfo(player, status, this);
            Rigidbody attackRigid = attack.GetComponent<Rigidbody>();
            attack.transform.rotation = Quaternion.LookRotation(attackDir);
            isAttack = false;
        }

        if (enemyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9)
        {
            bossState = state.idle;
        }

        //1차 토네이도 리프
        if ((status.GetCurrentHp() / (float)status.GetMaxHp()) < 0.8f && skillUseTornadoCount == 0)
        {
            isSkillOne = true;
            bossState = state.skill;
            enemyAnimator.SetBool("Skill", true);
            skillUseTornadoCount = 1;
        }
        //2차  토네이도 리프
        else if ((status.GetCurrentHp() / (float)status.GetMaxHp()) < 0.6f && skillUseTornadoCount == 1)
        {
            isSkillOne = true;
            bossState = state.skill;
            enemyAnimator.SetBool("Skill", true);
            skillUseTornadoCount = 2;
        }
        //3차  토네이도 리프
        else if ((status.GetCurrentHp() / (float)status.GetMaxHp()) < 0.4f && skillUseTornadoCount == 2)
        {
            isSkillOne = true;
            bossState = state.skill;
            enemyAnimator.SetBool("Skill", true);
            skillUseTornadoCount = 3;
        }

        if((status.GetCurrentHp() / (float)status.GetMaxHp()) < 0.5f && skillUseCherryCount == 0)
        {
            isSkillTwo = true;
            bossState = state.skill;
            enemyAnimator.SetBool("Skill", true);
            skillUseCherryCount = 1;
        }
        if ((status.GetCurrentHp() / (float)status.GetMaxHp()) < 0.1f && skillUseCherryCount == 1)
        {
            isSkillTwo = true;
            bossState = state.skill;
            enemyAnimator.SetBool("Skill", true);
            skillUseCherryCount = 2;
        }


        if (Input.GetKeyDown(KeyCode.Z))
        {
            isSkillOne = true;
            bossState = state.skill;
            timer = 0;
        }
    }
    private void SkillUpdate()
    {
        if(isSkillOne)
        {
            if (!sktornado) // 토네이도 스킬 업데이트 시작
            {

                Vector3 dir = player.transform.position - transform.position;
                skill = Instantiate(SkillEffect, new Vector3(player.transform.position.x, 1.2f, player.transform.position.z), player.transform.rotation);
                skill.transform.rotation = Quaternion.LookRotation(dir);
                sktornado = true;
            }
            else if (sktornado)
            {
                timer += Time.deltaTime;
                if (timer > 1)
                {
                    skillAttack = true;
                    timer = 0;
                    bossState = state.idle;
                    skill.transform.position = new Vector3(player.transform.position.x, 0.1f, player.transform.position.z);
                    enemyAnimator.SetBool("Skill", false);
                    isSkillOne = !isSkillOne;
                }
                skill.transform.position = new Vector3(player.transform.position.x, 1.2f, player.transform.position.z);
            } // 토네이도 스킬 업데이트 끝

        }
        if(isSkillTwo)
        {
             
                skill = Instantiate(SkillEffect2, transform.up, transform.rotation);
                sktornado = true;

        }
    }
   
    private void DieUpdate()
    {
        enemyAnimator.SetBool("Die", true);
    }

}
