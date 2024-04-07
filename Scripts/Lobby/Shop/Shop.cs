using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Shop : MonoBehaviour
{
    Button exitBtn;
    Lobby lobbyCom;

    // Start is called before the first frame update
    public void Init()
    {
        lobbyCom = GetComponentInParent<Lobby>();
        exitBtn = gameObject.transform.Find("ExitBtn").GetComponent<Button>();
        exitBtn.onClick.AddListener(ExitClick);
    }
    public void ExitClick()
    {
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Click, "Click");
        UserData.state = State.HOME;
        lobbyCom.ClickSetting();
    }
}
