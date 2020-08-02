using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
public class EndingListView : MonoBehaviour
{
    [SerializeField]
    private Transform _panelContents;
    [SerializeField]
    private Button _buttonReturn;
    [SerializeField]
    private Transform _endingContents;

    private Action<GameState, Dictionary<string, object>> _onChangeState;
    private EndingView _endingView;
    private const string KEY_USER_DATA_LIST = "keyUserDataList";

    private List<EndingDataPanel> _endingDataPanelList=new List<EndingDataPanel>();
    public void InitView(Action<GameState, Dictionary<string, object>> onChangeState)
    {
        var endingDataList=ResourceManager.Instance.LoadMasterDataAll<EndingData,EndingDataObjectList>();
        endingDataList=endingDataList.OrderBy(data => data.EndingNumber).ToList();

        var unlockedEndingIdList = GetUnlockedEndingIdList();
        for(int i=0;i<endingDataList.Count;i++)
        {
            EndingDataPanel panel = null;
            if (_endingDataPanelList.Count > i)
            {
                panel = _endingDataPanelList[i];
            }
            else
            {
                panel= ResourceManager.Instance.InstantiateViewParts<EndingDataPanel>(_panelContents);
                _endingDataPanelList.Add(panel);
            }
            var isUnlock = unlockedEndingIdList.Contains(endingDataList[i].Id);

            panel.SetPanel(endingDataList[i],isUnlock,OnPanelTapped);
        }

        _buttonReturn.onClick.RemoveAllListeners();
        _buttonReturn.onClick.AddListener(delegate {
            gameObject.SetActive(false);
            onChangeState(GameState.UpdateTitle, null);
        });
        gameObject.SetActive(true);
    }
    private void OnPanelTapped(long endingId)
    {
        if (_endingView == null)
        {
            _endingView = ResourceManager.Instance.InstantiateView<EndingView>(_endingContents);
        }
        _endingView.gameObject.SetActive(true);
        _endingView.InitView(endingId, _onChangeState, OnEndingViewTapped);

    }
    private void OnEndingViewTapped()
    {
        _endingView.gameObject.SetActive(false);
    }
    /// <summary>
    /// 開放済みエンディングId一覧を取得する
    /// </summary>
    /// <returns></returns>
    private List<long> GetUnlockedEndingIdList()
    {
        var unlockedEndingIdList = new List<long>();
        var userDatas = PlayerPrefsUtil.GetValue<UserData[]>(KEY_USER_DATA_LIST);


        if (userDatas != null && userDatas.Length > 0)
        {
            foreach (var data in userDatas)
            {
                foreach (var id in data.endingIds)
                {
                    if (!unlockedEndingIdList.Contains(id))
                    {
                        unlockedEndingIdList.Add(id);
                    }
                }
            }
        }
        return unlockedEndingIdList;
    }
    private void OnDestroy()
    {
        _buttonReturn.onClick.RemoveAllListeners();
    }
}
