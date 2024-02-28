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
        Container.BindFactory<Meat, Meat.Factory>().FromComponentInNewPrefab(MeatPrefab).WithGameObjectName("Raw Meat");
        Container.BindFactory<Clock, Clock.Factory>().FromComponentInNewPrefab(ClockPrefab).WithGameObjectName("Clock");
        Container.Bind<Oven>().FromInstance(oven);
        Container.Bind<Trash>().FromInstance(trash);
        Container.Bind<MyTable>().FromInstance(table);
    }
}