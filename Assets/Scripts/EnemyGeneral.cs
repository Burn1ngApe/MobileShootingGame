using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyGeneral : MonoBehaviour
{
    [SerializeField] private float _amountOfEnemies;
    [SerializeField] public GameObject _enemy;
    [SerializeField] private GameObject _player;

    [HideInInspector]
    public List<GameObject> _enemies = new List<GameObject>();



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

            var enemy = Instantiate(_enemy);
            enemy.transform.position = randomPoint;

            var enemyBeh = enemy.GetComponent<EnemyBehaviour>();
            enemyBeh._enemyGeneral = this;
            enemyBeh._player = _player;

            _enemies.Add(enemy);
        }
    }



    public void RespawnEnemies()
    {
        foreach(var enemy in _enemies)
        {
            Destroy(enemy);
        }

        _enemies.Clear();

        SpawnEnemies();
    }
}
