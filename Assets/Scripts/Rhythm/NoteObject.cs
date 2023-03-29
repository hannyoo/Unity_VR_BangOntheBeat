using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
{
    public bool life;

    public Note note = new Note();
    public LongNoteUI longNoteUI;

    public float speed = 5f;
    public int noteNumber;
    public float maxLongNoteCount { get; private set; }
    public float longNoteCount { get; private set; }
    public int controllerType { get; private set; }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void TimeOver()
    {
        life = false;
        ObjectPoolManager.GetInstance().ReturnObject(this);
    }

    public void SetControllerType(int a)
    {
        controllerType = a;
    }

    public void SetLongNoteCount(float a)
    {
        maxLongNoteCount = a;
        longNoteCount = maxLongNoteCount;
        longNoteUI.RefreshValue(a);
    }

    public void MinusLongNoteCount(float a)
    {
        longNoteCount -= a;
        Mathf.Clamp(longNoteCount, 0 ,100);
    }
}
