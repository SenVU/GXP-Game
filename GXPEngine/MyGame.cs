using System;                                   // System contains a lot of default C# libraries 
using GXPEngine;                                // GXPEngine contains the engine
using System.Drawing;
using GXPEngine.Core;
using TiledMapParser;                           // System.Drawing contains drawing tools such as Color definitions

public class MyGame : Game
{
    //Level level = new Level("data/Tilemaps/testMap.xml", true,false,true);
    Level level = new Level("lvl_one.tmx", false, false, false);
    public MyGame() : base(800, 600, false, false, 800, 600, false)     // Create a window that's 800x600 and NOT fullscreen
    {
        LoadScene(level);
    }

    // For every game object, Update is called every frame, by the engine:
    void Update()
    {

    }

    static void Main()
    {
        new MyGame().Start();
        CollisionManager.TriggersOnlyOnCollision = true;
    }

    public void LoadScene(Level level)
    {
        LoadScene(level.getFileName(), level.isLevelLoadLights(), level.isLevelLoadVignette(), level.isSetColors());
    }

    // ___ LOADER TAKEN FROM LightsParticleDenoHandout ___ \\
    public void LoadScene(string filename, bool loadLights = true, bool loadVignette = true, bool setColors = true)
    {
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
        loader.addColliders = false;
        loader.LoadObjectGroups(0); // contains the player
        

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
}