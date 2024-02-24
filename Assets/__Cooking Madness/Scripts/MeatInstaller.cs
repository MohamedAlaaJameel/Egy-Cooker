using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class MeatInstaller : MonoInstaller
{
    public GameObject MeatPrefab;
    public GameObject ClockPrefab;
    public Oven oven;
    public Table table;
    public MeatSpawner meatSpawner;


    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        Container.DeclareSignal<OnRawMeatCreationSignal>();
        Container.DeclareSignal<OnPlacedFoodOnOvenSignal>();


        Container.BindFactory<Meat, Meat.Factory>().FromComponentInNewPrefab(MeatPrefab).WithGameObjectName("Raw Meat");
        Container.BindFactory<Clock, Clock.Factory>().FromComponentInNewPrefab(ClockPrefab).WithGameObjectName("Clock");

        Container.Bind<MeatSpawner>().FromInstance(meatSpawner).AsSingle();
        Container.BindInterfacesTo<ClockSpawner>().AsSingle();

        Container.Bind<Oven>().FromInstance(oven);
        Container.Bind<Table>().FromInstance(table);
       // Container.BindSignal<OnRawMeatCreationSignal>().ToMethod<Oven>((myoven, mysignal) => myoven.AddToOven(mysignal.meatTransform)).FromResolve();


    }
}