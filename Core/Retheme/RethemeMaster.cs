using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CalamityMod.Events;
using Terraria.GameContent;
using CalamityMod.Items.Materials;
using ReLogic.Content;
using CalamityMod.NPCs.ProfanedGuardians;
using CalamityMod.Projectiles.Boss;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.NPCs.BrimstoneElemental;
using CalamityMod.NPCs.SupremeCalamitas;
using CalamityMod.NPCs.TownNPCs;
using Terraria.ID;
using System.Collections.Generic;
using CalamityMod;
using CalamityMod.Items.Placeables.Furniture;
using CalamityMod.NPCs.ExoMechs;
using CalamityMod.NPCs.AstrumAureus;
using CalamityMod.Items.Mounts;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.NPCs.ExoMechs.Ares;
using CalamityMod.NPCs.NormalNPCs;
using CalamityMod.Items.Armor;
using CalRemix.Core.Retheme.NoFab;
using Terraria.DataStructures;
using static Terraria.ModLoader.ModContent;
using CalamityMod.Projectiles.Melee.Spears;
using CalamityMod.Items.Accessories.Vanity;
using CalRemix.Core.World;
using CalamityMod.NPCs.DesertScourge;
using CalamityMod.NPCs.Crabulon;
using CalamityMod.NPCs.HiveMind;
using CalamityMod.NPCs.Perforator;
using CalamityMod.NPCs.Cryogen;
using CalamityMod.NPCs.Leviathan;
using CalamityMod.NPCs.CalClone;
using CalamityMod.NPCs.Providence;
using CalamityMod.NPCs.Yharon;
using CalamityMod.Systems;
using CalamityMod.Projectiles.Rogue;
using Newtonsoft.Json.Linq;
using System;

