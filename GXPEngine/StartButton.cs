using GXPEngine;
using TiledMapParser;

public class StartButton : AnimationSprite
{
    public StartButton(string filename, int cols, int rows, TiledObject obj) : base(filename, cols, rows)
    {

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && HitTestPoint(Input.mouseX, Input.mouseY+1100-(game.height/2)))
        {
            ((MyGame)game).LoadNextLevel();
            x = x + 1;
        }
    }
}

