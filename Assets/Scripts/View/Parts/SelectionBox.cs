using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
public class SelectionBox : MonoBehaviour
{
    [SerializeField]
    private RectTransform _content;


    private Dictionary<long,UserCharacterRatingInfo> userCharacterRatingInfoDic;

    private Action<UserCharacterRatingInfo[]> reflectRatingInfosCallBack;
    public void Init(ChoiceInfo[] choiceInfos, Dictionary<long, UserCharacterRatingInfo> iniUserRatingInfos,Action<UserCharacterRatingInfo[]> onReflectRatingInfos)
    {
        userCharacterRatingInfoDic = iniUserRatingInfos;
        reflectRatingInfosCallBack = onReflectRatingInfos;

        ClearBox();
        foreach(var info in choiceInfos)
        {
            var choicePanel = ResourceManager.Instance.InstantiateViewParts<ChoicePanel>(_content);
            choicePanel.SetPanel(info, UpgradeRating);
        }
    }
    private void ClearBox()
    {
        if (_content.childCount > 0)
        {
            foreach (Transform child in _content)
            {
                Destroy(child.gameObject);
            }
        }

    }
    private void UpgradeRating(long characterId,int rating)
    {

        if (userCharacterRatingInfoDic.ContainsKey(characterId))
        {
            userCharacterRatingInfoDic[characterId].rating += rating;
        }
        else
        {
            userCharacterRatingInfoDic.Add(characterId, new UserCharacterRatingInfo() {
                characterId=characterId,
                rating=rating
            });
        }
        

        var ratingInfos=userCharacterRatingInfoDic.Select(dic => dic.Value).ToArray();

        reflectRatingInfosCallBack?.Invoke(ratingInfos);
    }

    private void OnDestroy()
    {
        reflectRatingInfosCallBack = null;
    }

}
