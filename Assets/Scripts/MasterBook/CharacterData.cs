using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData : ScriptableObject, IMasterBookData
{
    [SerializeField]
    private long _id;

    [SerializeField]
    private string _name;
    [SerializeField]
    private string _memo;
    [SerializeField]
    private Sprite _sprite;
    [SerializeField]
    private GameObject _objPrefab;

    public long Id { get { return _id; } set { _id = value; } }
    public string Memo { get { return _memo; } }
    public string Name { get { return _name; } }
    public Sprite Sprite { get { return _sprite; } }
    public GameObject ObjPrefab { get { return _objPrefab; } }
}
