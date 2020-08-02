using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmData : ScriptableObject, IMasterBookData
{
    [SerializeField]
    private long _id;
    [SerializeField]
    private string _memo;
    [SerializeField]
    private AudioClip _clip;
    public long Id { get { return _id; } set { _id = value; } }
    public string Memo { get { return _memo; } }
    public AudioClip AudioClip { get { return _clip; } }
}
