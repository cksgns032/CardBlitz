using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserSetting : MonoBehaviour
{
    Image thumbnail;
    Text nickname;
    Text level;
    Text gold;
    Text gem;

    // Start is called before the first frame update
    public void Init()
    {
        thumbnail = gameObject.transform.Find("Thumbnail").GetComponent<Image>();
        nickname = gameObject.transform.Find("NickName").GetComponent<Text>();
        level = gameObject.transform.Find("Level").GetComponent<Text>();
        gold = gameObject.transform.Find("Gold").GetComponent<Text>();
        gem = gameObject.transform.Find("Gem").GetComponent<Text>();
    }

    public void SetMode()
    {
        if(UserData.state == State.HOME || UserData.state == State.OPTION)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
