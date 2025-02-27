using System.IO;
using CalRemix.Core.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CalRemix.Core
{
    public class WorldSaveSystem : ModSystem
    {
        public static bool HasDefeatedEgg
        {
            get;
            set;
        }

        public override void OnWorldLoad()
        {
            HasDefeatedEgg = false;
            NoxusEggCutsceneSystem.HasSummonedNoxus = false;
        }

        public override void OnWorldUnload()
        {
            HasDefeatedEgg = false;
            NoxusEggCutsceneSystem.HasSummonedNoxus = false;
        }

        public override void SaveWorldData(TagCompound tag)
        {
            if (HasDefeatedEgg)
                tag["HasDefeatedEgg"] = true;
            if (NoxusEggCutsceneSystem.HasSummonedNoxus)
                tag["HasSummonedNoxus"] = true;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            HasDefeatedEgg = tag.ContainsKey("HasDefeatedEgg");
            NoxusEggCutsceneSystem.HasSummonedNoxus = tag.ContainsKey("HasSummonedNoxus");
        }

        public override void NetSend(BinaryWriter writer)
        {
            BitsByte b1 = new();
            b1[0] = HasDefeatedEgg;
            b1[1] = NoxusEggCutsceneSystem.HasSummonedNoxus;

            writer.Write(b1);
        }

        public override void NetReceive(BinaryReader reader)
        {
            BitsByte b1 = reader.ReadByte();
            HasDefeatedEgg = b1[0];
            NoxusEggCutsceneSystem.HasSummonedNoxus = b1[1];
        }
    }
}
