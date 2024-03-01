using UnityEngine;
using System.Collections;

public class CoroutineUtility : MonoBehaviour
{
    public static CoroutineUtility instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Coroutine StartGlobalCoroutine(IEnumerator coroutine)
    {
        return StartCoroutine(coroutine);
    }
}