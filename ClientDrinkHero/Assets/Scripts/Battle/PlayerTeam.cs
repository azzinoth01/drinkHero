using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeam : MonoBehaviour
{
    private static PlayerTeam instance;
    public static PlayerTeam Instance => instance;

    [SerializeField] List<Transform> playerCharacterPositions;
    [SerializeField] List<GameObject> playerCharacterPrefabs;
    [SerializeField] List<GameObject> playerCharacters;

    float timer;
    float idleBlinkDelay;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    public bool InstantiatePlayerCharacter(int characterID, int slot)
    {
        characterID--;
        if (characterID >= playerCharacterPrefabs.Count || slot >= playerCharacterPositions.Count || playerCharacterPrefabs[characterID] == null)
            return false;

        playerCharacters.Add(Instantiate(playerCharacterPrefabs[characterID], playerCharacterPositions[slot].position + new Vector3(0f, -1f, 0f), Quaternion.identity,  playerCharacterPositions[slot]));
        return true;
    }

    public void PlayAnimation(string animation)
    {
        foreach (GameObject playerCharacter in playerCharacters)
        {
            playerCharacter.GetComponent<Animator>().Play(animation, -1, 0f);
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > idleBlinkDelay && playerCharacters.Count > 0)
        {
            timer -= idleBlinkDelay;
            idleBlinkDelay = Random.Range(1f, 3f);
            playerCharacters[Random.Range(0, playerCharacters.Count)].GetComponent<Animator>().SetTrigger("Blink");
        }
    }
}
