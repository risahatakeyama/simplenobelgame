using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventData : ScriptableObject, IMasterBookData
{
    [SerializeField]
    private long _id;
    [SerializeField]
    private string _memo;
    [SerializeField]
    private MiniGameType _gameType;
    [SerializeField][CustomId(nameof(PhraseData))]
    private long _successPhraseId;
    [SerializeField]
    [CustomId(nameof(PhraseData))]
    private long _failedPhraseId;

    public long Id { get { return _id; } set { _id = value; } }
    public string Memo { get { return _memo; } }

    public MiniGameType GameType { get { return _gameType; } }
    public long SuccessPhraseId { get { return _successPhraseId; } }
    public long FailedPhraseId { get { return _failedPhraseId; } }
}
