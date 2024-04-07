using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CardGroup : MonoBehaviour
{
    Card[] cards;
    List<Card> select;
    // Start is called before the first frame update
    public void Init()
    {
        for(int i = 0; i < 3; i++)
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

    public void SelectCard(Card obj)
    {
        select = new List<Card>();
        select.Add(obj);
        obj.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(obj.gameObject.GetComponent<RectTransform>().localPosition.x,
                                                                       obj.gameObject.GetComponent<RectTransform>().localPosition.y + 60,
                                                                       obj.gameObject.GetComponent<RectTransform>().localPosition.z);

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
                    if(obj.GetCardInfo().id == cards[i - 1].GetCardInfo().id)
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
                    if (obj.GetCardInfo().id == cards[i + 1].GetCardInfo().id)
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
                        if (obj.GetCardInfo().id == cards[i + 2].GetCardInfo().id)
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
                        if (obj.GetCardInfo().id == cards[i - 2].GetCardInfo().id)
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
                            if (obj.GetCardInfo().id == cards[i - 2].GetCardInfo().id)
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
                            if (obj.GetCardInfo().id == cards[i + 2].GetCardInfo().id)
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
    }
    public void DeSelect()
    {
        for(int i = 0; i < select.Count; i++)
        {
            select[i].gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(select[i].gameObject.GetComponent<RectTransform>().localPosition.x,
                                                                           select[i].gameObject.GetComponent<RectTransform>().localPosition.y-60,
                                                                           select[i].gameObject.GetComponent<RectTransform>().localPosition.z);
        }
    }
}
