using UnityEngine;
using System.Collections;

public class Test
{
    public Test(GameObject prefab)
    {
        Vector3 pos = Vector3.one;
        GameObject go = MonoBehaviour.Instantiate(Resources.Load("enemy", typeof(GameObject))) as GameObject;
       
    }


}
