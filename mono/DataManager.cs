using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace mono
{
    public static class DM
    {
       static Dictionary<string, CharacterData> CharacterData = new Dictionary<string, CharacterData>();


        public static void ReadJSON()
        {
            string json = File.ReadAllText("data/Enemies");
            CharacterData data = JsonSerializer.Deserialize<CharacterData>(json);
            CharacterData.Add(data.name)
        }
    }

    public class CharacterData
    {
        string name;
        string texture;
        int speed;
        int maxHP;
        int range;
        int damage;
        int attackSpeed;
        int projectileSpeed;
    }
}
