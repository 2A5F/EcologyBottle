using UnityEngine;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using System;

public class Effect : MonoBehaviour
{
    public EffectStopAction[] stops;
    bool destory = false;
    void Start()
    {
        Task.WhenAll(stops.Select(s => s.GetTask())).ContinueWith(t =>
        {
            destory = true;
            whenStop?.Invoke();
        });
    }

    public event Action whenStop;

    public void Then(Action action)
    {
        whenStop += action;
    }

    void Update()
    {
        if (destory)
        {
            Destroy(gameObject);
        }
    } 
}