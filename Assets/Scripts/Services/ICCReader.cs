using UnityEngine;
using System.Collections;
using SimpleJSON;

public interface ICCReader   
{
    JSONClass GetCCInfo(string name, HeroType heroType);

}
