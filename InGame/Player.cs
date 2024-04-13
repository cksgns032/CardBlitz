using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public struct Hero
{
    public int hp;// 피
    public int defence;// 방어력
    public float moveSpeed;// 이동속도
    public float attackSpeed;// 공격속도
    public float attackRange;// 공격범위
    public int attackCnt;// 공격가능 수
    public float attack;// 공격력
    public float criPercent;// 크리티컬 확률
    public float criAdd;// 크리티컬 데미지 곱셈
}

public class Player : MonoBehaviour
{
    GameObject damageObj;

    CapsuleCollider colder;
    NavMeshAgent agent;
    Rigidbody rigid;
    Animator ani;
    List<Player> enemyList = new List<Player>();
    EventButton btnEvent;
    BoxCollider attackRange;
    Hero info = new Hero();

    bool isDie = false;
    bool setInit = false;// 초기화 여부
    int layerInt;

    public void Init()
    {
        Debug.Log(gameObject.name);
        string size = gameObject.name.Split("_")[1].Split("(")[0];
        damageObj = transform.Find("DamagePos").gameObject;
        switch(size)
        {
            case "Small":
                damageObj.transform.position = new Vector3(damageObj.transform.position.x,6, damageObj.transform.position.z);
                break;
            case "Medium":
                damageObj.transform.position = new Vector3(damageObj.transform.position.x, 5, damageObj.transform.position.z);
                break;
            case "Big":
                damageObj.transform.position = new Vector3(damageObj.transform.position.x, 4, damageObj.transform.position.z);
                break;
        }
        info.hp = 100;
        info.attack = 100;
        info.attackCnt = 1;
        setInit = true;
        colder = GetComponent<CapsuleCollider>();
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
        ani = GetComponentInChildren<Animator>();
        if(ani != null)ani.enabled = true;
        attackRange = GetComponentInChildren<BoxCollider>(true);
        rigid = GetComponent<Rigidbody>();
        rigid.isKinematic = true;
    }

    // 이게 문제가 있음
    public void AgentMaskSet(string type,Team team)
    {
        int areaNum;
        switch (type)
        {
            case "TOP":
                areaNum = NavMesh.GetAreaFromName("TOP");
                agent.areaMask = 1 << areaNum;
                if(UserData.team == team)
                    transform.position = GameObject.FindGameObjectWithTag("TOPSPAWN").transform.position;
                else
                    transform.position = GameObject.FindGameObjectWithTag("ETOPSPAWN").transform.position;
                break;
            case "MIDDLE":
                areaNum = NavMesh.GetAreaFromName("MIDDLE");
                agent.areaMask = 1 << areaNum;
                if (UserData.team == team)
                    transform.position = GameObject.FindGameObjectWithTag("MIDDLESPAWN").transform.position;
                else
                    transform.position = GameObject.FindGameObjectWithTag("EMIDDLESPAWN").transform.position;

                break;
            case "BOTTOM":
                areaNum = NavMesh.GetAreaFromName("BOTTOM");
                agent.areaMask = 1 << areaNum;
                if (UserData.team == team)
                    transform.position = GameObject.FindGameObjectWithTag("BOTTOMSPAWN").transform.position;
                else
                    transform.position = GameObject.FindGameObjectWithTag("EBOTTOMSPAWN").transform.position;
                break;
        }
        agent.enabled = true;
        rigid.isKinematic = false;
        Debug.Log(gameObject.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "EVENTBUTTON")
        {
            btnEvent = other.gameObject.GetComponent<EventButton>();
            if(btnEvent.CheckState() == false && btnEvent.GetColor() != UserData.team)
            {
                btnEvent.Charging(true);
                agent.isStopped = true;
            }    
        }
        int heroLayer = LayerMask.NameToLayer("HERO");
        int enemyLayer = LayerMask.NameToLayer("ENEMY");
        
        if (gameObject.layer == heroLayer)
            layerInt = enemyLayer;
        else if (gameObject.layer == enemyLayer)
            layerInt = heroLayer;

        if (other.gameObject.layer == layerInt)
        {
            if (other.GetComponent<Player>() != null)
                enemyList.Add(other.GetComponent<Player>());
        }
        // 공격 사거리에 들어왔을 때
        Debug.Log(other.gameObject.name);
    }
    public void Attack()
    {
        Debug.Log("Attack");
        // 가까운 순으로 리스트를 정렬 후 
        enemyList.Sort(EnemySort);
        // 공격 수 만큼 for문을 돌려줘야함
        for (int i = 0; i < info.attackCnt; i++)
        {
            // 공격려을 넣어줘야함
            enemyList[i].Damage(info.attack);
        }
    }   
    int EnemySort(Player left, Player right)
    {
        float leftDis = Vector3.Distance(this.gameObject.transform.position, left.transform.position);
        float rightDis = Vector3.Distance(this.gameObject.transform.position, right.transform.position);
        if (leftDis > rightDis)
        {
            return -1;
        }
        else
        {
            return 1;
        }
    }
    public void Damage(float damage)
    {
        GameObject obj = PoolingManager.Instance.Pool.Get();
        DamageTxt txt = obj.GetComponent<DamageTxt>();
        if(txt != null)
        {
            txt.Setting(damageObj.transform.position,damage);
        }
        // 자기의 피가 닳아야 된다 damage만큼
        if(info.hp - damage > 0)
        {
            ani.SetTrigger("Hit");
        }
        // 만약 피가 0이하면 
        /*else
        {
            Die();
        }*/
    }
    public void Die()
    {
        isDie = true;
        for(int i = 0; i < enemyList.Count; i++)
        {
            if (enemyList[i] == gameObject)
            {
                enemyList.RemoveAt(i);
            }
        }
        ani.SetTrigger("Death");
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "EVENTBUTTON")
        {
            // 팀이 점령을 했는지 확인
            // 점령을 안 했으면 실행
            EventButton eventCom = other.gameObject.GetComponent<EventButton>();
            if(eventCom != null)
            {
                if(eventCom.GetColor() != UserData.team)
                {
                    if (btnEvent.ChargeImage(1 * Time.deltaTime, LayerMask.LayerToName(gameObject.layer)))
                    {
                        btnEvent.Charging(false);
                        Go();
                    }
                }
            }
        }
        if (other.gameObject.layer == layerInt)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            // 캐릭의 공격속도에 따라서 시간을 둬서 실행
            //ani.SetTrigger("Attack");
        }
    }
    void Go()
    {
        agent.isStopped = false;
        agent.velocity = Vector3.zero;
    }

    public bool GetState()
    {
        return isDie;
    }
    public void Destroy()
    {
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        if (setInit == false || isDie == true)
            return;
        if (agent.hasPath == false && agent.enabled == true)
        {
            //gameObject.layer = LayerMask.NameToLayer("HERO");
            if (gameObject.layer == LayerMask.NameToLayer("HERO"))
            {
                agent.SetDestination(GameObject.Find("EnemyGoal").transform.position);
            }
            else if (gameObject.layer == LayerMask.NameToLayer("ENEMY"))
            {
                agent.SetDestination(GameObject.Find("MyGoal").transform.position);
            }
        }
    }
}
