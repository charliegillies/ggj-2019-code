using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Pirate Spawn Config")]
public class PirateSpawnConfiguration : ScriptableObject {
    public int InitialWaveSize = 4;
    public int WaveIncreasePerTurnMin = 1;
    public int WaveIncreasePerTurnMax = 3;
}
