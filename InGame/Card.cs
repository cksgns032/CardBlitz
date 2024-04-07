using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.AI;
using Unity.VisualScripting;

public class Card : MonoBehaviour, IPointerDownHandler,IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    // 카드 데이터 받아 오기
    // 코스트, 타입, 공격력, 피, 이름
    int cost;
    int type;
    int attack;
    int hp;
    string cardName;

    RawImage rawImg;
    Player GameObj;// 프리펩 오브젝트
    GameObject CharPos;
    NavMeshAgent agent;
    Animator ani;
    Rigidbody rigid;

    CardInfo cardinfo;
    Image charImg;
    CardGroup cardGroup;

    public void OnBeginDrag(PointerEventData eventData)
    {
        /*GameObj = Instantiate<Player>(Resources.Load<Player>("Prefabs/Monster/" + cardName));
        GameObj.enabled = false;
        //GameObj.GetComponent<Player>().enabled = false;
        agent = GameObj.GetComponent<NavMeshAgent>();
        agent.enabled = false;
        ani = GameObj.GetComponentInChildren<Animator>();
        if (ani != null) ani.enabled = false;
        rawImg.transform.position = eventData.position;
        GameObj.gameObject.transform.parent = CharPos.transform;
        GameObj.gameObject.transform.position = CharPos.transform.position;*/
    }

    public void OnDrag(PointerEventData eventData)
    {
        //rawImg.transform.position = eventData.position;
        SelectLine();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        BtnClick();
    }

    public void Setting(CardInfo cardInfo)
    {
        cardGroup = GetComponentInParent<CardGroup>();
        cardinfo = cardInfo;
        switch (cardinfo.id)
        {
            case "0":
                cardName = "Catcher";
                break;
            case "1":
                cardName = "Fishguard";
                break;
            case "2":
                cardName = "Monkeydong";
                break;
        }
        charImg = gameObject.transform.GetChild(0).GetComponent<Image>();
        Sprite imga = Resources.Load<Sprite>("Image/Charactor/" + cardName);
        charImg.sprite = imga;
        CharPos = GameObject.Find("CharPos");
        rawImg = GameObject.FindObjectOfType<RawImage>();
    }
    public CardInfo GetCardInfo()
    {
        return cardinfo;
    }
    void BtnClick()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int layerMask = 1 << LayerMask.NameToLayer("TEAMLOAD");
        
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            //GameManager.Instance.CreateHero(hit.transform.gameObject.tag, GameObj, UserData.team);
            GameManager.Instance.ResetColor(hit.transform.gameObject.tag);
            /*if (GameObj) GameObj.gameObject.transform.parent = null;
            GameObj.enabled = true;
            GameObj.Init();
            GameObj.AgentMaskSet(hit.transform.gameObject.tag,UserData.team);*/
        }
        else
        {
            //Destroy(GameObj.gameObject);
        }
    }
    void SelectLine()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int layerMask = 1 << LayerMask.NameToLayer("TEAMLOAD");
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            GameManager.Instance.HitLine(hit.transform.tag);
        }
        else
            GameManager.Instance.HitLine("NON");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        cardGroup.SelectCard(this);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        cardGroup.DeSelect();
    }
}
