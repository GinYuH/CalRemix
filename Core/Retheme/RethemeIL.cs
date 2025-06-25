using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Reflection;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using CalamityMod.Projectiles.Rogue;
using CalRemix.Core.World;
using CalamityMod.NPCs.Providence;
using Terraria;
using CalamityMod.Systems;
using static Terraria.ModLoader.ModContent;
using CalamityMod.NPCs.Yharon;
using CalamityMod;

namespace CalRemix.Core.Retheme
{
    public class RethemeIL : ModSystem
    {
        public override void Load()
        {
            // IL.CalamityMod.NPCs.PreDraw += ;
            IL.CalamityMod.NPCs.Crabulon.Crabulon.PreDraw += Crabulon;
            IL.CalamityMod.NPCs.Cryogen.CryogenShield.PreDraw += CryogenShield;
            //IL.CalamityMod.NPCs.CalamityAIs.CalamityBossAIs.AstrumAureusAI.VanillaAstrumAureusAI += AureusAI;
            MonoModHooks.Modify(typeof(Providence).GetMethod("<PreDraw>g__drawProvidenceInstance|83_0", BindingFlags.NonPublic | BindingFlags.Instance), ProvidenceColors);

            #region Items
            // IL.CalamityMod.Items.Weapons.PreDraw += ;
            IL.CalamityMod.Items.Weapons.Melee.Exoblade.PostDrawInWorld += ExobladeGlow;
            IL.CalamityMod.Items.Weapons.Ranged.HeavenlyGale.PostDrawInWorld += GaleGlow;
            IL.CalamityMod.Items.Weapons.Ranged.Photoviscerator.PostDrawInWorld += VisGlow;
            IL.CalamityMod.Items.Weapons.Magic.SubsumingVortex.PostDrawInWorld += VortexGlow;
            IL.CalamityMod.Items.Weapons.Magic.VividClarity.PostDrawInWorld += ClarityGlow;
            IL.CalamityMod.Items.Weapons.Summon.CosmicImmaterializer.PostDrawInWorld += ImmaterializerGlow;
            IL.CalamityMod.Items.Weapons.Rogue.Celestus.PostDrawInWorld += CelestusGlow;
            IL.CalamityMod.Items.Weapons.Rogue.Supernova.PostDrawInWorld += SupernovaGlow;
            #endregion
            #region Projectiles
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

            MonoModHooks.Modify(typeof(ReaperProjectile).GetMethod("get_Texture", BindingFlags.Public | BindingFlags.Instance), TheOldReaper);
            IL.CalamityMod.Projectiles.Melee.DragonRageStaff.PreDraw += DragonRageStaff;
            IL.CalamityMod.Projectiles.Summon.FieryDraconid.PreDraw += FieryDraconid;
            IL.CalamityMod.Projectiles.Rogue.FinalDawnProjectile.PreDraw += FinalDawnProjectile;

            IL.CalamityMod.Projectiles.Melee.ExobladeProj.DrawBlade += ExobladeProj;
            IL.CalamityMod.Projectiles.Ranged.HeavenlyGaleProj.PreDraw += HeavenlyGaleProj;
            IL.CalamityMod.Projectiles.Ranged.PhotovisceratorHoldout.PostDraw += PhotoProj;
            IL.CalamityMod.Projectiles.Rogue.CelestusProj.PostDraw += CelestusProj;
            IL.CalamityMod.Projectiles.Rogue.SupernovaBomb.PreDraw += SupernovaProj;

            IL.CalamityMod.Projectiles.Melee.ViolenceThrownProjectile.PreDraw += ViolenceThrownProjectile;
            IL.CalamityMod.Projectiles.Boss.HolyBlast.PreDraw += HolyBlast;
            IL.CalamityMod.Projectiles.Enemy.HorsWaterBlast.AI += HorsWaterBlastAI;
            IL.CalamityMod.Projectiles.Enemy.HorsWaterBlast.OnKill += HorsWaterBlastOnKill;
            #endregion
        }
        public override void PostSetupContent()
        {
            On.CalamityMod.Systems.YharonBackgroundScene.IsSceneEffectActive += NoYharonScene;
        }
        #region NPCs
        private static void Crabulon(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchCallvirt(typeof(Color).GetMethod("get_Cyan"))))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.npcChanges ? typeof(Color).GetMethod("get_Blue") : typeof(Color).GetMethod("get_Cyan"));
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
                c.EmitDelegate(() => !CalRemixWorld.npcChanges ? d : "CalRemix/Core/Retheme/Cryogen/CryogenShield");
            }
        }
        private static void AureusAI(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdcR4((float)1.3)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.npcChanges ? (float)1.3 : (float)0.0);
            }
        }
        private static void ProvidenceColors(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchCall<Color>("get_Cyan")))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.npcChanges ? typeof(Color).GetMethod("get_Cyan") : typeof(Color).GetMethod("get_White"));
            }
            if (c.TryGotoNext(i => i.MatchCall<Color>("get_BlueViolet")))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.npcChanges ? typeof(Color).GetMethod("get_BlueViolet") : typeof(Color).GetMethod("get_White"));
            }
            if (c.TryGotoNext(i => i.MatchCall<Color>("get_BlueViolet")))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.npcChanges ? typeof(Color).GetMethod("get_BlueViolet") : typeof(Color).GetMethod("get_White"));
            }
        }
        #endregion
        #region Items
        private static void ExobladeGlow(ILContext il)
        {
            var c = new ILCursor(il);
            string d = "CalamityMod/Items/Weapons/Melee/ExobladeGlow";
            if (c.TryGotoNext(i => i.MatchLdstr(d)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.itemChanges ? d : "CalRemix/Assets/ExtraTextures/Blank");
            }
        }
        private static void GaleGlow(ILContext il)
        {
            var c = new ILCursor(il);
            string d = "CalamityMod/Items/Weapons/Ranged/HeavenlyGaleGlow";
            if (c.TryGotoNext(i => i.MatchLdstr(d)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.itemChanges ? d : "CalRemix/Assets/ExtraTextures/Blank");
            }
        }
        private static void VisGlow(ILContext il)
        {
            var c = new ILCursor(il);
            string d = "CalamityMod/Items/Weapons/Ranged/PhotovisceratorGlow";
            if (c.TryGotoNext(i => i.MatchLdstr(d)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.itemChanges ? d : "CalRemix/Assets/ExtraTextures/Blank");
            }
        }
        private static void VortexGlow(ILContext il)
        {
            var c = new ILCursor(il);
            string d = "CalamityMod/Items/Weapons/Magic/SubsumingVortexGlow";
            if (c.TryGotoNext(i => i.MatchLdstr(d)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.itemChanges ? d : "CalRemix/Assets/ExtraTextures/Blank");
            }
        }
        private static void ClarityGlow(ILContext il)
        {
            var c = new ILCursor(il);
            string d = "CalamityMod/Items/Weapons/Magic/VividClarityGlow";
            if (c.TryGotoNext(i => i.MatchLdstr(d)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.itemChanges ? d : "CalRemix/Assets/ExtraTextures/Blank");
            }
        }
        private static void ImmaterializerGlow(ILContext il)
        {
            var c = new ILCursor(il);
            string d = "CalamityMod/Items/Weapons/Summon/CosmicImmaterializerGlow";
            if (c.TryGotoNext(i => i.MatchLdstr(d)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.itemChanges ? d : "CalRemix/Assets/ExtraTextures/Blank");
            }
        }
        private static void CelestusGlow(ILContext il)
        {
            var c = new ILCursor(il);
            string d = "CalamityMod/Items/Weapons/Rogue/CelestusGlow";
            if (c.TryGotoNext(i => i.MatchLdstr(d)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.itemChanges ? d : "CalRemix/Assets/ExtraTextures/Blank");
            }
        }
        private static void SupernovaGlow(ILContext il)
        {
            var c = new ILCursor(il);
            string d = "CalamityMod/Items/Weapons/Rogue/SupernovaGlow";
            if (c.TryGotoNext(i => i.MatchLdstr(d)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.itemChanges ? d : "CalRemix/Assets/ExtraTextures/Blank");
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
                c.EmitDelegate(() => !CalRemixWorld.itemChanges ? "CalamityMod/Items/Weapons/Rogue/InfestedClawmerangProj" : "CalRemix/Core/Retheme/Crabulon/Shroomerang");
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
                c.EmitDelegate(() => !CalRemixWorld.itemChanges ? "CalamityMod/Projectiles/Summon/DankCreeperMinion" : "CalRemix/Core/Retheme/HiveMind/DankCreeperMinion");
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
                c.EmitDelegate(() => !CalRemixWorld.itemChanges ? d : "CalRemix/Core/Retheme/HiveMind/RotBall");
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
                c.EmitDelegate(() => !CalRemixWorld.itemChanges ? d : "CalRemix/Core/Retheme/Perfs/ToothBall");
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
                c.EmitDelegate(() => !CalRemixWorld.itemChanges ? "CalamityMod/Projectiles/Summon/CrimslimeMinion" : "CalRemix/Core/Retheme/SlimeGod/CrimslimeMinion");
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
                c.EmitDelegate(() => !CalRemixWorld.itemChanges ? "CalamityMod/Projectiles/Summon/CorroslimeMinion" : "CalRemix/Core/Retheme/SlimeGod/CorroslimeMinion");
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
                c.EmitDelegate(() => !CalRemixWorld.itemChanges ? "CalamityMod/Projectiles/Summon/GastricBelcher" : "CalRemix/Core/Retheme/Levi/GastricBelcher");
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
                c.EmitDelegate(() => !CalRemixWorld.itemChanges ? "CalamityMod/Items/Weapons/Rogue/LeonidProgenitor" : "CalRemix/Core/Retheme/Plague/LeonidProgenitor");
            }
        }
        private static void TheOldReaper(ILContext il)
        {
            var c = new ILCursor(il);
            string d = "CalamityMod/Items/Weapons/Rogue/TheOldReaper";
            if (c.TryGotoNext(i => i.MatchLdstr(d)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitLdstr("CalRemix/Core/Retheme/TheReaper");
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
                c.EmitDelegate(() => !CalRemixWorld.itemChanges ? "CalamityMod/Projectiles/Melee/DragonRageStaff" : "CalRemix/Core/Retheme/Yharon/DragonRageStaff");
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
                c.EmitDelegate(() => !CalRemixWorld.itemChanges ? "CalamityMod/Projectiles/Summon/FieryDraconid" : "CalRemix/Core/Retheme/Yharon/FieryDraconid");
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
                c.EmitDelegate(() => !CalRemixWorld.itemChanges ? "CalamityMod/Projectiles/Rogue/FinalDawnProjectile" : "CalRemix/Core/Retheme/Yharon/FinalDawnProjectile");
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
                c.EmitDelegate(() => !CalRemixWorld.itemChanges ? "CalamityMod/Items/Weapons/Melee/Exoblade" : "CalRemix/Core/Retheme/Exo/Blade");
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
                c.EmitDelegate(() => !CalRemixWorld.itemChanges ? "CalamityMod/Projectiles/Ranged/HeavenlyGaleProj" : "CalRemix/Core/Retheme/Exo/GaleProj");
            }
            if (c.TryGotoNext(i => i.MatchLdstr(e)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.itemChanges ? e : "CalRemix/Core/Retheme/Exo/GaleProjGlow");
            }
        }
        private static void PhotoProj(ILContext il)
        {
            var c = new ILCursor(il);
            string d = "CalamityMod/Projectiles/Ranged/PhotovisceratorHoldoutGlow";
            if (c.TryGotoNext(i => i.MatchLdstr(d)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.itemChanges ? d : "CalRemix/Core/Retheme/Exo/VisProjGlow");
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
                c.EmitDelegate(() => !CalRemixWorld.itemChanges ? d : "CalRemix/Core/Retheme/Exo/CelestusProjGlow");
            }
        }
        private static void SupernovaProj(ILContext il)
        {
            var c = new ILCursor(il);
            string d = "CalamityMod/Items/Weapons/Rogue/Supernova";
            if (c.TryGotoNext(i => i.MatchLdstr(d)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.itemChanges ? d : "CalRemix/Core/Retheme/Exo/Supernova");
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
                c.EmitDelegate(() => !CalRemixWorld.itemChanges ? "CalamityMod/Items/Weapons/Melee/Violence" : "CalRemix/Core/Retheme/Violence");
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
                c.EmitDelegate(() => CalRemixWorld.npcChanges ? "CalamityMod/Projectiles/Boss/HolyBlast" : "CalamityMod/Projectiles/Boss/HolyBlastNight");
            }
        }
        private static void HorsWaterBlastAI(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdcI4(33)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => CalRemixWorld.npcChanges ? 226 : 33);
            }
            if (c.TryGotoNext(i => i.MatchLdcI4(33)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => CalRemixWorld.npcChanges ? 226 : 33);
            }
        }
        private static void HorsWaterBlastOnKill(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdcI4(33)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => CalRemixWorld.npcChanges ? 226 : 33);
            }
            if (c.TryGotoNext(i => i.MatchLdcI4(33)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => CalRemixWorld.npcChanges ? 226 : 33);
            }
        }
        #endregion
        private static bool NoYharonScene(On.CalamityMod.Systems.YharonBackgroundScene.orig_IsSceneEffectActive orig, YharonBackgroundScene self, object player)
        {
            if (CalRemixWorld.npcChanges)
                return !NPC.AnyNPCs(NPCType<Yharon>()) && Main.LocalPlayer.Calamity().monolithYharonShader > 0;
            return orig(self, player);
        }
    }
}
