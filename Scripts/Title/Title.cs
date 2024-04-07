using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    Animation tap;
    Fade fade;
    Animation titleIdle;

    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(1280, 720, FullScreenMode.Windowed);

        AudioManager.Instance.Init();
        AudioManager.Instance.LoadSound(AudioManager.Type.BGM, "TitleSound");
        AudioManager.Instance.PlayBgm(true, "TitleSound");
        AudioManager.Instance.LoadSound(AudioManager.Type.SFX, "Click");
        //TCPClient.Instance.Init();
        tap = GetComponentInChildren<Animation>(true);
        tap.Play();

        titleIdle = GetComponent<Animation>();
        titleIdle.Play();

        if (fade == null) fade = Instantiate<Fade>(Resources.Load<Fade>("Prefabs/Fade"));
        fade.Init();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            AudioManager.Instance.PlaySfx(AudioManager.Sfx.Click, "Click");
            StartCoroutine(IENextScene());
        }
    }
    IEnumerator IENextScene()
    {
        //yield return new WaitForSeconds(1);

        fade.FadeOut();

        yield return new WaitForSeconds(1);

        AsyncOperation asyn = SceneManager.LoadSceneAsync("LobbyScene");
        gameObject.SetActive(false);
        while (!asyn.isDone)
        {
            //asyn.allowSceneActivation = false;
            yield return null;
        }
    }
}
