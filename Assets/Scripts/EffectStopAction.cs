using System;
using System.Threading.Tasks;
using UnityEngine;

public class EffectStopAction : MonoBehaviour
{
    public bool IsStoped { get; private set; } = false;

    public event Action WhenStop;
    public Task GetTask()
    {
        if (IsStoped) return Task.FromResult(true);
        var t = new TaskCompletionSource<bool>();
        WhenStop += () =>
        {
            t.SetResult(true);
        };
        return t.Task;
    }

    public void Stop()
    {
        IsStoped = true;
        WhenStop?.Invoke();
    }
}