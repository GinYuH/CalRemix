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
using CalamityMod.NPCs.SupremeCalamitas;
using Terraria.ID;
using System.Collections.Generic;
using CalamityMod;
using CalamityMod.Items.Placeables.Furniture;
using CalamityMod.NPCs.AstrumAureus;
using CalamityMod.Items.Mounts;
using CalamityMod.NPCs.ExoMechs.Ares;
using CalamityMod.Items.Armor;
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
using CalamityMod.Projectiles.Rogue;
using CalamityMod.Items.LoreItems;
using System;
using CalamityMod.Items.Accessories;
using Terraria.Localization;

namespace CalRemix.Core.Retheme
{
    public class RethemeMaster : ModSystem
    {
        internal static Dictionary<int, Asset<Texture2D>> NPCs = [];
        internal static Dictionary<int, Asset<Texture2D>> Items = [];
        internal static Dictionary<int, Asset<Texture2D>> Projs = [];
        internal static Dictionary<int, Asset<Texture2D>> Buffs = [];

        public override void Load()
        {
            SneakersRetheme.Load();
        }
        public override void Unload() => UnloadAll();
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
            foreach (KeyValuePair<int, string> p in RethemeList.Buffs)
            {
                Buffs.Add(p.Key, TextureAssets.Buff[p.Key]);
            }
            SneakersRetheme.SaveDefaultSneakersTextures();
        }
        private static void UnloadAll()
        {
            RethemeItem.ResetAnimations(true);
            RethemeItem.ChangeTextures(true);
            SneakersRetheme.ApplyAnimationsChanges(true);
            SneakersRetheme.ApplyTextureChanges(true);
        }
    }
    public class RethemeNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public static void ChangeTextures()
        {
            if (CalRemixWorld.npcChanges)
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
            if (RethemeList.NPCNames.Contains(npc.type))
                typeName = CalRemixHelper.LocalText($"Rename.NPCs.{npc.ModNPC.Name}").Value;
            else if (npc.type == NPCType<SupremeCalamitas>())
            {
                typeName = CalRemixHelper.LocalText($"Rename.NPCs.{npc.ModNPC.Name}").Value;
            }
            else if (typeName.Contains("Skeletron"))
            {
                typeName = typeName.Replace("Skeletron", CalRemixHelper.LocalText("Rename.NPCs.Skeletron").Value);
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
            else if (npc.type == NPCType<DesertScourgeHead>() || npc.type == NPCType<DesertScourgeBody>())
            {
                Texture2D head = Request<Texture2D>("CalRemix/Core/Retheme/DS/Body", AssetRequestMode.AsyncLoad).Value;

                if (npc.type == NPCType<DesertScourgeHead>())
                    head = Request<Texture2D>("CalRemix/Core/Retheme/DS/Head", AssetRequestMode.AsyncLoad).Value;
                else
                {
                    if (npc.ai[3] == 1)
                    {
                        head = Request<Texture2D>("CalRemix/Core/Retheme/DS/Body2", AssetRequestMode.AsyncLoad).Value;
                    }
                    else if (npc.ai[3] == 2)
                    {
                        head = Request<Texture2D>("CalRemix/Core/Retheme/DS/Body3", AssetRequestMode.AsyncLoad).Value;
                    }
                    else if (npc.ai[3] == 3)
                    {
                        head = Request<Texture2D>("CalRemix/Core/Retheme/DS/Body4", AssetRequestMode.AsyncLoad).Value;
                    }
                }

                Vector2 drawCenter = npc.Center - screenPos;
                float drawRotation = npc.rotation;

                spriteBatch.Draw(head, drawCenter, null, drawColor, drawRotation, head.Size() / 2f, npc.scale, SpriteEffects.None, 0f);

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
        public static void UpdateChanges()
        {
            ResetAnimations();
            ResetNames();
        }
        public static void ResetNames()
        {
            if (Main.LocalPlayer != null)
            {
                for (int i = 0; i < Main.LocalPlayer.inventory.Length; i++)
                {
                    Item item = Main.LocalPlayer.inventory[i];
                    if (RethemeList.ItemNames.Contains(item.type))
                    {
                        if (!CalRemixWorld.itemChanges)
                            NormalChanges(item);
                        else
                            item.ClearNameOverride();
                    }
                }
            }
        }
        public static void ResetAnimations(bool unloading = false)
        {
            if (CalRemixWorld.itemChanges && !unloading)
            {
                Main.RegisterItemAnimation(ItemType<WulfrumMetalScrap>(), new DrawAnimationVertical(6, 16));
            }
            else
            {
                if (!unloading)
                {
                    Main.RegisterItemAnimation(ItemType<WulfrumMetalScrap>(), new DrawAnimationVertical(1, 1));
                }
            }
        }
        public static void ChangeTextures(bool unloading = false)
        {
            if (CalRemixWorld.itemChanges && !unloading)
            {
                foreach (KeyValuePair<int, string> p in RethemeList.Items)
                {
                    //onl retexture items that aren't changed by sneakerhead mode already
                    if (!CalRemixWorld.sneakerheadMode || !SneakersRetheme.IsASneaker(p.Key))
                        TextureAssets.Item[p.Key] = Request<Texture2D>("CalRemix/Core/Retheme/" + p.Value);
                }
                foreach (KeyValuePair<int, string> p in RethemeList.Projs)
                {
                    TextureAssets.Projectile[p.Key] = Request<Texture2D>("CalRemix/Core/Retheme/" + p.Value);
                }
                foreach (KeyValuePair<int, string> p in RethemeList.Buffs)
                {
                    TextureAssets.Buff[p.Key] = Request<Texture2D>("CalRemix/Core/Retheme/" + p.Value);
                }
            }
            else
            {
                foreach (KeyValuePair<int, Asset<Texture2D>> p in RethemeMaster.Items)
                {
                    if (!CalRemixWorld.sneakerheadMode || !SneakersRetheme.IsASneaker(p.Key))
                        TextureAssets.Item[p.Key] = p.Value;
                }
                foreach (KeyValuePair<int, Asset<Texture2D>> p in RethemeMaster.Projs)
                {
                    TextureAssets.Projectile[p.Key] = p.Value;
                }
                foreach (KeyValuePair<int, Asset<Texture2D>> p in RethemeMaster.Buffs)
                {
                    TextureAssets.Buff[p.Key] = p.Value;
                }
            }
        }
        public override void SetStaticDefaults()
        {
            TextureAssets.Item[ItemID.ReaverShark] = Request<Texture2D>("CalRemix/Core/Retheme/ReaverShark");
        }
        public override void SetDefaults(Item item)
        {
            AbsoluteChanges(item);
            if (CalRemixWorld.itemChanges)
                NormalChanges(item);
            if (CalRemixWorld.sneakerheadMode && SneakersRetheme.IsASneaker(item.type) && SneakersRetheme.itemSneakerPairs.ContainsKey(item.type))
                SneakersRetheme.InitializeItem(item);
        }
        public static void NormalChanges(Item item)
        {
            if (RethemeList.ItemNames.Contains(item.type))
                item.SetNameOverride(CalRemixHelper.LocalText($"Rename.Items.Normal.{item.ModItem.Name}").Value);
        }
        public static void AbsoluteChanges(Item item)
        {
            string relic = "Rename.Items.Absolute.Relic";
            if (item.type == ItemType<LoreExoMechs>())
            {
                item.SetNameOverride(CalRemixHelper.LocalText($"Rename.Items.Absolute.{item.ModItem.Name}").Value);
            }
            else if (item.type == ItemType<OldDukeScales>())
            {
                item.SetNameOverride(CalRemixHelper.LocalText($"Rename.Items.Absolute.{item.ModItem.Name}").Value);
            }
            else if (item.type == ItemID.ReaverShark)
            {
                item.SetNameOverride(CalRemixHelper.LocalText("Rename.Items.Absolute.ReaverShark").Value);
                item.pick = 10;
            }
            else if (item.Name.Contains(CalRemixHelper.LocalText(relic).Value) && item.rare == ItemRarityID.Master)
            {
                item.SetNameOverride(item.Name.Replace(CalRemixHelper.LocalText(relic).Value, CalRemixHelper.LocalText("Rename.Items.Absolute.Treasure").Value));
            }
            else if (item.Name.Contains(Language.GetOrRegister("Mods.CalamityMod.Items.Materials.Bloodstone.DisplayName").Value))
            {
                item.SetNameOverride(item.Name.Replace(Language.GetOrRegister("Mods.CalamityMod.Items.Materials.Bloodstone.DisplayName").Value, CalRemixHelper.LocalText("Rename.Items.Absolute.Hemostone").Value));
            }
            else if (item.Name.Contains("Skeletron"))
            {
                item.SetNameOverride(item.Name.Replace(Language.GetOrRegister("Mods.CalamityMod.Items.Materials.Bloodstone.DisplayName").Value, CalRemixHelper.LocalText("Rename.Items.Absolute.Hemostone").Value));
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
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (SneakersRetheme.IsASneaker(item.type))
                SneakersRetheme.ModifyTooltips(item, tooltips);
        }

        public override bool PreDrawTooltipLine(Item item, DrawableTooltipLine line, ref int yOffset)
        {
            if (SneakersRetheme.IsASneaker(item.type))
                return SneakersRetheme.PreDrawTooltipLine(item, line);

            return true;
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
