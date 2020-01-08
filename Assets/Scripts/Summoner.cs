using UnityEngine;
using System.Collections;

public class Summoner : MonoBehaviour
{
    public bool SummonOnStart = false;
    [Space]
    public GameObject Target;
    public uint count = 1;
    [Space]
    public Transform parent;
    void Start()
    {
        if (SummonOnStart) Summon();
    }

    void Summon()
    {
        for (int i = 0; i < count; i++)
        {
            if(parent == null) Instantiate(Target, transform.position, transform.rotation);
            else Instantiate(Target, parent);
        }
    }
}
