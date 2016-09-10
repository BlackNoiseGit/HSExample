using UnityEngine;
using System.Collections;
using System;

public class DelayService : MonoBehaviour {

    public void DelayCallMethod(float delay, Action callback)
    {
        StartCoroutine(DelayCall(delay, callback));
    }

    private IEnumerator DelayCall(float delay, Action callback)
    {
        yield return new WaitForSeconds(delay);
        if (callback != null)
            callback();
    }
}
