using GXPEngine;
using TiledMapParser;

public class Coin : AnimationSprite
{
    public Coin(string filename, int cols, int rows, TiledObject obj) : base(filename, cols, rows) 
    {
        collider.isTrigger = true;
    }
}
