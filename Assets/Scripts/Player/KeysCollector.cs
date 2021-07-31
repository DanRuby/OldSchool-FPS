using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Keys { Red, Blue, Yellow, None }

/// <summary>
/// Класс, хранящий собранные ключи игроком
/// </summary>
public class KeysCollector : MonoBehaviour
{
    private const int KEYS_AMOUNT=3;
    public static bool[] CollectedKeys =>collectedKeys;

    private static bool[] collectedKeys = new bool[KEYS_AMOUNT];

    public void AddKey(Keys key) => collectedKeys[(int)key] = true;
}
