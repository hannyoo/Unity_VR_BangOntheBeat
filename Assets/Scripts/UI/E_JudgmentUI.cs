using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class E_JudgmentUI : MonoBehaviour
{
    [SerializeField] Image e_JudgmentImg;

    public void ChangeSprite(string name)
    {
        e_JudgmentImg.sprite = Resources.Load<Sprite>($"Image/{name}");
    }

    public void FadeImage()
    {
        StartCoroutine("IEFadeImage");
    }

    IEnumerator IEFadeImage()
    {
        Color setColor = new Color(255f,255f,255f,0f);
        e_JudgmentImg.color = setColor;
        while (e_JudgmentImg.color.a < 255f)
        {
            Color color = e_JudgmentImg.color;
            color.a += 0.01f;
            e_JudgmentImg.color = color;
            yield return null;
        }
    }
}
