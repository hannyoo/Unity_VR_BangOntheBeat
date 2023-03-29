using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheet
{
    [Header ("Description")]
    public string title;
    public string artist;

    [Header("Audio")]
    public int bpm;
    public int offset;
    public int[] signature;

    [Header("Note")]
    public List<Note> notes = new List<Note>();

    public AudioClip clip;
    public Sprite img;

    public float BarPerSec { get; private set; }
    public float BeatPerSec { get; private set; }

    public int BarPerMilliSec { get; private set; }
    public int BeatPerMilliSec { get; private set; }

    public void Init()
    {
        BarPerMilliSec = (int)(signature[0] / (bpm / 60f) * 1000);
        BeatPerMilliSec = BarPerMilliSec / 64;

        BarPerSec = BarPerMilliSec * 0.001f;
        BeatPerSec = BarPerMilliSec / 64f;
    }
}
