using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class CharacterBox : MonoBehaviour
{
    [SerializeField]
    private RectTransform _content;

    private List<CharacterPanel> _cacheCharacterPanelList=new List<CharacterPanel>();
    public void SetCharacterBox(DisplayCharacterInfo[] displayCharacterInfos,int phraseIndex)
    {
        var characterInfos = displayCharacterInfos.Where(info => info.phraseIndexs.Contains(phraseIndex)).ToList();
        if (characterInfos.Count > 0)
        {
            ClearBox();
            for(int i = 0; i < characterInfos.Count; i++)
            {
                CharacterPanel panel = null;
                if (_cacheCharacterPanelList.Count > i)
                {
                    panel = _cacheCharacterPanelList[i];
                }
                else
                {
                    panel= ResourceManager.Instance.InstantiateViewParts<CharacterPanel>(_content);
                    _cacheCharacterPanelList.Add(panel);
                }
                panel.SetPanel(characterInfos[i]);
            }

        }

    }
    private void ClearBox()
    {
        if (_content.childCount > 0)
        {
            foreach (Transform child in _content)
            {
                child.gameObject.SetActive(false);
            }
        }
    }
}
