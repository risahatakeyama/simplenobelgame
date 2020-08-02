using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
public class MainStoryView : MonoBehaviour
{
    [SerializeField]
    private Button _buttonChangeSearch;
    [SerializeField]
    private GameObject _objStoryBox;
    [SerializeField]
    private Button _phraseReadingTapArea;
    [SerializeField]
    private GameObject _nameBox;
    [SerializeField]
    private Text _textSpeakingCharacterName;
    [SerializeField]
    private Text _textPhrase;

    [SerializeField]
    private SelectionBox _selectionBox;
    [SerializeField]
    private CharacterBox _characterBox;
    [SerializeField]
    private SearchBox _searchBox;
    [SerializeField]
    private MenuBox _menuBox;


    private UserData _userData;

    private PhraseData phraseData;
    private long currentPhraseId;
    private int phraseIndex;

    private Action<GameState, Dictionary<string, object>> _onChangeState;

    public void InitView(Action<GameState,Dictionary<string,object>> onChangeState,long phraseId=default(long),UserData userData=null)
    {
        if (userData != null)
        {
            _userData = userData;
        }
        else if(_userData==null)
        {

            _userData = new UserData(1);
        }

        currentPhraseId = _userData.phraseId;

        

        if (phraseId != default(long))
        {
            currentPhraseId = phraseId;
        }


        _onChangeState = onChangeState;
        phraseData = SetBeginDisplay(currentPhraseId);

        phraseIndex= _userData.phraseIndex;
        _menuBox.Init(onChangeState,GetCurrentUserData);
        UpdateDisplay();


        _phraseReadingTapArea.onClick.RemoveAllListeners();
        _phraseReadingTapArea.onClick.AddListener(()=> {

            SoundManager.Instance.PlaySe(SeType.Tap);

            ProceedStory();


        });

        _buttonChangeSearch.onClick.RemoveAllListeners();
        _buttonChangeSearch.onClick.AddListener(()=> {
            SoundManager.Instance.PlaySe(SeType.Tap);
            _searchBox.gameObject.SetActive(!_searchBox.gameObject.activeSelf);
            _objStoryBox.gameObject.SetActive(!_objStoryBox.activeSelf);
        });


    }

    /// <summary>
    /// 表示を更新する
    /// </summary>
    private void UpdateDisplay()
    {
        SetCharacterName(phraseData.CharacterNameInfos, phraseIndex);
        SetPhraseVoice(phraseData.PhraseVoiceInfos, phraseIndex);
        SetBackGround(phraseData.PhraseBackGroundInfos, phraseIndex);
        SetBgm(phraseData.PhraseBgmInfos, phraseIndex);

        _characterBox.SetCharacterBox(phraseData.DisplayCharacterInfos, phraseIndex);
        _textPhrase.text = phraseData.Phrase[phraseIndex];
    }

    /// <summary>
    /// ストーリを進める
    /// </summary>
    private void ProceedStory()
    {

        phraseIndex++;

        if (phraseData.Phrase.Length - 1 < phraseIndex)
        {
            phraseIndex = 0;

            //エンディングIdを持っていればエンディング移行する
            if (phraseData.EndingId != default(long))
            {
                _onChangeState(GameState.InitEnding, new Dictionary<string, object>() {
                        {"endingId",phraseData.EndingId },
                        {"userData",_userData}
                    });
                return;
            }else if (phraseData.EventId!=default(long))
            {
                //イベントIDを持っていればイベント移行する
                _onChangeState(GameState.InitEvent, new Dictionary<string, object>() {
                    {"eventId",phraseData.EventId }
                });
            }
            else
            {

                currentPhraseId = phraseData.AvoidanceNextPhraseId;
                currentPhraseId = SetNextPhraseId(currentPhraseId, phraseData.NextPhraseInfos);

                phraseData = SetBeginDisplay(currentPhraseId);
            }


        }
        else if (phraseData.Phrase.Length - 1 == phraseIndex)
        {
            SetChoiceBox(phraseData.ChoiceDatas);

        }
        UpdateDisplay();

    }

