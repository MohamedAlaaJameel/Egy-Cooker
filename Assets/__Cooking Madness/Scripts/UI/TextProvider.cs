using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITextProvider
{
    public void SetText(string text);
}
public class TextMeshProProvider : ITextProvider
{
    private TMPro.TextMeshProUGUI textMesh;

    public TextMeshProProvider(TMPro.TextMeshProUGUI textMesh)
    {
        this.textMesh = textMesh;
    }

    public void SetText(string text)
    {
        textMesh.text = text;
    }
}


