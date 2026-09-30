using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace mono
{
    public static class TextureManager
    {
        public static Dictionary<string, Texture2D>? Textures { get; private set; }
        
        public static void CreateTextures(ContentManager content)
        {
            Textures = new Dictionary<string, Texture2D>();

            Textures.Add("enemy", content.Load<Texture2D>("enemy"));
            Textures.Add("player", content.Load<Texture2D>("player"));
            Textures.Add("wall", content.Load<Texture2D>("wall"));
            Textures.Add("grass", content.Load<Texture2D>("grass"));
            Textures.Add("projectile", content.Load<Texture2D>("knife"));
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
