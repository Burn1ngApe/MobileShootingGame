using UnityEngine;
using Zenject;

public class NewInstaller : MonoInstaller<NewInstaller>
{
    public Transform SpawnPoint;
    public GameObject PlayerPrefab;

    public override void InstallBindings()
    {
        var _playerNestedClass = Container.InstantiatePrefabForComponent<PlayerNestedClass>(PlayerPrefab, SpawnPoint.position, Quaternion.identity, null);

        Container
              .Bind<PlayerNestedClass>().FromInstance(_playerNestedClass).AsSingle();
    }

}