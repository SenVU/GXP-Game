using GXPEngine;
using TiledMapParser;

class Light : AnimationSprite
{
    public Light(string filename, int cols, int rows, TiledObject obj) : base(filename, cols, rows, -1, false, false)
    {
        switch (obj.GetIntProperty("type", 0))
        {
            case 0:
                blendMode = BlendMode.LIGHTING; // normal lighting
                break;
            case 1:
                blendMode = BlendMode.ADDITIVE; // for "volumetric" lighting effects
                break;
            case 2:
                blendMode = BlendMode.MULTIPLY; // make darker (e.g. vignette)
                break;
        }
        if (obj.HasProperty("color", "color"))
        {
            color = obj.GetColorProperty("color") >> 8;
        }
    }
}
