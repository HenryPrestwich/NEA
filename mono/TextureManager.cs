using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace mono
{
    public static class TextureManager
    {
        public static Dictionary<string, Texture2D>? Textures { get; private set; }
        
        public static void CreateTextures()
        {
            Textures = new Dictionary<string, Texture2D>();

        }
        public static Texture2D GetTexture(string key)
        {
            if (Textures != null)
            {
                if (Textures.ContainsKey(key))
                {
                    return Textures[key];
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
