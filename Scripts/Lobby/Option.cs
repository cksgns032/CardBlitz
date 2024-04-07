using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    Button close;
    Button exit;
    Slider bgmSlider;
    Slider soundSlider;

    Lobby lobby;
    // Start is called before the first frame update
    public void Init()
    {
        lobby = GetComponentInParent<Lobby>();
        close = gameObject.transform.Find("Close").GetComponent<Button>();
        close.onClick.AddListener(Close);
        exit = gameObject.transform.Find("ExitBtn").GetComponent<Button>();
        exit.onClick.AddListener(ExitClick);
        bgmSlider = gameObject.transform.Find("BGMSlider").GetComponent<Slider>();
        bgmSlider.value = UserData.bgmVolume;
        soundSlider = gameObject.transform.Find("SoundSlider").GetComponent<Slider>();
        soundSlider.value = UserData.soundVolume;
    }
    public void Close()
    {
        Application.Quit();
    }
    public void ExitClick()
    {
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Click, "Click");
        UserData.state = State.HOME;
        lobby.ClickSetting();
    }
    private void Update()
    {
        AudioManager.Instance.BGMVolume(bgmSlider.value);
        AudioManager.Instance.SoundVolume(soundSlider.value);
    }
}
