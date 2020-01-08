using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public bool 无限 = false;
    public float 内容 = 1000;
    public float 容量 = 1000;
    public float 回复速度 = 0.1f;

    public float 百分比 => 内容 / 容量;

    public float Eaten(float need)
    {
        if (无限) return need;
        内容 -= need;
        if(内容 < 0)
        {
            var 实际 = need + 内容;
            内容 = 0;
            return 实际;
        }
        return need;
    }

    virtual public void FixedUpdate()
    {
        内容 += 回复速度;
        if (内容 > 容量) 内容 = 容量;
    }
}