    private PhraseData SetBeginDisplay(long phraseId)
    {

        _selectionBox.gameObject.SetActive(false);

        var phraseData = ResourceManager.Instance.LoadMasterData<PhraseData, PhraseDataObjectList>(phraseId);

        return phraseData;
    }

    /// <summary>
    /// bgmを設定する
    /// </summary>
    /// <param name="phraseBgmInfos"></param>
    /// <param name="phraseIndex"></param>
    private void SetBgm(PhraseBgmInfo[] phraseBgmInfos,int phraseIndex)
    {
        var bgmInfo = phraseBgmInfos.FirstOrDefault(info => info.phraseIndexs.Contains(phraseIndex));
        if (bgmInfo != null)
        {
            SoundManager.Instance.PlayBgm(bgmInfo.bgmId);
        }
        else
        {
            if (phraseIndex == 0 && 
                phraseBgmInfos.Length > 0&&
                phraseBgmInfos[0].bgmId!=default(long)&&
                phraseBgmInfos[0].phraseIndexs.Length==0)
            {
                SoundManager.Instance.PlayBgm(phraseBgmInfos[0].bgmId);
            }
        }
    }

    /// <summary>
    /// 背景を設定する
    /// </summary>
    /// <param name="phraseBackGroundInfos"></param>
    /// <param name="phraseIndex"></param>
    private void SetBackGround(PhraseBackGroundInfo[] phraseBackGroundInfos,int phraseIndex)
    {

        var backGroundInfo = phraseBackGroundInfos.FirstOrDefault(info => info.phraseIndexs.Contains(phraseIndex));
        if (backGroundInfo != null)
        {

            SetBackGroundFromId(backGroundInfo.backGroundId);
        }
        else
        {
            if (phraseIndex == 0&&
                phraseBackGroundInfos.Length>0&& 
                phraseBackGroundInfos[0].backGroundId!=default(long)&&
                phraseBackGroundInfos[0].phraseIndexs.Length==0)
            {
                SetBackGroundFromId(phraseBackGroundInfos[0].backGroundId);
            }
        }
    }

    /// <summary>
    /// Idから背景を設定する
    /// </summary>
    /// <param name="backGoundId"></param>
    private void SetBackGroundFromId(long backGoundId)
    {

        _searchBox.SetSearchBox(backGoundId,_userData, UpdateHaveItems,UpdateUsedItem);

    }

    /// <summary>
    /// キャラクター名を設定する
    /// </summary>
    /// <param name="phraseCharacterNameInfos"></param>
    /// <param name="phraseIndex"></param>
    private void SetCharacterName(PhraseCharacterNameInfo[] phraseCharacterNameInfos,int phraseIndex)
    {
        var nameInfo = phraseCharacterNameInfos.FirstOrDefault(info=>info.phraseIndexs.Contains(phraseIndex));
        if (nameInfo != null)
        {
            SetCharacterNameFromId(nameInfo.characterId);
        }
        else
        {
            if (phraseIndex == 0 && phraseCharacterNameInfos.Length > 0)
            {
                SetCharacterNameFromId(phraseCharacterNameInfos[0].characterId);
            }
        }

    }

    /// <summary>
    /// Idからキャラクター名を設定する
    /// </summary>
    /// <param name="characterId"></param>
    private void SetCharacterNameFromId(long characterId)
    {
        var speakingCharacterData = ResourceManager.Instance.LoadMasterData<CharacterData, CharacterDataObjectList>(characterId);
        _nameBox.SetActive(speakingCharacterData != null);
        _textSpeakingCharacterName.text = speakingCharacterData != null ? speakingCharacterData.Name : string.Empty;
    }

