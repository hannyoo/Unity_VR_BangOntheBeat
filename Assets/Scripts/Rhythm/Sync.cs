using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sync : MonoBehaviour
{
    AudioSource playTik;
    public AudioClip tik;

    float musicBpm = 120f;
    float stdBpm = 60f;
    int musicTempo = 4;
    int stdTempo = 4;

    float tikTime = 0;
    float nextTime = 0;

    private void Start() {
        playTik = GetComponent<AudioSource>();
    }

    private void Update() {
        tikTime = (stdBpm / musicBpm) * (musicTempo / stdTempo);

        nextTime += Time.deltaTime;

        if (nextTime > tikTime)
        {
            StartCoroutine(PlayTic(tikTime));
            nextTime = 0;
        }
    }

    IEnumerator PlayTic(float tikTime)
    {
        Debug.Log(nextTime);
        playTik.PlayOneShot(tik);
        yield return new WaitForSeconds(tikTime);
    }
}
