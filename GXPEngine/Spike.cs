using GXPEngine;
using TiledMapParser;

public class Spike : AnimationSprite
{
    public Spike(string filename, int cols, int rows, TiledObject obj) : base(filename, cols, rows) 
    {
        collider.isTrigger = true;
    }
}
