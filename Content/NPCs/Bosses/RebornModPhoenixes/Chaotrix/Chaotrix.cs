using CalamityMod;
using CalRemix.Core.World;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.NPCs.Bosses.RebornModPhoenixes.Chaotrix
{
    public class Chaotrix : PhoenixAbstract
    {
        public override int damage => 100;
        public override int defense => 50;
        public override int health => 130000;
        public override int projType => ModContent.ProjectileType<FireWaveBomb>();
        public override int dustType => DustID.Torch;

        public override void OnKill()
        {
            RemixDowned.downedChaotrix = true;

            string key = "The seal of the seas has been broken! You can now mine Scoria Ore.";
            Color messageColor = Color.Red;
            CalRemixHelper.BroadcastLocalizedText(key, messageColor);
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            //npcLoot.Add(ModContent.ItemType<HydrogenTrophy>(), 1); crest
            //npcLoot.Add(ModContent.ItemType<HydrogenTrophy>(), 1); acc
            npcLoot.Add(ModContent.ItemType<HydrothermalHurl>(), 2 / 3);
        }
    }
}
