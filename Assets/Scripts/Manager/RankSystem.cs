using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public struct RankData
{
    public string PlayerName;
    public int score;
    public int maxCombo;
}

public class RankSystem : MonoBehaviour
{
    #region Singletone

    private static RankSystem instance = null;

    public static RankSystem GetInstance()
    {
        if (instance == null)
        {
            GameObject go = new GameObject("@RankSystem");
            instance = go.AddComponent<RankSystem>();

            DontDestroyOnLoad(go);
        }
        return instance;

    }
    #endregion

    [SerializeField] private int maxRankCount = 5;     //최대 랭크 표시 개수
    [SerializeField] private GameObject textPrefab;     //랭크 정보를 출력하는 Text UI 프리팹
    public Transform panelRankInfo;   //Text가 배치되는 부모 Panel Transform
    private RankData[] rankDataArray;                   //랭크 정보를 저장하는 RankData 타입의 배열
    List<GameObject> cloneList = new List<GameObject>();
    private int currentIndex = 0;
    public string playerName;

    public void StartRankSystem()
    {
        textPrefab = Resources.Load<GameObject>("UI/TextRankData");
        panelRankInfo = GameObject.FindGameObjectWithTag("RankPanelInfo").transform;
        rankDataArray = new RankData[maxRankCount];

        //1. 기존의 랭크 정보 불러오기
        LoadRankData();
        //2. 1등부터 차례로 현재 스테이지에서 획득한 점수와 비교
        CompareRank();
        //3. 랭크 정보 출력
        PrintRankData();

        SaveRankData();
    }

    public void SaveRankSystem()
    {
        textPrefab = Resources.Load<GameObject>("UI/TextRankData");
        rankDataArray = new RankData[maxRankCount];

        //1. 기존의 랭크 정보 불러오기
        LoadRankData();
        //2. 1등부터 차례로 현재 스테이지에서 획득한 점수와 비교
        EndCompareRank();

        SaveRankData();
    }

    private void LoadRankData()
    {
        int curMusic = SheetManager.GetInstance().curMusic;
        for (int i = 0; i < maxRankCount; ++i)
        {
            rankDataArray[i].PlayerName = PlayerPrefs.GetString("RankPlayerName" + i + curMusic);
            rankDataArray[i].score = PlayerPrefs.GetInt("RankScore" + i + curMusic);
            rankDataArray[i].maxCombo = PlayerPrefs.GetInt("RankMaxCombo" + i + curMusic);
            
        }
        Debug.Log("Load");
    }

    private void SpawnText(string print, Color color)
    {
        //Instatiate()로 textPrefab 복사체를 생성하고, clone 변수에 저장
        GameObject clone = ObjectPoolManager.GetInstance().GetRankPrefab();
        //clone의 TextMeshProUGUI 컴포넌트 정보를 얻어와 text변수에 저장
        TMP_Text text = clone.GetComponent<TMP_Text>();

        //생성한 Text UI 오브젝트의 부모를 panelRankInfo 오브젝트로 설정
        clone.transform.SetParent(panelRankInfo);
        //자식으로 등록되면서 크기가 변환될 수 있기 때문에 크기를 1로 설정
        clone.transform.localScale = Vector3.one;
        // 캔버스가 이미 각도가 틀어져 있어서 각도 맞춰주는 작업
        clone.transform.rotation = Quaternion.Euler(0, 85.189f, 0);
        //Text UI에 출력할 내용과 폰트 색상 설정
        text.text = print;
        text.color = color;

        cloneList.Add(clone);
    }

    private void CompareRank()
    {
        //현재 스테이지에서 달성한 정보
        RankData currentData = new RankData();
        currentData.PlayerName = PlayerPrefs.GetString("CurrentPlayerName");
        currentData.score = PlayerPrefs.GetInt("CurrentScore");
        currentData.maxCombo = PlayerPrefs.GetInt("CurrentMaxCombo");

        //1 ~ 5 등의 점수와 현재 스테이지에서 달성한 점수 비교
        for (int i = 0; i < maxRankCount; ++i)
        {
            if (currentData.score > rankDataArray[i].score)
            {
                //랭크에 들어갈 수 있는 점수를 달성했으면 반복문 중지
                currentIndex = i;
                break;
            }
        }
    }

    public void EndCompareRank()
    {
        //현재 스테이지에서 달성한 정보
        RankData currentData = new RankData();
        currentData.PlayerName = PlayerPrefs.GetString("CurrentPlayerName");
        currentData.score = PlayerPrefs.GetInt("CurrentScore");
        currentData.maxCombo = PlayerPrefs.GetInt("CurrentMaxCombo");

        //1 ~ 5 등의 점수와 현재 스테이지에서 달성한 점수 비교
        for (int i = 0; i < maxRankCount; ++i)
        {
            if (currentData.score > rankDataArray[i].score)
            {
                //랭크에 들어갈 수 있는 점수를 달성했으면 반복문 중지
                currentIndex = i;
                break;
            }
        }

        //currentData의 등수 아래로 점수를 한칸씩 밀어서 저장
        for (int i = maxRankCount - 1; i > 0; --i)
        {
            rankDataArray[i] = rankDataArray[i - 1];

            if (currentIndex == i - 1)
            {
                break;
            }
        }

        //새로운 점수를 랭크에 집어넣기
        rankDataArray[currentIndex] = currentData;
    }

    private void PrintRankData()
    {
        Color color = new Color32(0,243,144,255);

        panelRankInfo.transform.localScale = new Vector3(1f, 1f, 1f);

        for (int i = 0; i < maxRankCount; ++i)
        {
            //방금 플레이의 점수가 랭크에 등록되면 색상을 노란색으로 표시
            /*color = currentIndex != i ? Color.white : Color.yellow;*/

            //Text - TextMeshPro 생성 및 원하는 데이터 출력
            SpawnText((i + 1).ToString(), color);
            SpawnText(rankDataArray[i].PlayerName.ToString(), color);
            SpawnText(rankDataArray[i].score.ToString(), color);
            SpawnText(rankDataArray[i].maxCombo.ToString(), color);
        }

        panelRankInfo.transform.localScale = new Vector3(0.01f,0.01f,0.01f);
    }

    public void SaveRankData()
    {
        int curMusic = SheetManager.GetInstance().curMusic;
        for (int i = 0; i < maxRankCount; i++)
        {
            PlayerPrefs.DeleteKey("RankPlayerName" + i + curMusic);
            PlayerPrefs.DeleteKey("RankScore" + i + curMusic);
            PlayerPrefs.DeleteKey("RankMaxCombo" + i + curMusic);
        }

        for (int i = 0; i < maxRankCount; ++i)
        {
            PlayerPrefs.SetString("RankPlayerName" + i + curMusic, rankDataArray[i].PlayerName);
            PlayerPrefs.SetInt("RankScore" + i + curMusic, rankDataArray[i].score);
            PlayerPrefs.SetInt("RankMaxCombo" + i + curMusic, rankDataArray[i].maxCombo);
        }
        Debug.Log("Save");
    }

    public void ChangeRankTab()
    {
        for (int i = 0; i < cloneList.Count; i++)
        {
            ObjectPoolManager.GetInstance().ReturnRankPrefab(cloneList[i]);
        }

        cloneList.Clear();
        LoadRankData();
        PrintRankData();
    }

    public void CloneListClear()
    {
        cloneList.Clear();
    }
}
