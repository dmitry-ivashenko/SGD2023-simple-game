using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> SpawnPoints;

    public Enemy EnemyPrefab;
    
    public GameObject FxPrefab;

    public GameObject Player;

    public int EnemiesCount = 3;

    public float PlayerAttackDistance = 1f;
    
    private readonly List<Enemy> _enemies = new List<Enemy>();
    private Player _player;


    void Start()
    {
        _player = Player.GetComponent<Player>();
        _player.OnAttack += OnPlayerAttack;
    }

    private void OnPlayerAttack()
    {
        for (var index = _enemies.Count - 1; index >= 0; index--)
        {
            var enemy = _enemies[index];
            var distance = enemy.transform.position - _player.transform.position;
            if (distance.magnitude < PlayerAttackDistance)
            {
                SpawnFx(enemy.transform.position);
                _enemies.Remove(enemy);
                enemy.PlayDieAnimation();
            }
        }
    }

    private async Task SpawnFx(Vector3 position)
    {
        var go = Instantiate(FxPrefab);
        go.transform.position = position;
        await Task.Delay(3000);
        Destroy(go);
    }

    void Update()
    {
        if (_enemies.Count < EnemiesCount)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        var enemyGo = Instantiate(EnemyPrefab.gameObject);
        var enemy = enemyGo.GetComponent<Enemy>();
        _enemies.Add(enemy);

        var index = Random.Range(0, SpawnPoints.Count - 1);
        var spawnPoint = SpawnPoints[index];
        enemyGo.transform.position = spawnPoint.transform.position;

        enemy.Init(Player);
    }
}
