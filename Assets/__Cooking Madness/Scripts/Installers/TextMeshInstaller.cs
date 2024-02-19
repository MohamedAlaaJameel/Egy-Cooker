using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using Zenject;

public class TextMeshInstaller : MonoInstaller
{
    public TextMeshProUGUI textMesh;
    public override void InstallBindings()
    {
        Container.Bind<ITextProvider>().To<TextMeshProProvider>().AsSingle().WithArguments(textMesh);
    }
}
