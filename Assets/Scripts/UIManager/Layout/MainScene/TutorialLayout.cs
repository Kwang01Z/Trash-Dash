using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SPStudios.Tools;
public class TutorialLayout : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(!Singletons.Get<SaveManager>().GetData().isTutorialDone);
    }
}
