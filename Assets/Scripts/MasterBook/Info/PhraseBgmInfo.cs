[System.Serializable]
public class PhraseBgmInfo
{
    [CustomId(nameof(BgmData))]
    public long bgmId;
    public int[] phraseIndexs;
}
