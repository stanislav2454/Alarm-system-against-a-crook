// WaveData.cs
using UnityEngine;

[CreateAssetMenu(fileName = "NewWave", menuName = "Game/Wave Data")]
public class WaveData : ScriptableObject
{
    [System.Serializable]
    public class EnemyWave
    {
        public string waveName = "Wave 1";
        public int enemyCount = 5;
        public float spawnInterval = 2f;
        public bool hasBoss = false;
        public int bossHealthMultiplier = 3;
    }

    public EnemyWave[] waves = new EnemyWave[3];

    // Создайте 3 волны через Assets/Create/Game/Wave Data
    // Wave 1: 5 врагов, интервал 2с
    // Wave 2: 8 врагов, интервал 1.5с  
    // Wave 3: 10 врагов + босс, интервал 1с
}