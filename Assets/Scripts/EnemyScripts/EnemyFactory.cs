using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class EnemyFactory : MonoBehaviour
{
    [SerializeField] private float _amountOfEnemies;
    [SerializeField] public GameObject _enemyPrefab;

    [Inject]
    private CharacterController _characterController;


    [HideInInspector]
    public List<GameObject> _spawnedEnemies = new List<GameObject>();



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
        SpawnEnemies();
    }



    private void SpawnEnemies()
    {
        for(int i = 0; i < _amountOfEnemies; i++)
        {
            var randomPoint = GetRandomLocation();
            randomPoint.y = 1f;

            var enemy = Instantiate(_enemyPrefab);
            enemy.transform.position = randomPoint;

            var enemyBeh = enemy.GetComponent<EnemyBehaviour>();

            enemyBeh._player = _characterController.gameObject;
            enemyBeh._characterController = _characterController;
            enemyBeh._characterShooting = _characterController._characterShooting;


            _spawnedEnemies.Add(enemy);
        }
    }



    public void RespawnEnemies()
    {
        foreach(var enemy in _spawnedEnemies)
        {
            Destroy(enemy);
        }

        _spawnedEnemies.Clear();

        SpawnEnemies();
    }
}
