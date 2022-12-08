using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using CalamityMod.CalPlayer;
using CalRemix.NPCs.Bosses;
using CalamityMod.Buffs.Pets;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using CalamityMod.Items.TreasureBags;
using CalRemix.Items.Materials;
using System.Media;
using System.IO.Pipes;
using System.Reflection.Emit;
using System;
using MonoMod.Cil;
using Mono.Cecil.Cil;

// Hey, uh, Purified here. As of writing, all the code in this file is for IL editing, and is thus very sensitive.
// If you touch this stuff without knowing what you're doing it will cause the game to hard crash.
// Keep that in mind.

namespace CalRemix
{
    internal class CalRemixModSystem : ModSystem
    {
        public override void Load()
        {
            IL.Terraria.Player.ItemCheck_UseBossSpawners += HookDerellectSpawn;
            base.Load();
        }
        private static int implementation;
        private void HookDerellectSpawn(ILContext il)
        {

            var c = new ILCursor(il);
            if (!c.TryGotoNext(i => i.MatchLdcI4(134)))
                return;
            c.Index++;
            c.Emit(Mono.Cecil.Cil.OpCodes.Ldarg_0);
            c.EmitDelegate<Func<int, Player, int>>((id, player) =>
            {
                if (player.HasBuff(ModContent.BuffType<CalamityMod.Buffs.Pets.BloodBound>()))
                return ModContent.NPCType<NPCs.Bosses.DerellectBoss>();
                return id;
            });

            if (!c.TryGotoNext(i => i.MatchLdcR4(134f)))
                return;
            c.Index++;
            c.Emit(Mono.Cecil.Cil.OpCodes.Ldarg_0);
            c.EmitDelegate<Func<float, Player, float>>((id, player) =>
            {
                if (player.HasBuff(ModContent.BuffType<CalamityMod.Buffs.Pets.BloodBound>()))
                return ModContent.NPCType<NPCs.Bosses.DerellectBoss>();
                return id;
            });
        }
    }
}
