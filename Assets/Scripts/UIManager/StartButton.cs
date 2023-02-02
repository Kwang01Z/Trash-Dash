using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SPStudios.Tools;
public class StartButton : MonoBehaviour
{
    [SerializeField] Button m_startButton;
    private void Reset()
    {
        m_startButton = GetComponent<Button>();
    }
    private void OnEnable()
    {
        m_startButton.onClick.AddListener(delegate { StartGame(); });
    }
    private void OnDisable()
    {
        m_startButton.onClick.RemoveListener(delegate { StartGame(); });
    }
    void StartGame()
    {
        Singletons.Get<GameController>().LoadScence("Main");
    }

}
