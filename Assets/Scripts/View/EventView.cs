using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class EventView : MonoBehaviour
{
    [SerializeField]
    private Transform _contents;

    private EventData _eventData;
    private Action<GameState, Dictionary<string, object>> _onChangeState;
    public void InitView(long eventId,Action<GameState, Dictionary<string, object>> onChangeState)
    {
        _onChangeState = onChangeState;
        //eventDataからゲームスクリプト読み込み
        _eventData = ResourceManager.Instance.LoadMasterData<EventData, EventDataObjectList>(eventId);

        switch (_eventData.GameType) {

            case MiniGameType.ChairGame:
                //四つの椅子　黄金の椅子　貴族の椅子、庶民の椅子、奴隷の椅子
                //掛け金
                InitEventPanel<ChairSelectionGame>(eventId);
                break;
            case MiniGameType.CardGame:
                break;
            case MiniGameType.TreasureGame:
                break;
            case MiniGameType.MirrorGame:
                break;
        }




    }

    private void InitEventPanel<TGame>(long eventId)where TGame : EventPanel
    {
        var eventPanel = ResourceManager.Instance.InstantiateEventPanel<TGame>(_contents, eventId);

        if (eventPanel != null)
        {
            eventPanel.InitPanel(OnGameEnd);
        }
    }
    private void OnGameEnd(bool isWin,bool isRelease)
    {
        long phraseId=isWin ? _eventData.SuccessPhraseId:_eventData.FailedPhraseId;


        //ゲーム解除フラグがあるか（特別なフラグ
        if (isRelease)
        {

        }


        _onChangeState(GameState.InitMain,new Dictionary<string,object>(){
            {"phraseId",phraseId }
        });
    }
    private void OnDestroy()
    {
        _onChangeState = null;
    }
}
