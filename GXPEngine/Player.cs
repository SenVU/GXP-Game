using GXPEngine;
using GXPEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

public class Player : AnimationSprite
{
    Camera camera = new Camera(0,0,800,600);

    bool firstRun = true;

    float startX;
    float startY;

    float speed =4.6f;
    float gravity = .25f;
    float termVel = 5f;
    float jumpPower = 4.2f;

    float yVel = 0;

    bool switchedWorld = false;


    public Player(string filename, int cols, int rows, TiledObject obj) : base(filename, cols, rows)
    {
        this.game.AddChild(camera);
    }

    void Update()
    {
        if (firstRun)
        {
            firstRun = false;
            startX = x;
            startY = y;
        }
        float xMove = 0;
        if (Input.GetKey(Key.A))
        {
            xMove -= speed;
        }
        if (Input.GetKey(Key.D))
        {
            xMove += speed;
        }
        yVel = Math.Min(yVel + gravity, termVel);
        if (Input.GetKey(Key.W) && hasJumpCol())
        {
            yVel = -jumpPower;
        }
        

        if (Input.GetKeyDown(Key.SPACE)) 
        {
            if (switchedWorld) { y -= 1600; }
            else { y += 1600; }
            switchedWorld = !switchedWorld;
        }
        MoveUntilCollision(xMove, 0);
        MoveUntilCollision(0, yVel);
        camera.x = Math.Max(x+80,400); camera.y = Math.Min(y, 1200 + (switchedWorld ? 1600 : 0));

        if (y > 1600 + (switchedWorld ? 1600 : 0))
        {
            switchedWorld = false;
            x = startX;
            y = startY;
        }
    }

    bool hasJumpCol()
    {
        bool toReturn = false;
        y += 1f;
        toReturn = this.GetCollisions(false, true).Length > 0;
        y -= 1f;
        return toReturn;
    }
}