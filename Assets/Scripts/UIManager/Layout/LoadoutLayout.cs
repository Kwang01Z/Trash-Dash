using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SPStudios.Tools;
public class LoadoutLayout : MonoBehaviour
{
    [SerializeField] AccessoriesZone m_accessoriesZone;
    [SerializeField] PowerUpZone m_powerUpZone;
    PlayerData m_playerData;
    private void Reset()
    {
        m_accessoriesZone = GetComponentInChildren<AccessoriesZone>();
        m_powerUpZone = GetComponentInChildren<PowerUpZone>();
    }
    private void Start()
    {
        m_playerData = Singletons.Get<SaveManager>().GetData();
        HideAndShowLayout();
    }
    void HideAndShowLayout()
    {
        m_accessoriesZone.gameObject.SetActive(m_playerData.accessories.Count > 0);
        m_powerUpZone.gameObject.SetActive(m_playerData.consumables.Count > 0);
    }
}
