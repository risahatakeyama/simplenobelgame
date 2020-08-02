using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
public class EndingView : MonoBehaviour
{

    [SerializeField]
    private Image _imageBackGround;
    [SerializeField]
    private Button _tapArea;
    [SerializeField]
    private Text _textEndName;
    private const string KEY_USER_DATA_LIST = "keyUserDataList";
    public void InitView(long endingId,Action<GameState,Dictionary<string,object>> onChangeState,Action tapCallBack=null,UserData userData=null)
    {
        var endingData=ResourceManager.Instance.LoadMasterData<EndingData, EndingDataObjectList>(endingId);
        if (endingData == null)
        {
            return;
        }
        var backGroundData = ResourceManager.Instance.LoadMasterData<BackGroundData, BackGroundDataObjectList>(endingData.BackGroundId);
        if (backGroundData != null)
        {
            _imageBackGround.sprite = backGroundData.Sprites[0];
        }
        

        if (endingData.BgmId != default(long))
        {
            SoundManager.Instance.PlayBgm(endingData.BgmId);
        }

        _textEndName.text = endingData.Memo;

        _tapArea.onClick.RemoveAllListeners();
        _tapArea.onClick.AddListener(()=> {
            if (tapCallBack != null)
            {
                tapCallBack.Invoke();
            }
            else
            {
                //現在読み込んでいるユーザーデータのendingIdsに保存する
                var userDatas=PlayerPrefsUtil.GetValue<UserData[]>(KEY_USER_DATA_LIST);
                List<UserData> userDataList;
                if (userDatas == null || userDatas.Length <= 0)
                {
                    userDataList = new List<UserData>();
                    for (int i = 0; i < 5; i++)
                    {
                        var index = i;
                        userDataList.Add(new UserData(index + 1));
                    }
                }
                else
                {
                    userDataList = userDatas.ToList();
                }

                for(int i = 0; i < userDataList.Count; i++)
                {
                    if (userDataList[i].slotId == userData.slotId)
                    {
                        var endingIdList = userDataList[i].endingIds.ToList();
                        if (!endingIdList.Contains(endingId))
                        {
                            endingIdList.Add(endingId);
                        }
                        userDataList[i].endingIds = endingIdList.ToArray();
                    }
                    
                }
                PlayerPrefsUtil.SetValue<UserData[]>(KEY_USER_DATA_LIST,userDataList.ToArray());
                onChangeState(GameState.InitTitle, new Dictionary<string, object>());
            }

        });

    }
    private void OnDestroy()
    {
        _tapArea.onClick.RemoveAllListeners();
    }
}
