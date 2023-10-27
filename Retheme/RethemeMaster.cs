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
using CalamityMod.NPCs.BrimstoneElemental;
using CalamityMod.NPCs.SupremeCalamitas;
using CalamityMod.NPCs.TownNPCs;
using Terraria.ID;
using CalamityMod.Items.Placeables.Ores;
using System.Collections.Generic;
using CalamityMod;
using CalamityMod.Items.Placeables.Furniture;
using CalamityMod.Items.Placeables.FurnitureAbyss;

namespace CalRemix.Retheme
{
    public static class RethemeMaster
    {
        internal static List<int> Torch = new()
        {
            ItemID.RainbowTorch,
            ItemID.UltrabrightTorch,
            ItemID.IchorTorch,
            ItemID.BoneTorch,
            ItemID.CursedTorch,
            ItemID.DemonTorch,
            ItemID.IceTorch,
            ItemID.JungleTorch,
            ItemID.CrimsonTorch,
            ItemID.CorruptTorch,
            ItemID.HallowedTorch,
            ItemID.Torch,
            ItemType<AstralTorch>(),
            ItemType<SulphurousTorch>(),
            ItemType<GloomTorch>(),
            ItemType<AbyssTorch>(),
            ItemType<AlgalPrismTorch>(),
            ItemType<NavyPrismTorch>(),
            ItemType<RefractivePrismTorch>()
        };
        public static void RethemeNPCDefaults(NPC npc)
        {
            #region Resprites
            if (npc.type == NPCType<DesertScourgeHead>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Retheme/DS/Head");
            }
            else if (npc.type == NPCType<DesertScourgeBody>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Retheme/DS/Body");
            }
            else if (npc.type == NPCType<DesertScourgeTail>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Retheme/DS/Tail");
            }
            else if (npc.type == NPCType<DankCreeper>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Retheme/HiveMind/DankCreeper");
            }
            else if (npc.type == NPCType<HiveBlob>() || npc.type == NPCType<HiveBlob2>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Retheme/HiveMind/HiveBlob");
            }
            else if (npc.type == NPCType<DarkHeart>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Retheme/HiveMind/DarkHeart");
            }
            else if (npc.type == NPCType<PerforatorCyst>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Retheme/Perfs/Cyst");
            }
            else if (npc.type == NPCType<PerforatorHive>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Retheme/Perfs/Hive");
            }
            else if (npc.type == NPCType<SlimeGodCore>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Retheme/SlimeGod/Core");
                TextureAssets.NpcHeadBoss[npc.GetBossHeadTextureIndex()] = Request<Texture2D>("CalRemix/Retheme/SlimeGod/Map");
            }
            else if (npc.type == NPCType<CrimulanPaladin>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Retheme/SlimeGod/CrimulanSlimeGod");
            }
            else if (npc.type == NPCType<SplitCrimulanPaladin>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Retheme/SlimeGod/CrimulanSlimeGod");
            }
            else if (npc.type == NPCType<EbonianPaladin>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Retheme/SlimeGod/EbonianSlimeGod");
            }
            else if (npc.type == NPCType<SplitEbonianPaladin>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Retheme/SlimeGod/EbonianSlimeGod");
            }
            else if (npc.type == NPCType<Cryogen>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Retheme/Cryogen/CryogenPhase1");
            }
            else if (npc.type == NPCType<CryogenShield>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Retheme/Cryogen/CryogenShield");
            }
            else if (npc.type == NPCType<CalamitasClone>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Retheme/Cal/CalamitasClone");
            }
            else if (npc.type == NPCType<Cataclysm>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Retheme/Cal/Cataclysm");
            }
            else if (npc.type == NPCType<Catastrophe>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Retheme/Cal/Catastrophe");
            }
            else if (npc.type == NPCType<RavagerBody>())
            {
                TextureAssets.NpcHeadBoss[npc.GetBossHeadTextureIndex()] = Request<Texture2D>("CalRemix/Retheme/RavagerMap");
            }
            else if (npc.type == NPCType<AstrumDeusHead>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Retheme/AD/Head");
            }
            else if (npc.type == NPCType<AstrumDeusBody>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Retheme/AD/Body");
            }
            else if (npc.type == NPCType<AstrumDeusTail>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Retheme/AD/Tail");
            }
            else if (npc.type == NPCType<Yharon>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Retheme/Yharon/Yharon");
            }
            else if (npc.type == NPCType<Eidolist>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Retheme/Eidolist");
            }
            #endregion
            #region Renames
            if (npc.type == NPCType<BrimstoneElemental>())
            {
                npc.GivenName = "Calamity Elemental";
            }
            else if (npc.type == NPCType<BrimstoneHeart>())
            {
                npc.GivenName = "Calamity Heart";
            }
            #endregion
        }
        public static void RethemeTypeName(NPC npc, ref string typeName)
        {
            if (npc.type == NPCType<WITCH>())
            {
                typeName = "Calamity Witch";
            }
            else if (npc.type == NPCType<BrimstoneElemental>())
            {
                typeName = "Calamity Elemental";
            }
            else if (npc.type == NPCType<BrimstoneHeart>())
            {
                typeName = "Calamity Heart";
            }
        }
        public static void RethemeNPCPostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
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
            Texture2D mask = Request<Texture2D>("CalRemix/Retheme/Guardians/DreamMask" + num, AssetRequestMode.ImmediateLoad).Value;
            Rectangle sourceRectangle = new Rectangle(0, 0, mask.Width, mask.Height);
            Vector2 origin = sourceRectangle.Size() / 2f;
            Vector2 draw = npc.Center - screenPos + new Vector2(0f, npc.gfxOffY);
            SpriteEffects spriteEffects = (npc.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(mask, draw, sourceRectangle, drawColor, npc.rotation, origin, npc.scale, spriteEffects, 0f);
        }
        public static void RethemeItemDefaults(Item item)
        {
            #region Resprites
            if (item.type == ItemType<PearlShard>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/PearlShard");
            }
            else if (item.type == ItemType<BloodyWormFood>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/Perfs/BloodyWormFood");
            }
            else if (item.type == ItemType<BloodSample>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/Perfs/BloodSample");
            }
            else if (item.type == ItemType<Nadir>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/Nadir");
            }
            else if (item.type == ItemType<Violence>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/Violence");
            }
            #region Desert Scourge
            else if (item.type == ItemType<DesertMedallion>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/DS/DesertMedallion");
            }
            else if (item.type == ItemType<OceanCrest>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/DS/OceanCrest");
            }
            else if (item.type == ItemType<AquaticDischarge>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/DS/AquaticDischarge");
            }
            else if (item.type == ItemType<Barinade>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/DS/Barinade");
            }
            else if (item.type == ItemType<StormSpray>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/DS/StormSpray");
            }
            else if (item.type == ItemType<SeaboundStaff>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/DS/SeaboundStaff");
            }
            else if (item.type == ItemType<ScourgeoftheDesert>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/DS/ScourgeoftheDesert");
            }
            #endregion
            #region Crabulon
            else if (item.type == ItemType<DecapoditaSprout>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/Crabulon/DecapoditaSprout");
            }
            else if (item.type == ItemType<FungalClump>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/Crabulon/FungalClump");
            }
            else if (item.type == ItemType<MycelialClaws>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/Crabulon/MycelialClaws");
            }
            else if (item.type == ItemType<Fungicide>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/Crabulon/Fungicide");
            }
            else if (item.type == ItemType<HyphaeRod>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/Crabulon/HyphaeRod");
            }
            else if (item.type == ItemType<Mycoroot>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/Crabulon/Mycoroot");
            }
            else if (item.type == ItemType<InfestedClawmerang>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/Crabulon/Shroomerang");
            }
            #endregion
            #region Slime God
            else if (item.type == ItemType<ManaPolarizer>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/SlimeGod/Polarizer");
            }
            else if (item.type == ItemType<AbyssalTome>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/SlimeGod/AbyssalTome");
            }
            else if (item.type == ItemType<EldritchTome>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/SlimeGod/EldritchTome");
            }
            else if (item.type == ItemType<CrimslimeStaff>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/SlimeGod/CrimslimeStaff");
            }
            else if (item.type == ItemType<CorroslimeStaff>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/SlimeGod/CorroslimeStaff");
            }
            #endregion
            #region Exo
            else if (item.type == ItemType<MiracleMatter>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/Exo/Matter");
            }
            else if (item.type == ItemType<Exoblade>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/Exo/Blade");
            }
            else if (item.type == ItemType<HeavenlyGale>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/Exo/Gale");
            }
            else if (item.type == ItemType<Photoviscerator>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/Exo/Vis");
            }
            else if (item.type == ItemType<MagnomalyCannon>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/Exo/Cannon");
            }
            else if (item.type == ItemType<SubsumingVortex>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/Exo/Vortex");
            }
            else if (item.type == ItemType<VividClarity>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/Exo/Clarity");
            }
            else if (item.type == ItemType<CosmicImmaterializer>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/Exo/Im");
            }
            else if (item.type == ItemType<Celestus>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/Exo/Celestus");
            }
            else if (item.type == ItemType<Supernova>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/Exo/Supernova");
            }
            #endregion
            #endregion
            #region Text Changes
            if (item.type == ItemType<PearlShard>())
            {
                item.SetNameOverride("Conquest Fragment");
                item.rare = ItemRarityID.Orange;
            }
            else if (item.type == ItemType<InfestedClawmerang>())
            {
                item.SetNameOverride("Shroomerang");
            }
            else if (item.type == ItemType<PhantomicArtifact>())
            {
                item.SetNameOverride("Phantomic Soul Artifact");
            }
            else if (item.type == ItemType<UelibloomOre>())
            {
                item.SetNameOverride("Tarragon Ore");
            }
            else if (item.type == ItemType<UelibloomBar>())
            {
                item.SetNameOverride("Tarragon Bar");
            }
            else if (item.type == ItemType<CosmiliteBar>())
            {
                item.rare = ItemRarityID.Purple;
            }
            #endregion
        }
        public static void RethemeTooltips(Mod mod, Item item, List<TooltipLine> tooltips)
        {
            if (item.type == ItemType<PearlShard>())
            {
                var line = new TooltipLine(mod, "ConquestFragment", "\'Victory is yours!\'");
                tooltips.Add(line);
            }
            if (item.type == ItemType<PhantomicArtifact>())
            {
                var line = new TooltipLine(mod, "PhantomicSoulArtifact", "Judgement");
                tooltips.Add(line);
            }
            if (item.type == ItemType<GrandGelatin>())
            {
                var line = new TooltipLine(mod, "GrandGelatinRemix", "Reduces stealth costs by 3%");
                tooltips.Add(line);
            }
            if (item.type == ItemType<TheAbsorber>())
            {
                var line = new TooltipLine(mod, "AbsorberRemix", "Your health is capped at 50% while the accessory is visable");
                tooltips.Add(line);
            }
            if (item.type == ItemType<TheSponge>())
            {
                var line = new TooltipLine(mod, "SpongeRemix", "Effects of Ursa Sergeant, Amidias' Spark, Permafrost's Concocion, Flame-Licked Shell, Aquatic Heart, and Trinket of Chi\nYour health is capped at 50% while the accessory is visable");
                tooltips.Add(line);
            }
            if (item.type == ItemType<AmbrosialAmpoule>())
            {
                var line = new TooltipLine(mod, "AmbrosiaRemix", "Effects of Honew Dew, and increased mining speed and defense while underground");
                tooltips.Add(line);
            }
            if (item.type == ItemType<AbyssalDivingGear>())
            {
                var line = new TooltipLine(mod, "DivingGearRemix", "Pacifies all normal ocean enemies");
                tooltips.Add(line);
            }
            if (item.type == ItemType<AbyssalDivingSuit>())
            {
                var line = new TooltipLine(mod, "DivingSuitRemix", "Effects of Lumenous Amulet, Alluring Bait, and Aquatic Emblem\nReveals treasure while the accessory is visible");
                tooltips.Add(line);
            }
            if (item.type == ItemType<TheAmalgam>())
            {
                var line = new TooltipLine(mod, "AmalgamRemix", "Effects of Giant Pearl, Frost Flare, Void of Extinction, Purity, Plague Hive, Old Duke's Scales, Affliction, and The Evolution\nYou passively rain down brimstone flames and leave behind a trail of gas and bees\nMana Overloader effect while the accessory is visible");
                tooltips.Add(line);
            }
            if (item.type == ItemType<DesertMedallion>())
            {
                var line = new TooltipLine(mod, "MedallionRemix", "Drops from Cnidrions after defeating the Wulfrum Excavator");
                tooltips.Add(line);
            }
            if (item.type == ItemType<CryoKey>())
            {
                var line = new TooltipLine(mod, "CryoKeyRemix", "Drops from Primal Aspids");
                tooltips.Add(line);
            }
            if (item.type == ItemType<EyeofDesolation>())
            {
                var line = new TooltipLine(mod, "EyeofDesolationRemix", "Drops from Clamitas");
                tooltips.Add(line);
            }
            if (item.type == ItemType<Abombination>())
            {
                tooltips.FindAndReplace("the Jungle", "the Plagued Jungle");
                tooltips.FindAndReplace("the Jungle", "the Plagued Jungle [c/C61B40:(yes, she enrages in the normal Jungle)]");
            }
            if (Torch.Contains(item.type))
            {
                var line = new TooltipLine(mod, "TorchRemix", "Can be used as ammo for the Driftorcher");
                line.OverrideColor = Color.OrangeRed;
                tooltips.Add(line);
            }
        }
        public static void RethemeProjDefaults(Projectile projectile)
        {
            #region Resprites
            if (projectile.type == ProjectileType<AquaticDischargeProj>())
            {
                TextureAssets.Projectile[projectile.type] = Request<Texture2D>("CalRemix/Retheme/DS/AquaticDischarge");
            }
            else if (projectile.type == ProjectileType<ScourgeoftheDesertProj>())
            {
                TextureAssets.Projectile[projectile.type] = Request<Texture2D>("CalRemix/Retheme/DS/ScourgeoftheDesert");
            }
            else if (projectile.type == ProjectileType<MycorootProj>())
            {
                TextureAssets.Projectile[projectile.type] = Request<Texture2D>("CalRemix/Retheme/Crabulon/Mycoroot");
            }
            else if (projectile.type == ProjectileType<InfestedClawmerangProj>())
            {
                TextureAssets.Projectile[projectile.type] = Request<Texture2D>("CalRemix/Retheme/Crabulon/Shroomerang");
            }
            else if (projectile.type == ProjectileType<UnstableCrimulanGlob>())
            {
                TextureAssets.Projectile[projectile.type] = Request<Texture2D>("CalRemix/Retheme/SlimeGod/CBall");
            }
            else if (projectile.type == ProjectileType<UnstableEbonianGlob>())
            {
                TextureAssets.Projectile[projectile.type] = Request<Texture2D>("CalRemix/Retheme/SlimeGod/EBall");
            }
            else if (projectile.type == ProjectileType<AbyssBall>())
            {
                TextureAssets.Projectile[projectile.type] = Request<Texture2D>("CalRemix/Retheme/SlimeGod/EBall");
            }
            else if (projectile.type == ProjectileType<NadirSpear>())
            {
                TextureAssets.Projectile[projectile.type] = Request<Texture2D>("CalRemix/Retheme/NadirSpear");
            }
            else if (projectile.type == ProjectileType<VoidEssence>())
            {
                TextureAssets.Projectile[projectile.type] = Request<Texture2D>("CalRemix/Retheme/VoidEssence");
            }
            else if (projectile.type == ProjectileType<ExobladeProj>())
            {
                TextureAssets.Projectile[projectile.type] = Request<Texture2D>("CalRemix/Retheme/Exo/Blade");
            }
            else if (projectile.type == ProjectileType<CelestusProj>())
            {
                TextureAssets.Projectile[projectile.type] = Request<Texture2D>("CalRemix/Retheme/Exo/Celestus");
            }
            else if (projectile.type == ProjectileType<SupernovaBomb>())
            {
                TextureAssets.Projectile[projectile.type] = Request<Texture2D>("CalRemix/Retheme/Exo/Supernova");
            }
            #endregion
            #region Rename
            if (projectile.type == ProjectileType<BrimstoneBall>())
            {
                projectile.Name = "Calamity Fireball";
            }
            else if (projectile.type == ProjectileType<BrimstoneBarrage>())
            {
                projectile.Name = "Calamity Barrage";
            }
            else if (projectile.type == ProjectileType<BrimstoneFire>())
            {
                projectile.Name = "Calamity Fire";
            }
            else if (projectile.type == ProjectileType<BrimstoneHellblast>())
            {
                projectile.Name = "Calamity Hellblast";
            }
            else if (projectile.type == ProjectileType<BrimstoneHellblast2>())
            {
                projectile.Name = "Calamity Hellblast";
            }
            else if (projectile.type == ProjectileType<BrimstoneHellfireball>())
            {
                projectile.Name = "Calamity Hellfireball";
            }
            else if (projectile.type == ProjectileType<BrimstoneMonster>())
            {
                projectile.Name = "Calamity Monster";
            }
            else if (projectile.type == ProjectileType<BrimstoneRay>())
            {
                projectile.Name = "Calamity Ray";
            }
            else if (projectile.type == ProjectileType<BrimstoneTargetRay>())
            {
                projectile.Name = "Calamity Ray";
            }
            else if (projectile.type == ProjectileType<BrimstoneWave>())
            {
                projectile.Name = "Calamity Flame Skull";
            }
            #endregion
        }
    }
    public class RethemeIL : ModSystem
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
                Retheme(c, "CalRemix/Retheme/Crabulon/CrabulonGlow");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Crabulon/CrabulonAltGlow")))
                Retheme(c, "CalRemix/Retheme/Crabulon/CrabulonAltGlow");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Crabulon/CrabulonAttackGlow")))
                Retheme(c, "CalRemix/Retheme/Crabulon/CrabulonAttackGlow");
        }
        private void PerforatorCyst(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Perforator/PerforatorCystGlow")))
                Retheme(c, "CalRemix/Retheme/Perfs/CystGlow");
        }
        private void PerforatorHive(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Perforator/PerforatorHiveGlow")))
                Retheme(c, "CalRemix/Retheme/Perfs/HiveGlow");
        }
        private void Cryogen(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Cryogen/Cryogen_Phase")))
                Retheme(c, "CalRemix/Retheme/Cryogen/CryogenPhase");
        }
        private void CryogenShield(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Cryogen/CryogenShield")))
                Retheme(c, "CalRemix/Retheme/Cryogen/CryogenShield");
        }
        private void CalamitasClone(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/CalClone/CalamitasCloneGlow")))
                Retheme(c, "CalRemix/Retheme/Cal/CalamitasGlow");
        }
        private void Cataclysm(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/CalClone/CataclysmGlow")))
                Retheme(c, "CalRemix/Retheme/Cal/CataclysmGlow");
        }
        private void Catastrophe(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/CalClone/CatastropheGlow")))
                Retheme(c, "CalRemix/Retheme/Cal/CatastropheGlow");
        }
        private void AstrumDeusHead(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/AstrumDeus/AstrumDeusHeadGlow2")))
                Retheme(c, "CalRemix/Retheme/AD/HeadGlow");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/AstrumDeus/AstrumDeusHeadGlow")))
                Retheme(c, "CalRemix/Retheme/AD/HeadGlow");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/AstrumDeus/AstrumDeusHeadGlow3")))
                Retheme(c, "CalRemix/Retheme/AD/HeadGlow");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/AstrumDeus/AstrumDeusHeadGlow4")))
                Retheme(c, "CalRemix/Retheme/AD/HeadGlow");
        }
        private void AstrumDeusBody(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/AstrumDeus/AstrumDeusBodyAltSpectral")))
                Retheme(c, "CalRemix/Retheme/AD/Body");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/AstrumDeus/AstrumDeusBodyGlow2")))
                Retheme(c, "CalRemix/Retheme/AD/BodyGlow");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/AstrumDeus/AstrumDeusBodyGlow")))
                Retheme(c, "CalRemix/Retheme/AD/BodyGlow");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/AstrumDeus/AstrumDeusBodyAltGlow")))
                Retheme(c, "CalRemix/Retheme/AD/BodyGlow");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/AstrumDeus/AstrumDeusBodyGlow3")))
                Retheme(c, "CalRemix/Retheme/AD/BodyGlow");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/AstrumDeus/AstrumDeusBodyAltGlow2")))
                Retheme(c, "CalRemix/Retheme/AD/BodyGlow");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/AstrumDeus/AstrumDeusBodyGlow4")))
                Retheme(c, "CalRemix/Retheme/AD/BodyGlow");
        }
        private void AstrumDeusTail(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/AstrumDeus/AstrumDeusTailGlow")))
                Retheme(c, "CalRemix/Retheme/AD/TailGlow");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/AstrumDeus/AstrumDeusTailGlow2")))
                Retheme(c, "CalRemix/Retheme/AD/TailGlow");
        }
        private void Providence(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Providence/")))
                Retheme(c, "CalRemix/Retheme/Providence/");
        }
        private void Yharon(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Yharon/YharonGlowGreen")))
                Retheme(c, "CalRemix/Retheme/Yharon/YharonGlowGreen");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Yharon/YharonGlowPurple")))
                Retheme(c, "CalRemix/Retheme/Yharon/YharonGlowPurple");
        }
        #endregion
        #region Projectiles
        private void InfestedClawmerangProj(ILContext il)
        {
            var c = new ILCursor(il);
            var t = typeof(ModProjectile).GetMethod("get_Texture", BindingFlags.Public | BindingFlags.Instance);
            if (c.TryGotoNext(i => i.MatchCallvirt(t)))
                Retheme(c, "CalRemix/Retheme/Crabulon/Shroomerang");
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
                Retheme(c, "CalRemix/Retheme/MurasamaSlash");
        }
        private void ExobladeProj(ILContext il)
        {
            var c = new ILCursor(il);
            var t = typeof(ModProjectile).GetMethod("get_Texture", BindingFlags.Public | BindingFlags.Instance);
            if (c.TryGotoNext(i => i.MatchCallvirt(t)))
                Retheme(c, "CalRemix/Retheme/Exo/Blade");
        }
        private void HeavenlyGaleProj(ILContext il)
        {
            var c = new ILCursor(il);
            var t = typeof(ModProjectile).GetMethod("get_Texture", BindingFlags.Public | BindingFlags.Instance);
            if (c.TryGotoNext(i => i.MatchCallvirt(t)))
                Retheme(c, "CalRemix/Retheme/Exo/GaleProj");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/Projectiles/Ranged/HeavenlyGaleProjGlow")))
                Retheme(c, "CalRemix/Retheme/Exo/GaleProjGlow");
        }
        private void CelestusProj(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/Projectiles/Rogue/CelestusProjGlow")))
                Retheme(c, "CalRemix/Retheme/Blank");
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
                Retheme(c, "CalRemix/Retheme/Violence");
        }
        #endregion
        private static void Retheme(ILCursor c, string s)
        {
            c.Index++;
            c.Emit(OpCodes.Pop);
            c.Emit(OpCodes.Ldstr, s);
        }
    }
}
