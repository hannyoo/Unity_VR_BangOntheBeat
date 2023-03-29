using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JudgmentUI : MonoBehaviour
{
    public Image judgeImg;
    float height;

    Coroutine judgmentCo;

    public void InitJudgeIMG(string judge)
    {
        if (judgmentCo != null)
        {
            StopCoroutine(judgmentCo);
        }
        judgmentCo = StartCoroutine(IEInitJudgment(judge));
    }
    IEnumerator IEInitJudgment(string judge)
    {
        height = 0f;
        judgeImg.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0f);
        judgeImg.sprite = Resources.Load<Sprite>($"Image/{judge}");
        while (judgeImg.rectTransform.sizeDelta.y < 1)
        {
            height += 0.12f;
            judgeImg.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            yield return null;
        }
    }
}
