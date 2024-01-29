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
    Camera camera = new Camera(0,0, 768, 432);

    Sound swapSound = new Sound("data/sound/swap.mp3");

    float speed =.3f;
    float gravity = .004f;
    float termVel = 9.8f;
    float jumpPower = .5f;

    float yVel = 0;

    bool switchedWorld = false;


    public Player(string filename, int cols, int rows, TiledObject obj) : base(filename, cols, rows)
    {
        this.game.AddChild(camera);
    }

    void Update()
    {
        float xMove = 0;
        if (Input.GetKey(Key.A))
        {
            xMove -= speed;
            Mirror(true, false);
        }
        if (Input.GetKey(Key.D))
        {
            xMove += speed;
            Mirror(false,false);
        }
        yVel = Math.Min(yVel + gravity, HasJumpCol() ? 0.1f : termVel);
        if (Input.GetKey (Key.W) && HasJumpCol())
        {
            yVel = -jumpPower;
        }
        

        if (Input.GetKeyDown(Key.SPACE)) 
        {
            if (switchedWorld) { y -= 1600; }
            else { y += 1600; }
            switchedWorld = !switchedWorld;
            swapSound.Play();
        }
        MoveUntilCollision(xMove*Time.deltaTime, 0);
        MoveUntilCollision(0, yVel*Time.deltaTime);
        camera.x = Math.Max(x+80,400); camera.y = Math.Min(y, 1100 + (switchedWorld ? 1600 : 0));

        if (y > 1600 + (switchedWorld ? 1600 : 0))
        {
            ((MyGame)(this.game)).reload();
        }
    }

    void OnCollision(GameObject obj)
    {
        CheckCoin(obj);
    }

    void CheckCoin(GameObject obj)
    {
        
        if (obj is Coin)
        {
            ((MyGame)(this.game)).CollectCoin();
            obj.LateDestroy();
        }
    }

    bool HasJumpCol()
    {
        bool toReturn = false;
        y += 1f;
        toReturn = this.GetCollisions(false, true).Length > 0;
        y -= 1f;
        return toReturn;
    }
}