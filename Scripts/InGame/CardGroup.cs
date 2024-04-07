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
        
        for(var i = 0; i < cards.Length; i++)
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

        for(int i = 0; i < cards.Length; i++)
        {
            if(obj.gameObject == cards[i].gameObject)
            {
                cards[i].gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(cards[i].gameObject.GetComponent<RectTransform>().localPosition.x,
                                                                       cards[i].gameObject.GetComponent<RectTransform>().localPosition.y + 60,
                                                                       cards[i].gameObject.GetComponent<RectTransform>().localPosition.z);
                bool isleft = false;
                bool isright = false;
                Debug.Log(obj.gameObject.name);
                // 좌 우 확인
                if (i-1>=0)
                {
                    CardInfo card = cards[i - 1].GetCardInfo();
                    if(obj.GetCardInfo().id == cards[i - 1].GetCardInfo().id)
                    {
                        cards[i - 1].gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(cards[i - 1].gameObject.GetComponent<RectTransform>().localPosition.x,
                                                                       cards[i - 1].gameObject.GetComponent<RectTransform>().localPosition.y + 60,
                                                                       cards[i - 1].gameObject.GetComponent<RectTransform>().localPosition.z);
                        select.Add(cards[i - 1]);
                    }
                    isleft = true;
                }
                if (i + 1 < 5)
                {
                    if (obj.GetCardInfo().id == cards[i + 1].GetCardInfo().id)
                    {
                        cards[i + 1].gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(cards[i + 1].gameObject.GetComponent<RectTransform>().localPosition.x,
                                                                       cards[i + 1].gameObject.GetComponent<RectTransform>().localPosition.y + 60,
                                                                       cards[i + 1].gameObject.GetComponent<RectTransform>().localPosition.z);
                        select.Add(cards[i + 1]);
                    }
                    isright = true;
                }
                // 우를 못 하거나
                if (isleft == false)
                {
                    if (cards[i + 2] != null)
                    {
                        CardInfo card = cards[i + 2].GetCardInfo();
                        if (obj.GetCardInfo().id == cards[i + 2].GetCardInfo().id)
                        {
                            cards[i + 2].gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(cards[i + 2].gameObject.GetComponent<RectTransform>().localPosition.x,
                                                                           cards[i + 2].gameObject.GetComponent<RectTransform>().localPosition.y + 60,
                                                                           cards[i + 2].gameObject.GetComponent<RectTransform>().localPosition.z);
                            select.Add(cards[i + 2]);
                        }
                        isleft = true;
                    }
                }
                // 우를 못 하거나
                if (isright == false) 
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
                        isright = true;
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
