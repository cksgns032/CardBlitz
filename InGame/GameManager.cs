using BackEnd;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NavMeshBuilder = UnityEngine.AI.NavMeshBuilder;

public enum RESULT
{
    WIN,
    LOSE,
    DRAW,
}

public class GameManager : SingleTon<GameManager>
{
    // 유저의 카드 정보를 가져와서 랜덤한 5장을 선별 배포
    // 게임의 타이머 조절 
    // 게임 결과 나타냄

    int layerNum;
    List<GameObject> TopList = new List<GameObject>();
    List<GameObject> MiddleList = new List<GameObject>();
    List<GameObject> BottomList = new List<GameObject>();
    string currentTag = string.Empty;
    Color currentColor;
    GameObject map;
    GameUI gameUI;
    Fade fade;

    NavMeshData m_NavMesh;
    NavMeshDataInstance m_Instance;
    public Vector3 m_size = new Vector3(13, 1, 7);
    List<NavMeshBuildSource> m_Sources = new List<NavMeshBuildSource>();

    List<Player> allyList = new List<Player>();// 아군 오브젝트 리스트
    List<Player> enemyList = new List<Player>();// 적군 오브젝트 리스트

    bool isClaer = false;
    float timeNum = 65;

    // Start is called before the first frame update
    void Start()
    {
        var bro = Backend.Initialize(true); // 뒤끝 초기화

        // 뒤끝 초기화에 대한 응답값
        if (bro.IsSuccess())
        {
            Debug.Log("초기화 성공 : " + bro); // 성공일 경우 statusCode 204 Success
        }
        else
        {
            Debug.LogError("초기화 실패 : " + bro); // 실패일 경우 statusCode 400대 에러 발생
        }
        fade = GameObject.FindObjectOfType<Fade>();
        if (fade != null)
            fade.FadeIn();
        //AudioManager.Instance.LoadSound(AudioManager.Type.BGM,"BattleSound");
        //AudioManager.Instance.PlayBgm(true, "BattleSound");
        PoolingManager.Instance.Init();

        gameUI = GameObject.FindObjectOfType<GameUI>();
        gameUI.Init();
        gameUI.SetTime(timeNum);
        GameObject mapPos = GameObject.Find("MapPos"); 
        map = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Map"),mapPos.GetComponent<Transform>());
        for (int k = 0; k < map.GetComponentsInChildren<EventButton>().Length; k++)
        {
            map.GetComponentsInChildren<EventButton>()[k].Init();
        }
        m_NavMesh = new NavMeshData();
        m_Instance = NavMesh.AddNavMeshData(m_NavMesh);
        UpdateNavMesh(true);
        layerNum = LayerMask.NameToLayer("TEAMLOAD");
      
        for (int i = 0; i < map.transform.childCount; i++)
        {
            if(map.transform.GetChild(i).gameObject.layer == layerNum)
            {
                switch (map.transform.GetChild(i).gameObject.tag)
                {
                    case "TOP":
                        TopList.Add(map.transform.GetChild(i).gameObject);
                        break;
                    case "MIDDLE":
                        MiddleList.Add(map.transform.GetChild(i).gameObject);
                        break;
                    case "BOTTOM":
                        BottomList.Add(map.transform.GetChild(i).gameObject);
                        break;
                }
            }
        }
    }
    private void Update()
    {
        if (isClaer == false && timeNum > 0)
        {
            timeNum -= Time.deltaTime;
            gameUI.TimeUpdate(timeNum);
        }
        else if (isClaer == false && timeNum <= 0)
        {
            isClaer = true;
            gameUI.Result(RESULT.DRAW);
        }
    }
    // 게임 결과
    public void ResultGame(RESULT result)
    {
        gameUI.Result(result);
    }
    #region 맵생성
    public void UpdateNavMesh(bool navmesh)
    {
        AddSource(ref m_Sources);
        if (navmesh)
        {
            NavMeshBuilder.UpdateNavMeshDataAsync(m_NavMesh,
                                                  NavMesh.GetSettingsByID(0),
                                                  m_Sources,
                                                  QuantizedBounds());
        }
        else
        {
            NavMeshBuilder.UpdateNavMeshData(m_NavMesh,
                                             NavMesh.GetSettingsByID(0),
                                             m_Sources,
                                             QuantizedBounds());
        }
    }
    // 네브메쉬할 오브젝트 리스트
    void AddSource(ref List<NavMeshBuildSource> m_Sources)
    {
        for (int i = 0; i < map.transform.childCount; i++)
        {
            int layer1 = LayerMask.NameToLayer("ENEMYLOAD");
            int layer2 = LayerMask.NameToLayer("TEAMLOAD");
            int layer3 = LayerMask.NameToLayer("CENTER");
            if (map.transform.GetChild(i).gameObject.layer ==  layer1|| map.transform.GetChild(i).gameObject.layer == layer2 || map.transform.GetChild(i).gameObject.layer == layer3)
            {
                var mf = map.transform.GetChild(i).GetComponent<MeshFilter>();
                if (map.transform.GetChild(i).GetComponent<MeshFilter>() == null)
                    continue;

                var m = mf.sharedMesh;
                if (mf.sharedMesh == null)
                    continue;

                var s = new NavMeshBuildSource();
                s.shape = NavMeshBuildSourceShape.Mesh;
                s.sourceObject = m;
                s.transform = mf.transform.localToWorldMatrix;
                switch(map.transform.GetChild(i).gameObject.tag)
                {
                    case "TOP":
                        int areaNum = NavMesh.GetAreaFromName("TOP");
                        s.area = areaNum;
                        break;
                    case "MIDDLE":
                        areaNum = NavMesh.GetAreaFromName("MIDDLE");
                        s.area = areaNum;
                        break;
                    case "BOTTOM":
                        areaNum = NavMesh.GetAreaFromName("BOTTOM");
                        s.area = areaNum;
                        break;
                }
                
                m_Sources.Add(s);
            }
        }
    }
    // 베이크할 범위 설정
    Bounds QuantizedBounds()
    {
        var center = new Vector3(0, 0, 0);
        return new Bounds(Quantize(center, 0.1f * m_size), m_size);
    }
    static Vector3 Quantize(Vector3 v,Vector3 quant)
    {
        float x = quant.x * Mathf.Floor(v.x / quant.x);
        float y = quant.y * Mathf.Floor(v.y / quant.y);
        float z = quant.z * Mathf.Floor(v.z / quant.z);
        return new Vector3(x, y, z);
    }
    #endregion
    // layer = teamload
    //  tag  = objtag
    // 라인 선택
    public void HitLine(string objTag)
    {
        switch(objTag)
        {
            case "TOP":
                if (currentTag != string.Empty && currentTag != objTag)
                {
                    ResetColor(currentTag);
                }
                for (int i = 0; i < TopList.Count; i++)
                {
                    MeshRenderer mesh = TopList[i].GetComponent<MeshRenderer>();
                    if (currentColor == new Color(0, 0, 0, 0))
                        currentColor = mesh.material.color;
                    mesh.material.color = new Color(255, 255, 255);
                }
                currentTag = objTag;
                break;
            case "MIDDLE":
                if (currentTag != string.Empty && currentTag != objTag)
                {
                    ResetColor(currentTag);
                }
                for (int i = 0; i < MiddleList.Count; i++)
                {
                    MeshRenderer mesh = MiddleList[i].GetComponent<MeshRenderer>();
                    if (currentColor == new Color(0, 0, 0, 0))
                        currentColor = mesh.material.color;
                    mesh.material.color = new Color(255, 255, 255);
                }
                currentTag = objTag;
                break;
            case "BOTTOM":
                if (currentTag != string.Empty && currentTag != objTag)
                {
                    ResetColor(currentTag);
                }
                for (int i = 0; i < BottomList.Count; i++)
                {
                    MeshRenderer mesh = BottomList[i].GetComponent<MeshRenderer>();
                    if (currentColor == new Color(0,0,0,0))
                        currentColor = mesh.material.color;
                    mesh.material.color = new Color(255, 255, 255);
                }
                currentTag = objTag;
                break;
            case "NON":
                if (currentTag != objTag)
                {
                    ResetColor(currentTag);
                }
                break;
        }
        //TCPClient.Instance.SendPack(GameProtocolType.CREATEOBJ, objTag, "DarkNight", UserData.team);
    }
    // 바닥 색 변화
    public void ResetColor(string objtag)
    {
        switch (objtag)
        {
            case "TOP":
                for (int i = 0; i < TopList.Count; i++)
                {
                    MeshRenderer mesh = TopList[i].GetComponent<MeshRenderer>();
                    mesh.material.color = currentColor;
                }
                break;
            case "MIDDLE":
                for (int i = 0; i < MiddleList.Count; i++)
                {
                    MeshRenderer mesh = MiddleList[i].GetComponent<MeshRenderer>();
                    mesh.material.color = currentColor;
                }
                break;
            case "BOTTOM":
                for (int i = 0; i < BottomList.Count; i++)
                {
                    MeshRenderer mesh = BottomList[i].GetComponent<MeshRenderer>();
                    mesh.material.color = currentColor;
                }
                break;
            default:
                break;
        }
    }
    // 스폰위치에 유닛 소환
    public void CreateHero(string objTag,Player obj, Team team)
    {
        switch(objTag)
        {
            case "TOP":
                Vector3 pos = map.transform.Find("SpawnPos/TopPotal").transform.position;
                obj.gameObject.transform.position = new Vector3(pos.x, 1.2f, pos.z);
                break;
            case "MIDDLE":
                pos = map.transform.Find("SpawnPos/MiddlePotal").transform.position;
                obj.gameObject.transform.position = new Vector3(pos.x, 1.2f, pos.z);
                break;
            case "BOTTOM":
                pos = map.transform.Find("SpawnPos/BottomPotal").transform.position;
                obj.gameObject.transform.position = new Vector3(pos.x, 1.2f, pos.z);
                break;
        }
        obj.gameObject.layer = LayerMask.NameToLayer("HERO");
        // 오브젝트의 위치, 정보를 보냄
        TCPClient.Instance.CreateObj("DarkNight", objTag);

        if (team == UserData.team)// 내 팀이 소환을 했을 때
            allyList.Add(obj);
        else // 적 팀이 소환을 했을 때
            enemyList.Add(obj);
    }
    public void DieHero(string team, Player obj)
    {
        switch(team)
        {
            case "Red":
                for (int i = 0; i < allyList.Count; i++)
                {
                    if(obj == allyList[i])
                    {
                        allyList.Remove(allyList[i]);
                    }
                }
                break;
            case "Blue":
                for(int i = 0; i < enemyList.Count; i++)
                {
                    if (obj == enemyList[i])
                    {
                        enemyList.Remove(enemyList[i]);
                    }
                }
                break;
        }
    }
    // 이속 증가 버프
    public void SpeedUp()
    {
        List<Player> allObject = new List<Player>();
        allObject.AddRange(allyList);
        allObject.AddRange(enemyList);

        for (int i = 0; i < allObject.Count; i++)
        {
            allObject[i].GetComponent<NavMeshAgent>().speed *= 2f;
        }
    }
}
