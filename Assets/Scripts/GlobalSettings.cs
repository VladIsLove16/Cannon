using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSettings : MonoBehaviour 
{
    public static GlobalSettings instance;
    [SerializeField]
    public List<KeyValuePair<string, float>> Sounds=new List<KeyValuePair<string,float>>();
    [SerializeField]
    public MyDict SoundVolumes = new MyDict();
    public GlobalSettings()
    {
        if(instance == null)
            instance=this;
    }

}
