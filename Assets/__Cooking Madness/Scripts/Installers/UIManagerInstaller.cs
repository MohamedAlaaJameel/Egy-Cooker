using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UIManagerInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<LevelUIManager>().FromComponentInHierarchy().AsSingle();
    }
}