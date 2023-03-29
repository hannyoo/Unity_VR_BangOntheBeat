using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class TitleUI : MonoBehaviour
{
    public Button NextBtn;
    ScenesManager sM;
    public InputField inputField;
    public string playerName;
    public GameObject inputFieldPanel;
    public Button StartBtn;
    public TMP_Text Nametxt;
    public TMP_Text Warningtxt;
    void Start()
    {
        sM = ScenesManager.GetInstance();
        inputFieldPanel.SetActive(false);
        NextBtn.onClick.AddListener(InputFieldPanelOn);
        StartBtn.onClick.AddListener(OnClickStart);
    }

    void InputFieldPanelOn()
    {
        NextBtn.gameObject.SetActive(false);
        inputFieldPanel.SetActive(true);
        ShowKeyboard();
    }
    void OnClickStart()
    {
        if (inputField.text.Length >= 1)
        {
            playerName = inputField.text;
            PlayerPrefs.SetString("CurrentPlayerName", playerName);
            GameManager.GetInstance().player.SetPlayerName();
            Nametxt.text = $"{inputField.text}님, 환영합니다.";
            
            ScenesManager.GetInstance().ChangeScene(Scenes.MenuScene);
            StartBtn.onClick.RemoveAllListeners();
        }
        else
        {
            Nametxt.gameObject.SetActive(false);
            Warningtxt.text = "이름이 입력되지 않았습니다.";
            Warningtxt.gameObject.SetActive(true);
        }

    }
    private TouchScreenKeyboard keyboard;

    public void ShowKeyboard()
    {
        keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
    }
}
