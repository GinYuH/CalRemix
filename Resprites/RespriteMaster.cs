using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityMod.NPCs.NormalNPCs;
using CalamityMod.NPCs.SlimeGod;
using Microsoft.Xna.Framework.Graphics;
using CalamityMod.Events;
using Terraria.GameContent;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Projectiles.Melee.Spears;
using CalamityMod.Projectiles.Melee;
using static Terraria.ModLoader.ModContent;
using CalamityMod.NPCs.Ravager;
using ReLogic.Content;
using CalamityMod.NPCs.ProfanedGuardians;
using CalamityMod.NPCs.DesertScourge;
using CalamityMod.Projectiles.Boss;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Projectiles.Ranged;
using CalamityMod.Projectiles.Rogue;
using CalamityMod.Items.SummonItems;
using CalamityMod.Items.Accessories;
using CalamityMod.Projectiles.Melee.Shortswords;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using System.Reflection;
using CalamityMod.NPCs.HiveMind;
using CalamityMod.NPCs.Cryogen;
using CalamityMod.NPCs.CalClone;
using CalamityMod.NPCs.Providence;
using CalamityMod.NPCs.Perforator;
using CalamityMod.Projectiles.Magic;
using CalamityMod.NPCs.AstrumDeus;
using CalamityMod.NPCs.Yharon;
using CalamityMod.Systems;

