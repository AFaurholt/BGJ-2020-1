using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class InfoBoxFiller : MonoBehaviour
{
    private const string VersionTag = "{v}";
    private const string HashTag = "{hash}";

    private const string PatternDescription = "You can use tags to replace text with global values\n"
            + VersionTag + " = Application's version\n"
            + HashTag + " = Hash ID of application";

    // TODO: Add Naughty Attributes
    //[SerializeField, TextArea(3, 8), NaughtyAttributes.InfoBox(PatternDescription)]
    [SerializeField, TextArea(3, 8), Tooltip(PatternDescription)]
    private string pattern = "";

    private TMP_Text box = null;

    private TMP_Text Box {
        get {
            if (!box) {
                box = GetComponent<TMP_Text>();
            }
            return box;
        }
    }

    private void OnValidate()
    {
        UpdateText();
    }

    private void Awake()
    {
        UpdateText();
    }

    private void UpdateText()
    {
        var box = Box;
        box.text = InjectData(pattern);
    }

    public string InjectData(string pattern)
    {
        return pattern
            .Replace(VersionTag, VersionData.Version)
            .Replace(HashTag, VersionData.Hash);
    }
}
