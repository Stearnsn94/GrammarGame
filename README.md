# Grammar Grable  
2D Puzzle-Based Grammar Learning Prototype

## Overview

Grammar Grable is a 2D educational puzzle escape-room prototype where players reinforce grammar concepts through exploration, item collection, and contextual question solving. Progression is gated by grammar-based questions: a door presents a clue and the player must select the correct inventory item to advance.

The project mixes learning objectives with lightweight game systems, providing recognition and recall of grammar concepts through applied play.

## Core Systems Implemented

### Interactable Framework
World objects implement an Interactable interface, enabling consistent interaction logic for items, doors, and UI prompts.

### ScriptableObject Item System
Items are ScriptableObject assets containing answer text, wrong answer text, and icon sprites. Their values are dynamically updated at runtime by QuestionManager.

### Dynamic Question System
Different difficulty pools exist. When interacting with a door, the QuestionManager selects a question and assigns answer text and icons to items. The UI refreshes automatically so item representations match their current context.

### Inventory UI
Collected items appear as clickable buttons. The UI reflects dynamic name and icon changes. During question selection, clicking an item checks correctness and resumes play.

### Interaction Prompt
Displays “Press E to interact” when the player approaches an interactable. It automatically shows and hides on trigger enter and exit.

### Enemy and Damage System
Enemies detect player proximity, and deal periodic damage. The player has a health slider that updates visually, damage cooldown periods, and death leads to a game over scene.

### Player Animation
The player animates based on cardinal movement directions and idle orientation.

### Popup Text System
Displays floating text notifications when items are collected. Text appears above other sprites and follows world-to-screen conversion.

### Difficulty Selection System
A main menu allows difficulty selection (Easy, Medium, Hard). Selected difficulty is stored in a Difficulty Manager and used to select appropriate question pools.

### Scene Transition and Game Over System
Doors open when correctly answered and load the next scene. When the player dies, the game over scene loads and the last scene index is saved so restarting loads the correct level.

## UI Summary

The UI is minimal and functional:
Inventory panel with icons and names, interaction prompt, temporary feedback messages, popup collection text, health slider, main menu with difficulty, pause menu functions.

## Controls

Movement: WASD or Arrow Keys  
Interact: E  
Select Inventory Item: Left Click

## Editor Instructions

### Opening the Project
1. Open Unity Hub
2. Select Add
3. Browse to the folder and add the project
4. Open Assets/Scenes/MainMenu.unity

### Running in Editor
1. Press Play in Unity
2. Select play or controls on the menu
3. Enter the game scene to view mechanics and UI

### Building in the editor
1. Select file in the top left
2. Click Build and run

## Gameplay Summary

1. Move through the world  
2. Collect items  
3. Inventory updates  
4. Interact with doors  
5. Game pauses while player selects an answer  
6. Correct answer opens the door and loads the next area  
7. Incorrect answer resumes play with feedback  
8. Enemies chase and damage player, leading to game over when health reaches zero  

## Educational Purpose

Grammar Grable supports reinforcement of grammar learning through recognition, contextual application, and immediate feedback.

## Extensibility

Supports:
- Additional difficulty tiers  
- New question pools  
- Expanded enemy types  
- More UI systems  
- Additional progression features

## Credits

Prototype developed to demonstrate instructional gameplay patterns, interactive systems, UI management, ScriptableObject-driven logic, and Unity engine functionality.
