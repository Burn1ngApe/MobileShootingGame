using UnityEngine;
using Zenject;

public class NewInstaller : MonoInstaller<NewInstaller>
{
    public Transform _spawnPoint;
    public GameObject _playerPrefab;

    public override void InstallBindings()
    {
        var _characterController = Container.InstantiatePrefabForComponent<CharacterController>(_playerPrefab, _spawnPoint.position, Quaternion.identity, null);
        Container
              .Bind<CharacterController>().FromInstance(_characterController).AsSingle();


        var UiController = _characterController.UiController;
        Container.Bind<UIController>().FromInstance(UiController).AsSingle();
    }

}