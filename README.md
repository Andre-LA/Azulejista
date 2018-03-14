# Azulejista
Tool for creating tiles in Unity.

![Azulejista (iso) example](https://i.imgur.com/0LVpAdc.png)

## Introduction
Azulejista consists of 3 components:

* Azulejista Tile Maker;
* Azulejista Tile Selector;
* and Azulejista Tile Map.

### Azulejista Tile Maker
![Azulejista Tile Maker Screen Shot](https://i.imgur.com/mJhWxvn.png)

Azulejista Tile Maker is who creates the gameobjects of the tiles, it depends on 3 components:

* Azulejista Tile Map, which contains the map data;
* Transform, who will be the father of tiles;
* Azulejista Tile Selector, which will tell Azulejista Tile Maker which gameobject should be instantiated (auto tile).

For each space on a map line, the Azulejista Tile Maker will walk according to stepX, when finishing the line, it will return to the beginning of the map and walk the stepY * number of the next line, so you can customize the position of instancing of the Azulejista Tile Maker just changing the steps:

![Azulejista Tile Maker Demo: Default Tile Map](https://i.imgur.com/Cp2yAEP.png)  
![Azulejista Tile Maker Demo: Isometric Tile Map](https://i.imgur.com/CTKb1cK.png)

For each possible space, a zStep step (in z axis) will be taken to customize the depth, which is useful to ensure that the tiles are rendered in the correct order. 

**Generate Map** button will create the gameobjects in the editor itself, useful for already leaving the scene prepared, preventing gameobjects from being instantiated at runtime.
