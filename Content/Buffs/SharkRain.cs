using CalamityMod.NPCs.Abyss;
using CalamityMod.NPCs.AcidRain;
using CalamityMod.NPCs.Astral;
using CalamityMod.NPCs.GreatSandShark;
using CalRemix.Content.NPCs.Subworlds.GreatSea;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace CalRemix.Content.Buffs
{
    public class SharkRain : ModBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            if (Main.rand.NextBool(22))
            {
                if (NPC.CountNPCS(NPCID.Shark) < 20)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        WeightedRandom<int> sharkTypes = new WeightedRandom<int>();
                        sharkTypes.Add(NPCID.Shark, 5);
                        sharkTypes.Add(NPCID.SandShark, 0.01f);
                        sharkTypes.Add(NPCID.SandsharkCorrupt, 0.01f);
                        sharkTypes.Add(NPCID.SandsharkCrimson, 0.01f);
                        sharkTypes.Add(NPCID.SandsharkHallow, 0.01f);
                        sharkTypes.Add(NPCID.GoblinShark, 0.0075f);
                        sharkTypes.Add(NPCID.Sharkron, 0.0075f);
                        sharkTypes.Add(NPCID.Sharkron2, 0.0075f);
                        sharkTypes.Add(ModContent.NPCType<BullShark>(), 0.005f);
                        sharkTypes.Add(ModContent.NPCType<FusionFeeder>(), 0.01f);
                        sharkTypes.Add(ModContent.NPCType<ReaperShark>(), 0.001f);
                        sharkTypes.Add(ModContent.NPCType<GreatSandShark>(), 0.001f);
                        sharkTypes.Add(ModContent.NPCType<Mauler>(), 0.001f);
                        NPC.NewNPC(player.GetSource_Buff(buffIndex), (int)player.Center.X + Main.rand.Next(-2000, 2000), (int)player.Center.Y - 800 + Main.rand.Next(-100, 100), sharkTypes.Get());
                    }
                }
            }
        }
    }
}
