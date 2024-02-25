using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CalamityMod.Events;
using Terraria.GameContent;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Melee;
using ReLogic.Content;
using CalamityMod.NPCs.ProfanedGuardians;
using CalamityMod.Projectiles.Boss;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items.SummonItems;
using CalamityMod.Items.Accessories;
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
using CalamityMod.NPCs.AstrumAureus;
using CalamityMod.Items.Armor.Fearmonger;
using CalamityMod.Items.Mounts;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.NPCs.ExoMechs.Ares;
using CalamityMod.NPCs.NormalNPCs;
using CalamityMod.Items.Armor;
using CalRemix.Retheme.NoFab;
using Terraria.DataStructures;
using static Terraria.ModLoader.ModContent;
using CalamityMod.Projectiles.Melee.Spears;
using Terraria.Localization;
using CalamityMod.Rarities;

namespace CalRemix.Retheme
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
                EquipLoader.AddEquipTexture(Mod, "CalRemix/Retheme/NoFab/AshsCloak_Body", EquipType.Body, name: "AshsCloakBody");
                EquipLoader.AddEquipTexture(Mod, "CalRemix/Retheme/NoFab/AshsCloak_Legs", EquipType.Legs, name: "AshsCloakLegs");
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
        }
    }
    public class RNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
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
                Texture2D mask = Request<Texture2D>("CalRemix/Retheme/Guardians/DreamMask" + num, AssetRequestMode.ImmediateLoad).Value;
                Vector2 draw = npc.Center - screenPos + new Vector2(0f, npc.gfxOffY);
                SpriteEffects spriteEffects = (npc.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                spriteBatch.Draw(mask, draw, null, drawColor, npc.rotation, mask.Size() / 2f, npc.scale, spriteEffects, 0f);
            }
        }
    }
    public class RItem : GlobalItem
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
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(ItemType<Fabstaff>(), new DrawAnimationVertical(6, 6));
            TextureAssets.Item[ItemType<Fabstaff>()] = Request<Texture2D>("CalRemix/Retheme/NoFab/InterfacerStaff");
            TextureAssets.Item[ItemType<Fabsol>()] = Request<Texture2D>("CalRemix/Retheme/NoFab/DiscordianSigil");
            TextureAssets.Item[ItemType<CirrusDress>()] = Request<Texture2D>("CalRemix/Retheme/NoFab/AshsCloak");
        }
        public override void SetDefaults(Item item)
        {
            if (item.type == ItemType<Fabstaff>())
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
            else if (item.type == ItemType<CosmiliteBar>())
                item.rare = CalRemixWorld.cosmislag ? ItemRarityID.Purple : RarityType<DarkBlue>();
            else if (RethemeMaster.OriginalItemNames.ContainsKey(item.type))
            {
                string name = CalRemixWorld.itemChanges ? RethemeList.ItemNames.GetValueOrDefault(item.type) : RethemeMaster.OriginalItemNames.GetValueOrDefault(item.type);
                item.SetNameOverride(name);
            }
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
            Mod mod = Mod;
            if (CalRemixWorld.aspids)
            {
                if (item.type == ItemType<CryoKey>())
                {
                    var line = new TooltipLine(mod, "CryoKeyRemix", "Drops from Primal Aspids");
                    tooltips.Add(line);
                }
            }
            if (CalRemixWorld.clamitas)
            {
                if (item.type == ItemType<EyeofDesolation>())
                {
                    var line = new TooltipLine(mod, "EyeofDesolationRemix", "Drops from Clamitas");
                    tooltips.Add(line);
                }
            }
            if (CalRemixWorld.plaguetoggle)
            {
                if (item.type == ItemType<Abombination>())
                {
                    tooltips.FindAndReplace("the Jungle", "the Plagued Jungle");
                    tooltips.FindAndReplace("the Jungle", "the Plagued Jungle [c/C61B40:(yes, she enrages in the normal Jungle)]");
                }
            }
            if (CalRemixWorld.fearmonger)
            {
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
            }
            if (Torch.Contains(item.type))
            {
                var line = new TooltipLine(mod, "TorchRemix", "Can be used as ammo for the Driftorcher");
                line.OverrideColor = Color.OrangeRed;
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
        }
    }
    public class RProj : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public override void SetStaticDefaults()
        {
            if (!CalRemixWorld.npcChanges)
                return;
        }
        public override void SetDefaults(Projectile projectile)
        {
            if (!CalRemixWorld.npcChanges)
                return;
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
                Main.EntitySpriteDraw(Request<Texture2D>("CalRemix/Retheme/Perfs/SausageMakerSpear").Value, projectile.Center - Main.screenPosition, origin: Vector2.Zero, sourceRectangle: null, color: projectile.GetAlpha(lightColor), rotation: projectile.rotation, scale: projectile.scale, effects: SpriteEffects.None);
                return false;
            }
            return true;
        }
    }
}