    /// <summary>
    /// フレーズボイスを設定する
    /// </summary>
    /// <param name="phraseVoiceInfos"></param>
    /// <param name="phraseIndex"></param>
    private void SetPhraseVoice(PhraseVoiceInfo[] phraseVoiceInfos,int phraseIndex)
    {
        var voiceInfo = phraseVoiceInfos.FirstOrDefault(info => info.phraseIndexs.Contains(phraseIndex));
        if (voiceInfo != null)
        {
            SoundManager.Instance.PlayVoice(voiceInfo.voiceId);
        }
        else
        {
            SoundManager.Instance.StopVoice();
        }

    }

    /// <summary>
    /// 選択肢ボックスを設定する
    /// </summary>
    /// <param name="choiceInfos"></param>
    private void SetChoiceBox(ChoiceInfo[] choiceInfos)
    {
        if (choiceInfos.Length > 0)
        {

            _phraseReadingTapArea.gameObject.SetActive(false);
            var characterRatingDic = new Dictionary<long, UserCharacterRatingInfo>();
            foreach (var ratingInfo in _userData.characterRatingInfos)
            {
                characterRatingDic.Add(ratingInfo.characterId, ratingInfo);
            }
            _selectionBox.gameObject.SetActive(true);
            _selectionBox.Init(choiceInfos, characterRatingDic, SetUserRatingInfos);
        }

    }

    /// <summary>
    /// 次のフレーズIDを設定する
    /// </summary>
    /// <param name="currentPhraseId"></param>
    /// <param name="nextPhraseInfos"></param>
    /// <returns></returns>
    private long SetNextPhraseId(long currentPhraseId,NextPhraseInfo[] nextPhraseInfos)
    {

        if (nextPhraseInfos.Length > 0)
        {
            var maxRating = 
                _userData.characterRatingInfos
                .Where(info=> nextPhraseInfos.Select(nextInfo=>nextInfo.characterId).Contains(info.characterId))
                .Max(info => info.rating);

            var maxRatingCount =
                _userData.characterRatingInfos
                 .Where(info => nextPhraseInfos.Select(nextInfo => nextInfo.characterId).Contains(info.characterId))
                .Where(info => info.rating >= maxRating).Count();

            if (maxRatingCount == 1)
            {
                var targetRatingInfo = _userData.characterRatingInfos
                     .Where(info => nextPhraseInfos.Select(nextInfo => nextInfo.characterId).Contains(info.characterId))
                    .FirstOrDefault(info => info.rating >= maxRating);
                var nextPhraseInfo = nextPhraseInfos.FirstOrDefault(info => info.characterId == targetRatingInfo.characterId);
                currentPhraseId = nextPhraseInfo.phraseId;
            }
        }
        return currentPhraseId;
    }

    private void SetUserRatingInfos(UserCharacterRatingInfo[] characterRatingInfos)
    {
        _userData.characterRatingInfos = characterRatingInfos;
        _phraseReadingTapArea.gameObject.SetActive(true);

        ProceedStory();

    }

    private UserData UpdateHaveItems(KeyValuePair<long,long> haveItem1, KeyValuePair<long, long> haveItem2)
    {
        _userData.haveItem1 = haveItem1;
        _userData.haveItem2 = haveItem2;
        return _userData;
    }
    private UserData UpdateUsedItem(KeyValuePair<long, long> usedItem)
    {
        if (!_userData.usedItemDict.ContainsKey(usedItem.Key))
        {
            _userData.usedItemDict.Add(usedItem.Key,usedItem.Value);
        }
        
        return _userData;
    }
    private UserData GetCurrentUserData()
    {
        _userData.phraseId = currentPhraseId;
        _userData.phraseIndex = phraseIndex;
        return _userData;
    }
    private void OnDestroy()
    {
        _phraseReadingTapArea.onClick.RemoveAllListeners();
        _buttonChangeSearch.onClick.RemoveAllListeners();
        _onChangeState = null;
    }

}
