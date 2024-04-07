using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSetting : MonoBehaviour
{
    Button exitBtn;
    Lobby lobby;

    // Start is called before the first frame update
    public void Init()
    {
        lobby = GetComponentInParent<Lobby>();
        exitBtn = gameObject.transform.Find("ExitBtn").GetComponent<Button>();
        exitBtn.onClick.AddListener(ExitClick);
    }
    public void Setting()
    {
        // ������ ������ �ִ� ī�� ����
        // ������ ����ϰ� �ִ� ī�� ����
    }
    public void ExitClick()
    {
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Click, "Click");
        UserData.state = State.HOME;
        lobby.ClickSetting();
    }
}
