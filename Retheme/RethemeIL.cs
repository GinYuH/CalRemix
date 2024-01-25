using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityMod.Events;
using Terraria.GameContent;
using static Terraria.ModLoader.ModContent;
using CalamityMod.NPCs.Providence;
using CalamityMod.NPCs.AstrumAureus;
using System.Reflection;
using MonoMod.Cil;
using Terraria.GameContent.Bestiary;
using Mono.Cecil.Cil;
using CalamityMod.NPCs;
using CalamityMod.NPCs.Bumblebirb;
using CalamityMod.Projectiles.Magic;
using System;
using CalamityMod.Projectiles.Rogue;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Ranged;

namespace CalRemix.Retheme
{
    public class RethemeIL : ModSystem
    {
        public override void Load()
        {
            // IL.CalamityMod.NPCs.PreDraw += ;
            IL.CalamityMod.NPCs.Crabulon.Crabulon.PreDraw += Crabulon;
            IL.CalamityMod.NPCs.HiveMind.HiveMind.PreDraw += HiveMind;
            IL.CalamityMod.NPCs.Perforator.PerforatorCyst.PreDraw += PerforatorCyst;
            #region PerfWormHeck
            IL.CalamityMod.NPCs.Perforator.PerforatorBodyLarge.PreDraw += PerfLBody;
            IL.CalamityMod.NPCs.Perforator.PerforatorBodyMedium.PreDraw += PerfMBody;
            IL.CalamityMod.NPCs.Perforator.PerforatorBodySmall.PreDraw += PerfSBody;
            IL.CalamityMod.NPCs.Perforator.PerforatorHeadLarge.PreDraw += PerfLHead;
            IL.CalamityMod.NPCs.Perforator.PerforatorHeadMedium.PreDraw += PerfMHead;
            IL.CalamityMod.NPCs.Perforator.PerforatorHeadSmall.PreDraw += PerfSHead;
            IL.CalamityMod.NPCs.Perforator.PerforatorTailLarge.PreDraw += PerfLTail;
            IL.CalamityMod.NPCs.Perforator.PerforatorTailMedium.PreDraw += PerfMTail;
            IL.CalamityMod.NPCs.Perforator.PerforatorTailSmall.PreDraw += PerfSTail;
            #endregion
            IL.CalamityMod.NPCs.Perforator.PerforatorHive.PreDraw += PerforatorHive;
            IL.CalamityMod.NPCs.Cryogen.Cryogen.PreDraw += Cryogen;
            IL.CalamityMod.NPCs.Cryogen.CryogenShield.PreDraw += CryogenShield;
            IL.CalamityMod.NPCs.CalClone.CalamitasClone.PreDraw += CalamitasClone;
            IL.CalamityMod.NPCs.CalClone.Cataclysm.PreDraw += Cataclysm;
            IL.CalamityMod.NPCs.CalClone.Catastrophe.PreDraw += Catastrophe;
            IL.CalamityMod.NPCs.Leviathan.Leviathan.SetStaticDefaults += Leviathan;
            IL.CalamityMod.NPCs.Leviathan.Anahita.PreDraw += Anahita;
            MonoModHooks.Modify(typeof(CalamityAI).GetMethod("AstrumAureusAI", BindingFlags.Public | BindingFlags.Static), AureusAI);
            IL.CalamityMod.NPCs.AstrumAureus.AstrumAureus.PreDraw += AstrumAureus;
            IL.CalamityMod.NPCs.AstrumAureus.AureusSpawn.PreDraw += AureusSpawn;
            //IL.CalamityMod.NPCs.PlaguebringerGoliath.PlaguebringerGoliath.PreDraw += PBG;
            IL.CalamityMod.NPCs.AstrumDeus.AstrumDeusHead.PreDraw += AstrumDeusHead;
            IL.CalamityMod.NPCs.AstrumDeus.AstrumDeusBody.PreDraw += AstrumDeusBody;
            IL.CalamityMod.NPCs.AstrumDeus.AstrumDeusTail.PreDraw += AstrumDeusTail;
            MonoModHooks.Modify(typeof(CalamityAI).GetMethod("BumblebirbAI", BindingFlags.Public | BindingFlags.Static), BirbAI);
            IL.CalamityMod.NPCs.Bumblebirb.Bumblefuck.PreDraw += BirbDraw;
            IL.CalamityMod.NPCs.NormalNPCs.WildBumblefuck.SpawnChance += BirbSpawn;
            IL.CalamityMod.NPCs.Bumblebirb.Bumblefuck.SetBestiary += BirbBest;
            IL.CalamityMod.NPCs.Yharon.Yharon.PreDraw += Yharon;
            MonoModHooks.Modify(typeof(Providence).GetMethod("<PreDraw>g__drawProvidenceInstance|48_0", BindingFlags.NonPublic | BindingFlags.Instance), Providence);
            //MonoModHooks.Modify(typeof(ModLoader).Assembly.GetType("CalamityMod.WeakReferenceSupport").GetMethod("AddCalamityBosses", BindingFlags.NonPublic | BindingFlags.Static), BossChecklist);

            // IL.CalamityMod.Items.Weapons.PreDraw += ;
            On.CalamityMod.Items.SummonItems.ExoticPheromones.CanUseItem += Exotic;
            On.CalamityMod.Items.SummonItems.AstralChunk.CanUseItem += AstralChunk;
            IL.CalamityMod.Items.Weapons.Ranged.HeavenlyGale.PostDrawInWorld += HeavenlyGale;

            // IL.CalamityMod.Projectiles.PreDraw += ;
            IL.CalamityMod.Projectiles.Rogue.InfestedClawmerangProj.PreDraw += InfestedClawmerangProj;
            IL.CalamityMod.Projectiles.Summon.DankCreeperMinion.PreDraw += DankCreeperMinion;
            MonoModHooks.Modify(typeof(RotBallProjectile).GetMethod("get_Texture", BindingFlags.Public | BindingFlags.Instance), RotBallProjectile);
            IL.CalamityMod.Projectiles.Ranged.BloodClotFriendly.AI += BloodClotFriendly;
            IL.CalamityMod.Projectiles.Magic.BloodBeam.AI += BloodBeam;
            MonoModHooks.Modify(typeof(ToothBallProjectile).GetMethod("get_Texture", BindingFlags.Public | BindingFlags.Instance), ToothBallProjectile);
            IL.CalamityMod.Projectiles.Magic.EldritchTentacle.AI += EldritchTentacle;
            IL.CalamityMod.Projectiles.Summon.CrimslimeMinion.PreDraw += CrimslimeMinion;
            IL.CalamityMod.Projectiles.Summon.CorroslimeMinion.PreDraw += CorroslimeMinion;
            IL.CalamityMod.Projectiles.Summon.GastricBelcher.PreDraw += GastricBelcher;
            IL.CalamityMod.Projectiles.Rogue.LeonidProgenitorBombshell.PreDraw += LeonidProgenitorBombshell;
            IL.CalamityMod.Projectiles.Melee.DragonRageStaff.PreDraw += DragonRageStaff;
            IL.CalamityMod.Projectiles.Summon.FieryDraconid.PreDraw += FieryDraconid;
            IL.CalamityMod.Projectiles.Rogue.FinalDawnProjectile.PreDraw += FinalDawnProjectile;
            IL.CalamityMod.Projectiles.Melee.ExobladeProj.DrawBlade += ExobladeProj;
            IL.CalamityMod.Projectiles.Ranged.HeavenlyGaleProj.PreDraw += HeavenlyGaleProj;
            IL.CalamityMod.Projectiles.Rogue.CelestusProj.PostDraw += CelestusProj;
            IL.CalamityMod.Projectiles.Melee.ViolenceThrownProjectile.PreDraw += ViolenceThrownProjectile;
            IL.CalamityMod.Projectiles.Boss.HolyBlast.PreDraw += HolyBlast;
        }
        #region BossChecklist
        private static void BossChecklist(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.After, i => i.MatchLdstr("Dragonfolly"), i => i.MatchStloc(80)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldfld, (float)12.01);
            }
        }
        #endregion
        #region NPCs
        private static void Crabulon(ILContext il)
        {
            var c = new ILCursor(il);
            string d = "CalamityMod/NPCs/Crabulon/CrabulonGlow";
            string e = "CalamityMod/NPCs/Crabulon/CrabulonAltGlow";
            string f = "CalamityMod/NPCs/Crabulon/CrabulonAttackGlow";
            if (c.TryGotoNext(i => i.MatchLdstr(d)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.npcChanges ? d : "CalRemix/Retheme/Crabulon/CrabulonGlow");
            }
            if (c.TryGotoNext(i => i.MatchLdstr(e)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.npcChanges ? e : "CalRemix/Retheme/Crabulon/CrabulonAltGlow");
            }
            if (c.TryGotoNext(i => i.MatchLdstr(f)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.npcChanges ? f : "CalRemix/Retheme/Crabulon/CrabulonAttackGlow");
            }
            if (c.TryGotoNext(i => i.MatchCallvirt(typeof(Color).GetMethod("get_Cyan"))))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.npcChanges ? typeof(Color).GetMethod("get_Blue") : typeof(Color).GetMethod("get_Cyan"));
            }
        }
        private static void HiveMind(ILContext il)
        {
            var c = new ILCursor(il);
            string d = "CalamityMod/NPCs/HiveMind/HiveMindP2";
            if (c.TryGotoNext(i => i.MatchLdstr(d)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.npcChanges ? d : "CalRemix/Retheme/HiveMind/HiveMindP2");
            }
        }
        private static void PerforatorCyst(ILContext il)
        {
            var c = new ILCursor(il);
            string d = "CalamityMod/NPCs/Perforator/PerforatorCystGlow";
            if (c.TryGotoNext(i => i.MatchLdstr(d)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.npcChanges ? d : "CalRemix/Retheme/Perfs/CystGlow");
            }
        }
        #region PerfWormHeck
        private static void PerforatorHive(ILContext il)
        {
            var c = new ILCursor(il);
            string d = "CalamityMod/NPCs/Perforator/PerforatorHiveGlow";
            if (c.TryGotoNext(i => i.MatchLdstr(d)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.npcChanges ? d : "CalRemix/Retheme/Perfs/HiveGlow");
            }
        }
        private static void PerfLBody(ILContext il)
        {
            var c = new ILCursor(il);
            string d = "CalamityMod/NPCs/Perforator/PerforatorBodyLargeAlt";
            string e = "CalamityMod/NPCs/Perforator/PerforatorBodyLargeGlow";
            string f = "CalamityMod/NPCs/Perforator/PerforatorBodyLargeAltGlow";
            if (c.TryGotoNext(i => i.MatchLdstr(d)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.npcChanges ? d : "CalRemix/Retheme/Perfs/LBodyAlt");
            }
            if (c.TryGotoNext(i => i.MatchLdstr(e)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.npcChanges ? e : "CalRemix/Retheme/Perfs/LBodyGlow");
            }
            if (c.TryGotoNext(i => i.MatchLdstr(f)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.npcChanges ? f : "CalRemix/Retheme/Perfs/LBodyAltGlow");
            }
        }
        private static void PerfMBody(ILContext il)
        {
            var c = new ILCursor(il);
            string d = "CalamityMod/NPCs/Perforator/PerforatorBodyMediumGlow";
            if (c.TryGotoNext(i => i.MatchLdstr(d)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.npcChanges ? d : "CalRemix/Retheme/Perfs/MBodyGlow");
            }
        }
        private static void PerfSBody(ILContext il)
        {
            var c = new ILCursor(il);
            string d = "CalamityMod/NPCs/Perforator/PerforatorBodySmallGlow";
            if (c.TryGotoNext(i => i.MatchLdstr(d)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.npcChanges ? d : "CalRemix/Retheme/Perfs/SBodyGlow");
            }
        }
        private static void PerfLHead(ILContext il)
        {
            var c = new ILCursor(il);
            string d = "CalamityMod/NPCs/Perforator/PerforatorHeadLargeGlow";
            if (c.TryGotoNext(i => i.MatchLdstr(d)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.npcChanges ? d : "CalRemix/Retheme/Perfs/LHeadGlow");
            }
        }
        private static void PerfMHead(ILContext il)
        {
            var c = new ILCursor(il);
            string d = "CalamityMod/NPCs/Perforator/PerforatorHeadMediumGlow";
            if (c.TryGotoNext(i => i.MatchLdstr(d)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.npcChanges ? d : "CalRemix/Retheme/Perfs/MHeadGlow");
            }
        }
        private static void PerfSHead(ILContext il)
        {
            var c = new ILCursor(il);
            string d = "CalamityMod/NPCs/Perforator/PerforatorHeadSmallGlow";
            if (c.TryGotoNext(i => i.MatchLdstr(d)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.npcChanges ? d : "CalRemix/Retheme/Perfs/SHeadGlow");
            }
        }
        private static void PerfLTail(ILContext il)
        {
            var c = new ILCursor(il);
            string d = "CalamityMod/NPCs/Perforator/PerforatorTailLargeGlow";
            if (c.TryGotoNext(i => i.MatchLdstr(d)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.npcChanges ? d : "CalRemix/Retheme/Perfs/LTailGlow");
            }
        }
        private static void PerfMTail(ILContext il)
        {
            var c = new ILCursor(il);
            string d = "CalamityMod/NPCs/Perforator/PerforatorTailMediumGlow";
            if (c.TryGotoNext(i => i.MatchLdstr(d)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.npcChanges ? d : "CalRemix/Retheme/Perfs/MTailGlow");
            }
        }
        private static void PerfSTail(ILContext il)
        {
            var c = new ILCursor(il);
            string d = "CalamityMod/NPCs/Perforator/PerforatorTailSmallGlow";
            if (c.TryGotoNext(i => i.MatchLdstr(d)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.npcChanges ? d : "CalRemix/Retheme/Perfs/STailGlow");
            }
        }
        #endregion
        private static void Cryogen(ILContext il)
        {
            var c = new ILCursor(il);
            string d = "CalamityMod/NPCs/Cryogen/Cryogen_Phase";
            if (c.TryGotoNext(i => i.MatchLdstr(d)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.npcChanges ? d : "CalRemix/Retheme/Cryogen/CryogenPhase");
            }
        }
        private static void CryogenShield(ILContext il)
        {
            var c = new ILCursor(il);
            string d = "CalamityMod/NPCs/Cryogen/CryogenShield";
            if (c.TryGotoNext(i => i.MatchLdstr(d)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.npcChanges ? d : "CalRemix/Retheme/Cryogen/CryogenShield");
            }
        }
        private static void CalamitasClone(ILContext il)
        {
            var c = new ILCursor(il);
            string d = "CalamityMod/NPCs/CalClone/CalamitasCloneGlow";
            if (c.TryGotoNext(i => i.MatchLdstr(d)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.npcChanges ? d : "CalRemix/Retheme/Cal/CalamitasGlow");
            }
        }
        private static void Cataclysm(ILContext il)
        {
            var c = new ILCursor(il);
            string d = "CalamityMod/NPCs/CalClone/CataclysmGlow";
            if (c.TryGotoNext(i => i.MatchLdstr(d)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.npcChanges ? d : "CalRemix/Retheme/Cal/CataclysmGlow");
            }
        }
        private static void Catastrophe(ILContext il)
        {
            var c = new ILCursor(il);
            string d = "CalamityMod/NPCs/CalClone/CatastropheGlow";
            if (c.TryGotoNext(i => i.MatchLdstr(d)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.npcChanges ? d : "CalRemix/Retheme/Cal/CatastropheGlow");
            }
        }
        private static void Leviathan(ILContext il)
        {
            var c = new ILCursor(il);
            string d = "CalamityMod/NPCs/Leviathan/LeviathanAttack";
            if (c.TryGotoNext(i => i.MatchLdstr(d)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.npcChanges ? d : "CalRemix/Retheme/Levi/Levi2");
            }
        }
        private static void Anahita(ILContext il)
        {
            var c = new ILCursor(il);
            string d = "CalamityMod/NPCs/Leviathan/AnahitaStabbing";
            if (c.TryGotoNext(i => i.MatchLdstr(d)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.npcChanges ? d : "CalRemix/Retheme/Levi/AnahitaStab");
            }
        }
        private static void AstrumDeusHead(ILContext il)
        {
            var c = new ILCursor(il);
            string d = "CalamityMod/NPCs/AstrumDeus/AstrumDeusHeadGlow2";
            string e = "CalamityMod/NPCs/AstrumDeus/AstrumDeusHeadGlow";
            string f = "CalamityMod/NPCs/AstrumDeus/AstrumDeusHeadGlow3";
            string g = "CalamityMod/NPCs/AstrumDeus/AstrumDeusHeadGlow4";
            string cc = "CalRemix/Retheme/AD/HeadGlow";
            if (c.TryGotoNext(i => i.MatchLdstr(d)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.npcChanges ? d : cc);
            }
            if (c.TryGotoNext(i => i.MatchLdstr(e)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.npcChanges ? e : cc);
            }
            if (c.TryGotoNext(i => i.MatchLdstr(f)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.npcChanges ? f : cc);
            }
            if (c.TryGotoNext(i => i.MatchLdstr(g)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.npcChanges ? g : cc);
            }
        }
        private static void AstrumDeusBody(ILContext il)
        {
            var c = new ILCursor(il);
            string d = "CalamityMod/NPCs/AstrumDeus/AstrumDeusBodyAltSpectral";
            string e = "CalamityMod/NPCs/AstrumDeus/AstrumDeusBodyGlow2";
            string f = "CalamityMod/NPCs/AstrumDeus/AstrumDeusBodyGlow";
            string g = "CalamityMod/NPCs/AstrumDeus/AstrumDeusBodyAltGlow";
            string h = "CalamityMod/NPCs/AstrumDeus/AstrumDeusBodyGlow3";
            string j = "CalamityMod/NPCs/AstrumDeus/AstrumDeusBodyAltGlow2";
            string k = "CalamityMod/NPCs/AstrumDeus/AstrumDeusBodyGlow4";
            string cc = "CalRemix/Retheme/AD/BodyGlow";
            if (c.TryGotoNext(i => i.MatchLdstr(d)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.npcChanges ? d : "CalRemix/Retheme/AD/Body");
            }
            if (c.TryGotoNext(i => i.MatchLdstr(e)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.npcChanges ? e : cc);
            }
            if (c.TryGotoNext(i => i.MatchLdstr(f)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.npcChanges ? f : cc);
            }
            if (c.TryGotoNext(i => i.MatchLdstr(g)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.npcChanges ? g : cc);
            }
            if (c.TryGotoNext(i => i.MatchLdstr(h)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.npcChanges ? h : cc);
            }
            if (c.TryGotoNext(i => i.MatchLdstr(j)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.npcChanges ? j : cc);
            }
            if (c.TryGotoNext(i => i.MatchLdstr(k)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.npcChanges ? k : cc);
            }
        }
        private static void AstrumDeusTail(ILContext il)
        {
            var c = new ILCursor(il);
            string d = "CalamityMod/NPCs/AstrumDeus/AstrumDeusTailGlow";
            string e = "CalamityMod/NPCs/AstrumDeus/AstrumDeusTailGlow2";
            if (c.TryGotoNext(i => i.MatchLdstr(d)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.npcChanges ? d : "CalRemix/Retheme/AD/TailGlow");
            }
            if (c.TryGotoNext(i => i.MatchLdstr(e)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.npcChanges ? e : "CalRemix/Retheme/AD/TailGlow");
            }
        }
        private static void Providence(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Providence/")))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !Main.zenithWorld && CalRemixWorld.npcChanges ? "CalRemix/Retheme/Providence/" : "CalamityMod/NPCs/Providence/");
            }
            if (c.TryGotoNext(i => i.MatchCall<Color>("get_Cyan")))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Call, typeof(Color).GetMethod("get_White"));
            }
            if (c.TryGotoNext(i => i.MatchCall<Color>("get_BlueViolet")))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Call, typeof(Color).GetMethod("get_White"));
            }
            if (c.TryGotoNext(i => i.MatchCall<Color>("get_BlueViolet")))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Call, typeof(Color).GetMethod("get_White"));
            }
        }
        private static void Yharon(ILContext il)
        {
            var c = new ILCursor(il);
            string d = "CalamityMod/NPCs/Yharon/YharonGlowGreen";
            string e = "CalamityMod/NPCs/Yharon/YharonGlowPurple";
            if (c.TryGotoNext(i => i.MatchLdstr(d)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.npcChanges ? d : "CalRemix/Retheme/Yharon/YharonGlowGreen");
            }
            if (c.TryGotoNext(i => i.MatchLdstr(e)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.npcChanges ? e : "CalRemix/Retheme/Yharon/YharonGlowPurple");
            }
        }
        #region Aureus and Birb Reworks
        private static void AureusAI(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.After, i => i.MatchLdloc(15)))
            {
                c.RemoveRange(2);
                c.EmitDelegate((Player p) => p.GetModPlayer<CalRemixPlayer>());
                c.Emit(OpCodes.Ldfld, typeof(CalRemixPlayer).GetField("ZonePlague", BindingFlags.Public | BindingFlags.Instance));
            }
            if (c.TryGotoNext(i => i.MatchLdcR4((float)1.3)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldc_R4, (float)0.0);
            }
        }
        private static void AureusSpawn(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchCall<Color>("get_Cyan")))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Call, typeof(Color).GetMethod("get_White"));
            }
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/AstrumAureus/AureusSpawnGlow")))
                Rework(c, "CalRemix/Retheme/Plague/AureusSpawnGlow");
        }
        private static void AstrumAureus(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/AstrumAureus/AstrumAureusGlow")))
                Rework(c, "CalRemix/Retheme/Plague/AstrumAureusGlow");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/AstrumAureus/AstrumAureusRecharge")))
                Rework(c, "CalRemix/Retheme/Plague/AstrumAureusRecharge");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/AstrumAureus/AstrumAureusWalk")))
                Rework(c, "CalRemix/Retheme/Plague/AstrumAureusWalk");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/AstrumAureus/AstrumAureusWalkGlow")))
                Rework(c, "CalRemix/Retheme/Plague/AstrumAureusWalkGlow");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/AstrumAureus/AstrumAureusGlow")))
                Rework(c, "CalRemix/Retheme/Plague/AstrumAureusGlow");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/AstrumAureus/AstrumAureusJump")))
                Rework(c, "CalRemix/Retheme/Plague/AstrumAureusJump");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/AstrumAureus/AstrumAureusJumpGlow")))
                Rework(c, "CalRemix/Retheme/Plague/AstrumAureusJumpGlow");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/AstrumAureus/AstrumAureusStomp")))
                Rework(c, "CalRemix/Retheme/Plague/AstrumAureusStomp");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/AstrumAureus/AstrumAureusStompGlow")))
                Rework(c, "CalRemix/Retheme/Plague/AstrumAureusStompGlow");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/AstrumAureus/AstrumAureusGlow")))
                Rework(c, "CalRemix/Retheme/Plague/AstrumAureusGlow");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/AstrumAureus/AstrumAureusJump")))
                Rework(c, "CalRemix/Retheme/Plague/AstrumAureusJump");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/AstrumAureus/AstrumAureusJumpGlow")))
                Rework(c, "CalRemix/Retheme/Plague/AstrumAureusJumpGlow");
        }
        private static void BirbAI(ILContext il)
        {
            var c = new ILCursor(il);
            var t = typeof(Player).GetMethod("get_ZoneJungle", BindingFlags.Public | BindingFlags.Instance);
            if (c.TryGotoNext(i => i.MatchCallvirt(t)))
            {
                c.Remove();
                c.Emit(OpCodes.Callvirt, typeof(Player).GetMethod("get_ZoneDesert", BindingFlags.Public | BindingFlags.Instance));
            }
        }
        private static void BirbDraw(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Bumblebirb/BirbGlow")))
                Rework(c, "CalRemix/Retheme/Birb/BirbGlow");
        }
        private static void BirbBest(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.After, i => i.MatchLdcI4(0)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldsfld, typeof(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes).GetField("Desert", BindingFlags.Public | BindingFlags.Static));
            }
        }
        private static void BirbSpawn(ILContext il)
        {
            var c = new ILCursor(il);
            var t = typeof(Player).GetMethod("get_ZoneJungle", BindingFlags.Public | BindingFlags.Instance);
            if (c.TryGotoNext(i => i.MatchCallvirt(t)))
            {
                c.Remove();
                c.Emit(OpCodes.Callvirt, typeof(Player).GetMethod("get_ZoneDesert", BindingFlags.Public | BindingFlags.Instance));
            }
        }
        #endregion
        #endregion
        #region Items
        private static bool Exotic(On.CalamityMod.Items.SummonItems.ExoticPheromones.orig_CanUseItem orig, CalamityMod.Items.SummonItems.ExoticPheromones self, object player)
        {
            Player p = (Player)player;
            return (p.ZoneDesert && !NPC.AnyNPCs(NPCType<Bumblefuck>()) && !BossRushEvent.BossRushActive);
        }
        private static bool AstralChunk(On.CalamityMod.Items.SummonItems.AstralChunk.orig_CanUseItem orig, CalamityMod.Items.SummonItems.AstralChunk self, object player)
        {
            Player p = (Player)player;
            return (p.GetModPlayer<CalRemixPlayer>().ZonePlague || p.GetModPlayer<CalRemixPlayer>().ZonePlagueDesert) && !NPC.AnyNPCs(NPCType<AstrumAureus>()) && !BossRushEvent.BossRushActive;
        }
        private static void HeavenlyGale(ILContext il)
        {
            var c = new ILCursor(il);
            string d = "CalamityMod/Items/Weapons/Ranged/HeavenlyGaleGlow";
            if (c.TryGotoNext(i => i.MatchLdstr(d)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.itemChanges ? d : "CalRemix/Retheme/Blank");
            }
        }
        #endregion
        #region Projectiles
        private static void InfestedClawmerangProj(ILContext il)
        {
            var c = new ILCursor(il);
            var d = typeof(ModProjectile).GetMethod("get_Texture", BindingFlags.Public | BindingFlags.Instance);
            if (c.TryGotoNext(i => i.MatchCallvirt(d)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.itemChanges ? "CalamityMod/Projectiles/Rogue/InfestedClawmerangProj" : "CalRemix/Retheme/Crabulon/Shroomerang");
            }
        }
        private static void DankCreeperMinion(ILContext il)
        {
            var c = new ILCursor(il);
            var d = typeof(ModProjectile).GetMethod("get_Texture", BindingFlags.Public | BindingFlags.Instance);
            if (c.TryGotoNext(i => i.MatchCallvirt(d)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.itemChanges ? "CalamityMod/Projectiles/Summon/DankCreeperMinion" : "CalRemix/Retheme/HiveMind/DankCreeperMinion");
            }
        }
        private static void RotBallProjectile(ILContext il)
        {
            var c = new ILCursor(il);
            string d = "CalamityMod/Items/Weapons/Rogue/RotBall";
            if (c.TryGotoNext(i => i.MatchLdstr(d)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.itemChanges ? d : "CalRemix/Retheme/HiveMind/RotBall");
            }
        }
        private static void BloodBeam(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdcI4(5)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.npcChanges ? 5 : 118);
            }
        }
        private static void BloodClotFriendly(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdcI4(5)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.npcChanges ? 5 : 118);
            }
        }
        private static void ToothBallProjectile(ILContext il)
        {
            var c = new ILCursor(il);
            string d = "CalamityMod/Items/Weapons/Rogue/ToothBall";
            if (c.TryGotoNext(i => i.MatchLdstr(d)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.itemChanges ? d : "CalRemix/Retheme/Perfs/ToothBall");
            }
        }
        private static void EldritchTentacle(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdcI4(60)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.itemChanges ? 60 : 188);
            }
        }
        private static void CrimslimeMinion(ILContext il)
        {
            var c = new ILCursor(il);
            var d = typeof(ModProjectile).GetMethod("get_Texture", BindingFlags.Public | BindingFlags.Instance);
            if (c.TryGotoNext(i => i.MatchCallvirt(d)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.itemChanges ? "CalamityMod/Projectiles/Summon/CrimslimeMinion" : "CalRemix/Retheme/SlimeGod/CrimslimeMinion");
            }
        }
        private static void CorroslimeMinion(ILContext il)
        {
            var c = new ILCursor(il);
            var d = typeof(ModProjectile).GetMethod("get_Texture", BindingFlags.Public | BindingFlags.Instance);
            if (c.TryGotoNext(i => i.MatchCallvirt(d)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.itemChanges ? "CalamityMod/Projectiles/Summon/CorroslimeMinion" : "CalRemix/Retheme/SlimeGod/CorroslimeMinion");
            }
        }
        private static void GastricBelcher(ILContext il)
        {
            var c = new ILCursor(il);
            var d = typeof(ModProjectile).GetMethod("get_Texture", BindingFlags.Public | BindingFlags.Instance);
            if (c.TryGotoNext(i => i.MatchCallvirt(d)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.itemChanges ? "CalamityMod/Projectiles/Summon/GastricBelcher" : "CalRemix/Retheme/Levi/GastricBelcher");
            }
        }
        private static void LeonidProgenitorBombshell(ILContext il)
        {
            var c = new ILCursor(il);
            var d = typeof(ModProjectile).GetMethod("get_Texture", BindingFlags.Public | BindingFlags.Instance);
            if (c.TryGotoNext(i => i.MatchCallvirt(d)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.itemChanges ? "CalamityMod/Items/Weapons/Rogue/LeonidProgenitor" : "CalRemix/Retheme/Plague/LeonidProgenitor");
            }
        }
        private static void DragonRageStaff(ILContext il)
        {
            var c = new ILCursor(il);
            var d = typeof(ModProjectile).GetMethod("get_Texture", BindingFlags.Public | BindingFlags.Instance);
            if (c.TryGotoNext(i => i.MatchCallvirt(d)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.itemChanges ? "CalamityMod/Items/Weapons/Melee/DragonRageStaff" : "CalRemix/Retheme/Yharon/DragonRageStaff");
            }
        }
        private static void FieryDraconid(ILContext il)
        {
            var c = new ILCursor(il);
            var d = typeof(ModProjectile).GetMethod("get_Texture", BindingFlags.Public | BindingFlags.Instance);
            if (c.TryGotoNext(i => i.MatchCallvirt(d)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.itemChanges ? "CalamityMod/Items/Weapons/Summon/FieryDraconid" : "CalRemix/Retheme/Yharon/FieryDraconid");
            }
        }
        private static void FinalDawnProjectile(ILContext il)
        {
            var c = new ILCursor(il);
            var d = typeof(ModProjectile).GetMethod("get_Texture", BindingFlags.Public | BindingFlags.Instance);
            if (c.TryGotoNext(i => i.MatchCallvirt(d)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.itemChanges ? "CalamityMod/Items/Weapons/Rogue/FinalDawnProjectile" : "CalRemix/Retheme/Yharon/FinalDawnProjectile");
            }
        }
        private static void ExobladeProj(ILContext il)
        {
            var c = new ILCursor(il);
            var d = typeof(ModProjectile).GetMethod("get_Texture", BindingFlags.Public | BindingFlags.Instance);
            if (c.TryGotoNext(i => i.MatchCallvirt(d)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.itemChanges ? "CalamityMod/Items/Weapons/Melee/Exoblade" : "CalRemix/Retheme/Exo/Blade");
            }
        }
        private static void HeavenlyGaleProj(ILContext il)
        {
            var c = new ILCursor(il);
            var d = typeof(ModProjectile).GetMethod("get_Texture", BindingFlags.Public | BindingFlags.Instance);
            string e = "CalamityMod/Projectiles/Ranged/HeavenlyGaleProjGlow";
            if (c.TryGotoNext(i => i.MatchCallvirt(d)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.itemChanges ? "CalamityMod/Projectiles/Ranged/HeavenlyGaleProj" : "CalRemix/Retheme/Exo/GaleProj");
            }
            if (c.TryGotoNext(i => i.MatchLdstr(e)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.itemChanges ? e : "CalRemix/Retheme/Exo/GaleProjGlow");
            }
        }
        private static void CelestusProj(ILContext il)
        {
            var c = new ILCursor(il);
            string d = "CalamityMod/Projectiles/Rogue/CelestusProjGlow";
            if (c.TryGotoNext(i => i.MatchLdstr(d)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.itemChanges ? d : "CalRemix/Retheme/Blank");
            }
        }
        private static void ViolenceThrownProjectile(ILContext il)
        {
            var c = new ILCursor(il);
            var d = typeof(ModProjectile).GetMethod("get_Texture", BindingFlags.Public | BindingFlags.Instance);
            if (c.TryGotoNext(i => i.MatchCallvirt(d)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.itemChanges ? "CalamityMod/Projectiles/Melee/ViolenceThrownProjectile" : "CalRemix/Retheme/Violence");
            }
        }
        private static void HolyBlast(ILContext il)
        {
            var c = new ILCursor(il);
            var d = typeof(ModProjectile).GetMethod("get_Texture", BindingFlags.Public | BindingFlags.Instance);
            if (c.TryGotoNext(i => i.MatchCallvirt(d)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.itemChanges ? "CalamityMod/Projectiles/Boss/HolyBlast" : "CalamityMod/Projectiles/Boss/HolyBlastNight");
            }
        }
        #endregion
        private static void Rework(ILCursor c, string s)
        {
            c.Index++;
            c.Emit(OpCodes.Pop);
            c.Emit(OpCodes.Ldstr, s);
        }
    }
}
