using mono.Entities;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace mono
{
    public static class DM
    {
        private static Dictionary<CharacterType, CharacterData> CharacterData;


        public static void ReadJSON()
        {
            string json = File.ReadAllText("C:\\1Code\\NEA\\mono\\Data\\Characters.json");

            JsonSerializerOptions options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter());
            

            List<CharacterData> characterdata = JsonSerializer.Deserialize<List<CharacterData>>(json, options);

            CharacterData = characterdata.ToDictionary(e => e.Name);
        }

        public static CharacterData GetData(CharacterType name)
        {
            return CharacterData[name];
        }
    }

    public class CharacterData
    {
        public CharacterType Name { get; set; }
        public string Texture { get; set; }
        public int Speed { get; set; }
        public int MaxHP { get; set; }
        public int Range { get; set; }
        public int Damage { get; set; }
        public int AttackSpeed { get; set; }
        public int ProjectileSpeed { get; set; }
    }
}
