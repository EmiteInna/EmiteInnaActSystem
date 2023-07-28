using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DebugAbout : MonoBehaviour
{
    CancellationTokenSource cts;
    void Start()
    {
        cts = new CancellationTokenSource();
        var task = UniTask.RunOnThreadPool(() => xx(cts.Token));
    }
    async UniTask xx(CancellationToken cancellation)
    {
        var task = zz();
        await task;
        var task2 = UniTask.RunOnThreadPool(() => yy(cancellation));
        while (true)
        {
            await UniTask.Yield(cancellation);
            transform.position = transform.position + transform.forward * 0.01f;
        }
    }
    async UniTask yy(CancellationToken cancellation)
    {
        while (true)
        {
            await UniTask.Yield(cancellation);
            transform.position = transform.position + transform.right * 0.01f;
        }
    }
    async UniTask zz()
    {
        await UniTask.Delay(10000,true);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Pressed fuck");
            cts.Cancel();
        }
    }
}
