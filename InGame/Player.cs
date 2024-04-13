using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public struct Hero
{
    public int hp;// ��
    public int defence;// ����
    public float moveSpeed;// �̵��ӵ�
    public float attackSpeed;// ���ݼӵ�
    public float attackRange;// ���ݹ���
    public int attackCnt;// ���ݰ��� ��
    public float attack;// ���ݷ�
    public float criPercent;// ũ��Ƽ�� Ȯ��
    public float criAdd;// ũ��Ƽ�� ������ ����
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
    bool setInit = false;// �ʱ�ȭ ����
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

    // �̰� ������ ����
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
        // ���� ��Ÿ��� ������ ��
        Debug.Log(other.gameObject.name);
    }
    public void Attack()
    {
        Debug.Log("Attack");
        // ����� ������ ����Ʈ�� ���� �� 
        enemyList.Sort(EnemySort);
        // ���� �� ��ŭ for���� ���������
        for (int i = 0; i < info.attackCnt; i++)
        {
            // ���ݷ��� �־������
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
        // �ڱ��� �ǰ� ��ƾ� �ȴ� damage��ŭ
        if(info.hp - damage > 0)
        {
            ani.SetTrigger("Hit");
        }
        // ���� �ǰ� 0���ϸ� 
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
            // ���� ������ �ߴ��� Ȯ��
            // ������ �� ������ ����
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
            // ĳ���� ���ݼӵ��� ���� �ð��� �ּ� ����
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