namespace CalRemix.Resprites
{
    public class RespriteMaster : ModSystem
    {
        public override void Load()
        {
            // IL.CalamityMod.NPCs.PreDraw += ;
            IL.CalamityMod.NPCs.Crabulon.Crabulon.PreDraw += Crabulon;
            IL.CalamityMod.NPCs.Perforator.PerforatorCyst.PreDraw += PerforatorCyst;
            IL.CalamityMod.NPCs.Perforator.PerforatorHive.PreDraw += PerforatorHive;
            IL.CalamityMod.NPCs.Cryogen.Cryogen.PreDraw += Cryogen;
            IL.CalamityMod.NPCs.Cryogen.CryogenShield.PreDraw += CryogenShield;
            IL.CalamityMod.NPCs.CalClone.CalamitasClone.PreDraw += CalamitasClone;
            IL.CalamityMod.NPCs.CalClone.Cataclysm.PreDraw += Cataclysm;
            IL.CalamityMod.NPCs.CalClone.Catastrophe.PreDraw += Catastrophe;
            IL.CalamityMod.NPCs.AstrumDeus.AstrumDeusHead.PreDraw += AstrumDeusHead;
            IL.CalamityMod.NPCs.AstrumDeus.AstrumDeusBody.PreDraw += AstrumDeusBody;
            IL.CalamityMod.NPCs.AstrumDeus.AstrumDeusTail.PreDraw += AstrumDeusTail;
            IL.CalamityMod.NPCs.Yharon.Yharon.PreDraw += Yharon;
            MonoModHooks.Modify(typeof(Providence).GetMethod("<PreDraw>g__drawProvidenceInstance|46_0", BindingFlags.NonPublic | BindingFlags.Instance), Providence);

            // IL.CalamityMod.Projectiles.PreDraw += ;
            IL.CalamityMod.Projectiles.Rogue.InfestedClawmerangProj.PreDraw += InfestedClawmerangProj;
            IL.CalamityMod.Projectiles.Magic.EldritchTentacle.AI += EldritchTentacle;
            IL.CalamityMod.Projectiles.Melee.MurasamaSlash.PreDraw += MurasamaSlash;
            IL.CalamityMod.Projectiles.Melee.ExobladeProj.DrawBlade += ExobladeProj;
            IL.CalamityMod.Projectiles.Ranged.HeavenlyGaleProj.PreDraw += HeavenlyGaleProj;
            IL.CalamityMod.Projectiles.Rogue.CelestusProj.PostDraw += CelestusProj;
            IL.CalamityMod.Projectiles.Melee.ViolenceThrownProjectile.PreDraw += ViolenceThrownProjectile;
        }
        #region NPCs
        private void Crabulon(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Crabulon/CrabulonGlow")))
                Resprite(c, "CalRemix/Resprites/Crabulon/CrabulonGlow");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Crabulon/CrabulonAltGlow")))
                Resprite(c, "CalRemix/Resprites/Crabulon/CrabulonAltGlow");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Crabulon/CrabulonAttackGlow")))
                Resprite(c, "CalRemix/Resprites/Crabulon/CrabulonAttackGlow");
        }
        private void PerforatorCyst(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Perforator/PerforatorCystGlow")))
                Resprite(c, "CalRemix/Resprites/Perfs/CystGlow");
        }
        private void PerforatorHive(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Perforator/PerforatorHiveGlow")))
                Resprite(c, "CalRemix/Resprites/Perfs/HiveGlow");
        }
        private void Cryogen(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Cryogen/Cryogen_Phase")))
                Resprite(c, "CalRemix/Resprites/Cryogen/CryogenPhase");
        }
        private void CryogenShield(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Cryogen/CryogenShield")))
                Resprite(c, "CalRemix/Resprites/Cryogen/CryogenShield");
        }
        private void CalamitasClone(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/CalClone/CalamitasCloneGlow")))
                Resprite(c, "CalRemix/Resprites/Cal/CalamitasGlow");
        }
        private void Cataclysm(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/CalClone/CataclysmGlow")))
                Resprite(c, "CalRemix/Resprites/Cal/CataclysmGlow");
        }
        private void Catastrophe(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/CalClone/CatastropheGlow")))
                Resprite(c, "CalRemix/Resprites/Cal/CatastropheGlow");
        }
        private void AstrumDeusHead(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/AstrumDeus/AstrumDeusHeadGlow2")))
                Resprite(c, "CalRemix/Resprites/AD/HeadGlow");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/AstrumDeus/AstrumDeusHeadGlow")))
                Resprite(c, "CalRemix/Resprites/AD/HeadGlow");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/AstrumDeus/AstrumDeusHeadGlow3")))
                Resprite(c, "CalRemix/Resprites/AD/HeadGlow");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/AstrumDeus/AstrumDeusHeadGlow4")))
                Resprite(c, "CalRemix/Resprites/AD/HeadGlow");
        }
        private void AstrumDeusBody(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/AstrumDeus/AstrumDeusBodyAltSpectral")))
                Resprite(c, "CalRemix/Resprites/AD/Body");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/AstrumDeus/AstrumDeusBodyGlow2")))
                Resprite(c, "CalRemix/Resprites/AD/BodyGlow");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/AstrumDeus/AstrumDeusBodyGlow")))
                Resprite(c, "CalRemix/Resprites/AD/BodyGlow");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/AstrumDeus/AstrumDeusBodyAltGlow")))
                Resprite(c, "CalRemix/Resprites/AD/BodyGlow");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/AstrumDeus/AstrumDeusBodyGlow3")))
                Resprite(c, "CalRemix/Resprites/AD/BodyGlow");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/AstrumDeus/AstrumDeusBodyAltGlow2")))
                Resprite(c, "CalRemix/Resprites/AD/BodyGlow");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/AstrumDeus/AstrumDeusBodyGlow4")))
                Resprite(c, "CalRemix/Resprites/AD/BodyGlow");
        }
        private void AstrumDeusTail(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/AstrumDeus/AstrumDeusTailGlow")))
                Resprite(c, "CalRemix/Resprites/AD/TailGlow");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/AstrumDeus/AstrumDeusTailGlow2")))
                Resprite(c, "CalRemix/Resprites/AD/TailGlow");
        }
        private void Providence(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Providence/")))
                Resprite(c, "CalRemix/Resprites/Providence/");
        }
        private void Yharon(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Yharon/YharonGlowGreen")))
                Resprite(c, "CalRemix/Resprites/Yharon/YharonGlowGreen");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Yharon/YharonGlowPurple")))
                Resprite(c, "CalRemix/Resprites/Yharon/YharonGlowPurple");
        }
        #endregion

