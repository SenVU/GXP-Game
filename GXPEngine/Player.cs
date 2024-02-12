using GXPEngine;
using System;
using TiledMapParser;

public class Player : AnimationSprite
{
    bool LevelEndCollisionHit = false;

    Camera camera = new Camera(0,0, 768, 432);

    Sound swapSound = new Sound("data/sound/swap.mp3");

    // settings
    bool wallJump = true;

    float speed =.19f;
    float gravity = .7f;
    float termVel = 9.8f;
    float jumpPower = .22f;
    float wallJumpPower = .27f;

    float wallSlidingVel = 0.08f;

    int worldDistance = 1600;

    int playerDiesBelow = 1600;

    //end of settings
    enum WallJumpSide { none=0, left=1, right=2 }

    WallJumpSide currentWallJumpSide = WallJumpSide.none;
    WallJumpSide lastWallJumpSide = WallJumpSide.none;
    float yVel = 0;

    bool switchedWorld = false;


    public Player(string filename, int cols, int rows, TiledObject obj) : base(filename, cols, rows)
    {
        this.game.AddChild(camera);
    }

    void Update()
    {
        float xVel = 0;
        if (Input.GetKey(Key.A))
        {
            xVel -= speed;
            Mirror(true, false);
        }
        if (Input.GetKey(Key.D))
        {
            xVel += speed;
            Mirror(false, false);
        }
        yVel = Math.Min(yVel + gravity*(Time.deltaTime/1000f), HasJumpColAbs(false) ? wallSlidingVel : termVel);
        if (Input.GetKey (Key.W) && HasJumpCol(false))
        {
            yVel = currentWallJumpSide == WallJumpSide.none ? - jumpPower : -wallJumpPower;
            lastWallJumpSide = currentWallJumpSide;
        }
        

        if (Input.GetKeyDown(Key.SPACE)) 
        {
            if (switchedWorld) { y -= worldDistance; }
            else { y += 1600; }
            switchedWorld = !switchedWorld;
            swapSound.Play();
            ((MyGame)game).addUsedSwap();
        }
        MoveUntilCollision(xVel*Time.deltaTime, 0);
        MoveUntilCollision(0, yVel*Time.deltaTime);

        // camera controll
        if (((MyGame)game).getCurrentLevel().GetID() == 0)
        {
            camera.x = 400; camera.y = yCameraLowestHeight + (switchedWorld ? worldDistance : 0);
        } else
        {
            smoothCameraLookAhead(xVel, yVel);
        }

        if (y > playerDiesBelow + (switchedWorld ? worldDistance : 0))
        {
            ((MyGame)(this.game)).Reload();
        }
    }
    float xSmoothOrigin = 0;
    float ySmoothOrigin = 0;

    float xTimer = 0;
    float yTimer = 0;

    float yCameraLowestHeight = 1100;
    void smoothCameraLookAhead(float xVel, float yVel)
    {
        float cameraMaxDistance = 50f;
        float cameraSpeedX = .007f;
        float cameraSpeedY = .02f;

        float xOrigin = 120;
        
        float xMin = 400;

        int timeToActivate = 500;

        xTimer += (xVel / speed) * Time.deltaTime;
        if ((xVel > 0 && xTimer <0) || (xVel < 0 && xTimer > 0) || xVel==0) { xTimer = 0; }
        yTimer += (xVel / speed) * Time.deltaTime;
        if ((yVel > 0 && !HasJumpColAbs(true) && yTimer < 0) || (yVel < 0 && yTimer > 0) || yVel == 0) { yTimer = 0; }

        if (Math.Abs(xTimer)>timeToActivate) {
            xSmoothOrigin = lerp(xSmoothOrigin, (xVel / speed) * cameraMaxDistance, cameraSpeedX);
        } else
        {
            xSmoothOrigin = lerp(xSmoothOrigin, 0, cameraSpeedX);
        }
        if (Math.Abs(yTimer) > timeToActivate)
        {
            ySmoothOrigin = lerp(xSmoothOrigin, (yVel / termVel) * cameraMaxDistance, cameraSpeedY);
        } else
        {
            ySmoothOrigin = lerp(xSmoothOrigin, 0, cameraSpeedY);
        }

        //camera.x = lerp(camera.x, Math.Max(x + xSmoothOrigin + xOrigin, xMin), 0.1f);
        //camera.y = lerp(camera.y, Math.Min(y + ySmoothOrigin, yCameraLowestHeight + (switchedWorld ? worldDistance : 0)), 0.1f);

        camera.x = Math.Max(x + xSmoothOrigin + xOrigin, xMin); 
        camera.y = Math.Min(y + ySmoothOrigin, yCameraLowestHeight + (switchedWorld ? worldDistance : 0));
    }

    float lerp(float a, float b, float f)
    {
        return a + f * (b - a);
    }

    void OnCollision(GameObject obj)
    {
        if (LevelEndCollisionHit) return;
        if (obj is Coin)
        {
            ((MyGame)(this.game)).CollectCoin();
            obj.LateDestroy();
        }
        else if (obj is Spike)
        {
            ((MyGame)(this.game)).LateReload();
            LevelEndCollisionHit = true;
        }
        else if (obj is Door)
        {
            ((MyGame)(this.game)).LateLoadNextLevel();
            LevelEndCollisionHit = true;
        }
    }


    bool HasJumpCol(bool ignoreWalljump)
    {
        bool toReturn = false;
        y += 1f;
        if(this.GetCollisions(false, true).Length > 0)
        {
            toReturn = true;
            currentWallJumpSide = WallJumpSide.none;
        }
        y -= 1f;
        if (!toReturn && !ignoreWalljump && wallJump)
        {
            x += 1f;
            if (this.GetCollisions(false, true).Length > 0 && !(lastWallJumpSide == WallJumpSide.right))
            {
                toReturn = true;
                currentWallJumpSide = WallJumpSide.right;
            }
            x -= 2f;
            if (this.GetCollisions(false, true).Length > 0 && !(lastWallJumpSide==WallJumpSide.left))
            {
                toReturn = true;
                currentWallJumpSide = WallJumpSide.left;
            }
            x += 1f;
        }
        return toReturn;
    }

    bool HasJumpColAbs(bool ignoreWalljump)
    {
        bool toReturn = false;
        y += 1f;
        if (this.GetCollisions(false, true).Length > 0)
        {
            toReturn = true;
        }
        y -= 1f;
        if (!ignoreWalljump && wallJump)
        {
            x += 1f;
            if (this.GetCollisions(false, true).Length > 0)
            {
                toReturn = true;
            }
            x -= 2f;
            if (this.GetCollisions(false, true).Length > 0)
            {
                toReturn = true;
            }
            x += 1f;
        }
        return toReturn;
    }
}