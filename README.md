# GrammarGame  
2D Puzzle-Based Grammar Learning Prototype

# Educational and Entertainment Aspects
GrammarGame is designed to reinforce introductory grammar concepts through interactive puzzle solving. Players must identify the correct item associated with a grammar clue in order to progress through the level. This supports recognition, recall, and contextual understanding of grammar subjects within an applied environment.
The game presents a simple exploration-and-puzzle loop intended to remain enjoyable while maintaining its instructional purpose. Players collect items, investigate the environment, and unlock doors by answering grammar-related prompts. The flow of moving, interacting, and solving lightweight puzzles provides a sense of engagement without overwhelming complexity.

# Secondary Objectives
The prototype also demonstrates the following:
- A functioning interaction system using an Interactable interface
- An inventory management system built around ScriptableObject items
- Dynamic UI that updates as the player collects items
- A pause-based selection mechanic

These systems reinforce the educational goal while also creating a coherent and usable experience.

# UI
The interface is intentionally minimal and consistent. The inventory panel, interaction prompt, item buttons, and feedback messages are laid out clearly to ensure ease of use. Colors and layout choices emphasize readability and functional clarity, avoiding unnecessary visual clutter.

# Build Instructions

## Opening the Project
1. Open Unity Hub.
2. Select "Add".
3. Browse to the project folder and load it into the editor.
4. Then click on the project GrammarGame
5. Open the main scene located in: Assets/Scenes/MainMenu.unity

## Running the Project v1 - in the editor
1. With the scene open, press the Play button in the Unity Editor.
2. You can then click on any of the menu options, including the controls and difficulty.
3. Then you can hit the play button to load the next scene to view all the UI elements that will be a part of the game in a test scene.

## Controls
- Movement: WASD or Arrow Keys
- Interact: E
- Select inventory item: Left-click on UI button

## Gameplay UI Summary
- The player moves through a 2D environment.
- Item objects can be collected by approaching them and pressing E.
- Collected items appear in the inventory panel as clickable buttons.
- Interacting with a door triggers a grammar-related question.
- If the player has at least one item, the game pauses while the player selects an answer.
- Selecting the correct item unlocks the door; selecting an incorrect item resumes the game with feedback.
- Feedback messages automatically disappear after a brief delay.
- Pause menu in the bottom right corener to go back to the main menu, restart the game(unfinished), quit the game, or view the controls.
- Main menu fully integrated into the scene, can press play, view controls, and select difficulty(unimplemented into questions) for the question difficulty.

# View
All UI elements are accessible and visible during gameplay. Inventory content updates dynamically, interaction prompts appear when needed, and feedback messages are clearly displayed.

# Potential Error for no movement from input in editor
The prototype is using the Old Input Manager and might not function unless you navigate to:
Edit->Project Settings->Player->Other Settings->Active Input Handling-> from Both to Input Manager(Old)