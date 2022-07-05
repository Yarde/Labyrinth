# Galaxy Labyrinth
## _Maze traversal game with trivia educational theme_

[Play Here](https://yarde.github.io)

Engineering project for thesis:

_"Improving User Experience with the intelligent distribution of
trigger points in a procedurally generated dungeon"_

[Full work available here](https://drive.google.com/file/d/1Mk9INTGvit2bcKxxuW4uIkZ1EBHHcAPb/view?usp=sharing)

Project created with Unity3D and C#. Maze generation with _'Hunt and Kill'_ algorithm.

## Abstract
> Educational computer game that implements
a procedurally generated 3D maze intelligently populated with trigger points that trigger
questions when the player collides with them. While traversing the maze, the user will
find several trigger points with questions whose correct answers will lead to a better score.
The work aims to select appropriate methods for generating mazes and trigger points to
improve the user experience, motivate players using standard video game mechanics and
use the benefits of active learning.

## Tech

Solutions used to create this project:

- Unity3D, version 2020.3.4f1
- WebGL, for the build
- C# as a coding language
- Protobuf for client-server communication
- UniTask as async/await implementation
- DOTween for simple animations
- 
Not part of this repository:
- ASP.NET for server side
- Postgres as a database solution
- Python and multiple libraries for AI adaptive module
- Vue.js for web management portal

And of course Dillinger itself is open source with a [public repository][dill]
on GitHub.

## Installation

Galaxy maze requires Unity3D in version 2020.3.4f1

Open the scene and hit play, should work out of the box.

## License

MIT License

Copyright (c) 2021 Dawid Kaluża aka Yarde

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
