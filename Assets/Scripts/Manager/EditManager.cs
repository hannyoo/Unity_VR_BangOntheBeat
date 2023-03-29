using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class EditManager : MonoBehaviour
{
    #region Singletone

    private static EditManager instance = null;
    StringBuilder sbAddress = new StringBuilder();

    public static EditManager GetInstance()
    {
        if (instance == null)
        {
            GameObject go = new GameObject("@EitorManager");
            instance = go.AddComponent<EditManager>();

            DontDestroyOnLoad(go);
        }
        return instance;

    }
    #endregion

    AudioSource bgmPlayer;
    public AudioClip bgmClip;

    string d;
    string c;
    string t;
    string n;
    string j;

    private void Start()
    {
        BgmStart();
    }

    private void Update()
    {
        if (Input.GetKeyDown("f"))
        {
            InputShortNote(0);
        }
        if (Input.GetKeyDown("v"))
        {
            InputShortNote(1);
        }
        if (Input.GetKeyDown("g"))
        {
            InputShortNote(2);
        }
        if (Input.GetKeyDown("b"))
        {
            InputShortNote(3);
        }
        if (Input.GetKeyDown("h"))
        {
            InputShortNote(4);
        }
        if (Input.GetKeyDown("d"))
        {
            InputLongNoteStart(0);
        }
        if (Input.GetKeyUp("d"))
        {
            InputLongNoteEnd(0);
        }
        if (Input.GetKeyDown("c"))
        {
            InputLongNoteStart(1);
        }
        if (Input.GetKeyUp("c"))
        {
            InputLongNoteEnd(1);
        }
        if (Input.GetKeyDown("t"))
        {
            InputLongNoteStart(2);
        }
        if (Input.GetKeyUp("t"))
        {
            InputLongNoteEnd(2);
        }
        if (Input.GetKeyDown("n"))
        {
            InputLongNoteStart(3);
        }
        if (Input.GetKeyUp("n"))
        {
            InputLongNoteEnd(3);
        }
        if (Input.GetKeyDown("j"))
        {
            InputLongNoteStart(4);
        }
        if (Input.GetKeyUp("j"))
        {
            InputLongNoteEnd(4);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Pause();
        }
        if (Input.GetKeyDown("q"))
        {
            Export();
        }
    }

    void BgmStart()
    {
        bgmPlayer = gameObject.AddComponent<AudioSource>();
        bgmPlayer.clip = bgmClip;
        bgmPlayer.volume = 0.5f;
        bgmPlayer.Play();

    }

    void Pause()
    {
        if (bgmPlayer.isPlaying == true)
        {
            bgmPlayer.Pause();
            Debug.Log(bgmPlayer.time * 1000);
        }
        else
        {
            bgmPlayer.Play();
        }

    }

    float GetMilliSec()
    {
        return bgmPlayer.time * 1000;
    }

    void InputShortNote(int a)
    {
        sbAddress.AppendLine($"{(int)(bgmPlayer.time * 1000)}, 0, {a}");
        Debug.Log($"{(int)(bgmPlayer.time * 1000)}, 0, {a}");
    }

    void InputLongNoteStart(int a)
    {
        switch (a)
        {
            case 0:
                d = $"{(int)(bgmPlayer.time * 1000)}, 1, {a}";
                break;
            case 1:
                c = $"{(int)(bgmPlayer.time * 1000)}, 1, {a}";
                break;
            case 2:
                t = $"{(int)(bgmPlayer.time * 1000)}, 1, {a}";
                break;
            case 3:
                n = $"{(int)(bgmPlayer.time * 1000)}, 1, {a}";
                break;
            case 4:
                j = $"{(int)(bgmPlayer.time * 1000)}, 1, {a}";
                break;
        }
    }

    void InputLongNoteEnd(int a)
    {
        switch (a)
        {
            case 0:
                d = d + $", {(int)(bgmPlayer.time * 1000)}";
                sbAddress.AppendLine(d);
                Debug.Log(d);
                break;
            case 1:
                c = c + $", {(int)(bgmPlayer.time * 1000)}";
                sbAddress.AppendLine(c);
                Debug.Log(c);
                break;
            case 2:
                t = t + $", {(int)(bgmPlayer.time * 1000)}";
                sbAddress.AppendLine(t);
                Debug.Log(t);
                break;
            case 3:
                n = n + $", {(int)(bgmPlayer.time * 1000)}";
                sbAddress.AppendLine(n);
                Debug.Log(n);
                break;
            case 4:
                j = j + $", {(int)(bgmPlayer.time * 1000)}";
                sbAddress.AppendLine(j);
                Debug.Log(j);
                break;
        }
    }



    public void Export()
    {
        Debug.Log(sbAddress);
    }
}
