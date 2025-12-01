using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class EnemyCharacter : MonoBehaviour, Interactable
{
    PlayerCharacter playable_character;

    float enemy_speed = 5.0f; 

    void Awake()
    {   
        //call in the playable_character in order to get corridates of the player
        playable_character = GetComponent<PlayerCharacter>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void move_path(Vector2 move_pos)
    {
        
    }

    private void move_follow_player()
    {
        Transform enemy_follow = transform;

        enemy_follow.position = Vector2.MoveTowards(enemy_follow.position, playable_character.get_players_location(), enemy_speed * Time.deltaTime);
    }

    public void Interact(PlayerCharacter player)
    {
        //on interact it shoud drain the player.
        playable_character.drain_health(10);
    }
}
