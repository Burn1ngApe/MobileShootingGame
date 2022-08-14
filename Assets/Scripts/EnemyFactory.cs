using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class EnemyFactory : MonoBehaviour, IObserver
{
    [SerializeField] private float _amountOfEnemies;
    [SerializeField] public GameObject EnemyPrefab;

    [Inject]
    private PlayerNestedClass _playerNestedClass;

    private CharacterController _characterController;
    private UIController _uiController;

    [HideInInspector]
    public List<GameObject> SpawnedEnemies = new List<GameObject>();



    private Vector3 GetRandomLocation()
    {
        NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();

        int t = Random.Range(0, navMeshData.indices.Length - 3);

        Vector3 point = Vector3.Lerp(navMeshData.vertices[navMeshData.indices[t]], navMeshData.vertices[navMeshData.indices[t + 1]], Random.value);
        Vector3.Lerp(point, navMeshData.vertices[navMeshData.indices[t + 2]], Random.value);

        return point;
    }



    private void Start()
    {
        _characterController = _playerNestedClass.CharacterController;
        _uiController = _playerNestedClass.UIController;


        SpawnEnemies();
        _uiController.AddObserver(this);
    }



    private void SpawnEnemies()
    {
        for(int i = 0; i < _amountOfEnemies; i++)
        {
            var randomPoint = GetRandomLocation();
            randomPoint.y = 1f;

            var enemy = Instantiate(EnemyPrefab);
            enemy.transform.position = randomPoint;

            var enemyBeh = enemy.GetComponent<EnemyBehaviour>();

            enemyBeh.Player = _characterController.gameObject;
            enemyBeh.CharacterController = _characterController;
            enemyBeh.CharacterShooting = _characterController.characterShooting;


            SpawnedEnemies.Add(enemy);
        }
    }



    public void RespawnEnemies()
    {
        foreach(var enemy in SpawnedEnemies)
        {
            Destroy(enemy);
        }

        SpawnedEnemies.Clear();

        SpawnEnemies();
    }



    public void UpdateData()
    {
        RespawnEnemies();
    }
}
