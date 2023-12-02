# The Crawling Dead: Parasitic Wasp Simulator

This project was developed under the sponsorship of Dr. Barbara Sharanowski as part of the UCF CECS Computer Science Senior Design program.

# Authors
- Lester Miller
- Chrystian Orren
- Karina Quiroga
- Travon Ross
- Joseph Stewart

# Overview
The Crawling Dead is a 2D platformer mobile game, developed in Unity, developed in an effort to lead to a greater exposure of parasitic wasps to the average player. The game is meant to be both fun and informative, providing replayability as much as possible.

# Specifications
The game is played out over 6 scenes, not including the Title Card. The Main Menu lets the player design their wasp, while the GameWorld has the player searching for hosts. Each hosts triggers one of the 4 minigames: Caterpillar Minigame, Aphid Minigame, Connect The Dots Minigame, Digging Minigame. Each minigames have compatibility requirements shown within the GameManager GameObject.

All GameManagers inherit from either the Singleton or PersistentSingleton utility classes.

Player data is stored within player prefs, while most temporary data is stored using the persistent GameWorld GameManager and various Scriptable Objects, organized in the project window.

# Necessary Scenes
Title_Card
Main_Menu
GameWorld
AphidGame
CaterpillarMG
ConnectTheDotsMG
DiggingMG
GameOver

# Additional Credit
Syeda Humna - Art - fiverr (@syedahumna56)