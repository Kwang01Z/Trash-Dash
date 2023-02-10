using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SPStudios.Tools;
public class RunGameButton : MonoBehaviour
{
    [SerializeField] Button m_runGameButton;
    private void Reset()
    {
        m_runGameButton = GetComponent<Button>();
    }
    private void OnEnable()
    {
        m_runGameButton.onClick.AddListener(delegate { RunPressed(); });
    }
    private void OnDisable()
    {
        m_runGameButton.onClick.RemoveListener(delegate { RunPressed(); });
    }
    private void RunPressed()
    {
        Singletons.Get<SaveManager>().GetData().isTutorialDone = true;
        Singletons.Get<SaveManager>().Save();
        Singletons.Get<GameController>().LoadScence("RunGame");
    }
}
