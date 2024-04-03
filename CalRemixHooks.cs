using Terraria;
using Terraria.ModLoader;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using System.Reflection;

namespace CalRemix
{
    internal class CalRemixHooks : ModSystem
    {
        public override void Load()
        {
            IL.CalamityMod.Events.AcidRainEvent.TryStartEvent += AcidsighterToggle;
            IL.CalamityMod.Events.AcidRainEvent.TryToStartEventNaturally += AcidsighterToggle2;
            //IL_Player.ItemCheck_UseBossSpawners += HookDerellectSpawn;
        }
        private static void AcidsighterToggle(ILContext il)
        {
            var c = new ILCursor(il);
            var d = typeof(NPC).GetField("downedBoss1", BindingFlags.Public | BindingFlags.Static);
            if (c.TryGotoNext(i => i.MatchLdsfld(d)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.npcChanges ? d : typeof(CalRemixWorld).GetField("downedAcidsighter", BindingFlags.Public | BindingFlags.Static));
            }
        }
        private static void AcidsighterToggle2(ILContext il)
        {
            var c = new ILCursor(il);
            var d = typeof(NPC).GetField("downedBoss1", BindingFlags.Public | BindingFlags.Static);
            if (c.TryGotoNext(i => i.MatchLdsfld(d)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.npcChanges ? d : typeof(CalRemixWorld).GetField("downedAcidsighter", BindingFlags.Public | BindingFlags.Static));
            }
        }
        /*
        private static void HookDerellectSpawn(ILContext il)
        {
            // Hey, uh, Purified here. As of writing, all the code in this file is for IL editing, and is thus very sensitive.
            // If you touch this stuff without knowing what you're doing it will cause the game to hard crash.
            // Keep that in mind.
            var c = new ILCursor(il);
            if (!c.TryGotoNext(i => i.MatchLdcI4(134)))
                return;
            c.Index++;
            c.Emit(Mono.Cecil.Cil.OpCodes.Ldarg_0);
            c.EmitDelegate<Func<int, Player, int>>((id, player) =>
            {
                if (player.HasBuff(ModContent.BuffType<BloodBound>()))
                return ModContent.NPCType<DerellectBoss>();
                return id;
            });

            if (!c.TryGotoNext(i => i.MatchLdcR4(134f)))
                return;
            c.Index++;
            c.Emit(Mono.Cecil.Cil.OpCodes.Ldarg_0);
            c.EmitDelegate<Func<float, Player, float>>((id, player) =>
            {
                if (player.HasBuff(ModContent.BuffType<BloodBound>()))
                return ModContent.NPCType<DerellectBoss>();
                return id;
            });
        }
        */
    }
}
