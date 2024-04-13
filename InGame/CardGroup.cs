using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.WSA;

public class CardGroup : MonoBehaviour
{
    Card[] cards;
    List<Card> select;
    //RectTransform cursorArrow;
    // Start is called before the first frame update
    public void Init()
    {
        //cursorArrow = transform.Find("CursorArrow").GetComponent<RectTransform>();
        //카드 생성
        for (int i = 0; i < 3; i++)
        {
            CardInfo cardInfo = new CardInfo();
            cardInfo.level = 1;
            cardInfo.id = i.ToString();
            UserData.gameDeck.Add(cardInfo);
        }
        cards = GetComponentsInChildren<Card>(true);

        Shuffle();
    }
    public void Shuffle()
    {
        for (var i = 0; i < cards.Length; i++)
        {
            if (cards[i].gameObject.active == false)
                cards[i].gameObject.SetActive(true);
            int num = UnityEngine.Random.Range(0, UserData.gameDeck.Count);
            cards[i].Setting(UserData.gameDeck[num]);
        }
    }
    // 카드 추가
    public void AddCard()
    {

    }
    // 카드 정렬
    public void SorCard()
    {

    }
    /*public void CursorSet(PointerEventData eventData)
    {
        Vector2 currentPos = eventData.position;
        cursorArrow.transform.position = currentPos;
    }
    public RectTransform GetCursor() 
    {
        return cursorArrow;
    }*/

