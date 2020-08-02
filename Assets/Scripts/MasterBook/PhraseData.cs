using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhraseData : ScriptableObject, IMasterBookData
{
    [SerializeField]
    private long _id;
    [SerializeField]
    private string _memo;
    [SerializeField]
    private PhraseBgmInfo[] _phraseBgmInfos;

    [SerializeField]
    public PhraseBackGroundInfo[] _phraseBackGroundInfos;

    [SerializeField]
    private DisplayCharacterInfo[] _displayCharacterInfos;
    [SerializeField]
    private PhraseCharacterNameInfo[] _characterNameInfos;

    [SerializeField]
    private PhraseVoiceInfo[] _phraseVoiceInfos;

    [SerializeField]
    private string[] _phrase;

    [SerializeField]
    private ChoiceInfo[] _choiceInfos;
    [SerializeField][CustomId(nameof(PhraseData))]
    private long _avoidanceNextPhraseId;
    [SerializeField]
    private NextPhraseInfo[] _nextPhraseInfos;
    [SerializeField][CustomId(nameof(EndingData))]
    private long _endingId;
    [SerializeField]
    [CustomId(nameof(EventData))]
    private long _eventId;

    public long Id { get { return _id; } set { _id = value; } }
    public string Memo { get { return _memo; } }

    public PhraseBgmInfo[] PhraseBgmInfos { get { return _phraseBgmInfos; } }
    public PhraseBackGroundInfo[] PhraseBackGroundInfos { get { return _phraseBackGroundInfos;  } }
    public DisplayCharacterInfo[] DisplayCharacterInfos { get { return _displayCharacterInfos; } }
    public PhraseCharacterNameInfo[] CharacterNameInfos { get { return _characterNameInfos; } }
    public string[] Phrase { get { return _phrase; } }
    public PhraseVoiceInfo[] PhraseVoiceInfos { get { return _phraseVoiceInfos; } }

    public ChoiceInfo[] ChoiceDatas { get { return _choiceInfos; } }
    //グラフからでないと設定できないようにしてもよいかも
    public long AvoidanceNextPhraseId { get { return _avoidanceNextPhraseId; } set { _avoidanceNextPhraseId = value; } }
    public NextPhraseInfo[] NextPhraseInfos { get { return _nextPhraseInfos; } set { _nextPhraseInfos = value; } }


    public long EndingId { get { return _endingId; } }
    public long EventId { get { return _eventId; } }
}
