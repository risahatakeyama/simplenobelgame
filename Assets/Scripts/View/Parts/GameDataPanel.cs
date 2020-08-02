using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class GameDataPanel : MonoBehaviour
{
    [SerializeField]
    private Image _imageScreenShot;
    [SerializeField]
    private Text _textSlot;
    [SerializeField]
    private Text _textTime;
    [SerializeField]
    private Text _textStory;
    [SerializeField]
    private Button _button;

    public void Set(UserData userData,Action<long> callBack)
    {

        _textSlot.text = $"Slot- {userData.slotId}";


        //unixTimeをなおす
        var slotDataTime=DateTimeOffset.FromUnixTimeSeconds(userData.time).LocalDateTime;
        _textTime.text = slotDataTime.ToString();

        //slotから保存されたスクショを読み込む
        var screenShot=ResourceManager.Instance.LoadScreenShot(userData.slotId.ToString());
        _imageScreenShot.sprite = screenShot;

        var phraseData=ResourceManager.Instance.LoadMasterData<PhraseData, PhraseDataObjectList>(userData.phraseId);
        var story=phraseData.Phrase[userData.phraseIndex];
        _textStory.text = story;

        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(delegate {
            callBack?.Invoke(userData.slotId);
        });
    }
    private void OnDestroy()
    {
        _button.onClick.RemoveAllListeners();
    }

}
