using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ResultUI : MonoBehaviour
{
    [Header("텍스트")]
    [SerializeField] TMP_Text PerfectCount;
    [SerializeField] TMP_Text GoodCount;
    [SerializeField] TMP_Text BadCount;
    [SerializeField] TMP_Text MissCount;
    [SerializeField] TMP_Text Score;
    /*[SerializeField] TMP_Text Rank;*/
    [SerializeField] Image RankImg;
    [SerializeField] Button OKBtn;
    [SerializeField] GameObject resultUI;


    // Start is called before the first frame update
    void Start()
    {
        
        PerfectCount.text = GameManager.GetInstance().player.perfectCount.ToString();
        GoodCount.text = GameManager.GetInstance().player.goodCount.ToString();
        BadCount.text = GameManager.GetInstance().player.badCount.ToString();
        MissCount.text = GameManager.GetInstance().player.missCount.ToString();
        Score.text = GameManager.GetInstance().player.score.ToString();

        PlayerPrefs.SetInt("CurrentScore" , GameManager.GetInstance().player.score);
        PlayerPrefs.SetInt("CurrentMaxCombo" , GameManager.GetInstance().player.maxcombo);
        PlayerPrefs.SetString("CurrentPlayerName", GameManager.GetInstance().player.playerName);
        Debug.Log(GameManager.GetInstance().player.score);
        Debug.Log(GameManager.GetInstance().player.maxcombo);
        Debug.Log(GameManager.GetInstance().player.playerName);


        Debug.Log("현재 곡 결과 저장");
        /*RankImg = Resources.Load<Image>($"Image/Rank/{GameManager.GetInstance().player.rank}");*/
        OKBtn.onClick.AddListener(ToMainMenu);

    }

    void ToMainMenu()
    {
        RankSystem.GetInstance().SaveRankSystem();
        Debug.Log(GameManager.GetInstance().player.score);
        Debug.Log(GameManager.GetInstance().player.maxcombo);
        Debug.Log(GameManager.GetInstance().player.playerName);
        resultUI.gameObject.SetActive(false);
        GameManager.GetInstance().player.ResetPlayer();
        NoteManager.GetInstance().ResetNoteCount();
        ScenesManager.GetInstance().ChangeScene(Scenes.MenuScene);

    }

}
