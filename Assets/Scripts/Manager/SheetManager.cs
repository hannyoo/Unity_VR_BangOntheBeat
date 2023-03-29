using System.Collections.Generic;
using UnityEngine;

public class SheetManager : MonoBehaviour
{
    #region Singletone

    private static SheetManager instance = null;

    public static SheetManager GetInstance()
    {
        if (instance == null)
        {
            GameObject go = new GameObject("@SheetManager");
            instance = go.AddComponent<SheetManager>();

            DontDestroyOnLoad(go);
        }
        return instance;

    }
    #endregion

    TextAsset sourceFile;

    public int curMusic = 0;

    public string[] title = { "Bang", "Bones", "Welcome To Hell", "Lalalalalalalalalala", "Lucky Strike", "Underground Sound", "No War", "As The World Caves In", "Ditto", "A Thousand Miles", "Kiss Me More" };

    float speed = 1.0f;
    public float Speed
    {
        get
        {
            return speed;
        }
        set
        {
            speed = Mathf.Clamp(value, 1.0f, 5.0f);
        }
    }

    public Dictionary<string, Sheet> sheets = new Dictionary<string, Sheet>();

    public void Init(string title)
    {
        sourceFile = Resources.Load<TextAsset>($"Sheet/{title}/{title}");
    }

    public TextAsset GetSourceFile()
    {
        return sourceFile;
    }

    public void AddSheet(string key, Sheet sheet)
    {
        sheets.Add(key, sheet);
    }

    public string GetCurrentTitle()
    {
        return title[curMusic];
    }
}
