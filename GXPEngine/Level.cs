using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

public class Level{
    String levelFilename;
    bool levelLoadLights;
    bool levelLoadVignette;
    bool levelSetColors;
    public Level(string filename, bool loadLights = true, bool loadVignette = true, bool setColors = true) {
        levelFilename = filename;
        levelLoadLights = loadLights;
        levelLoadVignette = loadVignette;
        levelSetColors = setColors;
    }

    public string getFileName()
    {
        return levelFilename;
    }

    public bool isLevelLoadLights() {  
        return levelLoadLights; 
    }

    public bool isLevelLoadVignette() {  
        return levelLoadVignette; 
    }

    public bool isSetColors() { 
        return levelSetColors; 
    }

}

