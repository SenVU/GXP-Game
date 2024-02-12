using GXPEngine;
using TiledMapParser;

public class Door : AnimationSprite
{
    public Door(string filename, int cols, int rows, TiledObject obj) : base(filename, cols, rows) 
    {
        collider.isTrigger = true;
    }
}
