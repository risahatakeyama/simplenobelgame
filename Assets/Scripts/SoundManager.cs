using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SingletonMonobihavior<SoundManager>
{
    private AudioSource _bgmSource;

    private AudioSource _seSource;

    private AudioSource _voiceSource;

    public void Init(AudioSource bgm,AudioSource se,AudioSource voice)
    {
        _bgmSource = bgm;
        _seSource = se;
        _voiceSource = voice;

    }

    public void PlayBgm(BgmType bgmType)
    {
        PlayBgm((long)bgmType);
    }
    public void PlayBgm(long bgmId)
    {
        var bgmData = ResourceManager.Instance.LoadMasterData<BgmData, BgmDataObjectList>(bgmId);
        if (bgmData == null)
        {
            return;
        }
        if (_bgmSource.clip == bgmData.AudioClip)
        {
            return;
        }
        _bgmSource.clip = bgmData.AudioClip;
        _bgmSource.Play();
    }
    public void PlaySe(SeType seType)
    {
        PlaySe((long)seType);
    }
    public void PlaySe(long seId)
    {

        var seData = ResourceManager.Instance.LoadMasterData<SeData, SeDataObjectList>(seId);
        if (seData == null)
        {
            return;
        }

        _seSource.PlayOneShot(seData.AudioClip);
    }

    public void PlayVoice(long voiceId)
    {
        var voiceData = ResourceManager.Instance.LoadMasterData<VoiceData, VoiceDataObjectList>(voiceId);
        if (voiceData == null)
        {
            return;
        }
        _voiceSource.PlayOneShot(voiceData.AudioClip);
    }
    public void StopVoice()
    {
        _voiceSource.Stop();
    }

    
}
