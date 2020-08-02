using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData
{

    public long slotId { get; private set; }
    public long phraseId;
    public int phraseIndex;
    public long time;

    public UserCharacterRatingInfo[] characterRatingInfos;
    public long[] endingIds;
    public KeyValuePair<long,long> haveItem1;
    public KeyValuePair<long,long> haveItem2;
    public Dictionary<long,long> usedItemDict;

    public UserData(long slotId)
    {
        this.slotId = slotId;
        characterRatingInfos = new UserCharacterRatingInfo[] { };
        endingIds = new long[] { };
        phraseId = 1;
        haveItem1 = new KeyValuePair<long, long>();
        haveItem2 = new KeyValuePair<long, long>();
        usedItemDict = new Dictionary<long, long>();
            
    }
}
