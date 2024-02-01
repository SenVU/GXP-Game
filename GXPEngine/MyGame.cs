using System;                                   // System contains a lot of default C# libraries 
using GXPEngine;                                // GXPEngine contains the engine
using System.Drawing;
using GXPEngine.Core;
using TiledMapParser;
using System.Collections.Generic;                           // System.Drawing contains drawing tools such as Color definitions

public class MyGame : Game
{
    Level currentLevel;
    Level mainScreen = new Level(0, "mainscreen.tmx", true, false, false);
    Level levelOne = new Level(1, "lvl_one.tmx", true, false, false);
    Level levelTwo = new Level(2, "lvl_two.tmx", true, false, false);
    Level endScreen = new Level(0, "endScreen.tmx", true, false, false);

    List<Level> levelOrder = new List<Level>();

    Sound coinCollectSound = new Sound("data/sound/coin.mp3");

    Camera UICam;
    EasyDraw uiText;

    int coinsCollected = 0;

    int coinsAtLevelStart = 0;

    int swapsUsed = 0;

    bool toReload = false;
    bool toLoadNext = false;
    public MyGame() : base(768, 432, false, false, 1920, 1080, true)
    {
        targetFps = 1000;
        CollisionManager.TriggersOnlyOnCollision = true;

        levelOrder.Add(mainScreen);
        levelOrder.Add(levelOne);
        levelOrder.Add(levelTwo);
        levelOrder.Add(endScreen);

        currentLevel = levelOrder[0];

        LoadScene(currentLevel);
    }

    void Update()
    {
        if (currentLevel.GetID() != 0)
        {
            uiText.TextSize(10);
            uiText.TextAlign(CenterMode.Min, CenterMode.Min);
            uiText.Text("Coins: " + coinsCollected +
                        "\nSwaps: " + swapsUsed, true);
        }

        if (toReload)
        {
            Reload();
            toReload = false;
        }
        if (toLoadNext)
        {
            LoadNextLevel();
            toLoadNext = false;
        }
    }

    public void Reload()
    {
        LoadScene(currentLevel);
        coinsCollected = coinsAtLevelStart;
    }

    public void LateReload()
    {
        toReload = true;
    }

    public void LateLoadNextLevel()
    {
        toLoadNext = true;
    }

    public void LoadNextLevel()
    {
        int i = 0;
        foreach(Level level in levelOrder)
        {
            if (i+1==levelOrder.Count)
            {
                return;
            }
            if (currentLevel == level)
            {
                currentLevel = levelOrder[i + 1];
                coinsAtLevelStart = coinsCollected;
                Reload();
                return;
            }
            i++;
        }
        
    }

    void LoadUI() 
    {
        UICam = new Camera(0,0, 200, 100,false);
        uiText = new EasyDraw(200, 100, false);
        AddChild(UICam);
        AddChild(uiText);
        UICam.SetXY(-100, -50);
        uiText.SetXY(-200, -100);
    }

    public void CollectCoin()
    {
        coinsCollected++;
        coinCollectSound.Play();
    }

    public void addUsedSwap()
    {
        if (currentLevel.GetID() != 0) { swapsUsed++; }
    }
    public Level getCurrentLevel() { return currentLevel; }

    public void LoadScene(Level level)
    {
        LoadScene(level.GetFileName(), level.IsLevelLoadLights(), level.IsLevelLoadVignette(), level.IsSetColors());
        if (level.GetID() != 0) { LoadUI(); }
    }

    // ___ LOADER TAKEN FROM LightsParticleDemoHandout ADN EDITED___ \\
    public void LoadScene(string filename, bool loadLights = true, bool loadVignette = true, bool setColors = true)
    {
        UnloadScene();
        TiledLoader loader = new TiledLoader(filename);
        loader.autoInstance = true;

        // ------------- Convention (render order):

        // image layer 1 = background, lit
        // tile layer 0: normal tiles, lit (=main layer)
        // object layer 0: normal objects (e.g. player), rendered with main layer

        // object layer 1: lights. BlendMode.LIGHTING

        // image layer 0 = background, unlit: BlendMode.FILLEMPTY (+rendered after normal lights!)

        // object layer 2: "volumetric" lights. BlendMode.ADDITIVE

        // tile layer 1: foreground tiles, unlit (=rendered after lights)

        // object layer 3: darkening (vignette). BlendMode.MULTIPLY

        // background, lit:
        loader.addColliders = false;
        Pivot background = new Pivot();
        AddChild(background);
        loader.rootObject = background;
        loader.LoadImageLayers(1);
        if (setColors)
        {
            ((Sprite)background.GetChildren()[0]).SetColor(0.5f, 0.5f, 0.5f);
        }

        // main layer, lit:
        Pivot mainlayer = new Pivot();
        AddChild(mainlayer);
        loader.rootObject = mainlayer;
        loader.addColliders = true;
        loader.LoadTileLayers(0);
        loader.LoadObjectGroups(0); // contains the player and other objects
        loader.addColliders = false;

        // lights:
        if (loadLights)
        {
            loader.LoadObjectGroups(1); // contains normal lights
        }

        // background, unlit:
        Pivot unlit = new Pivot();
        AddChild(unlit);
        loader.rootObject = unlit;
        loader.LoadImageLayers(0);
        ((Sprite)unlit.GetChildren()[0]).blendMode = BlendMode.FILLEMPTY;

        loader.rootObject = this;
        if (loadLights)
        {
            loader.LoadObjectGroups(2); // "volumetric" lighting (additive)
        }

        // foreground:
        // (A SpriteBatch is used to easily set the color of all sprites with one command - see SpriteBatch.cs for more info)
        SpriteBatch foreground = new SpriteBatch();
        AddChild(foreground);
        loader.rootObject = foreground;
        loader.LoadTileLayers(1);
        foreground.Freeze();
        if (setColors)
        {
            foreground.SetColor(0.4f, 0.4f, 0.4f);
        }

        loader.rootObject = this;
        if (loadVignette)
        {
            loader.LoadObjectGroups(3); // vignette (multiply)
        }
    }

    void UnloadScene()
    {
        foreach(GameObject child in this.GetChildren())
        {
            child.LateDestroy();
        }
        
    }


    static void Main()
    {
        new MyGame().Start();
    }
}