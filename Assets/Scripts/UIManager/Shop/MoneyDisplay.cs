using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SPStudios.Tools;
public class MoneyDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_coinAmount;
    [SerializeField] TextMeshProUGUI m_premiumAmount;
    [SerializeField] Button m_AddButton;
    PlayerData m_playerData;
    private void Start()
    {
        m_playerData = Singletons.Get<SaveManager>().GetData();
    }
    private void OnEnable()
    {
        m_AddButton.onClick.AddListener(delegate { AddMoney(); });
    }
    private void OnDisable()
    {
        m_AddButton.onClick.RemoveListener(delegate { AddMoney(); });
    }
    // Update is called once per frame
    void Update()
    {
        DisplayMoney();
    }
    void DisplayMoney()
    {
        m_coinAmount.SetText(m_playerData.coins.ToString());
        m_premiumAmount.SetText(m_playerData.premium.ToString());
    }
    void AddMoney()
    {
        m_playerData.coins += 10000;
        m_playerData.premium += 100;
    }
}
