using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingData : ScriptableObject, IMasterBookData
{
    [SerializeField]
    private long _id;
    [SerializeField]
    private string _memo;
    [SerializeField]
    private string _description;
    [SerializeField][CustomId(nameof(BackGroundData))]
    private long _backGroundId;
    [SerializeField]
    [CustomId(nameof(BgmData))]
    private long _bgmId;
    [SerializeField]
    private long _endingNumber;

    public long Id { get { return _id; } set { _id = value; } }
    public string Memo { get { return _memo; } }
    public string Description { get { return _description; } }
    public long BackGroundId { get { return _backGroundId; } }
    public long BgmId { get { return _bgmId; } }
    public long EndingNumber { get { return _endingNumber; } }
}
