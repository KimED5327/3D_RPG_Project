﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    public static SpriteManager instance;

    public Sprite[] _spriteItem;
    public Sprite[] _spriteBuff;
    public Sprite[] _spriteBlock;
    public Sprite[] _spriteSwordSkill;
    public Sprite[] _spriteMageSkill;
    public Sprite[] _spriteKeyword;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else Destroy(gameObject);
    }

    public Sprite GetKeywordSprite(int keywordID)
    {
        return _spriteKeyword[keywordID - 1];
    }

    public Sprite GetItemSprite(int itemId)
    {
        return _spriteItem[itemId - 1];
    }

    public Sprite GetBuffSprite(int buffId)
    {
        return _spriteBuff[buffId - 1];
    }

    public Sprite GetBlockSprite(int blockNum)
    {
        return _spriteBlock[blockNum];
    }

    public Sprite GetSwordSkillSprite(int skillNum)
    {
        return _spriteSwordSkill[skillNum];
    }

    public Sprite GetMageSkillSprite(int skillNum)
    {
        return _spriteMageSkill[skillNum];
    }
}
