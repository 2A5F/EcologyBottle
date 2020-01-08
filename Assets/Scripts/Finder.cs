using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Finder : MonoBehaviour
{
    public float 半径 = 0.5f;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.6980f, 0.8745f, 0.6784f);
        Gizmos.DrawWireSphere(transform.position, 半径);
    }

    public bool Find<T>(out T t) where T : Component
    {
        var all = Physics.OverlapSphere(transform.position, 半径);
        var first = all.SelectMany(hit =>
        {
            if (hit.transform.TryGetComponent(out T c))
            {
                return new[] { c };
            }
            return new T[] { };
        }).FirstOrDefault();
        t = first;
        return first != null;
    }

    public void FindByEye<T>() where T : Component
    {

    }

    ConditionalWeakTable<Type, GameObject> objs = new ConditionalWeakTable<Type, GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}
