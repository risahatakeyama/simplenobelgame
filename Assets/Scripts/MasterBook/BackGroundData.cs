using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundData : ScriptableObject, IMasterBookData
{
    [SerializeField]
    private long _id;
    [SerializeField]
    private string _memo;
    [SerializeField]
    private Sprite[] _sprites=new Sprite[4];
    [SerializeField]
    private Color _spriteColor = Color.white;
    public long Id { get { return _id; } set { _id = value; } }
    public string Memo { get { return _memo; } }
    public Sprite[] Sprites { get { return _sprites; } }
    public Color SpriteColor { get { return _spriteColor; } }
}
