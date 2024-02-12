using System;

public class Level{
    int levelID;
    String levelFilename;
    bool levelLoadLights;
    bool levelLoadVignette;
    bool levelSetColors;
    public Level(int ID, string filename, bool loadLights = true, bool loadVignette = true, bool setColors = true) {
        levelID = ID;
        levelFilename = filename;
        levelLoadLights = loadLights;
        levelLoadVignette = loadVignette;
        levelSetColors = setColors;
    }

    public int GetID() 
    { 
        return levelID; 
    }

    public string GetFileName()
    {
        return levelFilename;
    }

    public bool IsLevelLoadLights() {  
        return levelLoadLights; 
    }

    public bool IsLevelLoadVignette() {  
        return levelLoadVignette; 
    }

    public bool IsSetColors() { 
        return levelSetColors; 
    }

}

