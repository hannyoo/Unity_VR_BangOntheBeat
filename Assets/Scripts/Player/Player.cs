using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public string playerName { get; private set; }
    public int score { get; private set; }
    public int maxHp { get; private set; }
    public int hp { get; private set; }
    public int currentCombo { get; private set; }
    public int maxcombo { get; private set; }
    public int perfectCount { get; private set; }
    public int goodCount { get; private set; }
    public int badCount { get; private set; }
    public int missCount { get; private set; }

    TitleUI titleUI;
    public Player(string playerName, int score, int maxHp, int hp, int currentCombo, 
        int maxcombo, int perfectCount, int goodCount, int badCount, int missCount)
    {
        this.playerName = playerName;
        this.score = score;
        this.maxHp = maxHp;
        this.hp = hp;
        this.currentCombo = currentCombo;
        this.maxcombo = maxcombo;
        this.perfectCount = perfectCount;
        this.goodCount = goodCount;
        this.badCount = badCount;
        this.missCount = missCount;

    }

    public void PlusHP(int plusHp)
    {
        currentCombo++;
        hp += plusHp;
        Mathf.Clamp(hp, 0, 100);
        if (maxcombo < currentCombo)
        { 
            maxcombo = currentCombo;
        }
        
    }

    public void MinusHP(int minusHp)
    {
        currentCombo = 0;
        hp -= minusHp;
        Mathf.Clamp(hp, 0, 100);
    }

    public void PlusScore(int plusScore)
    {
        score += plusScore;
    }

    public void ResetPlayer()
    {
        score = 0;
        maxHp = 100;
        hp = 100;
        currentCombo = 0;
        maxcombo = 0;
        perfectCount = 0;
        goodCount = 0;
        badCount = 0;
        missCount = 0;
    }

    public void CountCheck(int Count)
    {
        if (Count == 0)
        {
            perfectCount++;
            Debug.Log("퍼펙트 = " + perfectCount);
        }

        else if (Count == 1)
        {
            goodCount++;
            Debug.Log("굿 = " + goodCount);
        }


        else if (Count == 2)
        {
            badCount++;
            Debug.Log("배드 = " + badCount);
        }
        else if (Count == 3)
        {
            missCount++;
            Debug.Log("미스 = " + missCount);
        }
    }
    public void SetPlayerName()
    {
        playerName = PlayerPrefs.GetString("CurrentPlayerName");
    }
}
