using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Scenes
{
    TitleScene,
    MenuScene,
    GameScene,
    EndingScene,
}
public class ScenesManager : MonoBehaviour
{
    #region Singletone

    private static ScenesManager instance = null;

    public static ScenesManager GetInstance()
    {
        if (instance == null)
        {
            GameObject go = new GameObject("@ScenesManager");
            instance = go.AddComponent<ScenesManager>();

            DontDestroyOnLoad(go);
        }
        return instance;

    }
    #endregion

    #region Scene Control
    public Scenes currentScene;
    public CanvasGroup Fade_img;
    float fadeDuration = 1;
    GameObject fadeUI;

    public GameObject Loading;

    void Start()
    {
        fadeUI = UIManager.GetInstance().GetUI("FadeUI");
        Fade_img = fadeUI.GetComponentInChildren<CanvasGroup>();
        DontDestroyOnLoad(fadeUI);
        SceneManager.sceneLoaded += OnSceneLoaded; // 이벤트에 추가
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // 이벤트에서 제거*
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Fade_img.DOFade(0, fadeDuration)
        .OnStart(() =>
        {
            Debug.Log("OnSceneLoaded");
            Loading.SetActive(false);
        })
        .OnComplete(() =>
        {
            Fade_img.blocksRaycasts = false;
        });
    }

    public void ChangeScene(Scenes scene)
    {
        Fade_img.DOFade(1, fadeDuration)
        .OnStart(() =>
        {
            Fade_img.blocksRaycasts = true; //아래 레이캐스트 막기
        })
        .OnComplete(() =>
        {
            StartCoroutine(LoadScene(scene.ToString())); /// 씬 로드 코루틴 실행 ///
        });
        UIManager.GetInstance().ClearList(); // 씬이 바뀔때마다 UI매니저를 클리어해주겠다.
        RankSystem.GetInstance().CloneListClear();
                                             // PrevScene = currentScene;
    }
    IEnumerator LoadScene(string sceneName)
    {
        //Loading.SetActive(true); //로딩 화면을 띄움

        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false; //퍼센트 딜레이용

        float past_time = 0;
        float percentage = 0;

        while (!(async.isDone))
        {
            yield return null;

            past_time += Time.deltaTime;

            if (percentage >= 90)
            {
                percentage = Mathf.Lerp(percentage, 100, past_time);

                if (percentage == 100)
                {
                    async.allowSceneActivation = true; //씬 전환 준비 완료
                }
            }
            else
            {
                percentage = Mathf.Lerp(percentage, async.progress * 100f, past_time);
                if (percentage >= 90) past_time = 0;
            }
            //Loading_text.text = percentage.ToString("0") + "%"; //로딩 퍼센트 표기
        }
    }
}
#endregion