        #region Projectiles
        private void InfestedClawmerangProj(ILContext il)
        {
            var c = new ILCursor(il);
            var t = typeof(ModProjectile).GetMethod("get_Texture", BindingFlags.Public | BindingFlags.Instance);
            if (c.TryGotoNext(i => i.MatchCallvirt(t)))
                Resprite(c, "CalRemix/Resprites/Crabulon/Shroomerang");
        }
        private void EldritchTentacle(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdcI4(60)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldc_I4, 188);
            }
        }
        private void MurasamaSlash(ILContext il)
        {
            var c = new ILCursor(il);
            var t = typeof(ModProjectile).GetMethod("get_Texture", BindingFlags.Public | BindingFlags.Instance);
            if (c.TryGotoNext(i => i.MatchCallvirt(t)))
                Resprite(c, "CalRemix/Resprites/MurasamaSlash");
        }
        private void ExobladeProj(ILContext il)
        {
            var c = new ILCursor(il);
            var t = typeof(ModProjectile).GetMethod("get_Texture", BindingFlags.Public | BindingFlags.Instance);
            if (c.TryGotoNext(i => i.MatchCallvirt(t)))
                Resprite(c, "CalRemix/Resprites/Exo/Blade");
        }
        private void HeavenlyGaleProj(ILContext il)
        {
            var c = new ILCursor(il);
            var t = typeof(ModProjectile).GetMethod("get_Texture", BindingFlags.Public | BindingFlags.Instance);
            if (c.TryGotoNext(i => i.MatchCallvirt(t)))
                Resprite(c, "CalRemix/Resprites/Exo/GaleProj");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/Projectiles/Ranged/HeavenlyGaleProjGlow")))
                Resprite(c, "CalRemix/Resprites/Exo/GaleProjGlow");
        }
        private void CelestusProj(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/Projectiles/Rogue/CelestusProjGlow")))
                Resprite(c, "CalRemix/Resprites/Blank");
            if (c.TryGotoNext(i => i.MatchLdcI4(132)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldstr, 82);
            }
            if (c.TryGotoNext(i => i.MatchLdcI4(132)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldstr, 82);
            }
        }
        private void ViolenceThrownProjectile(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchCallvirt(typeof(ModProjectile).GetMethod("get_Texture", BindingFlags.Public | BindingFlags.Instance))))
                Resprite(c, "CalRemix/Resprites/Violence");
        }
        #endregion
        private static void Resprite(ILCursor c, string s)
        {
            c.Index++;
            c.Emit(OpCodes.Pop);
            c.Emit(OpCodes.Ldstr, s);
        }
    }
    public class RespriteNPC : GlobalNPC
    {
        public override void SetDefaults(NPC npc)
        {
            if (npc.type == NPCType<DesertScourgeHead>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Resprites/DS/Head");
            }
            else if (npc.type == NPCType<DesertScourgeBody>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Resprites/DS/Body");
            }
            else if (npc.type == NPCType<DesertScourgeTail>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Resprites/DS/Tail");
            }
            else if (npc.type == NPCType<DankCreeper>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Resprites/HiveMind/DankCreeper");
            }
            else if (npc.type == NPCType<HiveBlob>() || npc.type == NPCType<HiveBlob2>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Resprites/HiveMind/HiveBlob");
            }
            else if (npc.type == NPCType<DarkHeart>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Resprites/HiveMind/DarkHeart");
            }
            else if (npc.type == NPCType<PerforatorCyst>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Resprites/Perfs/Cyst");
            }
            else if (npc.type == NPCType<PerforatorHive>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Resprites/Perfs/Hive");
            }
            else if (npc.type == NPCType<SlimeGodCore>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Resprites/SlimeGod/Core");
                TextureAssets.NpcHeadBoss[npc.GetBossHeadTextureIndex()] = Request<Texture2D>("CalRemix/Resprites/SlimeGod/Map");
            }
            else if (npc.type == NPCType<CrimulanPaladin>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Resprites/SlimeGod/CrimulanSlimeGod");
            }
            else if (npc.type == NPCType<SplitCrimulanPaladin>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Resprites/SlimeGod/CrimulanSlimeGod");
            }
            else if (npc.type == NPCType<EbonianPaladin>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Resprites/SlimeGod/EbonianSlimeGod");
            }
            else if (npc.type == NPCType<SplitEbonianPaladin>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Resprites/SlimeGod/EbonianSlimeGod");
            }
            else if (npc.type == NPCType<Cryogen>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Resprites/Cryogen/CryogenPhase1");
            }
            else if (npc.type == NPCType<CryogenShield>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Resprites/Cryogen/CryogenShield");
            }
            else if (npc.type == NPCType<CalamitasClone>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Resprites/Cal/CalamitasClone");
            }
            else if (npc.type == NPCType<Cataclysm>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Resprites/Cal/Cataclysm");
            }
            else if (npc.type == NPCType<Catastrophe>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Resprites/Cal/Catastrophe");
            }
            else if (npc.type == NPCType<RavagerBody>())
            {
                TextureAssets.NpcHeadBoss[npc.GetBossHeadTextureIndex()] = Request<Texture2D>("CalRemix/Resprites/RavagerMap");
            }
            else if (npc.type == NPCType<AstrumDeusHead>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Resprites/AD/Head");
            }
            else if (npc.type == NPCType<AstrumDeusBody>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Resprites/AD/Body");
            }
            else if (npc.type == NPCType<AstrumDeusTail>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Resprites/AD/Tail");
            }
            else if (npc.type == NPCType<Yharon>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Resprites/Yharon/Yharon");
            }
            else if (npc.type == NPCType<Eidolist>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Resprites/Eidolist");
            }
        }
        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (npc.type == NPCType<ProfanedGuardianCommander>())
                MaskDraw(1, npc, spriteBatch, screenPos, drawColor);
            else if (npc.type == NPCType<ProfanedGuardianDefender>())
                MaskDraw(2, npc, spriteBatch, screenPos, drawColor);
            else if (npc.type == NPCType<ProfanedGuardianHealer>())
                MaskDraw(3, npc, spriteBatch, screenPos, drawColor);
        }
        private static void MaskDraw(int num, NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D mask = Request<Texture2D>("CalRemix/Resprites/Guardians/DreamMask" + num, AssetRequestMode.ImmediateLoad).Value;
            Rectangle sourceRectangle = new Rectangle(0, 0, mask.Width, mask.Height);
            Vector2 origin = sourceRectangle.Size() / 2f;
            Vector2 draw = npc.Center - screenPos + new Vector2(0f, npc.gfxOffY);
            SpriteEffects spriteEffects = (npc.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(mask, draw, sourceRectangle, drawColor, npc.rotation, origin, npc.scale, spriteEffects, 0f);
        }
    }
    public class RespriteItem : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (item.type == ItemType<PearlShard>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Resprites/PearlShard");
            }
            else if (item.type == ItemType<BloodyWormFood>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Resprites/Perfs/BloodyWormFood");
            }
            else if (item.type == ItemType<BloodSample>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Resprites/Perfs/BloodSample");
            }
            else if (item.type == ItemType<Nadir>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Resprites/Nadir");
            }
            else if (item.type == ItemType<Violence>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Resprites/Violence");
            }
            #region Desert Scourge
            else if (item.type == ItemType<DesertMedallion>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Resprites/DS/DesertMedallion");
            }
            else if (item.type == ItemType<OceanCrest>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Resprites/DS/OceanCrest");
            }
            else if (item.type == ItemType<AquaticDischarge>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Resprites/DS/AquaticDischarge");
            }
            else if (item.type == ItemType<Barinade>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Resprites/DS/Barinade");
            }
            else if (item.type == ItemType<StormSpray>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Resprites/DS/StormSpray");
            }
            else if (item.type == ItemType<SeaboundStaff>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Resprites/DS/SeaboundStaff");
            }
            else if (item.type == ItemType<ScourgeoftheDesert>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Resprites/DS/ScourgeoftheDesert");
            }
            #endregion
            #region Crabulon
            else if (item.type == ItemType<DecapoditaSprout>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Resprites/Crabulon/DecapoditaSprout");
            }
            else if (item.type == ItemType<FungalClump>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Resprites/Crabulon/FungalClump");
            }
            else if (item.type == ItemType<MycelialClaws>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Resprites/Crabulon/MycelialClaws");
            }
            else if (item.type == ItemType<Fungicide>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Resprites/Crabulon/Fungicide");
            }
            else if (item.type == ItemType<HyphaeRod>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Resprites/Crabulon/HyphaeRod");
            }
            else if (item.type == ItemType<Mycoroot>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Resprites/Crabulon/Mycoroot");
            }
            else if (item.type == ItemType<InfestedClawmerang>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Resprites/Crabulon/Shroomerang");
            }
            #endregion
            #region Slime God
            else if (item.type == ItemType<ManaPolarizer>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Resprites/SlimeGod/Polarizer");
            }
            else if (item.type == ItemType<AbyssalTome>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Resprites/SlimeGod/AbyssalTome");
            }
            else if (item.type == ItemType<EldritchTome>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Resprites/SlimeGod/EldritchTome");
            }
            else if (item.type == ItemType<CrimslimeStaff>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Resprites/SlimeGod/CrimslimeStaff");
            }
            else if (item.type == ItemType<CorroslimeStaff>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Resprites/SlimeGod/CorroslimeStaff");
            }
            #endregion
            #region Exo
            else if (item.type == ItemType<MiracleMatter>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Resprites/Exo/Matter");
            }
            else if (item.type == ItemType<Exoblade>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Resprites/Exo/Blade");
            }
            else if (item.type == ItemType<HeavenlyGale>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Resprites/Exo/Gale");
            }
            else if (item.type == ItemType<Photoviscerator>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Resprites/Exo/Vis");
            }
            else if (item.type == ItemType<MagnomalyCannon>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Resprites/Exo/Cannon");
            }
            else if (item.type == ItemType<SubsumingVortex>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Resprites/Exo/Vortex");
            }
            else if (item.type == ItemType<VividClarity>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Resprites/Exo/Clarity");
            }
            else if (item.type == ItemType<CosmicImmaterializer>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Resprites/Exo/Im");
            }
            else if (item.type == ItemType<Celestus>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Resprites/Exo/Celestus");
            }
            else if (item.type == ItemType<Supernova>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Resprites/Exo/Supernova");
            }
            #endregion
        }
    }
    public class RespriteProj : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public override void SetDefaults(Projectile projectile)
        {
            if (projectile.type == ProjectileType<AquaticDischargeProj>())
            {
                TextureAssets.Projectile[projectile.type] = Request<Texture2D>("CalRemix/Resprites/DS/AquaticDischarge");
            }
            else if (projectile.type == ProjectileType<ScourgeoftheDesertProj>())
            {
                TextureAssets.Projectile[projectile.type] = Request<Texture2D>("CalRemix/Resprites/DS/ScourgeoftheDesert");
            }
            else if (projectile.type == ProjectileType<MycorootProj>())
            {
                TextureAssets.Projectile[projectile.type] = Request<Texture2D>("CalRemix/Resprites/Crabulon/Mycoroot");
            }
            else if (projectile.type == ProjectileType<InfestedClawmerangProj>())
            {
                TextureAssets.Projectile[projectile.type] = Request<Texture2D>("CalRemix/Resprites/Crabulon/Shroomerang");
            }
            else if (projectile.type == ProjectileType<UnstableCrimulanGlob>())
            {
                TextureAssets.Projectile[projectile.type] = Request<Texture2D>("CalRemix/Resprites/SlimeGod/CBall");
            }
            else if (projectile.type == ProjectileType<UnstableEbonianGlob>())
            {
                TextureAssets.Projectile[projectile.type] = Request<Texture2D>("CalRemix/Resprites/SlimeGod/EBall");
            }
            else if (projectile.type == ProjectileType<AbyssBall>())
            {
                TextureAssets.Projectile[projectile.type] = Request<Texture2D>("CalRemix/Resprites/SlimeGod/EBall");
            }
            else if (projectile.type == ProjectileType<NadirSpear>())
            {
                TextureAssets.Projectile[projectile.type] = Request<Texture2D>("CalRemix/Resprites/NadirSpear");
            }
            else if (projectile.type == ProjectileType<VoidEssence>())
            {
                TextureAssets.Projectile[projectile.type] = Request<Texture2D>("CalRemix/Resprites/VoidEssence");
            }
            else if (projectile.type == ProjectileType<ExobladeProj>())
            {
                TextureAssets.Projectile[projectile.type] = Request<Texture2D>("CalRemix/Resprites/Exo/Blade");
            }
            else if (projectile.type == ProjectileType<CelestusProj>())
            {
                TextureAssets.Projectile[projectile.type] = Request<Texture2D>("CalRemix/Resprites/Exo/Celestus");
            }
            else if (projectile.type == ProjectileType<SupernovaBomb>())
            {
                TextureAssets.Projectile[projectile.type] = Request<Texture2D>("CalRemix/Resprites/Exo/Supernova");
            }
        }
        public override Color? GetAlpha(Projectile projectile, Color lightColor)
        {
            if ((!Main.dayTime || BossRushEvent.BossRushActive) && (projectile.type == ProjectileType<HolyBlast>() || projectile.type == ProjectileType<HolyBomb>() || projectile.type == ProjectileType<HolyFire>() || projectile.type == ProjectileType<HolyFire2>() || projectile.type == ProjectileType<HolyFlare>() || projectile.type == ProjectileType<MoltenBlob>() || projectile.type == ProjectileType<MoltenBlast>()))
                return (projectile.type == ProjectileType<HolyBlast>()) ? Color.DarkSlateBlue : Color.MediumPurple;
            return null;
        }
    }
}