    public int SelectCard(Card obj)
    {
        select = new List<Card>();
        select.Add(obj);
        obj.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(obj.gameObject.GetComponent<RectTransform>().localPosition.x,
                                                                       obj.gameObject.GetComponent<RectTransform>().localPosition.y + 60,
                                                                       obj.gameObject.GetComponent<RectTransform>().localPosition.z);
        //cursorArrow.anchoredPosition = obj.gameObject.GetComponent<RectTransform>().anchoredPosition;
        for (int i = 0; i < cards.Length; i++)
        {
            if(obj.gameObject == cards[i].gameObject)
            {
                // 왼쪽이 있냐
                bool isleft = false;
                // 오른쪽이 있냐
                bool isright = false;
                // 왼쪽에 같은게 있냐
                bool getleft = false;
                // 오른쪽에 같은게 있냐
                bool getright = false;

                Debug.Log(obj.gameObject.name);
                // 중앙 기준
                // 왼쪽 확인
                if (i-1>=0)
                {
                    //CardInfo card = cards[i - 1].GetCardInfo();
                    if (cards[i-1].GetCardInfo() != null && obj.GetCardInfo().id == cards[i - 1].GetCardInfo().id)
                    {
                        cards[i - 1].gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(cards[i - 1].gameObject.GetComponent<RectTransform>().localPosition.x,
                                                                       cards[i - 1].gameObject.GetComponent<RectTransform>().localPosition.y + 60,
                                                                       cards[i - 1].gameObject.GetComponent<RectTransform>().localPosition.z);
                        getleft = true;
                        select.Add(cards[i - 1]);
                    }
                    isleft = true;
                }
                // 오른쪽 확인
                if (i + 1 < 5)
                {
                    if (cards[i + 1].GetCardInfo() != null && obj.GetCardInfo().id == cards[i + 1].GetCardInfo().id)
                    {
                        cards[i + 1].gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(cards[i + 1].gameObject.GetComponent<RectTransform>().localPosition.x,
                                                                       cards[i + 1].gameObject.GetComponent<RectTransform>().localPosition.y + 60,
                                                                       cards[i + 1].gameObject.GetComponent<RectTransform>().localPosition.z);
                        getright = true;
                        select.Add(cards[i + 1]);
                    }
                    isright = true;
                }

                // 가생이 기준
                // 왼쪽이 없을 때
                if (isleft == false && isright == true && getright == true)
                {
                    if (cards[i + 2] != null)
                    {
                        //CardInfo card = cards[i + 2].GetCardInfo();
                        if (cards[i + 2].GetCardInfo() != null && obj.GetCardInfo().id == cards[i + 2].GetCardInfo().id)
                        {
                            cards[i + 2].gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(cards[i + 2].gameObject.GetComponent<RectTransform>().localPosition.x,
                                                                           cards[i + 2].gameObject.GetComponent<RectTransform>().localPosition.y + 60,
                                                                           cards[i + 2].gameObject.GetComponent<RectTransform>().localPosition.z);
                            select.Add(cards[i + 2]);
                        }
                    }
                }
                // 오른쪽이 없을 때
                else if (isright == false && isleft == true && getleft == true)
                {
                    if (cards[i - 2] != null)
                    {
                        if (cards[i - 2].GetCardInfo() != null && obj.GetCardInfo().id == cards[i - 2].GetCardInfo().id)
                        {
                            cards[i - 2].gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(cards[i - 2].gameObject.GetComponent<RectTransform>().localPosition.x,
                                                                           cards[i - 2].gameObject.GetComponent<RectTransform>().localPosition.y + 60,
                                                                           cards[i - 2].gameObject.GetComponent<RectTransform>().localPosition.z);
                            select.Add(cards[i - 2]);
                        }
                    }
                }
                // 두번째 구역까지
                if (select.Count < 3)
                {
                    if(getleft)
                    {
                        if(i-2 >= 0)
                        {
                            if (cards[i - 2].GetCardInfo() != null && obj.GetCardInfo().id == cards[i - 2].GetCardInfo().id)
                            {
                                cards[i - 2].gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(cards[i - 2].gameObject.GetComponent<RectTransform>().localPosition.x,
                                                                               cards[i - 2].gameObject.GetComponent<RectTransform>().localPosition.y + 60,
                                                                               cards[i - 2].gameObject.GetComponent<RectTransform>().localPosition.z);
                                select.Add(cards[i - 2]);
                            }
                        }
                    }
                    else if(getright)
                    {
                        if(i+2 < 5)
                        {
                            if (cards[i + 2].GetCardInfo() != null && obj.GetCardInfo().id == cards[i + 2].GetCardInfo().id)
                            {
                                cards[i + 2].gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(cards[i + 2].gameObject.GetComponent<RectTransform>().localPosition.x,
                                                                               cards[i + 2].gameObject.GetComponent<RectTransform>().localPosition.y + 60,
                                                                               cards[i + 2].gameObject.GetComponent<RectTransform>().localPosition.z);
                                select.Add(cards[i + 2]);
                            }
                        }
                    }
                }
            }
        }
        return select.Count;
    }
    public void DeSelect()
    {
        // 선택한 카드 삭제
        for (int i = 0; i < select.Count; i++)
        {
            select[i].gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(select[i].gameObject.GetComponent<RectTransform>().localPosition.x,
                                                                           select[i].gameObject.GetComponent<RectTransform>().localPosition.y - 60,
                                                                           select[i].gameObject.GetComponent<RectTransform>().localPosition.z);
        }
    }
    public void UseCard()
    {
        for (int k = 0; k < select.Count; k++)
        {
            select[k].gameObject.SetActive(false);
        }
        // 미사용 카드 정렬
        for (var j = 0; j < cards.Length - 1; j++)
        {
            if (cards[j].gameObject.active == true && cards[j + 1].gameObject.active == false)
            {
                cards[j + 1].gameObject.SetActive(true);
                cards[j + 1].Setting(cards[j].GetCardInfo());
                cards[j].gameObject.SetActive(false);
                cards[j].ResetCardInfo();
                for (int i = j; i >= 0; i--)
                {
                    if (cards[i].gameObject.active == true)
                    {
                        cards[i + 1].gameObject.SetActive(true);
                        cards[i + 1].Setting(cards[i].GetCardInfo());
                        cards[i].gameObject.SetActive(false);
                        cards[i].ResetCardInfo();
                    }
                }
                //j = -1;
            }
        }
    }
}
