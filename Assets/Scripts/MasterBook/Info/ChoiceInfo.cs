[System.Serializable]
public class ChoiceInfo
{
    public string selectionMessage;
    [CustomId(nameof(CharacterData))]
    public long targetCharacterId;
    public int updradeRating;
}
