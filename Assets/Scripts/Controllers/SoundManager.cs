﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance; 

    public static AudioClip Hammer_Hit_Body_Gore;
    public static AudioClip Hammer_Hit_Metal_Armor;

    public AudioSource musicPlayer;
    public AudioSource sfxPlayer; 

    private AudioClip menuMusic;
    private AudioClip gameplayMusic;
    private AudioClip deathSound;
    private State current;

    void OnEnable ()
    {
        SettingsContainer.OnToggleMusic += OnToggleMusic;
    }
    void OnDisable ()
    {
        SettingsContainer.OnToggleMusic -= OnToggleMusic;
    }

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this; 
        }
    }

    void Start ()
    {
        menuMusic = Resources.Load<AudioClip> ("Music/Dark Dungeon AMBIENT LOOP");
        gameplayMusic = Resources.Load<AudioClip> ("Music/Barren Combat LOOP");
        deathSound = Resources.Load<AudioClip> ("Music/Death PIANO");

        Hammer_Hit_Body_Gore = Resources.Load<AudioClip> ("Music/HAMMER_Hit_Body_Gore_stereo");
        Hammer_Hit_Metal_Armor = Resources.Load<AudioClip> ("Music/HAMMER_Hit_Metal_Armor_stereo"); 
        musicPlayer.loop = true;
        current = State.GAME;
    }

    void OnToggleMusic (bool b)
    {
        if (!b)
        {
            musicPlayer.Stop ();
        }
        else
        {
            musicPlayer.clip = getClip ();
            musicPlayer.time = Random.Range (0f, 25f);
            musicPlayer.Play ();
        }
    }

    private void Update ()
    {
        musicPlayer.volume = GameController.PAUSE ? 0f : .6f;
        if ((GameController.state == State.CHARACTER_SELECT) && current != State.CHARACTER_SELECT)
        {
            EnterMenu ();
            current = GameController.state;
        }
        else if ((GameController.state == State.GAME) && current != State.GAME)
        {
            EnterGameplay ();
            current = GameController.state;
        }

    }
    public void EnterMenu ()
    {
        if (!SettingsContainer.EnableMusic) return;
        musicPlayer.Stop ();
        musicPlayer.clip = menuMusic;
        musicPlayer.Play ();
    }

    public void EnterGameplay ()
    {
        if (!SettingsContainer.EnableMusic) return;
        musicPlayer.Stop ();
        musicPlayer.clip = gameplayMusic;
        musicPlayer.Play ();
    }

    private AudioClip getClip ()
    {
        if (GameController.state == State.CHARACTER_SELECT)
        {
            return menuMusic;
        }
        return gameplayMusic;
    }

    public void PlaySFX (AudioClip clip, float pitch)
    {
        // sfxPlayer.clip = clip; 
        // sfxPlayer.pitch = pitch; 
        // sfxPlayer.Play(); 
    }

}