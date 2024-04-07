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
    // ���� ������
    public static int uniqueID;// ���� ���̵�
    public static string Name;// �г���
    public static string Thumbnail;// �����
    public static int Level;// ����
    public static int Gold;// ���
    public static int Gem;// ��
    public static float soundVolume;// ����Ʈ �Ҹ�
    public static float bgmVolume;// ��� �Ҹ�
    // �ΰ��� 
    public static Team team;// ������ , �Ķ���
    public static State state;
    public static List<CardInfo> gameDeck = new List<CardInfo>();
}
