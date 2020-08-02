using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PhraseCharacterNameInfo 
{
    [CustomId(nameof(CharacterData))]
    public long characterId;
    public int[] phraseIndexs;
}
