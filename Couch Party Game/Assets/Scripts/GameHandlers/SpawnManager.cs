﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject[] allCharacters;
    [SerializeField] SpawnLocations[] allSpawnLocations;
    [SerializeField] Transform defaultSpawn;
    [SerializeField] bool onlyUniqueCharacters = true;

    [HideInInspector] public List<Player> localPlayers = new List<Player>();
    [HideInInspector] public List<Player> globalPlayers = new List<Player>();
    [HideInInspector] public GameObject lastLocalPlayer;

    public void GetSpawnData()
    {
        if(allCharacters.Length > 0 && defaultSpawn != null)
        {
            List<GameObject> availableCharacters = new List<GameObject>(allCharacters);
            List<SpawnLocations> availableSpawnLocations = new List<SpawnLocations>(allSpawnLocations);

            //availableSpawnLocations[1].RemoveLocation(0);
            //return;
            for (int i = 0; i < 2; i++)
            {
                GameObject selectedCharacter = availableCharacters[Random.Range(0, availableCharacters.Count)];

                if (onlyUniqueCharacters && availableCharacters.Count > 1)
                {
                    availableCharacters.Remove(selectedCharacter);
                }

                Player.Role characterRole = selectedCharacter.GetComponent<Player>().role;

                foreach(SpawnLocations roleLocation in availableSpawnLocations)
                {
                    if(roleLocation.requiredRole == characterRole)
                    {
                        Transform selectedSpawn = defaultSpawn;

                        if (roleLocation.locations.Length > 0)
                        {
                            selectedSpawn = roleLocation.locations[Random.Range(0, roleLocation.locations.Length)];
                            if (roleLocation.locations.Length > 1)
                            {
                                roleLocation.RemoveLocation(selectedSpawn);
                            }
                        }

                        //HERE THE PLAYER GETS ACTUALLY SPAWNED
                        ActualSpawn(selectedSpawn, selectedCharacter);
                    }
                }
            }
        }
    }

    void ActualSpawn(Transform location, GameObject character)
    {
        PlayerInput.Instantiate(gameObject);
        Player newCharacter = Instantiate(character, location.position, location.rotation).GetComponent<Player>();
        localPlayers.Add(newCharacter);
        globalPlayers.Add(newCharacter);
    }

    [System.Serializable]
    public struct SpawnLocations
    {
        public Player.Role requiredRole;
        public Transform[] locations;

        public void RemoveLocation(Transform target)
        {
            List<Transform> newLocations = new List<Transform>(locations);
            newLocations.Remove(target);
            locations = newLocations.ToArray();
        }

        public void RemoveLocation(int target)
        {
            List<Transform> newLocations = new List<Transform>(locations);
            newLocations.RemoveAt(target);
            locations = newLocations.ToArray();
        }
    }
}
