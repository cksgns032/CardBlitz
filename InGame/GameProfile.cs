using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameProfile : MonoBehaviour
{
    [SerializeField] Image thumbnail;
    [SerializeField] TMP_Text nick;
    [SerializeField] Slider hp;
    [SerializeField] Slider gauge;
    [SerializeField] Text gaugeNum;
    int gaugeInt = 0;

    // Start is called before the first frame update
    public void Init()
    {
        thumbnail = GetComponentInChildren<Image>();
        nick = GetComponentInChildren<TMP_Text>();
        hp = gameObject.transform.Find("Hp").GetComponent<Slider>();
        hp.value = 1;
        gauge = gameObject.transform.Find("Gauge").GetComponent<Slider>();
        gauge.maxValue = 5;
        gauge.value = 0;
        gaugeNum = gauge.transform.Find("GaugeNume").GetComponent<Text>();
        gaugeNum.text = gaugeInt.ToString();
        GageFill();
    }
    public void GetColor(Team team)
    {
        if (team == Team.Red)
            thumbnail.color = Color.red;
        else
            thumbnail.color = Color.blue;
    }
    public void GageFill()
    {
        StartCoroutine(GaugeFill(5));
    }
    IEnumerator GaugeFill(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        gauge.value++;
        gaugeInt++;
        gaugeNum.text = gaugeInt.ToString();
        if (gauge.value < 5)
            StartCoroutine(GaugeFill(5));
    }
}
