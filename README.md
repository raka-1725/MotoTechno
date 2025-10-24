# MotoTechno

<img src="./Documents/MotoTechno Banner.png" width=1000>

---

* Engine Configuration

|Engine|Version|
|------|-------|
|Unity| 6000.1.17f1|

---

- [Overview](#overview)
- [Feature](#features)
- [Core Classes](#core-classes)
- [Features To Implement](#features-to-implement)
- [ScreenCaptures](#screen-captures)
- [Issues/Bugs](#issuesbugs)
- [Credits](#credits)

---

# Overview
The player drives/rides motorcycle with either First Person View or Third Person View, and race against other NPCs. The player will be awarded to point to customize their motorcycle, and they can change the stats of the vehicle as well.
NPC's are still in progress of development, therefore there is only solo mode as of now.

---

# Features
- ## MotorcycleController
  - A customizable motorcycle controller
  - Handles Input to the accel/brake, energy regen and steer of the motorcycle
  - Uses/Accepts Scriptable Object to change stats of the motorcycle
    
- ## MotorcycleCusomization
  - A scriptable object to change the stats of player motorcycle
  - Motor, Brake, Regen, Steer, Color are customizable

- ## NPC/AI
  - A NPC/AI trained by Machine Learning to give more competitiveness to the game.
  - Customizable number of NPC per race 

# Core-Classes
```MotorcycleController``` - A class that manages all the controll of the motorcycle including input and energy management of the motorcycle.
* Input Handling
* Vehicle Behaviour including energy management
* Cusomizable variables
* Exporting outputs to wheel collider

```MotorcycleCustomization``` - "ScriptableObject" - A script that contains various stats to make players able to change settings of the motorcycle. It has settings to change the strength of the motor/brake/regen etc...
* Scriptable Object


```MotorcycleUI``` - A class that controls and manages all the UI elements of motorcycle.
* Uses TextMeshProUGUI


```MotorcycleController_NPC``` - A class that manages all the controll of the NPC motorcycle. Requires all the components for ML-Agents.
* Config
  |Python|pip|ML-Agents|
  |------|---|---------|
  |3.10.11|1.1.0|3.0.0|

* Controls awarding/Punishing AI
* Handles Inputs for AI

```LapChecker``` - A class that counts every lap time and how many laps did player go. 
* Counts Lap/Lap time
* Each sector time is tracked

  
--more will be added--

---

# Features to Implement
Game start/stop feature, race start feature, more data for Machine Learning Agent to generate better NPC AI, store system to customize motorcycle.

---

# Screen Captures

<img src="./Documents/MT_Cap2.png" width=400>    <img src="./Documents/MT_Cap4.png" width=400>
<img src="./Documents/MT_Cap5.png" width=400>    <img src="./Documents/MT_Cap6.png" width=400>


---

# Issues/Bugs
No bugs yet

---

# Credits

Unity Engine, C#, Unity ML-Agent


