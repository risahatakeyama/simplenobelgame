[System.Serializable]
public class PhraseBackGroundInfo
{
    [CustomId(nameof(BackGroundData))]
    public long backGroundId;
    public int[] phraseIndexs;
}
