using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Home : MonoBehaviour//Lobby//
{
    LobbyCard lobbyCard;
    UserSetting userSetting;
    CardSetting cardSetting;
    Shop shop;
    Lobby lobby;
    Fade fade;
    Loading loading;

    Button cardBtn;
    Button settingBtn;
    Button shopBtn;
    Button gameStart;
    Button option;

    GameObject btnGroup;
    // Start is called before the first frame update
    //public void Init()
    public void Init()
    {
        // 퀘스트 세팅

        // 버튼 이벤트 설정
        lobby = GetComponentInParent<Lobby>();

        btnGroup = gameObject.transform.Find("BtnGroup").gameObject;

        cardBtn = btnGroup.transform.Find("Card").GetComponent<Button>();
        cardBtn.onClick.AddListener(lobby.Card);

        settingBtn = btnGroup.transform.Find("CardSetting").GetComponent<Button>();
        settingBtn.onClick.AddListener(lobby.Setting);

        shopBtn = btnGroup.transform.Find("Shop").GetComponent<Button>();
        shopBtn.onClick.AddListener(lobby.Shop);

        gameStart = gameObject.transform.Find("GameStart").GetComponent<Button>();//btnGroup.transform.Find("GameStart").GetComponent<Button>();
        gameStart.onClick.AddListener(GameStart);

        option = gameObject.transform.Find("Setting").GetComponent<Button>();//btnGroup.transform.Find("Setting").GetComponent<Button>();
        option.onClick.AddListener(lobby.Option);

        loading = GetComponentInChildren<Loading>(true);
        loading.Init();

        fade = GameObject.FindObjectOfType<Fade>();
        if (fade != null)
            fade.FadeIn();
    }
    public void GameStart()
    {
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Click, "Click");

        loading.AniPlay();

        StartCoroutine(IENextScene());

        //TCPClient.Instance.SendPack(GameProtocolType.Ready,UserData.uniqueID);
    }
    IEnumerator IENextScene()
    {
        //yield return new WaitForSeconds(1);

        //fade.FadeOut();

        yield return new WaitForSeconds(1);

        AsyncOperation asyn = SceneManager.LoadSceneAsync("GameScene");
        gameObject.SetActive(false);
        while (!asyn.isDone)
        {
            //asyn.allowSceneActivation = false;
            yield return null;
        }

    }
}
