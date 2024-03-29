using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class MeatInstaller : MonoInstaller
{
    public GameObject MeatPrefab;
    public GameObject ClockPrefab;
    public Oven oven;
    public MyTable table;
    public Trash trash;
    public MeatSpawner meatSpawner;


    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        Container.DeclareSignal<int>();
        Container.BindSignal<int>().ToMethod<Oven>(ov => ov.DespawnClock).FromResolve();
        Container.BindFactory<Meat, Meat.Factory>().FromComponentInNewPrefab(MeatPrefab).WithGameObjectName("Raw Meat");
        Container.BindMemoryPool<Clock, Clock.Pool>().WithInitialSize(4).FromComponentInNewPrefab(ClockPrefab).WithGameObjectName("Clock");
        Container.Bind<Oven>().FromInstance(oven);
        Container.Bind<Trash>().FromInstance(trash);
        Container.Bind<MyTable>().FromInstance(table);
    }
}