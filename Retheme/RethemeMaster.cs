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
using CalamityMod.NPCs.Other;
using CalamityMod.NPCs.BrimstoneElemental;
using CalamityMod.NPCs.SupremeCalamitas;
using CalamityMod.NPCs.TownNPCs;
using Terraria.ID;
using CalamityMod.Items.Placeables.Ores;
using System.Collections.Generic;
using CalamityMod;
using CalamityMod.Items.Placeables.Furniture;
using CalamityMod.Items.Placeables.FurnitureAbyss;
using CalamityMod.NPCs.ExoMechs;
using CalamityMod.NPCs.Crabulon;
using CalamityMod.Items.PermanentBoosters;
using CalamityMod.Projectiles.Typeless;
using System;
using CalamityMod.NPCs.Leviathan;
using CalamityMod.Projectiles.Summon;
using CalamityMod.NPCs.AstrumAureus;
using CalamityMod.CalPlayer;
using CalamityMod.NPCs;
using CalamityMod.NPCs.PlaguebringerGoliath;
using CalamityMod.Items.Armor.Fearmonger;

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
                TextureAssets.NpcHeadBoss[npc.GetBossHeadTextureIndex()] = Request<Texture2D>("CalRemix/Retheme/DS/Map");
            }
            else if (npc.type == NPCType<DesertScourgeBody>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Retheme/DS/Body");
            }
            else if (npc.type == NPCType<DesertScourgeTail>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Retheme/DS/Tail");
            }
            else if (npc.type == NPCType<DesertNuisanceHead>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Retheme/DS/NHead");
                TextureAssets.NpcHeadBoss[npc.GetBossHeadTextureIndex()] = Request<Texture2D>("CalRemix/Retheme/DS/NMap");
            }
            else if (npc.type == NPCType<DesertNuisanceBody>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Retheme/DS/NBody");
            }
            else if (npc.type == NPCType<DesertNuisanceTail>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Retheme/DS/NTail");
            }
            else if (npc.type == NPCType<Crabulon>())
            {
                TextureAssets.NpcHeadBoss[npc.GetBossHeadTextureIndex()] = Request<Texture2D>("CalRemix/Retheme/Crabulon/Map");
            }
            else if (npc.type == NPCType<HiveTumor>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Retheme/HiveMind/HiveTumor");
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
            else if (npc.type == NPCType<HiveMind>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Retheme/HiveMind/HiveMind");
                TextureAssets.NpcHeadBoss[npc.GetBossHeadTextureIndex()] = Request<Texture2D>("CalRemix/Retheme/HiveMind/Map");
            }
            else if (npc.type == NPCType<PerforatorCyst>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Retheme/Perfs/Cyst");
            }
            else if (npc.type == NPCType<PerforatorBodyLarge>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Retheme/Perfs/LBody");
            }
            else if (npc.type == NPCType<PerforatorBodyMedium>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Retheme/Perfs/MBody");
            }
            else if (npc.type == NPCType<PerforatorBodySmall>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Retheme/Perfs/SBody");
            }
            else if (npc.type == NPCType<PerforatorHeadLarge>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Retheme/Perfs/LHead");
                TextureAssets.NpcHeadBoss[npc.GetBossHeadTextureIndex()] = Request<Texture2D>("CalRemix/Retheme/Perfs/LMap");
            }
            else if (npc.type == NPCType<PerforatorHeadMedium>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Retheme/Perfs/MHead");
                TextureAssets.NpcHeadBoss[npc.GetBossHeadTextureIndex()] = Request<Texture2D>("CalRemix/Retheme/Perfs/MMap");
            }
            else if (npc.type == NPCType<PerforatorHeadSmall>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Retheme/Perfs/SHead");
                TextureAssets.NpcHeadBoss[npc.GetBossHeadTextureIndex()] = Request<Texture2D>("CalRemix/Retheme/Perfs/SMap");
            }
            else if (npc.type == NPCType<PerforatorTailLarge>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Retheme/Perfs/LTail");
            }
            else if (npc.type == NPCType<PerforatorTailMedium>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Retheme/Perfs/MTail");
            }
            else if (npc.type == NPCType<PerforatorTailSmall>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Retheme/Perfs/STail");
            }
            else if (npc.type == NPCType<PerforatorHive>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Retheme/Perfs/Hive");
                TextureAssets.NpcHeadBoss[npc.GetBossHeadTextureIndex()] = Request<Texture2D>("CalRemix/Retheme/Perfs/Map");
            }
            else if (npc.type == NPCType<CrimsonSlimeSpawn>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Retheme/SlimeGod/CrimsonSlimeSpawn");
            }
            else if (npc.type == NPCType<CrimsonSlimeSpawn2>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Retheme/SlimeGod/CrimsonSlimeSpawn2");
            }
            else if (npc.type == NPCType<CorruptSlimeSpawn>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Retheme/SlimeGod/CorruptSlimeSpawn");
            }
            else if (npc.type == NPCType<CorruptSlimeSpawn2>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Retheme/SlimeGod/CorruptSlimeSpawn2");
            }
            else if (npc.type == NPCType<SlimeGodCore>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Retheme/SlimeGod/Core");
                TextureAssets.NpcHeadBoss[npc.GetBossHeadTextureIndex()] = Request<Texture2D>("CalRemix/Retheme/SlimeGod/Map");
            }
            else if (npc.type == NPCType<CrimulanPaladin>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Retheme/SlimeGod/CrimulanSlimeGod");
                TextureAssets.NpcHeadBoss[npc.GetBossHeadTextureIndex()] = Request<Texture2D>("CalRemix/Retheme/SlimeGod/CMap");
            }
            else if (npc.type == NPCType<SplitCrimulanPaladin>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Retheme/SlimeGod/CrimulanSlimeGod");
                TextureAssets.NpcHeadBoss[npc.GetBossHeadTextureIndex()] = Request<Texture2D>("CalRemix/Retheme/SlimeGod/CSMap");
            }
            else if (npc.type == NPCType<EbonianPaladin>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Retheme/SlimeGod/EbonianSlimeGod");
                TextureAssets.NpcHeadBoss[npc.GetBossHeadTextureIndex()] = Request<Texture2D>("CalRemix/Retheme/SlimeGod/EMap");
            }
            else if (npc.type == NPCType<SplitEbonianPaladin>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Retheme/SlimeGod/EbonianSlimeGod");
                TextureAssets.NpcHeadBoss[npc.GetBossHeadTextureIndex()] = Request<Texture2D>("CalRemix/Retheme/SlimeGod/ESMap");
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
            else if (npc.type == NPCType<Anahita>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Retheme/Levi/Anahita");
                TextureAssets.NpcHeadBoss[npc.GetBossHeadTextureIndex()] = Request<Texture2D>("CalRemix/Retheme/Levi/AnaMap");
            }
            else if (npc.type == NPCType<Leviathan>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Retheme/Levi/Levi");
                TextureAssets.NpcHeadBoss[npc.GetBossHeadTextureIndex()] = Request<Texture2D>("CalRemix/Retheme/Levi/LeviMap");
            }
            else if (npc.type == NPCType<AquaticAberration>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Retheme/Levi/AquaticAberration");
            }
            else if (npc.type == NPCType<RavagerBody>())
            {
                TextureAssets.NpcHeadBoss[npc.GetBossHeadTextureIndex()] = Request<Texture2D>("CalRemix/Retheme/RavagerMap");
            }
            else if (npc.type == NPCType<AstrumDeusHead>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Retheme/AD/Head");
                TextureAssets.NpcHeadBoss[npc.GetBossHeadTextureIndex()] = Request<Texture2D>("CalRemix/Retheme/AD/Map");
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
            else if (npc.type == NPCType<THELORDE>() && Main.zenithWorld)
            {
                npc.HitSound = SoundID.NPCHit49;
            }
            else if (npc.type == NPCType<Eidolist>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Retheme/Eidolist");
            }
            #endregion
        }
        public static void RethemeTypeName(NPC npc, ref string typeName)
        {
            if (npc.type == NPCType<BrimstoneElemental>())
            {
                typeName = "Calamity Elemental";
            }
            else if (npc.type == NPCType<Draedon>())
            {
                typeName = "Draedon, the Living Intellect of Samuel Graydron";
            }
            else if (npc.type == NPCType<BrimstoneHeart>())
            {
                typeName = "Calamity Heart";
            }
            else if (npc.type == NPCType<SupremeCalamitas>())
            {
                typeName = "Brimdeath Witch, Calitas Jane";
            }
            else if (npc.type == NPCType<WITCH>())
            {
                typeName = "Calamity Witch";
            }
            else if (npc.type == NPCType<THELORDE>() && Main.zenithWorld)
            {
                typeName = "Vision of the Tyrant";
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
            if (npc.type == NPCType<THELORDE>() && Main.zenithWorld)
            {
                THELORDE lorde = npc.ModNPC as THELORDE;
                if (!lorde.Dying && npc.life > 0)
                {
                    Texture2D value = Request<Texture2D>("CalRemix/Retheme/LORDE/VotTEyes").Value;
                    Vector2 vector = new(value.Width / 4, value.Height / 14);
                    Vector2 position = npc.Center - screenPos - new Vector2((float)value.Width / 2f, (float)value.Height / 7f) * npc.scale / 2f + vector * npc.scale + new Vector2(0f, npc.gfxOffY);
                    Rectangle value2 = value.Frame(2, 7, 0, 1);
                    SpriteEffects effects = (npc.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                    spriteBatch.Draw(value, position, value2, new Color(255, 255, 255, 255), npc.rotation, vector, npc.scale, effects, 0f);
                }
            }
        }
        private static void MaskDraw(int num, NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (npc.life > 0)
            {
                Texture2D mask = Request<Texture2D>("CalRemix/Retheme/Guardians/DreamMask" + num, AssetRequestMode.ImmediateLoad).Value;
                Vector2 draw = npc.Center - screenPos + new Vector2(0f, npc.gfxOffY);
                SpriteEffects spriteEffects = (npc.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                spriteBatch.Draw(mask, draw, null, drawColor, npc.rotation, mask.Size() / 2f, npc.scale, spriteEffects, 0f);
            }
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
            else if (item.type == ItemType<MushroomPlasmaRoot>())
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
            #region Hive Mind
            else if (item.type == ItemType<RottenMatter>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/HiveMind/RottenMatter");
            }
            else if (item.type == ItemType<Teratoma>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/HiveMind/Teratoma");
            }
            else if (item.type == ItemType<RottenBrain>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/HiveMind/RottenBrain");
            }
            else if (item.type == ItemType<FilthyGlove>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/HiveMind/FilthyGlove");
            }
            else if (item.type == ItemType<PerfectDark>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/HiveMind/PerfectDark");
            }
            else if (item.type == ItemType<Shadethrower>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/HiveMind/Shadethrower");
            }
            else if (item.type == ItemType<ShaderainStaff>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/HiveMind/ShaderainStaff");
            }
            else if (item.type == ItemType<DankStaff>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/HiveMind/DankStaff");
            }
            else if (item.type == ItemType<RotBall>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/HiveMind/RotBall");
            }
            #endregion
            #region Perforators
            else if (item.type == ItemType<BloodSample>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/Perfs/BloodSample");
            }
            else if (item.type == ItemType<BloodyWormFood>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/Perfs/BloodyWormFood");
            }
            else if (item.type == ItemType<BloodyWormTooth>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/Perfs/BloodyWormTooth");
            }
            else if (item.type == ItemType<BloodstainedGlove>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/Perfs/BloodstainedGlove");
            }
            else if (item.type == ItemType<Aorta>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/Perfs/Aorta");
            }
            else if (item.type == ItemType<VeinBurster>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/Perfs/VeinBurster");
            }
            else if (item.type == ItemType<SausageMaker>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/Perfs/SausageMaker");
            }
            else if (item.type == ItemType<Eviscerator>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/Perfs/Eviscerator");
            }
            else if (item.type == ItemType<BloodBath>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/Perfs/BloodBath");
            }
            else if (item.type == ItemType<FleshOfInfidelity>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/Perfs/FleshOfInfidelity");
            }
            else if (item.type == ItemType<ToothBall>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/Perfs/ToothBall");
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
            #region Levi
            else if (item.type == ItemType<PearlofEnthrallment>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/Levi/Pearl");
            }
            else if (item.type == ItemType<Greentide>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/Levi/Greentide");
            }
            else if (item.type == ItemType<Leviatitan>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/Levi/Leviatitan");
            }
            else if (item.type == ItemType<AnahitasArpeggio>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/Levi/AnahitasArpeggio");
            }
            else if (item.type == ItemType<Atlantis>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/Levi/Atlantis");
            }
            else if (item.type == ItemType<GastricBelcherStaff>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/Levi/GastricBelcherStaff");
            }
            else if (item.type == ItemType<BrackishFlask>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/Levi/BrackishFlask");
            }
            else if (item.type == ItemType<LeviathanAmbergris>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/Levi/LeviathanAmbergris");
            }
            #endregion
            #region Astrum Deus
            else if (item.type == ItemType<HideofAstrumDeus>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/AD/HideofAstrumDeus");
            }
            else if (item.type == ItemType<TheMicrowave>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/AD/TheMicrowave");
            }
            else if (item.type == ItemType<StarSputter>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/AD/StarSputter");
            }
            else if (item.type == ItemType<StarShower>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/AD/StarShower");
            }
            else if (item.type == ItemType<StarspawnHelixStaff>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/AD/StarspawnHelixStaff");
            }
            else if (item.type == ItemType<RegulusRiot>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/AD/RegulusRiot");
            }
            #endregion
            #region Yharon
            else if (item.type == ItemType<YharonSoulFragment>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/Yharon/YharonSoulFragment");
            }
            else if (item.type == ItemType<DragonRage>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/Yharon/DragonRage");
            }
            else if (item.type == ItemType<DragonsBreath>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/Yharon/DragonsBreath");
            }
            else if (item.type == ItemType<ChickenCannon>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/Yharon/ChickenCannon");
            }
            else if (item.type == ItemType<PhoenixFlameBarrage>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/Yharon/DragonFlameBarrage");
            }
            else if (item.type == ItemType<TheBurningSky>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/Yharon/TheBurningSky");
            }
            else if (item.type == ItemType<FinalDawn>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/Yharon/FinalDawn");
            }
            else if (item.type == ItemType<Wrathwing>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Retheme/Yharon/Wrathwing");
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
            else if (item.type == ItemType<SoulPiercer>())
            {
                item.SetNameOverride("Stream Gouge");
            }
            else if (item.type == ItemType<StreamGouge>())
            {
                item.SetNameOverride("Soul Piercer");
            }
            else if (item.type == ItemType<PhoenixFlameBarrage>())
            {
                item.SetNameOverride("Dragon Flame Barrage");
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
            if (item.type == ItemType<FearmongerGreathelm>())
            {
                tooltips.FindAndReplace("+60 max mana and ", "");
                tooltips.FindAndReplace("20% increased summon damage and +2 max minions", "+1 max minions");
                for (int i = 0; i < tooltips.Count; i++)
                {
                    if (tooltips[i].Text.Contains("Pumpkin"))
                    {
                        tooltips.RemoveAt(i);
                        break;
                    }
                }
                tooltips.Add(new TooltipLine(mod, "FearmongerRemix", "+Set bonus: +1 max minions\nThe minion damage nerf while wielding weaponry is reduced\nAll minion attacks grant regeneration"));
            }
            if (item.type == ItemType<FearmongerPlateMail>())
            {
                tooltips.FindAndReplace("+100 max life and ", "");
                for (int i = 0; i < tooltips.Count; i++)
                {
                    if (tooltips[i].Text.Contains("Pumpkin"))
                    {
                        tooltips.RemoveAt(i);
                    }
                }
                tooltips.Add(new TooltipLine(mod, "FearmongerRemix", "+Set bonus: 1 max minions\nThe minion damage nerf while wielding weaponry is reduced\nAll minion attacks grant regeneration"));
            }
            if (item.type == ItemType<FearmongerGreaves>())
            {
                for (int i = 0; i < tooltips.Count; i++)
                {
                    if (tooltips[i].Text.Contains("Pumpkin"))
                    {
                        tooltips.RemoveAt(i);
                        break;
                    }
                }
                tooltips.Add(new TooltipLine(mod, "FearmongerRemix", "+Set bonus: +1 max minions\nThe minion damage nerf while wielding weaponry is reduced\nAll minion attacks grant regeneration"));
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
            else if (projectile.type == ProjectileType<FungalClumpMinion>())
            {
                TextureAssets.Projectile[projectile.type] = Request<Texture2D>("CalRemix/Retheme/Crabulon/FungalClumpProj");
            }
            else if (projectile.type == ProjectileType<RotBallProjectile>())
            {
                TextureAssets.Projectile[projectile.type] = Request<Texture2D>("CalRemix/Retheme/HiveMind/RotBall");
            }
            else if (projectile.type == ProjectileType<ToothBallProjectile>())
            {
                TextureAssets.Projectile[projectile.type] = Request<Texture2D>("CalRemix/Retheme/Perfs/ToothBall");
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
            else if (projectile.type == ProjectileType<WaterElementalMinion>())
            {
                TextureAssets.Projectile[projectile.type] = Request<Texture2D>("CalRemix/Retheme/Levi/Anahita");
            }
            else if (projectile.type == ProjectileType<GastricBelcher>())
            {
                TextureAssets.Projectile[projectile.type] = Request<Texture2D>("CalRemix/Retheme/Levi/Gastric");
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
        public static Color? RethemeProjAlpha(Projectile projectile)
        {
            if ((!Main.dayTime || BossRushEvent.BossRushActive) && (projectile.type == ProjectileType<HolyBlast>() || projectile.type == ProjectileType<HolyBomb>() || projectile.type == ProjectileType<HolyFire>() || projectile.type == ProjectileType<HolyFire2>() || projectile.type == ProjectileType<HolyFlare>() || projectile.type == ProjectileType<MoltenBlob>() || projectile.type == ProjectileType<MoltenBlast>()))
                return Color.MediumPurple;
            return null;
        }
    }
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
            //IL.CalamityMod.NPCs.PlaguebringerGoliath.PlaguebringerGoliath.PreDraw += PBG;
            //IL.CalamityMod.NPCs.CalamityAI.AstrumAureusAI += AureusAI;
            IL.CalamityMod.NPCs.AstrumDeus.AstrumDeusHead.PreDraw += AstrumDeusHead;
            IL.CalamityMod.NPCs.AstrumDeus.AstrumDeusBody.PreDraw += AstrumDeusBody;
            IL.CalamityMod.NPCs.AstrumDeus.AstrumDeusTail.PreDraw += AstrumDeusTail;
            IL.CalamityMod.NPCs.Yharon.Yharon.PreDraw += Yharon;
            IL.CalamityMod.NPCs.Other.THELORDE.PreDraw += LORDE;
            MonoModHooks.Modify(typeof(Providence).GetMethod("<PreDraw>g__drawProvidenceInstance|46_0", BindingFlags.NonPublic | BindingFlags.Instance), Providence);
            //MonoModHooks.Modify(typeof(CalamityMod.CalamityMod).Assembly.GetType("WeakReferenceSupport").GetMethod("AddCalamityBosses", BindingFlags.NonPublic | BindingFlags.Instance), BossChecklist);

            // IL.CalamityMod.Items.Weapons.PreDraw += ;
            IL.CalamityMod.Items.Weapons.Ranged.HeavenlyGale.PostDrawInWorld += HeavenlyGale;

            // IL.CalamityMod.Projectiles.PreDraw += ;
            IL.CalamityMod.Projectiles.Rogue.InfestedClawmerangProj.PreDraw += InfestedClawmerangProj;
            IL.CalamityMod.Projectiles.Magic.EldritchTentacle.AI += EldritchTentacle;
            IL.CalamityMod.Projectiles.Melee.MurasamaSlash.PreDraw += MurasamaSlash;
            IL.CalamityMod.Projectiles.Melee.ExobladeProj.DrawBlade += ExobladeProj;
            IL.CalamityMod.Projectiles.Ranged.HeavenlyGaleProj.PreDraw += HeavenlyGaleProj;
            IL.CalamityMod.Projectiles.Rogue.CelestusProj.PostDraw += CelestusProj;
            IL.CalamityMod.Projectiles.Melee.ViolenceThrownProjectile.PreDraw += ViolenceThrownProjectile;
            IL.CalamityMod.Projectiles.Boss.HolyBlast.PreDraw += HolyBlast;
        }
        #region BossChecklist
        private void BossChecklist(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/DesertScourge/DesertScourge_BossChecklist")))
                Retheme(c, "CalRemix/Retheme/DS/BC");
        }
        #endregion
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
        private void HiveMind(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/HiveMind/HiveMindP2")))
                Retheme(c, "CalRemix/Retheme/HiveMind/HiveMindP2");
        }
        private void PerforatorCyst(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Perforator/PerforatorCystGlow")))
                Retheme(c, "CalRemix/Retheme/Perfs/CystGlow");
        }
        #region PerfWormHeck
        private void PerforatorHive(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Perforator/PerforatorHiveGlow")))
                Retheme(c, "CalRemix/Retheme/Perfs/HiveGlow");
        }
        #endregion
        private void PerfLBody(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Perforator/PerforatorBodyLargeAlt")))
                Retheme(c, "CalRemix/Retheme/Perfs/LBodyAlt");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Perforator/PerforatorBodyLargeGlow")))
                Retheme(c, "CalRemix/Retheme/Perfs/LBodyGlow");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Perforator/PerforatorBodyLargeAltGlow")))
                Retheme(c, "CalRemix/Retheme/Perfs/LBodyAltGlow");
        }
        private void PerfMBody(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Perforator/PerforatorBodyMediumGlow")))
                Retheme(c, "CalRemix/Retheme/Perfs/MBodyGlow");
        }
        private void PerfSBody(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Perforator/PerforatorBodySmallGlow")))
                Retheme(c, "CalRemix/Retheme/Perfs/SBodyGlow");
        }
        private void PerfLHead(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Perforator/PerforatorHeadLargeGlow")))
                Retheme(c, "CalRemix/Retheme/Perfs/LHeadGlow");
        }
        private void PerfMHead(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Perforator/PerforatorHeadMediumGlow")))
                Retheme(c, "CalRemix/Retheme/Perfs/MHeadGlow");
        }
        private void PerfSHead(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Perforator/PerforatorHeadSmallGlow")))
                Retheme(c, "CalRemix/Retheme/Perfs/SHeadGlow");
        }
        private void PerfLTail(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Perforator/PerforatorTailLargeGlow")))
                Retheme(c, "CalRemix/Retheme/Perfs/LTailGlow");
        }
        private void PerfMTail(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Perforator/PerforatorTailMediumGlow")))
                Retheme(c, "CalRemix/Retheme/Perfs/MTailGlow");
        }
        private void PerfSTail(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Perforator/PerforatorTailSmallGlow")))
                Retheme(c, "CalRemix/Retheme/Perfs/STailGlow");
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
        private void Leviathan(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Leviathan/LeviathanAttack")))
                Retheme(c, "CalRemix/Retheme/Levi/Levi2");
        }
        private void Anahita(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Leviathan/AnahitaStabbing")))
                Retheme(c, "CalRemix/Retheme/Levi/AnahitaStab");
        }/*
        private void PBG(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/PlaguebringerGoliath/PlaguebringerGoliathGlow")))
                Retheme(c, "CalRemix/Retheme/Plague/PBGGlow");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/PlaguebringerGoliath/PlaguebringerGoliathChargeTex")))
                Retheme(c, "CalRemix/Retheme/Plague/PBG");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/PlaguebringerGoliath/PlaguebringerGoliathChargeTexGlow")))
                Retheme(c, "CalRemix/Retheme/Plague/PBGGlow");
        }*/
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
            /*
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
            }*/
        }
        private void Yharon(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Yharon/YharonGlowGreen")))
                Retheme(c, "CalRemix/Retheme/Yharon/YharonGlowGreen");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Yharon/YharonGlowPurple")))
                Retheme(c, "CalRemix/Retheme/Yharon/YharonGlowPurple");
        }
        private void LORDE(ILContext il)
        {
            var c = new ILCursor(il);
            var t = typeof(ModNPC).GetMethod("get_Texture", BindingFlags.Public | BindingFlags.Instance);
            if (c.TryGotoNext(i => i.MatchCallvirt(t)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => Main.zenithWorld ? "CalRemix/Retheme/LORDE/VotT" : "CalamityMod/NPCs/Other/THELORDE");
            }
        }
        #endregion
        #region Items
        private void HeavenlyGale(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/Items/Weapons/Ranged/HeavenlyGaleGlow")))
                Retheme(c, "CalRemix/Retheme/Blank");
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
        }
        private void ViolenceThrownProjectile(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchCallvirt(typeof(ModProjectile).GetMethod("get_Texture", BindingFlags.Public | BindingFlags.Instance))))
                Retheme(c, "CalRemix/Retheme/Violence");
        }
        private void HolyBlast(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchCallvirt(typeof(ModProjectile).GetMethod("get_Texture", BindingFlags.Public | BindingFlags.Instance))))
                Retheme(c, "CalamityMod/Projectiles/Boss/HolyBlastNight");
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
