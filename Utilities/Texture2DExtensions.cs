using UnityEngine;

namespace BetterMonitor.Utilities
{
    public static class Texture2DExtensions
    {
        public static Sprite ToSprite(this Texture2D texture,
                                      float          pixelsPerUnit = 100,
                                      SpriteMeshType meshType      = SpriteMeshType.Tight)
        {
            return Sprite.Create(texture,
                                 new Rect(0, 0, texture.width, texture.height),
                                 Vector2.one * 0.5f,
                                 pixelsPerUnit, 0, meshType);
        }
    }
}
