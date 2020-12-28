﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Chapter UI")]
    [SerializeField] GameObject _goChapter = null;
    [SerializeField] GameObject _goRequestion = null;
    [SerializeField] Image _imgSelectChapter = null;
    [SerializeField] Text _txtSelectChapter = null;

    [Header("Slot")]
    [SerializeField] ChapterSlot[] _slots = null;
    [SerializeField] Sprite _lockImage = null;

    [Header("Balloon")]
    [SerializeField] GameObject _goBalloon = null;
    [SerializeField] Text _txtBalloon = null;
    [SerializeField] string[] _balloonStr = null;

    int _currentBalloon = -1;
    public static int _choiceChapter = -1;

    private void Awake()
    {
        for (int i = 1; i < _slots.Length; i++)
            _slots[i].LockSlot(_lockImage);
    }

    void Update()
    {
        // 테스트용
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _slots[1].UnLockSlot();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _slots[2].UnLockSlot();
        }

        // 캐릭터를 터치하면 말풍선 On
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out RaycastHit hit, 1000))
            {
                _goBalloon.SetActive(true);
                _currentBalloon = GetRandomNumber();
                _txtBalloon.text = _balloonStr[_currentBalloon];
            }
        }
    }

    int GetRandomNumber()
    {
        int random = 0;
        while (true)
        {
            random = Random.Range(0, _balloonStr.Length);
            if (_currentBalloon != random)
                return random;
        }
    }

    public void OnTouchBalloon()
    {
        _goBalloon.SetActive(false);
    }


    public void OnTouchBookShelf()
    {
        _goChapter.SetActive(true);
    }

    public void OnTouchChapter(int index)
    {
        _choiceChapter = index;

        _goRequestion.SetActive(true);
        _imgSelectChapter.sprite = _slots[index].GetSprite();
        _txtSelectChapter.text = _slots[index].GetName() + " 모험담을\n작성하시겠습니까?";

    }
    public void OnTouchChapterCancel()
    {
        _goChapter.SetActive(false);
    }
    public void OnTouchQuestionOK()
    {
        _goRequestion.SetActive(false);
        LoadingScene.LoadScene("GameScene");
    }
    public void OnTouchQuestionCancel()
    {
        _goRequestion.SetActive(false);
    }
}