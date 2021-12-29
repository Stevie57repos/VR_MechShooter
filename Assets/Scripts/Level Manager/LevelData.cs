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
}
