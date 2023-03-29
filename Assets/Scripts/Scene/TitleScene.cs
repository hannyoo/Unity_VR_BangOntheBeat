using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScene : MonoBehaviour
{
    void Awake()
    {
        for (int i = 0; i < SheetManager.GetInstance().title.Length; i++)
        {
            SheetManager.GetInstance().Init(SheetManager.GetInstance().title[i]);
            PaserManager.GetInstance().Paser(SheetManager.GetInstance().title[i]);
        }

        UIManager.GetInstance().OpenUI("TitleUI");
        UIManager.GetInstance().OpenUI("FadeUI");
        TitleUI titleUI = UIManager.GetInstance().GetUI("TitleUI").GetComponent<TitleUI>();
        AudioManager.GetInstance().PlayBgm("Tocatta");
    }
}
