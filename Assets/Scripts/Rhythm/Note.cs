using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Note
{
    public int time;
    public int type;
    public int line;
    public int tail;

    public Note(int time, int type, int line, int tail)
    {
        this.time = time;
        this.type = type;
        this.line = line;
        this.tail = tail;
    }
}
