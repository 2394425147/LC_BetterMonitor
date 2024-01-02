using UnityEngine;

namespace BetterMonitor.Utilities
{
    public static class BytesExtensions
    {
        public static Sprite ToSprite(this byte[]    bytes,
                                      int            width         = 256,
                                      int            height        = 256,
                                      float          pixelsPerUnit = 100,
                                      SpriteMeshType meshType      = SpriteMeshType.Tight)
        {
            var texture2D = bytes.ToTexture2D(width, height);
            return texture2D.ToSprite(pixelsPerUnit, meshType);
        }

        public static Texture2D ToTexture2D(this byte[] bytes, int width = 256, int height = 256)
        {
            // Create new "empty" texture
            var texture2D = new Texture2D(width, height);
            return texture2D.LoadImage(bytes) ? texture2D : null;
        }
    }
}