namespace CalRemix.Core.Retheme
{
    public class RethemeMaster : ModSystem
    {
        internal static Dictionary<int, Asset<Texture2D>> NPCs = new();
        internal static Dictionary<int, Asset<Texture2D>> Items = new();
        internal static Dictionary<int, Asset<Texture2D>> Projs = new();
        internal static Dictionary<int, string> OriginalItemNames = new();
        public override void Load()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                EquipLoader.AddEquipTexture(Mod, "CalRemix/Core/Retheme/NoFab/AshsCloak_Body", EquipType.Body, name: "AshsCloakBody");
                EquipLoader.AddEquipTexture(Mod, "CalRemix/Core/Retheme/NoFab/AshsCloak_Legs", EquipType.Legs, name: "AshsCloakLegs");
            }
        }
        public override void PostSetupContent()
        {
            foreach (KeyValuePair<int, string> p in RethemeList.NPCs)
            {
                NPCs.Add(p.Key, TextureAssets.Npc[p.Key]);
            }
            foreach (KeyValuePair<int, string> p in RethemeList.Items)
            {
                Items.Add(p.Key, TextureAssets.Item[p.Key]);
            }
            foreach (KeyValuePair<int, string> p in RethemeList.Projs)
            {
                Projs.Add(p.Key, TextureAssets.Projectile[p.Key]);
            }
            foreach (KeyValuePair<int, string> p in RethemeList.ItemNames)
            {
                OriginalItemNames.Add(p.Key, Lang.GetItemName(p.Key).Value);
            }
            On.CalamityMod.Systems.YharonBackgroundScene.IsSceneEffectActive += NoYharonScene;
        }
        private static bool NoYharonScene(On.CalamityMod.Systems.YharonBackgroundScene.orig_IsSceneEffectActive orig, YharonBackgroundScene self, object player)
        {
            if (CalRemixWorld.npcChanges)
                return false;
            return orig(self, player);
        }
    }
    public class RethemeNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public static void ChangeTextures()
        {
            if (!CalRemixWorld.npcChanges)
            {
                foreach (KeyValuePair<int, string> p in RethemeList.NPCs)
                {
                    TextureAssets.Npc[p.Key] = Request<Texture2D>("CalRemix/Core/Retheme/" + p.Value);
                }
            }
            else
            {
                foreach (KeyValuePair<int, Asset<Texture2D>> p in RethemeMaster.NPCs)
                {
                    TextureAssets.Npc[p.Key] = p.Value;
                }
            }
        }
        public static void UpdateTextures()
        {
            DesertScourgeBody.BodyTexture2 = NPCTextureChange("DS/Body2", "DesertScourge/DesertScourgeBody2");
            DesertScourgeBody.BodyTexture3 = NPCTextureChange("DS/Body2", "DesertScourge/DesertScourgeBody3");
            DesertScourgeBody.BodyTexture4 = NPCTextureChange("DS/Body2", "DesertScourge/DesertScourgeBody4");
            Crabulon.Texture_Glow = NPCTextureChange("Crabulon/CrabulonGlow", "Crabulon/CrabulonGlow");
            Crabulon.AltTexture_Glow = NPCTextureChange("Crabulon/CrabulonAltGlow", "Crabulon/CrabulonAltGlow");
            Crabulon.AttackTexture_Glow = NPCTextureChange("Crabulon/CrabulonAttackGlow", "Crabulon/CrabulonAttackGlow");
            HiveMind.Phase2Texture = NPCTextureChange("HiveMind/HiveMindP2", "HiveMind/HiveMindP2");
            #region Perforators
            PerforatorCyst.GlowTexture = NPCTextureChange("Perfs/CystGlow", "Perforator/PerforatorCystGlow");
            PerforatorHive.GlowTexture = NPCTextureChange("Perfs/HiveGlow", "Perforator/PerforatorHiveGlow");
            PerforatorBodyLarge.AltTexture = NPCTextureChange("Perfs/LBodyAlt", "Perforator/PerforatorBodyLargeAlt");
            PerforatorBodyLarge.AltTexture_Glow = NPCTextureChange("Perfs/LBodyAltGlow", "Perforator/PerforatorBodyLargeAltGlow");
            PerforatorBodyLarge.Texture_Glow = NPCTextureChange("Perfs/LBodyGlow", "Perforator/PerforatorBodyLargeGlow");
            PerforatorBodyMedium.GlowTexture = NPCTextureChange("Perfs/MBodyGlow", "Perforator/PerforatorBodyMediumGlow");
            PerforatorBodySmall.GlowTexture = NPCTextureChange("Perfs/SBodyGlow", "Perforator/PerforatorBodySmallGlow");
            PerforatorHeadLarge.GlowTexture = NPCTextureChange("Perfs/LHeadGlow", "Perforator/PerforatorHeadLargeGlow");
            PerforatorHeadMedium.GlowTexture = NPCTextureChange("Perfs/MHeadGlow", "Perforator/PerforatorHeadMediumGlow");
            PerforatorHeadSmall.GlowTexture = NPCTextureChange("Perfs/SHeadGlow", "Perforator/PerforatorHeadSmallGlow");
            PerforatorTailLarge.GlowTexture = NPCTextureChange("Perfs/LTailGlow", "Perforator/PerforatorTailLargeGlow");
            PerforatorTailMedium.GlowTexture = NPCTextureChange("Perfs/MTailGlow", "Perforator/PerforatorTailMediumGlow");
            PerforatorTailSmall.GlowTexture = NPCTextureChange("Perfs/STailGlow", "Perforator/PerforatorTailSmallGlow");
            #endregion

            Cryogen.Phase2Texture = NPCTextureChange("Cryogen/CryogenPhase2", "Cryogen/Cryogen_Phase2");
            Cryogen.Phase3Texture = NPCTextureChange("Cryogen/CryogenPhase3", "Cryogen/Cryogen_Phase3");
            Cryogen.Phase4Texture = NPCTextureChange("Cryogen/CryogenPhase4", "Cryogen/Cryogen_Phase4");
            Cryogen.Phase5Texture = NPCTextureChange("Cryogen/CryogenPhase5", "Cryogen/Cryogen_Phase5");
            Cryogen.Phase6Texture = NPCTextureChange("Cryogen/CryogenPhase6", "Cryogen/Cryogen_Phase6");
            CalamitasClone.GlowTexture = NPCTextureChange("Cal/CalamitasGlow", "CalClone/CalamitasCloneGlow");
            Anahita.ChargeTexture = NPCTextureChange("Levi/AnahitaStab", "Leviathan/AnahitaStabbing");
            Leviathan.AttackTexture = NPCTextureChange("Levi/LeviAttack", "Leviathan/LeviathanAttack");

            AstrumAureus.JumpTexture = NPCTextureChange("Plague/AstrumAureusJump", "AstrumAureus/AstrumAureusJump");
            AstrumAureus.RechargeTexture = NPCTextureChange("Plague/AstrumAureusRecharge", "AstrumAureus/AstrumAureusRecharge");
            AstrumAureus.StompTexture = NPCTextureChange("Plague/AstrumAureusStomp", "AstrumAureus/AstrumAureusStomp");
            AstrumAureus.WalkTexture = NPCTextureChange("Plague/AstrumAureusWalk", "AstrumAureus/AstrumAureusWalk");
            AstrumAureus.Texture_Glow = NPCTextureChange("Plague/AstrumAureusGlow", "AstrumAureus/AstrumAureusGlow");
            AstrumAureus.JumpTexture_Glow = NPCTextureChange("Plague/AstrumAureusJumpGlow", "AstrumAureus/AstrumAureusJumpGlow");
            AstrumAureus.StompTexture_Glow = NPCTextureChange("Plague/AstrumAureusStompGlow", "AstrumAureus/AstrumAureusStompGlow");
            AstrumAureus.WalkTexture_Glow = NPCTextureChange("Plague/AstrumAureusWalkGlow", "AstrumAureus/AstrumAureusWalkGlow");

            #region Providence
            ProvTC(ref Providence.TextureAlt, "TextureAlt");
            ProvTC(ref Providence.TextureAltNight, "TextureAltNight");
            ProvTC(ref Providence.TextureAltNight_Glow, "TextureAltNight_Glow");
            ProvTC(ref Providence.TextureAltNight_Glow_2, "TextureAltNight_Glow_2");
            ProvTC(ref Providence.TextureAlt_Glow, "TextureAlt_Glow");
            ProvTC(ref Providence.TextureAlt_Glow_2, "TextureAlt_Glow_2");
            ProvTC(ref Providence.TextureAttack, "TextureAttack");
            ProvTC(ref Providence.TextureAttackAlt, "TextureAttackAlt");
            ProvTC(ref Providence.TextureAttackAltNight, "TextureAttackAltNight");
            ProvTC(ref Providence.TextureAttackAltNight_Glow, "TextureAttackAltNight_Glow");
            ProvTC(ref Providence.TextureAttackAltNight_Glow_2, "TextureAttackAltNight_Glow_2");
            ProvTC(ref Providence.TextureAttackAlt_Glow, "TextureAttackAlt_Glow");
            ProvTC(ref Providence.TextureAttackAlt_Glow_2, "TextureAttackAlt_Glow_2");
            ProvTC(ref Providence.TextureAttackNight, "TextureAttackNight");
            ProvTC(ref Providence.TextureAttackNight_Glow, "TextureAttackNight_Glow");
            ProvTC(ref Providence.TextureAttackNight_Glow_2, "TextureAttackNight_Glow_2");
            ProvTC(ref Providence.TextureAttack_Glow, "TextureAttack_Glow");
            ProvTC(ref Providence.TextureAttack_Glow_2, "TextureAttack_Glow_2");
            ProvTC(ref Providence.TextureDefense, "TextureDefense");
            ProvTC(ref Providence.TextureDefenseAlt, "TextureDefenseAlt");
            ProvTC(ref Providence.TextureDefenseAltNight, "TextureDefenseAltNight");
            ProvTC(ref Providence.TextureDefenseAltNight_Glow, "TextureDefenseAltNight_Glow");
            ProvTC(ref Providence.TextureDefenseAltNight_Glow_2, "TextureDefenseAltNight_Glow_2");
            ProvTC(ref Providence.TextureDefenseAlt_Glow, "TextureDefenseAlt_Glow");
            ProvTC(ref Providence.TextureDefenseAlt_Glow_2, "TextureDefenseAlt_Glow_2");
            ProvTC(ref Providence.TextureDefenseNight, "TextureDefenseNight");
            ProvTC(ref Providence.TextureDefenseNight_Glow, "TextureDefenseNight_Glow");
            ProvTC(ref Providence.TextureDefenseNight_Glow_2, "TextureDefenseNight_Glow_2");
            ProvTC(ref Providence.TextureDefense_Glow, "TextureDefense_Glow");
            ProvTC(ref Providence.TextureDefense_Glow_2, "TextureDefense_Glow_2");
            ProvTC(ref Providence.TextureNight, "TextureNight");
            ProvTC(ref Providence.TextureNight_Glow, "TextureNight_Glow");
            ProvTC(ref Providence.TextureNight_Glow_2, "TextureNight_Glow_2");
            ProvTC(ref Providence.Texture_Glow, "Texture_Glow");
            ProvTC(ref Providence.Texture_Glow_2, "Texture_Glow_2");
            #endregion
            Yharon.GlowTexturePurple = NPCTextureChange("Yharon/YharonGlowPurple", "Yharon/YharonGlowPurple");
            Yharon.GlowTextureGreen = NPCTextureChange("Yharon/YharonGlowGreen", "Yharon/YharonGlowGreen");

        }
        private static Asset<Texture2D> NPCTextureChange(string remix, string original)
        {
            return CalRemixWorld.npcChanges ? Request<Texture2D>("CalRemix/Core/Retheme/" + remix) : Request<Texture2D>("CalamityMod/NPCs/" + original);
        }
        private static void PerfTC(ref Asset<Texture2D> asset, string name)
        {
            string newName = name.Replace("_", "");
            newName = newName.Replace("Texture", "Providence");
            asset = NPCTextureChange($"Providence/{newName}", $"Providence/{newName}");
        }
        private static void ProvTC(ref Asset<Texture2D> asset, string name)
        {
            string newName = name.Replace("_", "");
            newName = newName.Replace("Texture", "Providence");
            if (newName.Contains("Glow"))
                newName = "Glowmasks/" + newName;
            if (newName.Contains("Night"))
                newName = newName.Replace("Night", "") + "Night";
            asset = NPCTextureChange($"Providence/{newName}", $"Providence/{newName}");
        }
        public override void ModifyTypeName(NPC npc, ref string typeName)
        {
            if (!CalRemixWorld.npcChanges)
                return;
            if (npc.type == NPCType<BrimstoneElemental>())
            {
                typeName = "Calamity Elemental";
            }
            else if (npc.type == NPCType<ThiccWaifu>())
            {
                typeName = "Aether Valkyrie";
            }
            else if (npc.type == NPCType<AstrumAureus>())
            {
                typeName = "Astrum Viridis";
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
                SupremeCalamitas cirrus = npc.ModNPC as SupremeCalamitas;
                if (!cirrus.cirrus)
                    typeName = "Brimdeath Witch, Calitas Jane";
            }
            else if (npc.type == NPCType<WITCH>())
            {
                typeName = "Calamity Witch";
            }
            else if (typeName.Contains("Skeletron"))
            {
                typeName = typeName.Replace("Skeletron", "Dungen");
            }
        }

        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (!CalRemixWorld.npcChanges)
                return true;

            if (npc.type == NPCID.SkeletronHead)
            {
                bool rushingSkeletron = (npc.ai[1] == 1f || npc.ai[1] == 2f);
                drawColor = Color.Lerp(drawColor, Color.White, 0.5f);

                    Texture2D hand;
                if (npc.life / (float)npc.lifeMax > 0.98f)
                    hand = Request<Texture2D>("CalRemix/Core/Retheme/Skeletron/DungenHand2", AssetRequestMode.AsyncLoad).Value;
                else
                    hand = Request<Texture2D>("CalRemix/Core/Retheme/Skeletron/DungenHand", AssetRequestMode.AsyncLoad).Value;

                Vector2 drawCenter = npc.Center - screenPos;
                float rotationBase = Main.GlobalTimeWrappedHourly * 3f;

                for (int i = 0; i < 8; i++)
                {
                    SpriteEffects spriteEffects = (i % 2 == 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                    float rotation = i / 8f * MathHelper.TwoPi + rotationBase;
                    Vector2 drawPos = drawCenter + rotation.ToRotationVector2() * (rushingSkeletron ? 30f : 80f);

                    spriteBatch.Draw(hand, drawPos, null, drawColor, rotation - MathHelper.PiOver2, new Vector2(hand.Width / 2, 20), npc.scale, spriteEffects, 0f);
                }

                Texture2D face;
                if (npc.life / (float)npc.lifeMax > 0.98f)
                    face = Request<Texture2D>("CalRemix/Core/Retheme/Skeletron/Dungen3", AssetRequestMode.AsyncLoad).Value;
                else if (rushingSkeletron)
                    face = Request<Texture2D>("CalRemix/Core/Retheme/Skeletron/Dungen2", AssetRequestMode.AsyncLoad).Value;
                else
                    face = Request<Texture2D>("CalRemix/Core/Retheme/Skeletron/Dungen", AssetRequestMode.AsyncLoad).Value;

                float drawRotation = npc.rotation;
                if (rushingSkeletron)
                    drawRotation = (float)Math.Sin(drawRotation * 1f) * 0.2f;

                spriteBatch.Draw(face, drawCenter, null, drawColor, drawRotation, face.Size() / 2f, npc.scale, SpriteEffects.None, 0f);

                return false;
            }

            return true;
        }

        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (!CalRemixWorld.npcChanges)
                return;
            if (npc.type == NPCType<ProfanedGuardianCommander>())
                MaskDraw(1, npc, spriteBatch, screenPos, drawColor);
            else if (npc.type == NPCType<ProfanedGuardianDefender>())
                MaskDraw(2, npc, spriteBatch, screenPos, drawColor);
            else if (npc.type == NPCType<ProfanedGuardianHealer>())
                MaskDraw(3, npc, spriteBatch, screenPos, drawColor);
        }
        private static void MaskDraw(int num, NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (npc.life > 0)
            {
                Texture2D mask = Request<Texture2D>("CalRemix/Core/Retheme/Guardians/DreamMask" + num).Value;
                Vector2 draw = npc.Center - screenPos + new Vector2(0f, npc.gfxOffY);
                SpriteEffects spriteEffects = (npc.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                spriteBatch.Draw(mask, draw, null, drawColor, npc.rotation, mask.Size() / 2f, npc.scale, spriteEffects, 0f);
            }
        }
    }
    public class RethemeItem : GlobalItem
    {
        public static void ChangeTextures()
        {
            if (!CalRemixWorld.itemChanges)
            {
                foreach (KeyValuePair<int, string> p in RethemeList.Items)
                {
                    TextureAssets.Item[p.Key] = Request<Texture2D>("CalRemix/Core/Retheme/" + p.Value);
                }
                foreach (KeyValuePair<int, string> p in RethemeList.Projs)
                {
                    TextureAssets.Projectile[p.Key] = Request<Texture2D>("CalRemix/Core/Retheme/" + p.Value);
                }
                Main.RegisterItemAnimation(ItemType<WulfrumMetalScrap>(), new DrawAnimationVertical(6, 16));
            }
            else
            {
                foreach (KeyValuePair<int, Asset<Texture2D>> p in RethemeMaster.Items)
                {
                    TextureAssets.Item[p.Key] = p.Value;
                }
                foreach (KeyValuePair<int, Asset<Texture2D>> p in RethemeMaster.Projs)
                {
                    TextureAssets.Projectile[p.Key] = p.Value;
                }
                Main.RegisterItemAnimation(ItemType<WulfrumMetalScrap>(), new DrawAnimationVertical(1, 1));
            }
        }
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(ItemType<Fabstaff>(), new DrawAnimationVertical(6, 6));
            TextureAssets.Item[ItemType<CirrusCouch>()] = Request<Texture2D>("CalRemix/Assets/ExtraTextures/Blank");
            TextureAssets.Item[ItemType<CrystalHeartVodka>()] = Request<Texture2D>("CalRemix/Assets/ExtraTextures/Blank");
            TextureAssets.Item[ItemType<Fabstaff>()] = Request<Texture2D>("CalRemix/Core/Retheme/NoFab/InterfacerStaff");
            TextureAssets.Item[ItemType<Fabsol>()] = Request<Texture2D>("CalRemix/Core/Retheme/NoFab/DiscordianSigil");
            TextureAssets.Item[ItemType<CirrusDress>()] = Request<Texture2D>("CalRemix/Core/Retheme/NoFab/AshsCloak");
        }
        public override void SetDefaults(Item item)
        {
            if (item.type == ItemType<CirrusCouch>() || item.type == ItemType<CrystalHeartVodka>())
            {
                item.SetNameOverride("N/A");
                item.createTile = -1;
            }
            else if (item.type == ItemType<Fabstaff>())
            {
                item.SetNameOverride("Interfacer Staff");
                item.UseSound = AresTeslaCannon.TeslaOrbShootSound with { Pitch = 0.5f, PitchVariance = 0.2f, Volume = 0.5f };
            }
            else if (item.type == ItemType<Fabsol>())
            {
                item.SetNameOverride("Discordian Sigil");
                item.mountType = MountType<HorseMount>();
                item.UseSound = SoundID.Item113;
            }
            else if (item.type == ItemType<CirrusDress>())
            {
                item.SetNameOverride("Ash's Cloak");
                item.rare = ItemRarityID.Pink;
                item.Calamity().devItem = false;
                item.defense -= 8;
                item.bodySlot = EquipLoader.GetEquipSlot(Mod, "AshsCloakBody", EquipType.Body);
            }
            else if (RethemeMaster.OriginalItemNames.ContainsKey(item.type))
            {
                string name = CalRemixWorld.itemChanges ? RethemeList.ItemNames.GetValueOrDefault(item.type) : RethemeMaster.OriginalItemNames.GetValueOrDefault(item.type);
                item.SetNameOverride(name);
            }
            else if (item.Name.Contains("Skeletron"))
            {
                item.SetNameOverride(item.Name.Replace("Skeletron", "Dungen"));
            }
        }
        public override void UpdateInventory(Item item, Player player)
        {
            if (item.type == ItemType<CirrusCouch>() || item.type == ItemType<CrystalHeartVodka>())
                item.stack = 0;
        }
        public override void PostUpdate(Item item)
        {
            if (item.type == ItemType<CirrusCouch>() || item.type == ItemType<CrystalHeartVodka>())
                item.stack = 0;
        }
        public override void UpdateEquip(Item item, Player player)
        {
            if (item.type == ItemType<CirrusDress>())
            {
                player.Calamity().cirrusDress = false;
                player.GetDamage<MagicDamageClass>() -= 0.05f;
                player.GetCritChance<MagicDamageClass>() -= 5f;
            }
        }
        public override void SetMatch(int armorSlot, int type, bool male, ref int equipSlot, ref bool robes)
        {
           if (equipSlot == EquipLoader.GetEquipSlot(Mod, "AshsCloakBody", EquipType.Body))
            {
                robes = true;
                equipSlot = EquipLoader.GetEquipSlot(Mod, "AshsCloakLegs", EquipType.Legs);
            }
        }
        public override bool CanUseItem(Item item, Player player)
        {
            if (item.type == ItemType<ClockGatlignum>() && CalRemixWorld.itemChanges)
            {
                return false;
            }
            return true;
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type == ItemType<CirrusCouch>() || item.type == ItemType<CrystalHeartVodka>())
                tooltips.Clear();
        }
    }
    public class RethemeProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public override void SetStaticDefaults()
        {
            if (!CalRemixWorld.npcChanges)
                return;
        }
        public override void SetDefaults(Projectile projectile)
        {
            if (CalRemixWorld.itemChanges)
            {
                if (projectile.type == ProjectileType<SupernovaBomb>())
                {
                    projectile.width = projectile.height = 54;
                }
            }
            if (CalRemixWorld.npcChanges)
            {
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
            }
        }
        public override Color? GetAlpha(Projectile projectile, Color lightColor)
        {
            if (!CalRemixWorld.npcChanges)
                return null;
            if ((!Main.dayTime || BossRushEvent.BossRushActive) && (projectile.type == ProjectileType<HolyBlast>() || projectile.type == ProjectileType<HolyBomb>() || projectile.type == ProjectileType<HolyFire>() || projectile.type == ProjectileType<HolyFire2>() || projectile.type == ProjectileType<HolyFlare>() || projectile.type == ProjectileType<MoltenBlob>() || projectile.type == ProjectileType<MoltenBlast>()))
                return Color.MediumPurple;
            return null;
        }
        public override bool PreDraw(Projectile projectile, ref Color lightColor)
        {
            if (!CalRemixWorld.itemChanges)
                return true;
            if (projectile.type == ProjectileType<SausageMakerSpear>())
            {
                Main.EntitySpriteDraw(Request<Texture2D>("CalRemix/Core/Retheme/Perfs/SausageMakerSpear").Value, projectile.Center - Main.screenPosition, origin: Vector2.Zero, sourceRectangle: null, color: projectile.GetAlpha(lightColor), rotation: projectile.rotation, scale: projectile.scale, effects: SpriteEffects.None);
                return false;
            }
            return true;
        }
    }
}
