using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class Antenna : MonoBehaviour
{
    [Tooltip("Index 0 is always the first signal enabled")] public List<AntennaSignal> unlockableSignals;

    private AntennaSignal currentSignal;
}
