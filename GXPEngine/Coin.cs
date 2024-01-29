using GXPEngine;
using GXPEngine.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

public class Coin : AnimationSprite
{
    public Coin(string filename, int cols, int rows, TiledObject obj) : base(filename, cols, rows) 
    {
        collider.isTrigger = true;
    }
}
