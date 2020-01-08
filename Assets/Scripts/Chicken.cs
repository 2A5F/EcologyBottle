using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using static Utils;

public class Chicken : MonoBehaviour
{
    [Range(0, 1000)]
    public float 生命 = 1000;
    [Range(0, 1000)]
    public float 饥饿 = 1000;

    [Space]
    public AnimationCurve 概率增加 = new AnimationCurve(new Keyframe(0f, 0f, 0f, 0f), new Keyframe(1f, 1f, 3f, 0f));

    [Space]
    [Tooltip("越大越难扣")]
    public uint 扣饥饿随机尝试次数 = 100;
    public uint 最大扣饥饿时常 = 10000;
    public uint 当前饥饿时长 = 0;
    public Vector2 扣饥饿随机范围 = new Vector2(50, 100);
    public uint 寻食尝试次数 = 100;

    [Space]
    public float 一口能吃 = 100;

    [Space]
    public float 回血基数 = 100;

    [Space]
    public float 随机行走范围 = 5;
    public Vector2 随机行走时长 = new Vector2(500, 3000);
    public uint 随机行走计时 = 0;
    public uint 下一个随机行走时长 = 0;

    [Space]
    public Finder 嘴查找器;
    public Finder 嗅觉查找器;
    public Finder 视觉查找器;

    [Space]
    public NavMeshAgent agent;
    public float 停止距离 = 0.1f;

    [Space]
    public Effect 死亡粒子效果;

    void Start()
    {

    }

    bool destroy = false;
    bool died = false; 
    bool FindFood = false;
    void FixedUpdate()
    {
        if (destroy) Destroy(gameObject);
        if (died || destroy) return;
        当前饥饿时长++;
        if (当前饥饿时长 > 最大扣饥饿时常) 当前饥饿时长 = 0;
        var 饥饿平均 = (((float)当前饥饿时长 / (float)最大扣饥饿时常) + (1 - (生命 / 1000))) / 2;
        var 饥饿概率 = 概率增加.Evaluate(饥饿平均);  
        if (1.RangeTo(Mathf.RoundToInt(扣饥饿随机尝试次数 * 饥饿概率)).Select(_ => new System.Random().NextDouble() <= 饥饿概率).AllTrue())
        {
            var 扣饥饿 = UnityEngine.Random.Range(扣饥饿随机范围.x, 扣饥饿随机范围.y);
            饥饿 -= 扣饥饿;
            if(饥饿 <= 0)
            {
                饥饿 = 0;
                生命 -= 扣饥饿;
            }
            当前饥饿时长 = 0;
        }

        var 寻食概率 = 概率增加.Evaluate(1 - (饥饿 / 1000));
        if (1.RangeTo(Mathf.RoundToInt(寻食尝试次数 * 寻食概率)).Select(_ => new System.Random().NextDouble() <= 寻食概率).AllTrue())
        {
            Debug.Log("饿了");
            if (嗅觉查找器.Find<Food>(out var food))
            {
                FindFood = true;
                随机行走计时 = 下一个随机行走时长;
                Debug.Log($"找到食物 {food.transform.position}");
                agent.SetDestination(food.transform.position);
            }
        }

        if (!FindFood)
        {
            随机行走计时++;
            if (随机行走计时 >= 下一个随机行走时长)
            {
                随机行走计时 = 0;
                下一个随机行走时长 = Convert.ToUInt32(Mathf.RoundToInt(UnityEngine.Random.Range(随机行走时长.x, 随机行走时长.y)));
                agent.SetDestination(transform.position - UnityEngine.Random.insideUnitCircle.Y() * 5);
            }
        }
        else
        {
            if (Mathf.Abs(Vector3.Distance(agent.destination, transform.position)) <= 停止距离)
            {
                agent.isStopped = true;
                agent.isStopped = false;
                FindFood = false;
                if (嘴查找器.Find<Food>(out var food))
                {
                    饥饿 += food.Eaten(Mathf.Min(一口能吃, 1000 - 饥饿));
                }
            }
        }

        if (生命 < 0)
        {
            died = true;
            agent.isStopped = true;
            Instantiate(死亡粒子效果, transform.position, transform.rotation).Then(() =>
            {
                destroy = true;
            });
        }
        if (生命 < 1000)
        {
            var 饥饿比值 = 饥饿 / 1000;
            var 回血概率 = 概率增加.Evaluate(饥饿比值);
            var 回血值 = 回血概率 * 回血基数;
            生命 += 回血值;
            if (生命 > 1000) 生命 = 1000;
        }
    }

    private void OnDrawGizmos()
    {
        if(agent.hasPath)
        {
            var p = agent.path;
            p.corners.Aggregate(p.corners[0], (last, now) => {
                Gizmos.DrawLine(last, now);
                return now;
            });
        }
    }
}
