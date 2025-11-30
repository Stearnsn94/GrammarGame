using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class EnemyCharacter : MonoBehaviour, Interactable
{
    PlayerCharacter playable_character;

    void Awake()
    {   
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

    private void follow_player()
    {
        
    }

    public void Interact(PlayerCharacter player)
    {
        //on interact it shoud drain the player.
        playable_character.drain_health(10);
    }
}
