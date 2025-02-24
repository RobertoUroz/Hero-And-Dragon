# Hero And Dragon

**Hero And Dragon** is an AR project developed in Unity using ARFoundation. Inspired by the legendary Sant Jordi, this interactive experience allows users to deploy and interact with two virtual characters—a hero and a dragon—in the real world.

## Overview

In this project, users can:
- Deploy both a hero and a dragon into the AR scene via an in-app menu (menu visibility pending).
- Detect and track multiple markers simultaneously, enabling the placement of multiple virtual objects.
- Interact with the characters by clicking on them to trigger animations.
- Apply geometric transformations to each character relative to their initial marker position.
- Rotate and scale the characters either via pinch/drag gestures or using on-screen sliders.

## Features

- **Multi-Marker Detection:**  
  Supports tracking multiple markers at once to place and manage virtual objects in the real world.

- **Character Deployment & Interaction:**  
  Two characters (hero & dragon) can be deployed into the AR environment. When a character is clicked, it triggers a custom animation.

- **Gesture-Based Transformations:**  
  Users can scale the characters using a two-finger pinch gesture and rotate them with a horizontal swipe.

- **Slider Controls:**  
  In addition to gestures, users can adjust rotation and scaling via on-screen sliders.

- **Custom Animations:**  
  Pre-programmed animations are applied when interacting with the characters to enhance the AR experience.

## Technologies Used

- **Unity Engine:** Core development platform for creating the 3D AR experience.
- **ARFoundation:** Provides cross-platform support for augmented reality features.
- **C#:** Scripting language used for game mechanics, interactions, and UI logic.
- **Gesture Detection:** Built-in Unity functionalities (or custom scripts) handle pinch, rotate, and tap gestures.
- **User Interface:** Custom UI elements for sliders and menus to control transformations.

## Installation & Setup

1. **Clone the Repository:**

   ```bash
   git clone https://github.com/RobertoUroz/Hero-And-Dragon.git
   ```

2. **Open in Unity:**
  - Launch **Unity Hub**.
  - Click **Add** and select the cloned `Hero-And-Dragon` folder.
  - Open the project in Unity Editor (recommended Unity version: 2020.3 LTS or later).

3. **Configure ARFoundation Settings:**
  - Ensure that ARFoundation and required AR packages are installed via the Package Manager.
  - Set up your project for the target platform (Android or iOS) through **File > Build Settings**.

4. **Build and Deploy:**
  - Connect your mobile device.
  - In **Build Settings**, select your target platform and click **Build and Run**.
  - Alternatively, build the APK (Android) or Xcode project (iOS) and deploy to your device.

## Usage

- **Deploy Characters:**  
  Use the in-app menu to deploy the hero and the dragon into your environment.

- **Interact with Characters:**  
  Tap on either character to trigger their respective animations.

- **Gesture Controls:**
  - **Pinch:** Scale the character.
  - **Swipe (horizontal):** Rotate the character.

- **Slider Controls:**  
  Adjust on-screen sliders to precisely rotate or scale the characters.

## Project Structure

- **Assets:**  
  Contains Unity scenes, 3D models, animations, textures, and scripts.

- **Scripts:**  
  Custom C# scripts handling AR interactions, gesture recognition, and UI logic.

- **Scenes:**  
  Unity scene files that set up the AR environment and UI.

- **Packages:**  
  Contains all project dependencies managed via Unity Package Manager.

## Credits & Acknowledgements

- **Developer:** [Roberto Uroz](https://github.com/RobertoUroz)
- **Inspiration:** Inspired by the legendary tale of Sant Jordi, merging medieval themes with modern AR technology.

## License

This project is open-source and available under the [MIT License](LICENSE).
