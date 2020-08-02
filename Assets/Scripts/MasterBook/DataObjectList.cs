using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class DataObjectList<T> : ScriptableObject
        where T : ScriptableObject, IMasterBookData
    {
    public List<T> dataObjectList;
    }




