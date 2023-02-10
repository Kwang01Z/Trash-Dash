using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DividerLeaderBoard : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI txtIndex;
    int index;
    // Update is called once per frame
    void Update()
    {
        index = int.Parse(txtIndex.text);
        image.enabled = (index % 2 == 0);
    }
}
