# MAZE GENERATOR
- Author: **James**
- Created: 2024-04
- Resources: **Maze Generating in Unity Tutorial** (https://www.youtube.com/watch?v=ya1HyptE5uc&list=WL&index=3&t=255s)

## Summary
A WPF C# project that generates a random recursive maze.

![Screenshot of Application](https://github.com/jamestechpublic/MazeGenerator/blob/master/Screenshot%202024-04-18%20103418.png)

## Details
- Contains one static class "MazeGen2" that returns a 2D array of your maze size (columns and rows).
- Each node in the array contains WallStatus flags to indicate which of its four walls are enabled.

## Notes
- The WPF Canvas element draws the maze flipped horizontally 'cause 0,0 is at the bottom left.
- There is no start and end specified. Simply choose nodes, like in the corners, and disabled the appropriate wallstatus to open a wall.
- The wall states are binary flags. Add and remove walls for a node as follows:
  - maze[10,10] |= WallStatus.LEFT   // enables a wall (OR)
  - maze[10,10] &= ~WallStatus.RIGHT  // disables a wall (AND NOT). remember the not ~ infront to inverse the bits
