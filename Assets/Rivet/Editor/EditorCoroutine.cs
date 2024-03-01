using System;
using System.Collections;
using UnityEditor;

public class EditorCoroutine
{
    public static EditorCoroutine Start(IEnumerator routine)
    {
        EditorCoroutine coroutine = new EditorCoroutine(routine);
        coroutine.Start();
        return coroutine;
    }

    readonly IEnumerator routine;
    EditorCoroutine(IEnumerator routine)
    {
        this.routine = routine;
    }

    void Start()
    {
        EditorApplication.update += Update;
    }
    public void Stop()
    {
        EditorApplication.update -= Update;
    }

    void Update()
    {
        if (!routine.MoveNext())
        {
            Stop();
        }
    }
}