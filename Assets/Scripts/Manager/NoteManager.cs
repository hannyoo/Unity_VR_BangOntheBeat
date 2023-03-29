using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NoteManager : MonoBehaviour
{
    #region Singletone

    private static NoteManager instance = null;

    public static NoteManager GetInstance()
    {
        if (instance == null)
        {
            GameObject go = new GameObject("@NoteManager");
            instance = go.AddComponent<NoteManager>();

            DontDestroyOnLoad(go);
        }
        return instance;

    }
    #endregion

    Note curNote;
    int curNoteTime;
    int prevControllType;
    bool isLongNote;
    bool controllSwich=true;
    Coroutine startCoroutine;
    Coroutine coroutine;

    public List<NoteObject> notes = new List<NoteObject>();
    public List<GameObject> guides = new List<GameObject>();
    public List<Coroutine> noteCoroutines = new List<Coroutine>();

    public readonly Vector3[] linpos =
    {
        new Vector3(-3f, .5f),
        new Vector3(-1.2f, -.5f),
        new Vector3(0f, .5f),
        new Vector3(1.2f, -.5f),
        new Vector3(3f, .5f),
    };

    int next = 0;
    int prev = 0;

    private void Update()
    {
        if (GameObject.FindGameObjectWithTag("LongNote") == null)
        {
            isLongNote = false;
        }
    }

    public void StartGame()
    {
        SetCreateTime(SheetManager.GetInstance().GetCurrentTitle(), next);
        startCoroutine = StartCoroutine(IEGenTimer(SheetManager.GetInstance().sheets[SheetManager.GetInstance().GetCurrentTitle()].BarPerMilliSec * 0.001f));
    }

    void SetCreateTime(string title, int a)
    {
        if (next == SheetManager.GetInstance().sheets[SheetManager.GetInstance().GetCurrentTitle()].notes.Count)
        {
            Debug.Log("노트 없음");
            AudioManager.GetInstance().FadeOutBGM();
            GameManager.GetInstance().GameOver(1, next, "Judge_Complete");
            StopCoroutine(startCoroutine);
            return;
        }
        curNote = SheetManager.GetInstance().sheets[title].notes[a];
        curNoteTime = curNote.time;
    }

    IEnumerator IEGenTimer(float interval)
    {
        while (true)
        {
            if (next == SheetManager.GetInstance().sheets[SheetManager.GetInstance().title[SheetManager.GetInstance().curMusic]].notes.Count)
            {
                break;
            }
            Gen(SheetManager.GetInstance().GetCurrentTitle());
            yield return new WaitForSeconds(interval / 64);
        }
    }

    void Gen(string title)
    {
        if (curNoteTime < AudioManager.GetInstance().GetMilliSec())
        {
            switch (curNote.type)
            {
                case 0:
                    if (!isLongNote)
                    {
                        int controllType = SwichControllType();
                        prevControllType = controllType;
                    }
                    NoteObject note = ObjectPoolManager.GetInstance().GetNote(prevControllType);
                    note.note = SheetManager.GetInstance().sheets[title].notes[next];
                    note.SetPosition(linpos[note.note.line]);

                    note.SetControllerType(prevControllType);
                    note.noteNumber = next;
                    note.life = true;
                    notes.Add(note);
                    next++;
                    SetCreateTime(title, next);
                    coroutine = StartCoroutine("Jugement");
                    noteCoroutines.Add(coroutine);
                    break;
                case 1:
                    isLongNote = true;
                    int _controllType = SwichControllType();
                    prevControllType = _controllType;
                    NoteObject _note = ObjectPoolManager.GetInstance().GetLongNote(_controllType);
                    _note.note = SheetManager.GetInstance().sheets[title].notes[next];
                    _note.SetPosition(linpos[_note.note.line]);

                    _note.SetControllerType(_controllType);
                    _note.noteNumber = next;
                    _note.life = true;
                    notes.Add(_note);
                    next++;
                    SetCreateTime(title, next);
                    coroutine = StartCoroutine("LongNoteJugement");
                    noteCoroutines.Add(coroutine);
                    break;
            }
        }
    }

    IEnumerator Jugement()
    {
        NoteObject note = notes[prev];
        Transform[] model = note.GetComponentsInChildren<Transform>();
        model[1].localScale = new Vector3(0f, 0f, 0f);
        prev = next;
        yield return new WaitForSeconds(SheetManager.GetInstance().sheets[SheetManager.GetInstance().GetCurrentTitle()].BarPerMilliSec * 0.001f * 0.5f);
        model[1].DOScale(0.7f, SheetManager.GetInstance().sheets[SheetManager.GetInstance().GetCurrentTitle()].offset * 0.001f * 0.5f);
        //StartCoroutine(GrowBigNote(model[1]));
        yield return new WaitForSeconds(SheetManager.GetInstance().sheets[SheetManager.GetInstance().GetCurrentTitle()].BarPerMilliSec * 0.001f * 0.5f);
        if (note != null)
        {
            note.life = false;
            ObjectPoolManager.GetInstance().ReturnObject(note);
            GameManager.GetInstance().Miss(); // 미스 판정
        }
    }

    IEnumerator LongNoteJugement()
    {
        NoteObject note = notes[prev];

        prev = next;
        float a = Mathf.Round((note.note.tail - note.note.time) * 0.001f);
        note.SetLongNoteCount(a);
        yield return new WaitForSeconds((note.note.tail - note.note.time) * 0.001f);
        if (note != null)
        {
            note.life = false;
            GameManager.GetInstance().CheckLongJugement(note);
            ObjectPoolManager.GetInstance().ReturnLongNote(note);
        }
    }

    public void StopNoteCoroutine(NoteObject note) // 특정 코루틴 찾아서 스톱하는 함수
    {
        StopCoroutine(noteCoroutines[note.noteNumber]);
    }

    IEnumerator GrowBigNote(Transform model)
    {
        while (model.lossyScale.x < 0.8)
        {
            model.localScale += new Vector3(0.002f, 0.002f, 0.002f);
            yield return null;
        }
    }

    int SwichControllType()
    {
        if (!controllSwich)
        {
            controllSwich = true;
            return 0;
        }
        controllSwich = false;
        return 1;
    }

    public void ResetNoteCount()
    {
        next = 0;
        prev = 0;
        notes.Clear();
        guides.Clear();
        noteCoroutines.Clear();
    }

    public void StopStartCoroutine()
    {
        StopCoroutine(startCoroutine);
    }

    public int curNoteNumber()
    {
        return next;
    }
}
