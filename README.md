
# OpenGL Renderer in C#

  

This is a 3D OpenGL Renderer written in C#, using C++ OpenGL bindings for the rendering pipeline. The intention of this project was to learn how the OpenGL API works using C#, so it can be used in a future project, which means this project will only be used as a reference, and will not be updated anymore.

  

## Technologies

- [Eric Freed's C# glfw wrapper](https://github.com/ForeverZer0/glfw-net) for window, context and surfacescreation and management. 
- [Daniel Cronqvist's OpenGL Bindings](https://gist.github.com/dcronqvist/8e0c594532748e8fc21133ac6e3e8514/) for accessing the OpenGL API.
- [Thomas Müller's stbi-sharp](https://github.com/tom94/stbi-sharp) for image manipulation when dealing with textures.
- My own fork of [Dave Kerr's glmnet](https://github.com/vininew921/glmnet) for matrix manipulation.

## Screenshots
Here are a few screenshots of a plane being renderer with a texture, as well as a cube being used as a light source.

- Direct Light

![enter image description here](https://i.imgur.com/PzFo84D.png)

- Spot Light

![enter image description here](https://i.imgur.com/LzKklNY.png)

- Point Light

![enter image description here](https://i.imgur.com/TrHqwnX.png)