using UnityEngine;
using Zenject;
using System.Collections.Generic;
using System.Threading.Tasks;

public class NewInstaller : MonoInstaller<NewInstaller>
{
    public Transform _spawnPoint;
    public GameObject _playerPrefab;

    public override void InstallBindings()
    {
        var _characterController = Container.InstantiatePrefabForComponent<CharacterController>(_playerPrefab, _spawnPoint.position, Quaternion.identity, null);

        Container
              .Bind<CharacterController>().FromInstance(_characterController).AsSingle();
    }

}