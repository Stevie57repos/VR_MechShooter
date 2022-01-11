using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/LevelData")]
[System.Serializable]
public class LevelData : ScriptableObject
{
    public List<EnemyWave> _waveDataList;
    public float maxLevelDuration;

    public List<float> GetSpawnWaveTimesList()
    {
        List<float> list = new List<float>();
        for(int i = 0; i < _waveDataList.Count; i++)
        {
            list.Add(_waveDataList[i].spawnWaveTime);
        }
        return list;
    }

    public int GetEnemyPoolAmount(EnemyController prefab)
    {
        int enemyAmount = 0;
        for(int i = 0; i < _waveDataList.Count; i++)
        {
            if(_waveDataList[i].enemyPrefab == prefab)
                enemyAmount+= _waveDataList[i].enemyAmount;
        }
        return enemyAmount;
    }
}
