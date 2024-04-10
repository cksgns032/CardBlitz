using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
    TMP_Text timer;
    GameProfile myProfile;
    GameProfile enemyProfile;
    Text resultTxt;
    Animation resultAni;
    CardGroup cardGroup;

    Team enemyColor;
    // Start is called before the first frame update
    public void Init()
    {
        // ��� �ִϸ��̼�
        resultTxt = gameObject.transform.Find("ResultText").GetComponent<Text>();
        resultTxt.gameObject.SetActive(false);
        resultAni = resultTxt.gameObject.GetComponent<Animation>();

        // �� ����
        myProfile = gameObject.transform.Find("MyProfile").GetComponent<GameProfile>();
        myProfile.Init();
        myProfile.GetColor(UserData.team);

        // �� �� ����
        if (UserData.team == Team.Red)
            enemyColor = Team.Blue;
        else
            enemyColor = Team.Red;

        // �� ����
        enemyProfile = gameObject.transform.Find("EnemyProfile").GetComponent<GameProfile>();
        enemyProfile.Init();
        enemyProfile.GetColor(enemyColor);

        cardGroup = gameObject.GetComponentInChildren<CardGroup>(true);
        cardGroup.Init();
    }
    //Ÿ�̸�
    public void SetTime(float timeNum)
    {
        timer = transform.Find("Timer").GetComponent<TMP_Text>();
        string str1 = string.Format("{0:00}", (int)(timeNum / 60 % 60));
        string str2 = string.Format("{0:00}", (int)(timeNum % 60));
        timer.text = str1 + ":" + str2;
    }
    public void TimeUpdate(float num)
    {
        if (num <= 0)
        {
            return;
        }
        //num -= Time.deltaTime;
        string str1 = string.Format("{0:00}", (int)(num / 60 % 60));
        string str2 = string.Format("{0:00}", (int)(num % 60));
        timer.text = str1 + ":" + str2;
        if (num == 60)
        {
            GameManager.Instance.SpeedUp();
        }
    }
    // ��� ��Ÿ��
    public void Result(RESULT result)
    {
        resultTxt.gameObject.SetActive(true);
        resultTxt.color = new Color(255, 97, 97, 255);

        switch (result)
        {
            case RESULT.WIN:
                resultTxt.text = "WIN!!";
                resultAni.Play("ResultWin");
                break;
            case RESULT.LOSE:
                resultTxt.text = "Lose..";
                resultAni.Play("ResultLose");
                break;
            case RESULT.DRAW:
                resultTxt.text = "Draw";
                resultAni.Play("ResultDraw");
                break;
        }
        Invoke("LobbyGo", 5000);
    }
    public void LobbyGo()
    {

    }
}
