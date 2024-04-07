using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lobby : MonoBehaviour
{
    Home home;
    LobbyCard lobbyCard;
    UserSetting userSetting;
    CardSetting cardSetting;
    Shop shop;
    Option option;

    // Start is called before the first frame update
    void Start()
    {
        int setWidth = 1920;
        int setHeight = 1080;
        float fixedAspect = setWidth / setHeight;

        int deviceWidth = Screen.width;
        int deviceHeight = Screen.height;
        float current = Screen.width / Screen.height;
        CanvasScaler canvas;
        canvas = gameObject.GetComponent<CanvasScaler>();

        if (current > fixedAspect)
            canvas.matchWidthOrHeight = 1;
        else
        {
            canvas.matchWidthOrHeight = 0;
        }

        AudioManager.Instance.LoadSound(AudioManager.Type.BGM, "TitleSound");
        AudioManager.Instance.PlayBgm(true, "TitleSound");
        AudioManager.Instance.LoadSound(AudioManager.Type.SFX, "Click");

        home = gameObject.GetComponentInChildren<Home>(true);
        home.Init();
        lobbyCard = gameObject.GetComponentInChildren<LobbyCard>(true);
        lobbyCard.Init();
        userSetting = gameObject.GetComponentInChildren<UserSetting>(true);
        userSetting.Init();
        cardSetting = gameObject.GetComponentInChildren<CardSetting>(true);
        cardSetting.Init();
        shop = gameObject.GetComponentInChildren<Shop>(true);
        shop.Init();    
        option = gameObject.GetComponentInChildren<Option>(true);
        option.Init();
        UserData.state = State.HOME;
        ClickSetting();
    }
    public void Card()
    {
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Click, "Click");
        UserData.state = State.CARD;
        ClickSetting();
    }
    public void Setting()
    {
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Click, "Click");
        UserData.state = State.SETTING;
        ClickSetting();
    }
    public void Shop()
    {
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Click, "Click");
        UserData.state = State.SHOP;
        ClickSetting();
    }
    public void Option()
    {
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Click, "Click");
        UserData.state = State.OPTION;
        ClickSetting();
    }
    public void Home()
    {
        UserData.state = State.HOME;
        ClickSetting();
    }
    public void ClickSetting()
    {
        switch (UserData.state)
        {
            case State.HOME:
                home.gameObject.SetActive(true);
                lobbyCard.gameObject.SetActive(false);
                cardSetting.gameObject.SetActive(false);
                shop.gameObject.SetActive(false);
                option.gameObject.SetActive(false);
                break;
            case State.CARD:
                home.gameObject.SetActive(false);
                lobbyCard.gameObject.SetActive(true);
                cardSetting.gameObject.SetActive(false);
                shop.gameObject.SetActive(false);
                option.gameObject.SetActive(false);
                lobbyCard.Setting();
                break;
            case State.SETTING:
                home.gameObject.SetActive(false);
                lobbyCard.gameObject.SetActive(false);
                cardSetting.gameObject.SetActive(true);
                shop.gameObject.SetActive(false);
                option.gameObject.SetActive(false);
                cardSetting.Setting();
                break;
            case State.SHOP:
                home.gameObject.SetActive(false);
                lobbyCard.gameObject.SetActive(false);
                cardSetting.gameObject.SetActive(false);
                shop.gameObject.SetActive(true);
                option.gameObject.SetActive(false);
                break;
            case State.OPTION:
                home.gameObject.SetActive(true);
                lobbyCard.gameObject.SetActive(false);
                cardSetting.gameObject.SetActive(false);
                shop.gameObject.SetActive(false);
                option.gameObject.SetActive(true);
                break;
        }
        userSetting.SetMode();
    }
}
