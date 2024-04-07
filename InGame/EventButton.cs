using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Type
{
    TOP,// 생산속도 증가(아군)
    MIDDLE,// 이속 상승(아군)
    BOTTOM,// 방어력 감소(적)
}
public class EventButton : MonoBehaviour
{
    Image img;
    MeshRenderer mesh;

    bool charging = false;
    Team getColor = Team.None;
    public Type posType;
    // Start is called before the first frame update
    public void Init()
    {
        mesh = gameObject.transform.Find("Button 01").GetComponent<MeshRenderer>();
        img = gameObject.GetComponentInChildren<Canvas>(true).GetComponentInChildren<Image>(true);
        img.enabled = true;
        img.fillAmount = 0;
    }

    public bool ChargeImage(float num, string team)
    {
        img.fillAmount += num;
        if(img.fillAmount == 1)
        {
            switch(posType)
            {
                case Type.TOP:
                    // 코스트 생산속도 증가
                    break;
                case Type.MIDDLE:
                    // 아군 이속 상승
                    break;
                case Type.BOTTOM:
                    // 적군 방어력 감소
                    break;
            }
            switch(team)
            {
                case "ENEMY":
                    if(UserData.team == Team.Blue)
                    {
                        mesh.material = Resources.Load<Material>("Prefabs/Material/RedButton");
                        getColor = Team.Red;
                    }
                    else
                    {
                        mesh.material = Resources.Load<Material>("Prefabs/Material/BlueButton");
                        getColor = Team.Blue;

                    }
                    break;
                case "HERO":
                    if (UserData.team == Team.Blue)
                    {
                        mesh.material = Resources.Load<Material>("Prefabs/Material/BlueButton");
                        getColor = Team.Blue;
                    }
                    else
                    {
                        mesh.material = Resources.Load<Material>("Prefabs/Material/RedButton");
                        getColor = Team.Red;
                    }
                    break;
            }
            img.fillAmount = 0;
            img.enabled = false;
            return true;
        }
        return false;
    }
    public void Charging(bool state)
    {
        charging = state;
        if(state == false)
        {
            img.fillAmount = 0;
        }
    }
    public bool CheckState()
    {
        return charging;
    }
    public Team GetColor()
    {
        return getColor;
    }
}
