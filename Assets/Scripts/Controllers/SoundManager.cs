using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public AudioSource musicPlayer;
    private AudioClip menuMusic;
    private AudioClip gameplayMusic;
    private AudioClip deathSound;
    public AudioSource sfxPlayer;
    private State current;
    // Use this for initialization
    void Start () {
        menuMusic = Resources.Load<AudioClip>("Music/Dark Dungeon AMBIENT LOOP");
        gameplayMusic = Resources.Load<AudioClip>("Music/Barren Combat LOOP");
        deathSound = Resources.Load<AudioClip>("Music/Death PIANO");
        musicPlayer.loop = true;
        current = State.GAME;
    }

    private void Update()
    {

        Debug.Log(PlayerController.Instance.attributes.Health);
            if ((GameController.state == State.CHARACTER_SELECT) && current != State.CHARACTER_SELECT)
            {
                EnterMenu();
                current = GameController.state;
            }
            else if ((GameController.state == State.GAME) && current != State.GAME)
            {
                EnterGameplay();
                current = GameController.state;
            }            
         

            
        
    }
    public void EnterMenu()
    {
        musicPlayer.Stop();
        musicPlayer.clip = menuMusic;
        musicPlayer.Play();
    }

    public void EnterGameplay()
    {
        musicPlayer.Stop();
        musicPlayer.clip = gameplayMusic;
        musicPlayer.Play();
    }

    public void OnDeath()
    {
        sfxPlayer.PlayOneShot(deathSound);
    }
}
