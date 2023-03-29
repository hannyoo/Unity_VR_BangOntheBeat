using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    // 첫번째 카메라 앵글, 메인메뉴 관련
    [Header("Main")]
    [SerializeField] Button SelectMusicBtn;
    [SerializeField] GameObject xrOrigin;
    [SerializeField] Button MainMenuZoomBtn;
    [SerializeField] Button OptionBtn;
    [SerializeField] Button MainExitBtn;

    // 음악 고르는 창 관련
    [Header("MusicSelect")]
    [SerializeField] TMP_Text txtSongName;
    [SerializeField] TMP_Text txtSongArtist;
    [SerializeField] TMP_Text txtBPM;
    [SerializeField] Image ImgDisk;
    [SerializeField] TMP_Text txtNoteCount;
    [SerializeField] Button RankingBtn;
    [SerializeField] Button StartBtn;
    [SerializeField] Button ToMainBtn;

    // 곡 리스트 변경관련 버튼
    [SerializeField] Button ListUpBtn;
    [SerializeField] Button ListDownBtn;

    //옵션 창 관련
    [Header("Option")]
    [SerializeField] GameObject SoundPanel;
    [SerializeField] GameObject OptionPanel;
    [SerializeField] Button EndingBtn;
    [SerializeField] Button SoundBtn;
    [SerializeField] Button SoundBackBtn;
    [SerializeField] Button MainBackBtn;
    [SerializeField] Slider Bgm;
    [SerializeField] Slider Sfx;
    [SerializeField] Slider Gbm;

    // 랭킹 창 관련
    [Header("Ranking")]
    [SerializeField] GameObject RankingPanel;
    [SerializeField] Button RankBackBtn;
    [SerializeField] Button RankUpBtn;
    [SerializeField] Button RankDownBtn;
    [SerializeField] TMP_Text txtRankSongName;
    
    // 엔딩크래딧
    [SerializeField] Button EndingExitBtn;

    // 페이드용 간판이미지
    [Header("FadeImg")]
    [SerializeField] Image MainMenuImg;
    [SerializeField] Image OptionImg;
    [SerializeField] Image SelectImg;
    [SerializeField] Image RankImg;

    // 기타
    public List<Sheet> sheetList = new List<Sheet>();
    Coroutine coroutineBgm;
    Vector3 dest;
    Vector3 rot;
    bool isSoundPanel;


    // Start is called before the first frame update
    void Start()
    {
        SetSheetList(SheetManager.GetInstance().curMusic);
        SetOrigin();
        OnclickSetting();
        Debug.Log($"Player : {GameManager.GetInstance().player.playerName}");
        AudioManager.GetInstance().PlaySfx("NeonArrow");
        AudioManager.GetInstance().SfxPlayer.loop = true;
        AudioManager.GetInstance().SfxPlayer.volume = 0.3f;
    }
    void Update()
    {
        if (isSoundPanel) // 사운드 조절 패널에 있을 때만 볼륨 조절할 수 있도록
        {
            SetVolume();
        }
    }
    void SetOrigin() // XROrigin의 첫 위치 세팅
    {
        xrOrigin = GameObject.FindGameObjectWithTag("XROrigin");
        xrOrigin.transform.position = new Vector3(-22.7f, 8.1f, 49.7f);
        xrOrigin.transform.localEulerAngles = new Vector3(0, 46.535f, 0);
    }

    void OnclickSetting() // 온클릭 세팅
    {
        MainMenuZoomBtn.onClick.AddListener(MainMenuOn);
        SelectMusicBtn.onClick.AddListener(SelectMusic);
        ToMainBtn.onClick.AddListener(SelecttoMain);
        StartBtn.onClick.AddListener(GameStartOn);
        OptionBtn.onClick.AddListener(OptionOn);
        MainExitBtn.onClick.AddListener(Exit);
        SoundBtn.onClick.AddListener(SoundOn);
        SoundBackBtn.onClick.AddListener(SoundBack);
        MainBackBtn.onClick.AddListener(MainBack);
        ListUpBtn.onClick.AddListener(NextSheet);
        ListDownBtn.onClick.AddListener(PriorSheet);
        RankingBtn.onClick.AddListener(RankOn);
        RankBackBtn.onClick.AddListener(ExitRank);
        RankUpBtn.onClick.AddListener(NextSheet);
        RankDownBtn.onClick.AddListener(PriorSheet);
        EndingBtn.onClick.AddListener(Ending);
        EndingExitBtn.onClick.AddListener(EndingExit);
    }
    // 페이드효과 관련 함수
    IEnumerator FadeOut(Image img) // 페이드 들어가는 코루틴
    {
        img.DOFade(0, 2.3f);
        yield return new WaitForSeconds(2);
        img.gameObject.SetActive(false);
    }
    void FadeIn(Image img) // 페이드 
    {
        img.gameObject.SetActive(true);
        img.DOFade(1, 0.5f);
    }
    // 옵션과 메인메뉴 연결 버튼////
    void OptionOn()
    {
        
        dest = new Vector3(1.8f, 30, 23.7f);
        rot = new Vector3(0, 86.986f, 0);
        CameraMove(dest);
        CameraRotate(rot);
        StartCoroutine(FadeOut(OptionImg));
        FadeIn(MainMenuImg);
    }
    void MainBack()
    {
        dest = new Vector3(-12, 11, 52.4f);
        rot = new Vector3(0, 0.255f, 0);
        CameraMove(dest);
        CameraRotate(rot);
        FadeIn(OptionImg);
        StartCoroutine(FadeOut(MainMenuImg));
    }    
    void SoundOn() // 옵션 사운드 패널 온오프버튼//
    {
        SoundPanel.gameObject.SetActive(true);
        OptionPanel.gameObject.SetActive(false);
        Gbm.value = AudioManager.GetInstance().curGameVolum;
        Bgm.value = AudioManager.GetInstance().curBGMVolum;
        Sfx.value = AudioManager.GetInstance().curSFXVolum;
        isSoundPanel = true;
    }
    void SoundBack()
    {
        SoundPanel.gameObject.SetActive(false);
        OptionPanel.gameObject.SetActive(true);
        AudioManager.GetInstance().curGameVolum = Gbm.value;
        AudioManager.GetInstance().curBGMVolum = Bgm.value;
        AudioManager.GetInstance().curSFXVolum = Sfx.value;
        isSoundPanel = false;
    }

    // 사운드패널 볼륨 조절 함수
    void SetVolume()
    {
        AudioManager.GetInstance().GameBgmPlayer.volume = Gbm.value;
        AudioManager.GetInstance().SfxPlayer.volume = Sfx.value;
        AudioManager.GetInstance().FindGunAudio();
        AudioManager.GetInstance().leftAudio.volume = Sfx.value;
        AudioManager.GetInstance().rightAudio.volume = Sfx.value;
        AudioManager.GetInstance().MenuBgmPlayer.volume = Bgm.value;
    }
    // 종료버튼
    void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }
    // 메인메뉴로 갈 수 있게 눌러주는 버튼
    void MainMenuOn()
    {
        Debug.Log("MainMenuOn");
        MainMenuZoomBtn.gameObject.SetActive(false);
        dest = new Vector3(-12, 11, 52.4f);
        rot = new Vector3(0, 0.255f, 0);
        CameraMove(dest);
        CameraRotate(rot);
        AudioManager.GetInstance().SfxPlayer.loop = false;
        AudioManager.GetInstance().SfxPlayer.volume = 1f;
        StartCoroutine(FadeStart(MainMenuImg));
    }
    IEnumerator FadeStart(Image img) // 페이드 들어가는 코루틴
    {
        img.DOFade(0, 1);
        yield return new WaitForSeconds(1);
        img.gameObject.SetActive(false);
    }


    // ///////////메인 메뉴 패널과 음악 선택창 연결 /////////
    void SelectMusic()
    {
        dest = new Vector3(-10.2f, 20.3f, 39.59f);
        rot = new Vector3(0, -55.8f, 0);
        CameraMove(dest);
        CameraRotate(rot);
        AudioManager.GetInstance().GetBGMTime();
        ChangeMusic(SheetManager.GetInstance().GetCurrentTitle());
        FadeIn(MainMenuImg);
        StartCoroutine(FadeOut(SelectImg));
    }
    void SelecttoMain()
    {
        dest = new Vector3(-12, 11, 52.4f);
        rot = new Vector3(0, 357.45f, 0);
        CameraMove(dest);
        CameraRotate(rot);
        AudioManager.GetInstance().ReturnBGM();
        StartCoroutine(FadeOut(MainMenuImg));
        FadeIn(SelectImg);
        
    }
    /// <summary>
    /// 게임시작 함수
    /// </summary>
    void GameStartOn()
    {
        dest = new Vector3(-3.1f, 27f, 15.1f);
        rot = new Vector3(40, 362.259f, 0);
        CameraMove(dest);
        CameraRotate(rot);
        AudioManager.GetInstance().GameBgmPlayer.Stop();
        AudioManager.GetInstance().PlaySfx("GameSceneStart");

        Invoke("GameStart", 2);
        StartCoroutine(StartMove());
        FadeIn(SelectImg);

    }
    IEnumerator StartMove()
    {
        yield return new WaitForSeconds(1);
        dest = new Vector3(0f, -0.79f, -10.92f);
        rot = new Vector3(0, 0, 0);
        CameraMove(dest);
        CameraRotate(rot);
    }

    void GameStart()
    {
        ScenesManager.GetInstance().ChangeScene(Scenes.GameScene);
    }
    /// <summary>
    /// 카메라 움직이는 함수
    /// </summary>
    /// <param name="dest"></param>
    void CameraMove(Vector3 dest)
    {
        xrOrigin.transform.DOMoveX(dest.x, 0.7f).SetEase(Ease.InOutQuad);
        xrOrigin.transform.DOMoveY(dest.y, 1f).SetEase(Ease.InOutQuad);
        xrOrigin.transform.DOMoveZ(dest.z, 0.5f).SetEase(Ease.InOutQuad);
    }
    void CameraRotate(Vector3 rot)
    {
        xrOrigin.transform.DORotate(rot, 1f, RotateMode.FastBeyond360);
    }

    /////////////// 음악선택 창////////////////////
    void SetSheetList(int curMusic)
    {
        string title = SheetManager.GetInstance().title[curMusic];
        txtSongName.text = SheetManager.GetInstance().sheets[title].title;
        txtRankSongName.text = SheetManager.GetInstance().sheets[title].title;
        txtSongArtist.text = SheetManager.GetInstance().sheets[title].artist;
        txtBPM.text = "BPM :" + SheetManager.GetInstance().sheets[title].bpm.ToString();
        ImgDisk.sprite = SheetManager.GetInstance().sheets[title].img;
        txtNoteCount.text = "Note :" + SheetManager.GetInstance().sheets[title].notes.Count.ToString();
    }
    public void ChangeMusic(string title)
    {
        if (coroutineBgm != null)
        {
            StopCoroutine(coroutineBgm);
        }
        coroutineBgm = StartCoroutine(IEChangeMusic(title));
    }

    IEnumerator IEChangeMusic(string title) // 음악변경 코루틴
    {
        AudioManager.GetInstance().MenuBgmPlayer.Stop();
        AudioManager.GetInstance().GameBgmPlayer.Stop();
        //sfx 사운드 재생
        yield return new WaitForSeconds(1f);
        AudioManager.GetInstance().PlayGameBgm(title);
    }

    void NextSheet() //시트리스트 업
    { 
        if (++SheetManager.GetInstance().curMusic > SheetManager.GetInstance().sheets.Count - 1)
            SheetManager.GetInstance().curMusic = 0;
        SetSheetList(SheetManager.GetInstance().curMusic);
        RankSystem.GetInstance().ChangeRankTab();
        ChangeMusic(SheetManager.GetInstance().GetCurrentTitle());
        AudioManager.GetInstance().PlaySfx("CD_In");
    }

    void PriorSheet() //시트리스트 다운
    {
        if (--SheetManager.GetInstance().curMusic < 0)
            SheetManager.GetInstance().curMusic = SheetManager.GetInstance().sheets.Count - 1;
        SetSheetList(SheetManager.GetInstance().curMusic);
        RankSystem.GetInstance().ChangeRankTab();
        ChangeMusic(SheetManager.GetInstance().GetCurrentTitle());
        AudioManager.GetInstance().PlaySfx("CD_Out");
    }

    /////////////////////랭크시스템////////////////////////////////////////
    
    void RankOn() // 랭크창 켜기
    {
        dest = new Vector3(-13.6f, 29.2f, 40.3f);
        rot = new Vector3(0, 87, 0);
        CameraMove(dest);
        CameraRotate(rot);
        RankingPanel.SetActive(true);
        StartCoroutine(FadeOut(RankImg));
        FadeIn(SelectImg);
    }

    void ExitRank() //랭크 나가기
    {
        dest = new Vector3(-10.2f, 20.3f, 39.59f);
        rot = new Vector3(0, -55.8f, 0);
        CameraMove(dest);
        CameraRotate(rot);
        FadeIn(RankImg);
        StartCoroutine(FadeOut(SelectImg));
    }
    void Ending() // 엔딩크래딧 들어가기
    {
        dest = new Vector3(-54.6f, 31.7f, 57.8f);
        rot = new Vector3(0, -97.65f, 0);
        CameraMove(dest);
        CameraRotate(rot);
        FadeIn(OptionImg);
    }
    void EndingExit() // 엔딩크래딧 나가기
    {
        dest = new Vector3(1.8f, 30, 23.7f);
        rot = new Vector3(0, 86.986f, 0);
        CameraMove(dest);
        CameraRotate(rot);
        StartCoroutine(FadeOut(OptionImg));
    }
}
