using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CalamityMod.Events;
using Terraria.GameContent;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Melee;
using static Terraria.ModLoader.ModContent;
using ReLogic.Content;
using CalamityMod.NPCs.ProfanedGuardians;
using CalamityMod.Projectiles.Boss;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items.SummonItems;
using CalamityMod.Items.Accessories;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using System.Reflection;
using CalamityMod.NPCs.Providence;
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
using CalamityMod.NPCs.AstrumAureus;
using CalamityMod.NPCs;
using CalamityMod.Items.Armor.Fearmonger;
using CalamityMod.NPCs.Bumblebirb;
using Terraria.GameContent.Bestiary;
using Terraria.DataStructures;
using CalamityMod.Items.Mounts;
using CalamityMod.Items.Weapons.Ranged;
using System;
using CalamityMod.Items.Weapons.DraedonsArsenal;
using CalamityMod.NPCs.ExoMechs.Ares;

namespace CalRemix.Retheme
{
    public class RethemeMaster
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
        public class RNPC : GlobalNPC
        {
            public override bool InstancePerEntity => true;
            private int LORDEhead = -1;
            public override void Load()
            {
                LORDEhead = Mod.AddBossHeadTexture("CalRemix/Retheme/LORDE/VotTMap", -1);
            }
            public override void SetStaticDefaults()
            {
                if (!CalRemixWorld.resprites)
                    return;
                foreach (KeyValuePair<int, string> p in RethemeList.NPCs)
                {
                    TextureAssets.Npc[p.Key] = Request<Texture2D>("CalRemix/Retheme/" + p.Value);
                }
            }
            public override void BossHeadSlot(NPC npc, ref int index)
            {
                if (!CalRemixWorld.resprites)
                    return;
                int LORDEheadSlot = LORDEhead;
                if (npc.type == NPCType<THELORDE>() && LORDEhead != -1 && Main.zenithWorld)
                    index = LORDEheadSlot;
            }
            public override void SetDefaults(NPC npc)
            {
                if (!CalRemixWorld.resprites)
                    return;
                if (RethemeList.BossHeads.ContainsKey(npc.type))
                {
                    RethemeList.BossHeads.TryGetValue(npc.type, out string v);
                    TextureAssets.NpcHeadBoss[npc.GetBossHeadTextureIndex()] = Request<Texture2D>("CalRemix/Retheme/" + v);
                }
                if (npc.type == NPCType<THELORDE>() && Main.zenithWorld)
                {
                    npc.HitSound = SoundID.NPCHit49;
                }
            }
            public override void ModifyTypeName(NPC npc, ref string typeName)
            {
                if (!CalRemixWorld.renames)
                    return;
                if (npc.type == NPCType<BrimstoneElemental>())
                {
                    typeName = "Calamity Elemental";
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
            public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
            {
                if (!CalRemixWorld.resprites)
                    return;
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
            public override Color? GetAlpha(NPC npc, Color drawColor)
            {
                if (!Main.zenithWorld && npc.type == NPCType<Providence>())
                    return new Color(255, 255, 255, 255);
                return null;
            }
        }
        public class RItem : GlobalItem
        {
            public override void SetStaticDefaults()
            {
                if (!CalRemixWorld.resprites)
                    return;
                foreach (KeyValuePair<int, string> p in RethemeList.Items)
                {
                    TextureAssets.Item[p.Key] = Request<Texture2D>("CalRemix/Retheme/" + p.Value);
                }
                Main.RegisterItemAnimation(ItemType<WulfrumMetalScrap>(), new DrawAnimationVertical(6, 16));
                Main.RegisterItemAnimation(ItemType<Fabstaff>(), new DrawAnimationVertical(6, 6));
            }
            public override void SetDefaults(Item item)
            {
                if (item.type == ItemType<ClockGatlignum>())
                {
                    item.SetNameOverride("Clockwork Bar");
                    item.damage = 0;
                    item.DamageType = DamageClass.Default;
                    item.shoot = ProjectileID.None;
                    item.useAnimation = default;
                    item.useTime = default;
                }
                else if (item.type == ItemType<Fabstaff>())
                    item.UseSound = AresTeslaCannon.TeslaOrbShootSound with { Pitch = 0.5f, PitchVariance = 0.2f, Volume = 0.5f };
                if (!CalRemixWorld.renames)
                    return;
                #region Text Changes
                if (item.type == ItemType<InfestedClawmerang>())
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
                else if (item.type == ItemType<Fabstaff>())
                {
                    item.SetNameOverride("Interfacer Staff");
                }
                else if (item.type == ItemType<Fabsol>())
                {
                    item.SetNameOverride("Discordian Sigil");
                }
                #endregion
            }
            public override bool CanUseItem(Item item, Player player)
            {
                if (item.type == ItemType<ClockGatlignum>())
                {
                    return false;
                }
                return true;
            }
            public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
            {
                if (!CalRemixWorld.renames)
                    return;
                Mod mod = Mod;
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
            }
        }
        public class RProj : GlobalProjectile
        {
            public override bool InstancePerEntity => true;
            public override void SetStaticDefaults()
            {
                if (!CalRemixWorld.resprites)
                    return;
                foreach (KeyValuePair<int, string> p in RethemeList.Projs)
                {
                    TextureAssets.Projectile[p.Key] = Request<Texture2D>("CalRemix/Retheme/" + p.Value);
                }
            }
            public override void SetDefaults(Projectile projectile)
            {
                if (!CalRemixWorld.renames)
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
                if (!CalRemixWorld.resprites)
                    return null;
                if ((!Main.dayTime || BossRushEvent.BossRushActive) && (projectile.type == ProjectileType<HolyBlast>() || projectile.type == ProjectileType<HolyBomb>() || projectile.type == ProjectileType<HolyFire>() || projectile.type == ProjectileType<HolyFire2>() || projectile.type == ProjectileType<HolyFlare>() || projectile.type == ProjectileType<MoltenBlob>() || projectile.type == ProjectileType<MoltenBlast>()))
                    return Color.MediumPurple;
                return null;
            }
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
            IL.CalamityMod.NPCs.Other.THELORDE.PreDraw += LORDE;
           // MonoModHooks.Modify(typeof(Providence).GetMethod("<PreDraw>g__drawProvidenceInstance|46_0", BindingFlags.NonPublic | BindingFlags.Instance), Providence);
            //MonoModHooks.Modify(typeof(ModLoader).Assembly.GetType("CalamityMod.WeakReferenceSupport").GetMethod("AddCalamityBosses", BindingFlags.NonPublic | BindingFlags.Static), BossChecklist);

            // IL.CalamityMod.Items.Weapons.PreDraw += ;
            On.CalamityMod.Items.SummonItems.ExoticPheromones.CanUseItem += Exotic;
            On.CalamityMod.Items.SummonItems.AstralChunk.CanUseItem += AstralChunk;
            IL.CalamityMod.Items.Weapons.Ranged.HeavenlyGale.PostDrawInWorld += HeavenlyGale;

            // IL.CalamityMod.Projectiles.PreDraw += ;
            IL.CalamityMod.Projectiles.Rogue.InfestedClawmerangProj.PreDraw += InfestedClawmerangProj;
            IL.CalamityMod.Projectiles.Magic.EldritchTentacle.AI += EldritchTentacle;
            //IL.CalamityMod.Projectiles.Melee.MurasamaSlash.PreDraw += MurasamaSlash;
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
            if (c.TryGotoNext(MoveType.After,i => i.MatchLdstr("Dragonfolly"), i => i.MatchStloc(80)))
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
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Crabulon/CrabulonGlow")))
                Retheme(c, "CalRemix/Retheme/Crabulon/CrabulonGlow");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Crabulon/CrabulonAltGlow")))
                Retheme(c, "CalRemix/Retheme/Crabulon/CrabulonAltGlow");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Crabulon/CrabulonAttackGlow")))
                Retheme(c, "CalRemix/Retheme/Crabulon/CrabulonAttackGlow");
        }
        private static void HiveMind(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/HiveMind/HiveMindP2")))
                Retheme(c, "CalRemix/Retheme/HiveMind/HiveMindP2");
        }
        private static void PerforatorCyst(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Perforator/PerforatorCystGlow")))
                Retheme(c, "CalRemix/Retheme/Perfs/CystGlow");
        }
        #region PerfWormHeck
        private static void PerforatorHive(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Perforator/PerforatorHiveGlow")))
                Retheme(c, "CalRemix/Retheme/Perfs/HiveGlow");
        }
        #endregion
        private static void PerfLBody(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Perforator/PerforatorBodyLargeAlt")))
                Retheme(c, "CalRemix/Retheme/Perfs/LBodyAlt");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Perforator/PerforatorBodyLargeGlow")))
                Retheme(c, "CalRemix/Retheme/Perfs/LBodyGlow");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Perforator/PerforatorBodyLargeAltGlow")))
                Retheme(c, "CalRemix/Retheme/Perfs/LBodyAltGlow");
        }
        private static void PerfMBody(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Perforator/PerforatorBodyMediumGlow")))
                Retheme(c, "CalRemix/Retheme/Perfs/MBodyGlow");
        }
        private static void PerfSBody(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Perforator/PerforatorBodySmallGlow")))
                Retheme(c, "CalRemix/Retheme/Perfs/SBodyGlow");
        }
        private static void PerfLHead(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Perforator/PerforatorHeadLargeGlow")))
                Retheme(c, "CalRemix/Retheme/Perfs/LHeadGlow");
        }
        private static void PerfMHead(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Perforator/PerforatorHeadMediumGlow")))
                Retheme(c, "CalRemix/Retheme/Perfs/MHeadGlow");
        }
        private static void PerfSHead(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Perforator/PerforatorHeadSmallGlow")))
                Retheme(c, "CalRemix/Retheme/Perfs/SHeadGlow");
        }
        private static void PerfLTail(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Perforator/PerforatorTailLargeGlow")))
                Retheme(c, "CalRemix/Retheme/Perfs/LTailGlow");
        }
        private static void PerfMTail(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Perforator/PerforatorTailMediumGlow")))
                Retheme(c, "CalRemix/Retheme/Perfs/MTailGlow");
        }
        private static void PerfSTail(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Perforator/PerforatorTailSmallGlow")))
                Retheme(c, "CalRemix/Retheme/Perfs/STailGlow");
        }
        private static void Cryogen(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Cryogen/Cryogen_Phase")))
                Retheme(c, "CalRemix/Retheme/Cryogen/CryogenPhase");
        }
        private static void CryogenShield(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Cryogen/CryogenShield")))
                Retheme(c, "CalRemix/Retheme/Cryogen/CryogenShield");
        }
        private static void CalamitasClone(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/CalClone/CalamitasCloneGlow")))
                Retheme(c, "CalRemix/Retheme/Cal/CalamitasGlow");
        }
        private static void Cataclysm(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/CalClone/CataclysmGlow")))
                Retheme(c, "CalRemix/Retheme/Cal/CataclysmGlow");
        }
        private static void Catastrophe(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/CalClone/CatastropheGlow")))
                Retheme(c, "CalRemix/Retheme/Cal/CatastropheGlow");
        }
        private static void Leviathan(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Leviathan/LeviathanAttack")))
                Retheme(c, "CalRemix/Retheme/Levi/Levi2");
        }
        private static void Anahita(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Leviathan/AnahitaStabbing")))
                Retheme(c, "CalRemix/Retheme/Levi/AnahitaStab");
        }
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
                Retheme(c, "CalRemix/Retheme/Plague/AureusSpawnGlow");
        }
        private static void AstrumAureus(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/AstrumAureus/AstrumAureusGlow")))
                Retheme(c, "CalRemix/Retheme/Plague/AstrumAureusGlow");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/AstrumAureus/AstrumAureusRecharge")))
                Retheme(c, "CalRemix/Retheme/Plague/AstrumAureusRecharge");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/AstrumAureus/AstrumAureusWalk")))
                Retheme(c, "CalRemix/Retheme/Plague/AstrumAureusWalk");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/AstrumAureus/AstrumAureusWalkGlow")))
                Retheme(c, "CalRemix/Retheme/Plague/AstrumAureusWalkGlow");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/AstrumAureus/AstrumAureusGlow")))
                Retheme(c, "CalRemix/Retheme/Plague/AstrumAureusGlow");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/AstrumAureus/AstrumAureusJump")))
                Retheme(c, "CalRemix/Retheme/Plague/AstrumAureusJump");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/AstrumAureus/AstrumAureusJumpGlow")))
                Retheme(c, "CalRemix/Retheme/Plague/AstrumAureusJumpGlow");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/AstrumAureus/AstrumAureusStomp")))
                Retheme(c, "CalRemix/Retheme/Plague/AstrumAureusStomp");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/AstrumAureus/AstrumAureusStompGlow")))
                Retheme(c, "CalRemix/Retheme/Plague/AstrumAureusStompGlow"); 
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/AstrumAureus/AstrumAureusGlow")))
                Retheme(c, "CalRemix/Retheme/Plague/AstrumAureusGlow");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/AstrumAureus/AstrumAureusJump")))
                Retheme(c, "CalRemix/Retheme/Plague/AstrumAureusJump");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/AstrumAureus/AstrumAureusJumpGlow")))
                Retheme(c, "CalRemix/Retheme/Plague/AstrumAureusJumpGlow");
        }
        /*
        private static void PBG(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/PlaguebringerGoliath/PlaguebringerGoliathGlow")))
                Retheme(c, "CalRemix/Retheme/Plague/PBGGlow");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/PlaguebringerGoliath/PlaguebringerGoliathChargeTex")))
                Retheme(c, "CalRemix/Retheme/Plague/PBG");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/PlaguebringerGoliath/PlaguebringerGoliathChargeTexGlow")))
                Retheme(c, "CalRemix/Retheme/Plague/PBGGlow");
        }*/
        private static void AstrumDeusHead(ILContext il)
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
        private static void AstrumDeusBody(ILContext il)
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
        private static void AstrumDeusTail(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/AstrumDeus/AstrumDeusTailGlow")))
                Retheme(c, "CalRemix/Retheme/AD/TailGlow");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/AstrumDeus/AstrumDeusTailGlow2")))
                Retheme(c, "CalRemix/Retheme/AD/TailGlow");
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
                Retheme(c, "CalRemix/Retheme/Birb/BirbGlow");
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
        private static void Providence(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Providence/")))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !Main.zenithWorld ? "CalRemix/Retheme/Providence/" : "CalamityMod/NPCs/Providence/");
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
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Yharon/YharonGlowGreen")))
                Retheme(c, "CalRemix/Retheme/Yharon/YharonGlowGreen");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/NPCs/Yharon/YharonGlowPurple")))
                Retheme(c, "CalRemix/Retheme/Yharon/YharonGlowPurple");
        }
        private static void LORDE(ILContext il)
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
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/Items/Weapons/Ranged/HeavenlyGaleGlow")))
                Retheme(c, "CalRemix/Retheme/Blank");
        }
        #endregion
        #region Projectiles
        private static void InfestedClawmerangProj(ILContext il)
        {
            var c = new ILCursor(il);
            var t = typeof(ModProjectile).GetMethod("get_Texture", BindingFlags.Public | BindingFlags.Instance);
            if (c.TryGotoNext(i => i.MatchCallvirt(t)))
                Retheme(c, "CalRemix/Retheme/Crabulon/Shroomerang");
        }
        private static void EldritchTentacle(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdcI4(60)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldc_I4, 188);
            }
        }
        private static void MurasamaSlash(ILContext il)
        {
            var c = new ILCursor(il);
            var t = typeof(ModProjectile).GetMethod("get_Texture", BindingFlags.Public | BindingFlags.Instance);
            if (c.TryGotoNext(i => i.MatchCallvirt(t)))
                Retheme(c, "CalRemix/Retheme/MurasamaSlash");
        }
        private static void ExobladeProj(ILContext il)
        {
            var c = new ILCursor(il);
            var t = typeof(ModProjectile).GetMethod("get_Texture", BindingFlags.Public | BindingFlags.Instance);
            if (c.TryGotoNext(i => i.MatchCallvirt(t)))
                Retheme(c, "CalRemix/Retheme/Exo/Blade");
        }
        private static void HeavenlyGaleProj(ILContext il)
        {
            var c = new ILCursor(il);
            var t = typeof(ModProjectile).GetMethod("get_Texture", BindingFlags.Public | BindingFlags.Instance);
            if (c.TryGotoNext(i => i.MatchCallvirt(t)))
                Retheme(c, "CalRemix/Retheme/Exo/GaleProj");
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/Projectiles/Ranged/HeavenlyGaleProjGlow")))
                Retheme(c, "CalRemix/Retheme/Exo/GaleProjGlow");
        }
        private static void CelestusProj(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchLdstr("CalamityMod/Projectiles/Rogue/CelestusProjGlow")))
                Retheme(c, "CalRemix/Retheme/Blank");
        }
        private static void ViolenceThrownProjectile(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => i.MatchCallvirt(typeof(ModProjectile).GetMethod("get_Texture", BindingFlags.Public | BindingFlags.Instance))))
                Retheme(c, "CalRemix/Retheme/Violence");
        }
        private static void HolyBlast(ILContext il)
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
