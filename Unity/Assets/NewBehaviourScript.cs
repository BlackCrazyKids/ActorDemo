using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class NewBehaviourScript : MonoBehaviour {

    [ListDrawerSettings(HideAddButton = true, HideRemoveButton = true)]
    public List<DataStruct> Table = new List<DataStruct>();

    private void Awake()
    {
        for (int i = 0; i < 5; i++)
        {
            Table.Add(new DataStruct());
        }
    }

}

public class DataStruct : ScriptableObject
{
    public int _int;
    public long _long;
    public float _float;
    public double _double;
    public bool _bool;
    public string _string;
}
