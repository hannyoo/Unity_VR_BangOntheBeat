using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class PaserManager : MonoBehaviour
{
    #region Singletone

    private static PaserManager instance = null;

    public static PaserManager GetInstance()
    {
        if (instance == null)
        {
            GameObject go = new GameObject("@PaserManager");
            instance = go.AddComponent<PaserManager>();

            DontDestroyOnLoad(go);
        }
        return instance;

    }
    #endregion

    enum Step
    {
        Description,
        Audio,
        Note,
    }
    Step curStep = Step.Description;

    public Sheet sheet;
    public AudioClip clip;
    public Sprite img;

    public void Paser(string title)
    {
        sheet = new Sheet();
        string readLine = string.Empty;

        using (StringReader sr = new StringReader(SheetManager.GetInstance().GetSourceFile().text))
        {
            readLine = sr.ReadLine();

            while (readLine != null)
            {
                if (readLine.StartsWith("[Description]"))
                {
                    curStep = Step.Description;
                    readLine = sr.ReadLine();
                }
                else if (readLine.StartsWith("[Audio]"))
                {
                    curStep = Step.Audio;
                    readLine = sr.ReadLine();
                }
                else if (readLine.StartsWith("[Note]"))
                {
                    curStep = Step.Note;
                    readLine = sr.ReadLine();
                }

                switch (curStep)
                {
                    case Step.Description:
                        if (readLine.StartsWith("Title"))
                            sheet.title = readLine.Split(':')[1].Trim();
                        else if (readLine.StartsWith("Artist"))
                            sheet.artist = readLine.Split(':')[1].Trim();
                        break;
                    case Step.Audio:
                        if (readLine.StartsWith("BPM"))
                            sheet.bpm = int.Parse(readLine.Split(':')[1].Trim());
                        else if (readLine.StartsWith("Offset"))
                            sheet.offset = int.Parse(readLine.Split(':')[1].Trim());
                        else if (readLine.StartsWith("Signature"))
                        {
                            string[] _s = readLine.Split(':');
                            _s = _s[1].Split('/');
                            int[] sign = { int.Parse(_s[0].Trim()), int.Parse(_s[1].Trim()) };
                            sheet.signature = sign;
                        }
                        break;
                    case Step.Note:
                        if (string.IsNullOrEmpty(readLine))
                            break;

                        string[] s = readLine.Split(',');
                        int time = int.Parse(s[0].Trim());
                        int type = int.Parse(s[1].Trim());
                        int line = int.Parse(s[2].Trim());
                        int tail = -1;
                        if (s.Length > 3)
                            tail = int.Parse(readLine.Split(',')[3].Trim());
                        if (type == 0)
                            time -= sheet.offset;
                        sheet.notes.Add(new Note(time, type, line, tail));
                        break;
                }

                readLine = sr.ReadLine();
            }

        }

        GetClip(sheet.title);
        GetImg(sheet.title);

        sheet.clip = clip;
        sheet.img = img;

        SheetManager.GetInstance().AddSheet(title, sheet);

    }

    public void GetClip(string title)
    {
        clip = Resources.Load<AudioClip>($"Sheet/{title}/{title}");
        clip.name = title;
    }

    public void GetImg(string title)
    {
        img = Resources.Load<Sprite>($"Sheet/{title}/{title}");
        img.name = title;
    }
}
