using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ThemeZone : ZoneSelectorBase
{
    [SerializeField] TextMeshProUGUI m_themeNameDisplay;
    [SerializeField] Image m_themeIcon;
    [SerializeField] MeshFilter skyMeshFilter;
    [SerializeField] MeshFilter UIGroundFilter;
    public override void LoadDatabase()
    {
        base.LoadDatabase();
        StartCoroutine(ThemeDatabase.LoadDatabase());
    }
    public override void ValidateData()
    {
        base.ValidateData();
        StartCoroutine(PopulateTheme());
    }
    public IEnumerator PopulateTheme()
    {
        ThemeData t = null;

        while (t == null)
        {
            t = ThemeDatabase.GetThemeData(m_dataPlayer.themes[m_dataPlayer.usedTheme]);
            yield return null;
        }

        m_themeNameDisplay.text = t.themeName;
        m_themeIcon.sprite = t.themeIcon;

        skyMeshFilter.sharedMesh = t.skyMesh;
        UIGroundFilter.sharedMesh = t.UIGroundMesh;
    }
    public override void AutoHideButton()
    {
        base.AutoHideButton();
        m_buttonNext.gameObject.SetActive(m_dataPlayer.usedTheme < m_dataPlayer.themes.Count - 1);
        m_buttonPre.gameObject.SetActive(m_dataPlayer.usedTheme > 0);
    }
    public override void ButtonNextPressed()
    {
        m_dataPlayer.usedTheme++;
        ValidateData();
    }

    public override void ButtonPrePressed()
    {
        m_dataPlayer.usedTheme--;
        ValidateData();
    }
}
