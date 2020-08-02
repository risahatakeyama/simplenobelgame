using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class CharacterPanel : MonoBehaviour
{
    [SerializeField]
    private Image _imageCharacter;
    [SerializeField]
    private RectTransform _characterPos;

    private List<GameObject> _cacheGameObjectList=new List<GameObject>();
    private GameObject _currentCharacterObj;
    public void SetPanel(DisplayCharacterInfo displayCharacterInfo)
    {
        if (displayCharacterInfo.characterId != default(long))
        {
            var characterData = ResourceManager.Instance.LoadMasterData<CharacterData, CharacterDataObjectList>(displayCharacterInfo.characterId);

            if (characterData.Sprite != null)
            {
                _characterPos.anchoredPosition = displayCharacterInfo.diaplayPos;
                _imageCharacter.enabled = true;
                _imageCharacter.sprite = characterData.Sprite;
            }
            else
            {
                _imageCharacter.enabled = false;
            }

            if (characterData.ObjPrefab != null)
            {
                if (_currentCharacterObj != null)
                {
                    _currentCharacterObj.SetActive(false);
                }
                _currentCharacterObj = _cacheGameObjectList.FirstOrDefault(cacheObj=>cacheObj.name == characterData.ObjPrefab.name);
                if (_currentCharacterObj == null)
                {
                    _currentCharacterObj = Instantiate(characterData.ObjPrefab, this.gameObject.transform);
                    _cacheGameObjectList.Add(_currentCharacterObj);
                }

                this.gameObject.transform.GetComponent<RectTransform>().anchoredPosition = displayCharacterInfo.diaplayPos;

            }
            _currentCharacterObj.SetActive(characterData.ObjPrefab != null);

        }


        gameObject.SetActive(displayCharacterInfo.characterId != default(long));
    }
}
