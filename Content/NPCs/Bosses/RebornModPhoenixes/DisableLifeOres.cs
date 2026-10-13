using static Terraria.ModLoader.ModContent;
using Terraria;
using Terraria.ModLoader;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using CalRemix.Core.World;
using CalamityMod.Tiles.Ores;

namespace CalRemix.Content.NPCs.Bosses.RebornModPhoenixes
{
    internal class DisableCryonicPerennialOres : ModSystem
    {
        public override void Load()
        {
            IL.CalamityMod.NPCs.Cryogen.Cryogen.OnKill += DisableCryonic;
            IL.CalamityMod.NPCs.CalamityGlobalNPC.OnKill += DisablePerennial;
        }

        public static void DisablePerennial(ILContext il)
        {
            var cursor = new ILCursor(il);
            if (!cursor.TryGotoNext(MoveType.After, i => i.MatchLdsfld<NPC>("downedPlantBoss")))
            {
                CalRemix.instance.Logger.Error("DisablePerennial: Could not find first downed");
                return;
            }
            if (!cursor.TryGotoNext(MoveType.Before, i => i.MatchLdsfld<NPC>("downedPlantBoss")))
            {
                CalRemix.instance.Logger.Error("DisablePerennial: Could not find second downed");
                return;
            }
            if (!cursor.TryGotoNext(i => i.OpCode.FlowControl == FlowControl.Cond_Branch))
            {
                CalRemix.instance.Logger.Error("DisablePerennial: Could not find conditional");
                return;
            }
            cursor.Next.OpCode = OpCodes.Brtrue;
        }

        public static void DisableCryonic(ILContext il)
        {
            var cursor = new ILCursor(il);
            if (!cursor.TryGotoNext(i => i.MatchCallOrCallvirt<CalamityMod.DownedBossSystem>("get_downedCryogen")))
            {
                CalRemix.instance.Logger.Error("DisableCryonic: Could not find downed");
                return;
            }
            if (!cursor.TryGotoNext(i => i.OpCode.FlowControl == FlowControl.Cond_Branch))
            {
                CalRemix.instance.Logger.Error("DisableCryonic: Could not find conditional");
                return;
            }
            cursor.Next.OpCode = OpCodes.Brtrue;
        }
    }

    public class DisableScoriaOre : GlobalTile
    {
        public override bool CanKillTile(int i, int j, int type, ref bool blockDamaged)
        {
            if (type == TileType<ScoriaOre>())
                return RemixDowned.downedChaotrix;
            
            return base.CanKillTile(i, j, type, ref blockDamaged);
        }

        public override bool CanExplode(int i, int j, int type)
        {
            if (type == TileType<ScoriaOre>())
                return RemixDowned.downedChaotrix;

            return base.CanExplode(i, j, type);
        }

        public override bool CanReplace(int i, int j, int type, int tileTypeBeingPlaced)
        {
            if (type == TileType<ScoriaOre>())
                return RemixDowned.downedChaotrix;

            return base.CanReplace(i, j, type, tileTypeBeingPlaced);
        }
    }
}
