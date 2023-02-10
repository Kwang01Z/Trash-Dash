using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopTabsSwitch : MonoBehaviour
{
    [Header("Items")]
    [SerializeField] Button m_ItemTab;
    [SerializeField] GameObject m_ItemLayout;
    [Header("Characters")]
    [SerializeField] Button m_CharacerTab;
    [SerializeField] GameObject m_CharacterLayout;
    [Header("Accessories")]
    [SerializeField] Button m_AccessoryTab;
    [SerializeField] GameObject m_AccessoryLayout;
    [Header("Themes")]
    [SerializeField] Button m_ThemeTab;
    [SerializeField] GameObject m_ThemeLayout;
    private void OnEnable()
    {
        m_ItemTab.onClick.AddListener(delegate { OnChangeShopLayout(1); });
        m_CharacerTab.onClick.AddListener(delegate { ActiveCharacter(); });
        m_AccessoryTab.onClick.AddListener(delegate { OnChangeShopLayout(3); });
        m_ThemeTab.onClick.AddListener(delegate { OnChangeShopLayout(4); });
    }
    private void OnDisable()
    {
        m_ItemTab.onClick.RemoveListener(delegate { OnChangeShopLayout(1); });
        m_CharacerTab.onClick.RemoveListener(delegate { ActiveCharacter(); });
        m_AccessoryTab.onClick.RemoveListener(delegate { OnChangeShopLayout(3); });
        m_ThemeTab.onClick.RemoveListener(delegate { OnChangeShopLayout(4); }); 
    }
    void ActiveCharacter()
    {
        m_ItemLayout.gameObject.SetActive(false);
        m_CharacterLayout.gameObject.SetActive(true);
        m_AccessoryLayout.gameObject.SetActive(false);
        m_ThemeLayout.gameObject.SetActive(false);
    }
    void OnChangeShopLayout(int index)
    {
        switch (index)
        {
            case 1: 
                m_ItemLayout.gameObject.SetActive(true);
                m_CharacterLayout.gameObject.SetActive(false);
                m_AccessoryLayout.gameObject.SetActive(false);
                m_ThemeLayout.gameObject.SetActive(false);
                break;
            case 2:
                m_CharacterLayout.gameObject.SetActive(true);
                m_ItemLayout.gameObject.SetActive(false);
                m_AccessoryLayout.gameObject.SetActive(false);
                m_ThemeLayout.gameObject.SetActive(false);
                break;
            case 3:
                m_AccessoryLayout.gameObject.SetActive(true);
                m_ItemLayout.gameObject.SetActive(false);
                m_CharacterLayout.gameObject.SetActive(false);
                m_ThemeLayout.gameObject.SetActive(false);
                break;
            case 4:
                m_ThemeLayout.gameObject.SetActive(true);
                m_ItemLayout.gameObject.SetActive(false);
                m_CharacterLayout.gameObject.SetActive(false);
                m_AccessoryLayout.gameObject.SetActive(false);
                break;
        }
    }
}
