using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomIdAttribute : PropertyAttribute
{
    public string _dataName { get; private set; }
    public CustomIdAttribute(string dataName)
    {
        _dataName = dataName;
    }
}
