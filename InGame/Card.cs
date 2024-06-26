using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.AI;
using Unity.VisualScripting;
using System;
using BackEnd.Socketio;

public class Card : MonoBehaviour, IPointerDownHandler,IPointerUpHandler, IEndDragHandler, IDragHandler
{
    // 카드 데이터 받아 오기
    // 코스트, 타입, 공격력, 피, 이름
    string cardName;
    int getNum;

    CardInfo cardinfo;
    Image charImg;
    CardGroup cardGroup;

    public void OnDrag(PointerEventData eventData)
    {
        /*Vector2 dir = eventData.position - (Vector2)cardGroup.GetCursor().transform.position;
        cardGroup.GetCursor().transform.up = dir.normalized;*/
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
    }
    public CardInfo GetCardInfo()
    {
        return cardinfo;
    }
    public void ResetCardInfo()
    {
        cardinfo = null;
    }
    void BtnClick()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int layerMask = 1 << LayerMask.NameToLayer("TEAMLOAD");
        
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            string monName = "";
            switch (getNum)
            {
                case 1:
                    monName = cardName + "_Small";
                    break;
                case 2:
                    monName = cardName + "_Medium";
                    break;
                case 3:
                    monName = cardName + "_Big";
                    break;
            }
            /*Player monObj = Instantiate<Player>(Resources.Load<Player>("Prefabs/Monster/" + monName));
            monObj.Init();*/
            GameManager.Instance.CreateHero(hit.transform.gameObject.tag, monName, UserData.team);
            GameManager.Instance.ResetColor(hit.transform.gameObject.tag);
            cardGroup.UseCard();
        }
        getNum = 0;
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
        /*cardGroup.CursorSet(eventData);
        Vector2 dir = eventData.position - (Vector2)cardGroup.GetCursor().transform.position;
        cardGroup.GetCursor().transform.up = dir.normalized;*/
        getNum = cardGroup.SelectCard(this);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        cardGroup.DeSelect();
    }
}
