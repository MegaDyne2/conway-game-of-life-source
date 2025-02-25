# Conway's Game of Life - Unity & Console Versions

## Overview
This project includes two separate implementations of **Conway’s Game of Life**:
- A **Unity version** with an interactive UI and customizable settings.
- A **Windows Command Prompt version** that runs directly in the console.

Both implementations follow the core rules of Conway’s Game of Life and emphasize modularity, efficiency, and user customization.

---

## File Structure

### **Main Files**
- **`Main.cs`** – The core script used for the Unity version.
- **`Program.cs`** – The source code for the Windows Command Prompt version.

### **Project Archives**
- **`ConwayGameOfLifeUnity.zip`** – Complete Unity project files.
- **`Conway_GameOfLife_Project.rar`** – Full source code archive.

---

## Unity Version

### **Features**
- **Board Customization**:
  - Adjustable board size (**8x8 to 50x50**).
  - Adjustable cell size (**25 to 99 pixels**).
- **Pattern Support**:
  - **True Random**
  - **Test Random**
  - **Blinker**
  - **Toad**
  - **Beacon**
  - **Glider**
- **Simulation Control**:
  - Click cells to toggle alive/dead.
  - Step through generations manually.
  - Enable auto-run with a customizable timer (**0.1s to 3s per step**).

### **How to Use**
1. Extract **`ConwayGameOfLifeUnity.zip`** and open the Unity project.
2. Adjust the board settings using the UI.
3. Choose a pattern or manually set live/dead cells.
4. Click **Next Generation** to step forward or enable auto-run.

---

## Windows Command Prompt Version - Source code only

### **Features**
- **Runs in a Console Window** with a **text-based grid representation**.
- **Supports predefined patterns**:
  - **RANDOM** – Default if no pattern is specified.
  - **TRUE RANDOM** – Generates a completely random board.
  - **BLINKER** – A three-cell vertical oscillator.
  - **TOAD** – A period-2 oscillator.
  - **BEACON** – A four-cell block oscillator.
- **Customizable Settings via Command-Line Arguments**:
  - `Arg[0]` - **BoardName** *(Defaults to RANDOM if not specified, case-insensitive)*  
    ```
    Available Keys: RANDOM, TRUE RANDOM, BLINKER, TOAD, BEACON
    ```
  - `Arg[1]` - **Use Test Random Example** *(1 = enable)*
  - `Arg[2]` - **Generational Count** *(Minimum = 4)*
  - `Arg[3]` - **Board Height** *(Minimum = 8)*
  - `Arg[4]` - **Board Width** *(Minimum = 8)*
  - `Arg[5]` - **Wait For Key Press Between Generations** *(1 = on)*
