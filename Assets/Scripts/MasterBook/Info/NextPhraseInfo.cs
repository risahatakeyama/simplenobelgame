[System.Serializable]
public class NextPhraseInfo
{
    [CustomId(nameof(CharacterData))]
    public long characterId;
   [CustomId(nameof(PhraseData))]
    public long phraseId;
}
