using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SPStudios.Tools;
public class PopupLayerButton : MonoBehaviour
{
    [SerializeField] Button m_button;
    [SerializeField] GameObject m_layer;
    [SerializeField] bool isOpen = true;
    private void Reset()
    {
        m_button = GetComponent<Button>();
    }
    private void OnEnable()
    {
        m_button.onClick.AddListener(delegate { OpenOrHideLayer(); });
    }
    private void OnDisable()
    {
        m_button.onClick.RemoveListener(delegate { OpenOrHideLayer(); });
    }
    void OpenOrHideLayer()
    {
        Singletons.Get<SaveManager>().Save();
        m_layer.SetActive(isOpen);
    }
}
