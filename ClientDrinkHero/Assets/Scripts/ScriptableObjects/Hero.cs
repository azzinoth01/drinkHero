using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hero_", menuName = "Drink Hero/Hero")]
public class Hero : ScriptableObject
{
    public int ID;
    public string Name;

    [Header("Secretary")]
    public GameObject SecretaryPrefab;
    public Sprite SecretarySpezialTouchImage;
    public List<string> SecretaryQuotes;
    public string SecretarySpezialTouchQuote;
}