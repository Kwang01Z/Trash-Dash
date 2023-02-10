using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;

public class CharNameZone : ZoneSelectorBase
{
    [SerializeField] TextMeshProUGUI m_textName;
    [SerializeField] AccessoriesZone m_accessoriesZone;
    bool m_IsLoadingCharacter;
    protected List<int> m_OwnedAccesories = new List<int>();
    public Transform charPosition;
    protected const float k_OwnedAccessoriesCharacterOffset = -0.1f;
    protected int k_UILayer;
    protected readonly Quaternion k_FlippedYAxisRotation = Quaternion.Euler(0f, 180f, 0f);
    protected const float k_CharacterRotationSpeed = 45f;
    GameObject m_Character;
    private void Awake()
    {
        StartCoroutine(CharacterDatabase.LoadDatabase());
    }
    public override void LoadDatabase()
    {
        base.LoadDatabase();
    }
    public override void ValidateData()
    {
        base.ValidateData();
        if (!m_dataPlayer.characters[m_dataPlayer.usedCharacter].Equals(m_textName.text))
        {
            m_textName.SetText(m_dataPlayer.characters[m_dataPlayer.usedCharacter]);
            StartCoroutine(PopulateCharacters());
        }
    }
    public override void AutoHideButton()
    {
        base.AutoHideButton();
        m_buttonNext.gameObject.SetActive(m_dataPlayer.usedCharacter < m_dataPlayer.characters.Count - 1);
        m_buttonPre.gameObject.SetActive(m_dataPlayer.usedCharacter > 0);
    }
    public override void ButtonNextPressed()
    {
        m_dataPlayer.usedCharacter++;
        ValidateData();
    }

    public override void ButtonPrePressed()
    {
        m_dataPlayer.usedCharacter--;
        ValidateData();
    }
    public IEnumerator PopulateCharacters()
    {
        m_dataPlayer.usedAccessory = -1;
        
        if (!m_IsLoadingCharacter)
        {
            m_IsLoadingCharacter = true;
            GameObject newChar = null;
            while (newChar == null)
            {
                Character c = CharacterDatabase.GetCharacter(m_dataPlayer.characters[m_dataPlayer.usedCharacter]);

                if (c != null)
                {
                    m_OwnedAccesories.Clear();
                    for (int i = 0; i < c.accessories.Length; ++i)
                    {
                        // Check which accessories we own.
                        string compoundName = c.characterName + ":" + c.accessories[i].accessoryName;
                        if (m_dataPlayer.accessories.Contains(compoundName))
                        {
                            m_OwnedAccesories.Add(i);
                        }
                    }

                    Vector3 pos = charPosition.transform.position;
                    if (m_OwnedAccesories.Count > 0)
                    {
                        pos.x = k_OwnedAccessoriesCharacterOffset;
                    }
                    else
                    {
                        pos.x = 0.0f;
                    }

                    charPosition.transform.position = pos;

                    AsyncOperationHandle op = Addressables.InstantiateAsync(c.characterName);
                    yield return op;
                    if (op.Result == null || !(op.Result is GameObject))
                    {
                        Debug.LogWarning(string.Format("Unable to load character {0}.", c.characterName));
                        yield break;
                    }
                    newChar = op.Result as GameObject;

                    k_UILayer = LayerMask.NameToLayer("UI");
                    Helpers.SetRendererLayerRecursive(newChar, k_UILayer);
                    newChar.transform.SetParent(charPosition, false);
                    newChar.transform.rotation = k_FlippedYAxisRotation;
                    int randIdle = Random.Range(0, 5);
                    newChar.GetComponent<Character>().animator.SetInteger("RandomIdle", randIdle);
                    if (m_Character != null)
                        Addressables.ReleaseInstance(m_Character);

                    m_Character = newChar;
                    m_Character.transform.localPosition = Vector3.right * 1000;
                    yield return new WaitForEndOfFrame();
                    m_Character.transform.localPosition = Vector3.zero;

                    /*SetupAccessory();*/
                }
                else
                    yield return new WaitForSeconds(1.0f);
            }
            m_IsLoadingCharacter = false;
        }
        m_accessoriesZone.m_OwnedAccesories = m_OwnedAccesories;
        m_accessoriesZone.m_ObjChar = m_Character;
        m_accessoriesZone.ValidateData();
    }
    public override void OtherUpdate()
    {
        base.OtherUpdate();
        if (m_Character != null)
        {
            m_Character.transform.Rotate(0, k_CharacterRotationSpeed * Time.deltaTime, 0, Space.Self);
        }
    }
}
