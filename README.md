# Switch-Block-Nokia3310Jam
Source code for my submission to the 3rd Nokia3310 game jam made in C# with the monogame framework


The Game1.cs contains the primary loop of the game, such as the update and draw functions.

The Tiled Folder/Files is a class I created for reading tilemap information from a .tmx file(the output of the stilemap building software Tiled)
It reads the xml and contructs the data structures to contain the information for a tilemap, such as the location of tiles, colliders etc.

Animation contains simple classes for scrolling/static backgrounds

Managers/Collision Manager handles collisions between colliders on the map and any sprite

Sprite is mainly just for the player, I originally intended for there to be more sprites.

Control/Camera handles following a sprite with a transform matrix that stays within map bounds,
as well as scaling the 84x48 resolution up properly so there is no stretching etc.

Controls/Input is to allow for multiple buttons to be registered as the same input 
