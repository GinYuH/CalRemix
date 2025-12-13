using static Terraria.ModLoader.ModContent;
using Terraria;
using Terraria.ModLoader;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using System.Reflection;
using CalRemix.Core.Subworlds;
using CalRemix.UI;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.UI;
using CalamityMod;
using CalRemix.Content.NPCs.Bosses.Hydrogen;
using CalRemix.Content.NPCs.Eclipse;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using CalRemix.Core.World;
using Terraria.GameContent.UI.States;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader.UI;
using System.IO;
using CalRemix.UI.Anomaly109;
using CalamityMod.NPCs.HiveMind;
using CalamityMod.Events;
using CalamityMod.NPCs.TownNPCs;
using CalamityMod.NPCs.Perforator;
using CalRemix.UI.Title;
using CalRemix.Core.Scenes;
using CalRemix.World;
using MonoMod.RuntimeDetour;
using CalRemix.Content.Items.ZAccessories;
using CalamityMod.Items.Weapons.Rogue;
using CalRemix.Content.Items.Weapons;
using System.Diagnostics;
using Terraria.GameContent.Liquid;
using Terraria.Graphics.Light;
using SubworldLibrary;
using CalRemix.Content.Tiles;
using CalRemix.Content.Items.Armor;
using CalRemix.Content.Tiles.Subworlds.Horizon;
using System.Threading.Tasks.Dataflow;
using CalRemix.Content.Prefixes;
using CalRemix.Content.NPCs.Subworlds.Sealed;
using CalamityMod.NPCs.Cryogen;
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
            if (type == ModContent.TileType<ScoriaOre>())
                return RemixDowned.downedChaotrix;
            
            return base.CanKillTile(i, j, type, ref blockDamaged);
        }

        public override bool CanExplode(int i, int j, int type)
        {
            if (type == ModContent.TileType<ScoriaOre>())
                return RemixDowned.downedChaotrix;

            return base.CanExplode(i, j, type);
        }

        public override bool CanReplace(int i, int j, int type, int tileTypeBeingPlaced)
        {
            if (type == ModContent.TileType<ScoriaOre>())
                return RemixDowned.downedChaotrix;

            return base.CanReplace(i, j, type, tileTypeBeingPlaced);
        }
    }
}
