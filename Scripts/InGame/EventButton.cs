using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Type
{
    TOP,// ����ӵ� ����(�Ʊ�)
    MIDDLE,// �̼� ���(�Ʊ�)
    BOTTOM,// ���� ����(��)
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
                    // �ڽ�Ʈ ����ӵ� ����
                    break;
                case Type.MIDDLE:
                    // �Ʊ� �̼� ���
                    break;
                case Type.BOTTOM:
                    // ���� ���� ����
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
