using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    HOME,
    CARD,
    SHOP,
    OPTION,
    SETTING,
    LOADING,
    GAME,
}
public enum Team
{
    Red = 0,
    Blue = 1,
    None = 2,
}
public static class UserData
{
    // 유저 데이터
    public static int uniqueID;// 고유 아이디
    public static string Name;// 닉네임
    public static string Thumbnail;// 썸네일
    public static int Level;// 레벨
    public static int Gold;// 골드
    public static int Gem;// 젬
    public static float soundVolume;// 이펙트 소리
    public static float bgmVolume;// 배경 소리
    // 인게임 
    public static Team team;// 빨간팀 , 파란팀
    public static State state;
    public static List<CardInfo> gameDeck = new List<CardInfo>();
}
