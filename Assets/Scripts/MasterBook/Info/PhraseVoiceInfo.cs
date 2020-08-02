[System.Serializable]
public class PhraseVoiceInfo
{

   
    [CustomId(nameof(VoiceData))]
    public long voiceId;
    public int[] phraseIndexs;
}
