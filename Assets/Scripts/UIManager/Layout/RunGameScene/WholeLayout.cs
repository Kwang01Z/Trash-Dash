using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.AddressableAssets;

public class WholeLayout : MonoBehaviour
{
	[SerializeField] Button m_PauseButton;

	[Header("Score Zone")]
	[SerializeField] TextMeshProUGUI scoreText;
	[SerializeField] TextMeshProUGUI multiplierText;
	[SerializeField] TextMeshProUGUI distanceText;

	[Header("Coin Zone")]
	[SerializeField] TextMeshProUGUI coinText;
	[SerializeField] TextMeshProUGUI premiumText;

	[Header("Life Zone")]
	[SerializeField] Transform lifeRectTransform;

	[Header("Countdown")]
	[SerializeField] Transform m_CountdownRectTransform;
	[SerializeField] TextMeshProUGUI countdownText;

	[Header("Inventory")]
	[SerializeField] Image inventoryIcon;
	[SerializeField] Button inventoryButton;
	protected Image[] m_LifeHearts;
	protected int k_MaxLives = 3;

	[Header("Prefabs")]
	public GameObject PowerupIconPrefab;
	public RectTransform powerupZone;

	[Header("Database")]
	[SerializeField] ConsumableDatabase consumableDatabase;
	public Button PauseButton { get { return m_PauseButton; } }


	public Consumable inventory;
	private void Start()
    {
		consumableDatabase.Load();
		m_LifeHearts = new Image[k_MaxLives];
		for (int i = 0; i < k_MaxLives; ++i)
		{
			m_LifeHearts[i] = lifeRectTransform.GetChild(i).GetComponent<Image>();
		}
	}
    public void UpdateUI(GamePlayManager manager)
	{
		coinText.text = manager.trackManager.characterController.coins.ToString();
		premiumText.text = manager.trackManager.characterController.premium.ToString();

		for (int i = 0; i < 3; ++i)
		{

			if (manager.trackManager.characterController.currentLife > i)
			{
				m_LifeHearts[i].color = Color.white;
			}
			else
			{
				m_LifeHearts[i].color = Color.black;
			}
		}

		scoreText.text = manager.trackManager.score.ToString();
		multiplierText.text = "x " + manager.trackManager.multiplier;

		distanceText.text = Mathf.FloorToInt(manager.trackManager.worldDistance).ToString() + "m";

		if (manager.trackManager.timeToStart >= 0)
		{
			countdownText.gameObject.SetActive(true);
			countdownText.text = Mathf.Ceil(manager.trackManager.timeToStart).ToString();
			m_CountdownRectTransform.localScale = Vector3.one * (1.0f - (manager.trackManager.timeToStart - Mathf.Floor(manager.trackManager.timeToStart)));
		}
		else
		{
			m_CountdownRectTransform.localScale = Vector3.zero;
		}

		// Consumable
		if (manager.PlayerData.consumables.Count > 0 && manager.PlayerData.usedConsumable > 0)
		{
			inventory = ConsumableDatabase.GetConsumbale(manager.PlayerData.usedConsumable);
			inventoryIcon.transform.parent.gameObject.SetActive(true);
			inventoryIcon.sprite = inventory.icon;
			inventoryButton.onClick.AddListener(delegate { UseInventory(manager); });
		}
		else
		{
			inventoryIcon.transform.parent.gameObject.SetActive(false);
			inventoryButton.onClick.RemoveListener(delegate { UseInventory(manager); });
		}
			
	}
	public void UseInventory(GamePlayManager manager)
	{
		if (inventory != null && inventory.CanBeUsed(manager.trackManager.characterController))
		{
			manager.trackManager.characterController.UseConsumable(inventory);
			manager.PlayerData.UseConsumable(inventory);
			manager.PlayerData.usedConsumable = Consumable.ConsumableType.NONE;
			inventory = null;
		}
		inventoryIcon.transform.parent.gameObject.SetActive(false);
		inventoryButton.onClick.RemoveListener(delegate { UseInventory(manager); });
	}


	
}
