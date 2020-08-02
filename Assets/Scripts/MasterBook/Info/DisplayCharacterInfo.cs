using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class DisplayCharacterInfo 
{
    
    [CustomId(nameof(CharacterData))]
    public long characterId;
    public Vector2 diaplayPos=new Vector2();
    public int[] phraseIndexs;
}
