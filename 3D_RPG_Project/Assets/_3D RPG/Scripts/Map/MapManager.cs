﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;

    [SerializeField] Map[] _maps = null;
    Map _currentMap;

    string _moveMap;
    string _priorMap;

    private void Awake()
    {
        if(instance == null)
        {
            // 싱글턴 및 시작맵 생성
            instance = this;
            _currentMap = _maps[0];
            _priorMap = _currentMap.GetMapName();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    // 맵 체인지
    public void ChangeMap(string moveMapName)
    {
        _priorMap = _currentMap.GetMapName();

        for (int i = 0; i < _maps.Length; i++)
        {
            _moveMap = _maps[i].GetMapName();
            if (_moveMap == moveMapName)
            {
                // 교체
                _currentMap = _maps[i];
                break;
            }
        }

        StartCoroutine(MapLoading());
    }

    IEnumerator MapLoading()
    {
        ScreenEffect.instance._isFinished = false;
        ScreenEffect.instance.ExecuteFadeOut();

        yield return new WaitUntil(() => ScreenEffect.instance._isFinished);

        // 로딩 후 ActiveMap 실행
        LoadingScene.LoadScene("GameScene_HJ");

        QuestManager.instance.UpdateQuestHudOnStart();
        if (QuestManager.instance.GetOngoingQuest() == null) Debug.Log("현재 진행중인 퀘스트가 없습니다.");
        else Debug.Log(QuestManager.instance.GetOngoingQuest().GetQuestID() + "번 퀘스트가 진행 중입니다.");

        Debug.Log(QuestDB.instance.GetQuest(QuestManager.instance.GetOngoingQuest().GetQuestID()).GetTitle());
    }

    public void ActiveMap()
    {
        Transform tfPlayer = FindObjectOfType<PlayerStatus>().transform;

        // 맵 생성 후, 플레이어 스폰 위치 조정.
        Instantiate(_currentMap.gameObject);
        _currentMap.SearchSpawnPoint(tfPlayer, _priorMap);
    }
}
