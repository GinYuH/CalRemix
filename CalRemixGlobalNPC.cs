using Terraria;
using Terraria.ModLoader;
using CalamityMod.NPCs.TownNPCs;
using CalRemix.Items.Materials;
using CalRemix.Tiles;
using Microsoft.Xna.Framework;
using CalamityMod.NPCs.DesertScourge;
using CalamityMod;
using CalamityMod.Items.SummonItems;
using CalamityMod.NPCs.Abyss;
using CalamityMod.NPCs.AcidRain;
using CalamityMod.NPCs.PrimordialWyrm;
using CalamityMod.NPCs.AstrumAureus;
using CalamityMod.NPCs.BrimstoneElemental;
using CalamityMod.NPCs.NormalNPCs;
using CalamityMod.NPCs.Ravager;
using CalamityMod.NPCs.SlimeGod;
using CalamityMod.NPCs.SupremeCalamitas;
using CalamityMod.NPCs.SulphurousSea;
using CalRemix.NPCs;
using CalRemix.NPCs.Bosses;
using CalRemix.Items;
using CalRemix.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.NPCs.Bumblebirb;
using System.Collections.Generic;
using CalRemix.Projectiles.Accessories;
using CalRemix.Projectiles.Weapons;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework.Graphics;
using CalamityMod.NPCs.Providence;
using CalamityMod.Events;
using System;
using Terraria.GameContent;
using System.IO;
using Terraria.DataStructures;
using Terraria.Chat;
using Terraria.Localization;
using CalamityMod.NPCs.Crabulon;
using CalamityMod.NPCs.ExoMechs;
using CalRemix.Items.Weapons;
using CalamityMod.Items.TreasureBags;
using CalamityMod.NPCs.ExoMechs.Artemis;
using CalamityMod.Items.Potions;

namespace CalRemix
{
    public class CalRemixGlobalNPC : GlobalNPC
    {
        bool SlimeBoost = false;
        public bool vBurn = false;
        public int bossKillcount = 0;
        public float shadowHit = 1;
        private bool useDefenseFrames;
        private int frameUsed;
        private int say = 0;
        public static int wulfyrm = -1;
        public int clawed = 0;
        public static int aspidCount = 0;
        public Vector2 clawPosition = Vector2.Zero;
        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }

        public List<int> BossSlimes = new List<int> 
        { 
            NPCID.KingSlime,
            NPCID.QueenSlimeBoss,
            ModContent.NPCType<AstrumAureus>(),
            ModContent.NPCType<EbonianPaladin>(),
            ModContent.NPCType<CrimulanPaladin>(),
            ModContent.NPCType<SplitEbonianPaladin>(),
            ModContent.NPCType<SplitCrimulanPaladin>(),
            ModContent.NPCType<SlimeGodCore>(),
            ModContent.NPCType<LifeSlime>(),
            ModContent.NPCType<CragmawMire>()
        };

        public List<int> Slimes = new List<int>
        {
            1, 16, 59, 71, 81, 138, 121, 122, 141, 147, 183, 184, 204, 225, 244, 302, 333, 335, 334, 336, 537,
            NPCID.SlimeSpiked,
            NPCID.QueenSlimeMinionBlue,
            NPCID.QueenSlimeMinionPink,
            NPCID.QueenSlimeMinionPurple,
            ModContent.NPCType<AeroSlime>(),
            ModContent.NPCType<CalamityMod.NPCs.Astral.AstralSlime>(),
            ModContent.NPCType<CalamityMod.NPCs.PlagueEnemies.PestilentSlime>(),
            ModContent.NPCType<BloomSlime>(),
            ModContent.NPCType<CalamityMod.NPCs.Crags.InfernalCongealment>(),
            ModContent.NPCType<PerennialSlime>(),
            ModContent.NPCType<CryoSlime>(),
            ModContent.NPCType<GammaSlime>(),
            ModContent.NPCType<IrradiatedSlime>(),
            ModContent.NPCType<AuricSlime>(),
            ModContent.NPCType<CorruptSlimeSpawn>(),
            ModContent.NPCType<CorruptSlimeSpawn2>(),
            ModContent.NPCType<CrimsonSlimeSpawn>(),
            ModContent.NPCType<CrimsonSlimeSpawn2>()
        };
        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            if (npc.type == ModContent.NPCType<Crabulon>())
            {
                if (DateTime.Today.ToString("dd/MM").Equals("01/04") && Main.rand.NextBool(100))
                    Talk("Buy Delicious Meat! So Very Delicious! 20% Off! Buy Today!", Color.LightSkyBlue);
                else
                    Talk("Hello, are you here to place a delivery for my world-famous Delicious Meat, made with Frosted Pigron and Blue Truffles (now 70% bluer)?", Color.LightSkyBlue);
            }
        }
        public override bool PreAI(NPC npc)
        {
            if (CalamityUtils.CountProjectiles(ModContent.ProjectileType<Claw>()) <= 0)
            {
                clawed = 0;
            }
            clawed--;
            CalRemixPlayer player = Main.LocalPlayer.GetModPlayer<CalRemixPlayer>();

            bool assortgel = player.assortegel;
            bool amalgam = player.amalgel;
            bool godfather = player.godfather;
            bool tvo = player.tvo;

            bool bossrush = CalamityMod.Events.BossRushEvent.BossRushActive;
            bool normalSlime = Slimes.Contains(npc.type);
            bool bossSlime = BossSlimes.Contains(npc.type);

            // Kill passive slimes if none of the accessories are on
            if (npc.GetGlobalNPC<CalRemixGlobalNPC>().SlimeBoost && !assortgel && !amalgam && !godfather)
            {
                npc.active = false;
                return false;
            }
            // Godfather causes slimes to try to assimilate into goozma
            if (godfather && !bossrush)
            {
                if (normalSlime || bossSlime)
                {
                    if (!npc.GetGlobalNPC<CalRemixGlobalNPC>().SlimeBoost)
                    {
                        npc.life = npc.lifeMax;
                        npc.chaseable = false;
                        npc.friendly = true;
                        npc.GetGlobalNPC<CalRemixGlobalNPC>().SlimeBoost = true;
                    }
                    if (Main.LocalPlayer.ownedProjectileCounts[ModContent.ProjectileType<CriticalSlimeCore>()] == 1)
                    {
                        for (int i = 0; i < Main.maxProjectiles; i++)
                        {
                            Projectile target = Main.projectile[i];
                            if (target.type == ModContent.ProjectileType<CriticalSlimeCore>())
                            {
                                Vector2 direction = target.Center - npc.Center;
                                direction.Normalize();
                                npc.velocity = direction * 20;
                                npc.noTileCollide = true;
                            }
                        }
                    }
                }
            }
            // Behavior if you DONT have godfather
            else
            {
                // If other passive slime accessories, and the slime isn't a boss, target the player's target
                if (normalSlime && (assortgel || amalgam))
                {
                    if (!npc.GetGlobalNPC<CalRemixGlobalNPC>().SlimeBoost)
                    {
                        if (amalgam)
                        {
                            npc.lifeMax = (int)(npc.lifeMax * 22f);
                            npc.damage = (int)(npc.damage * 12f);
                        }
                        else
                        {
                            npc.lifeMax = (int)(npc.lifeMax * 6f);
                            npc.damage = (int)(npc.damage * 12f);
                        }
                        npc.life = npc.lifeMax;
                        npc.chaseable = false;
                        npc.friendly = true;
                        npc.GetGlobalNPC<CalRemixGlobalNPC>().SlimeBoost = true;
                    }
                    if (Main.LocalPlayer.MinionAttackTargetNPC > 0 && !Slimes.Contains(Main.npc[Main.LocalPlayer.MinionAttackTargetNPC].type))
                    {
                        Vector2 direction = Main.npc[Main.LocalPlayer.MinionAttackTargetNPC].Center - npc.Center;
                        direction.Normalize();
                        npc.velocity = direction * 20;
                        npc.noTileCollide = true;
                    }
                    else
                    {
                        npc.noTileCollide = false;
                    }
                    for (int i = 0; i < Main.maxNPCs; i++)
                    {
                        NPC target = Main.npc[i];
                        Rectangle thisrect = npc.getRect();
                        Rectangle theirrect = target.getRect();
                        if (target.immune[npc.whoAmI] == 0 && thisrect.Intersects(theirrect) && target.whoAmI != npc.whoAmI && npc.active && target.active && !target.dontTakeDamage && !normalSlime)
                        {
                            if (bossSlime && amalgam)
                            {

                            }
                            else
                            {
                                target.StrikeNPC(npc.CalculateHitInfo(npc.damage, 0));
                                target.immune[npc.whoAmI] = 10;
                                if (target.damage > 0)
                                    npc.StrikeNPC(npc.CalculateHitInfo(target.damage, 0));
                            }
                        }

                    }
                }
                // If it's a boss and you have gemalgamation, attack enemies
                if (bossSlime && amalgam && !assortgel && !bossrush)
                {
                    if (!npc.GetGlobalNPC<CalRemixGlobalNPC>().SlimeBoost)
                    {
                        npc.boss = false;
                        npc.friendly = true;
                        npc.lifeMax = (int)(npc.lifeMax * 22f);
                        npc.damage = (int)(npc.damage * 12f);
                        npc.life = npc.lifeMax;
                        npc.chaseable = false;
                        npc.GetGlobalNPC<CalRemixGlobalNPC>().SlimeBoost = true;
                    }
                    if (Main.LocalPlayer.MinionAttackTargetNPC > 0 && !Slimes.Contains(Main.npc[Main.LocalPlayer.MinionAttackTargetNPC].type) && !BossSlimes.Contains(Main.npc[Main.LocalPlayer.MinionAttackTargetNPC].type))
                    {
                        Vector2 direction = Main.npc[Main.LocalPlayer.MinionAttackTargetNPC].Center - npc.Center;
                        direction.Normalize();
                        npc.velocity = direction * 20;
                        npc.noTileCollide = true;
                    }
                    else
                    {
                        npc.velocity.X *= 0.98f;
                        if (npc.velocity.Y < 10)
                            npc.velocity.Y += 1;
                        npc.noTileCollide = false;
                    }
                    for (int i = 0; i < Main.maxNPCs; i++)
                    {
                        NPC target = Main.npc[i];
                        Rectangle thisrect = npc.getRect();
                        Rectangle theirrect = target.getRect();
                        if (target.immune[npc.whoAmI] == 0 && thisrect.Intersects(theirrect) && target.whoAmI != npc.whoAmI && npc.active && target.active && !target.dontTakeDamage && !Slimes.Contains(target.type) && !bossSlime)
                        {
                            target.StrikeNPC(target.CalculateHitInfo(npc.damage,  0));
                            target.immune[npc.whoAmI] = 10;
                            if (target.damage > 0)
                                npc.StrikeNPC(npc.CalculateHitInfo(target.damage, 0));
                        }

                    }
                    return false;
                }
                // if it's a boss and you have assortagelatin, do nothing and become passive
                else if (bossSlime && assortgel && !amalgam && !bossrush)
                {
                    npc.damage = 0;
                    // slime god is specifically excluded because hes stupid and i hate him
                    if (npc.type == ModContent.NPCType<SlimeGodCore>())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            if (npc.type == ModContent.NPCType<Crabulon>())
            {
                if (say == 1 && npc.life < (npc.lifeMax * 3 / 4))
                {
                    Talk("You must be kidding. You're just another one of those desperate Delicious Meat fans that don't care to pay up for our hard work that was put into making these. For shame.", Color.LightSkyBlue);
                    say = 2;
                }
                if (say == 2 && npc.life < (npc.lifeMax / 3))
                {
                    Talk("You remind me of that giant mushroom pig flying fish thing. If it could, it would easily butcher you whole, while you're blinded by your depression or whatever.", Color.LightSkyBlue);
                    say = 3;
                }
            }
            return true;
        }
        public override void AI(NPC npc)
        {
            CalRemixPlayer modPlayer = Main.LocalPlayer.GetModPlayer<CalRemixPlayer>();
            if (npc.type == ModContent.NPCType<MicrobialCluster>() && npc.catchItem == 0)
            {
                npc.catchItem = ModContent.ItemType<DisgustingSeawater>();
            }
            if (npc.type == ModContent.NPCType<FAP>()) // MURDER the drunk princess
            {
                npc.active = false;
            }
            /*if (npc.type == ModContent.NPCType<Bumblefuck>() && Main.LocalPlayer.ZoneDesert)
            {
                npc.localAI[1] = 0;
            }*/
            if (npc.type == ModContent.NPCType<AureusSpawn>() && (modPlayer.nuclegel || modPlayer.assortegel) && !CalamityMod.Events.BossRushEvent.BossRushActive)
            {
                npc.active = false;
            }
            if (npc.type == ModContent.NPCType<WulfrumAmplifier>())
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC exc = Main.npc[i];
                    if (npc.Distance(exc.Center) <= npc.ai[1] && exc.ModNPC is WulfwyrmHead)
                    {
                        exc.ModNPC<WulfwyrmHead>().PylonCharged = true;
                    }
                }
            }
        }
        public override void PostAI(NPC npc)
        {
            if (!CalamityMod.CalPlayer.CalamityPlayer.areThereAnyDamnBosses && !CalamityLists.enemyImmunityList.Contains(npc.type))
            {
                if (npc.GetGlobalNPC<CalamityMod.NPCs.CalamityGlobalNPC>().pearlAura > 0)
                    npc.AddBuff(ModContent.BuffType<CalamityMod.Buffs.StatDebuffs.GlacialState>(), 60);
            }
            if (npc.GetGlobalNPC<CalRemixGlobalNPC>().clawed > 0)
            {
                npc.position.X = MathHelper.Lerp(npc.position.X, clawPosition.X - npc.width / 2, 0.1f);
                npc.position.Y = MathHelper.Lerp(npc.position.Y, clawPosition.Y - npc.height / 2, 0.1f);
                npc.velocity = Vector2.Zero;
                    npc.position += new Vector2(Main.rand.NextFloat(-1f, 2f), Main.rand.NextFloat(-1f, 2f));
                npc.frameCounter += 2;
            }
        }
        public override void ModifyTypeName(NPC npc, ref string typeName)
        {
            if (npc.type == ModContent.NPCType<WITCH>())
            {
                typeName = "Calamity Witch";
            }
            else if (npc.type == ModContent.NPCType<BrimstoneElemental>())
            {
                typeName = "Calamity Elemental";
            }
            else if (npc.type == ModContent.NPCType<BrimstoneHeart>())
            {
                typeName = "Calamity Heart";
            }
        }
        public override void SetDefaults(NPC npc)
        {
            if (npc.type == ModContent.NPCType<BrimstoneElemental>())
            {
                npc.GivenName = "Calamity Elemental";
            }
            else if (npc.type == ModContent.NPCType<BrimstoneHeart>())
            {
                npc.GivenName = "Calamity Heart";
            }
            /*else if (npc.type == ModContent.NPCType<Bumblefuck>())
            {
                npc.damage = 80;
                npc.lifeMax = 58500;
                npc.defense = 20;
                npc.value = Item.buyPrice(gold: 10);
            }
            else if (npc.type == ModContent.NPCType<Bumblefuck2>())
            {
                npc.damage = 60;
                npc.lifeMax = 3375;
            }*/
            else if (npc.type == ModContent.NPCType<SlimeGodCore>())
            {
                TextureAssets.Npc[npc.type] = ModContent.Request<Texture2D>("CalRemix/Resprites/SlimeGod/SlimeGodCore");
                TextureAssets.NpcHeadBoss[npc.GetBossHeadTextureIndex()] = ModContent.Request<Texture2D>("CalRemix/Resprites/SlimeGod/SlimeGodCore_Head_Boss");
            }
            else if (npc.type == ModContent.NPCType<Eidolist>())
            {
                TextureAssets.Npc[npc.type] = ModContent.Request<Texture2D>("CalRemix/Resprites/Eidolist");
            }
        }
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == ModContent.NPCType<DesertScourgeHead>())
            {
                npcLoot.Add(ModContent.ItemType<ParchedScale>(), 1, 25, 30);
                npcLoot.Remove(npcLoot.DefineNormalOnlyDropSet().Add(DropHelper.PerPlayer(ModContent.ItemType<PearlShard>(), 1, 25, 30)));
            }
            /*else if (npc.type == ModContent.NPCType<Bumblefuck>())
            {
                npcLoot.Remove(npcLoot.DefineNormalOnlyDropSet().Add(ModContent.ItemType<EffulgentFeather>(), 1, 25, 30));
            }
            else*/
            if (npc.type == ModContent.NPCType<PrimordialWyrmHead>())
            {
                npcLoot.Add(ModContent.ItemType<SubnauticalPlate>(), 1, 22, 34);
            }
            else if (npc.type == ModContent.NPCType<MirageJelly>())
            {
                npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<MirageJellyItem>(), 7, 5));
            }
            else if (npc.type == ModContent.NPCType<CragmawMire>())
            {
                LeadingConditionRule postPolter = npcLoot.DefineConditionalDropSet(() => DownedBossSystem.downedPolterghast);
                postPolter.Add(ModContent.ItemType<NucleateGello>(), 10, hideLootReport: !DownedBossSystem.downedPolterghast);
                postPolter.AddFail(ModContent.ItemType<NucleateGello>(), hideLootReport: DownedBossSystem.downedPolterghast);
            }
            else if (npc.type == ModContent.NPCType<NuclearTerror>())
            {
                npcLoot.Add(ModContent.ItemType<Microxodonta>(), 3);
            }
            else if (npc.type == NPCID.GraniteFlyer)
            {
                npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<CosmicStone>(), 20, 10));
            }
            else if (npc.type == ModContent.NPCType<Horse>())
            {
                npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<RockStone>(), 5, 3));
            }
            else if (npc.type == ModContent.NPCType<Providence>())
            {
                LeadingConditionRule hm = npcLoot.DefineConditionalDropSet(() => !Main.expertMode);
                hm.Add(ModContent.ItemType<ProfanedNucleus>(), 4);
            }
            else if (npc.type == ModContent.NPCType<Cnidrion>())
            {
                LeadingConditionRule postDS = npcLoot.DefineConditionalDropSet(() => CalRemixWorld.downedExcavator);
                postDS.Add(ModContent.ItemType<DesertMedallion>(), 1, hideLootReport: !CalRemixWorld.downedExcavator);
            }
            else if (npc.type == NPCID.ManEater || CalamityLists.hornetList.Contains(npc.type) || npc.type == NPCID.SpikedJungleSlime || npc.type == NPCID.JungleSlime)
            {
                LeadingConditionRule hm = npcLoot.DefineConditionalDropSet(() => Main.hardMode);
                hm.Add(ModContent.ItemType<EssenceofBabil>(), 4, hideLootReport: !Main.hardMode);
            }
            else if (npc.type == NPCID.AngryTrapper || CalamityLists.mossHornetList.Contains(npc.type) || npc.type == NPCID.Derpling)
            {
                npcLoot.Add(ModContent.ItemType<EssenceofBabil>(), 3);
            }
            else if (npc.type == NPCID.Plantera)
            {
                LeadingConditionRule exp = npcLoot.DefineConditionalDropSet(() => !Main.expertMode);
                exp.Add(ModContent.ItemType<EssenceofBabil>(), 1, 4, 8, hideLootReport: Main.expertMode);
            }
            else if (npc.type == NPCID.Wolf)
            {
                LeadingConditionRule postPolter = npcLoot.DefineConditionalDropSet(() => Main.expertMode);
                postPolter.Add(ModContent.ItemType<CoyoteVenom>(), 3, hideLootReport: !Main.expertMode);
                postPolter.AddFail(ModContent.ItemType<CoyoteVenom>(), 4, hideLootReport: Main.expertMode);
            }
            else if (npc.type == ModContent.NPCType<SupremeCalamitas>())
            {
                npcLoot.Add(ModContent.ItemType<YharimBar>(), 1, 6, 8);
            }
            else if (npc.type == ModContent.NPCType<Crabulon>())
            {
                npcLoot.Add(ModContent.ItemType<DeliciousMeat>(), 1, 4, 7);
            }
            else if (npc.type == NPCID.DukeFishron)
            {
                npcLoot.Add(ModContent.ItemType<DeliciousMeat>(), 2, 45, 92);
            }
            if (npc.boss && bossKillcount > 5)
            {
                //npcLoot.Add(ModContent.ItemType<PearlShard>(), 1, 2, 4);
            }
            else if (!npc.SpawnedFromStatue && npc.value > 0 && NPC.killCount[npc.type] >= 25)
            {
                //npcLoot.Add(ModContent.ItemType<PearlShard>(), 5, 1, 1);
            }
            else if (NPCID.Sets.DemonEyes[npc.type])
            {
                npcLoot.AddIf(() => Main.LocalPlayer.armor[0].type == ItemID.WoodHelmet && Main.LocalPlayer.armor[1].type == ItemID.WoodBreastplate && Main.LocalPlayer.armor[2].type == ItemID.WoodGreaves, ModContent.ItemType<Ogscule>());
            }
        }
        public override void OnKill(NPC npc)
        {
            if (Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().tvo)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ModContent.ProjectileType<LumChunk>(), 0, 0, Main.myPlayer);
                }
            }
            else if (Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().cart)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ModContent.ProjectileType<CalHeart>(), 0, 0, Main.myPlayer);
                }
            }
            if (npc.boss)
            {
                bossKillcount++;
            }
            if (npc.type == ModContent.NPCType<Horse>())
                CalRemixWorld.downedEarth = true;
        }

        public override void LoadData(NPC npc, TagCompound tag)
        {
            bossKillcount = tag.GetInt("bossKillcount");
        }

        public override void SaveData(NPC npc, TagCompound tag)
        {
            tag["bossKillcount"] = bossKillcount;
        }
        public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter)
        {
            if (npc.type == ModContent.NPCType<Providence>() && !Main.dayTime)
            {
                binaryWriter.Write(useDefenseFrames);
                binaryWriter.Write(frameUsed);
                for (int i = 0; i < 4; i++)
                {
                    binaryWriter.Write(npc.Calamity().newAI[i]);
                }
            }
            if (npc.type == ModContent.NPCType<Crabulon>())
            {
                binaryWriter.Write(say);
            }
            if (BossSlimes.Contains(npc.type) || Slimes.Contains(npc.type))
            {
                binaryWriter.Write(npc.GetGlobalNPC<CalRemixGlobalNPC>().SlimeBoost);
            }
        }
        public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader)
        {
            if (npc.type == ModContent.NPCType<Providence>() && !Main.dayTime)
            {
                useDefenseFrames = binaryReader.ReadBoolean();
                frameUsed = binaryReader.ReadInt32();
                for (int i = 0; i < 4; i++)
                {
                    npc.Calamity().newAI[i] = binaryReader.ReadSingle();
                }
            }
            if (npc.type == ModContent.NPCType<Crabulon>())
            {
                say = binaryReader.ReadInt32();
            }
            if (BossSlimes.Contains(npc.type) || Slimes.Contains(npc.type))
            {
                npc.GetGlobalNPC<CalRemixGlobalNPC>().SlimeBoost = binaryReader.ReadBoolean();
            }
        }
        public override void FindFrame(NPC npc, int frameHeight)
        {
            if (npc.type == ModContent.NPCType<Providence>() && !Main.dayTime)
            {
                if (npc.ai[0] == 2f && npc.ai[0] == 5f)
                {
                    if (!useDefenseFrames)
                    {
                        useDefenseFrames = true;
                    }
                }
                else
                {
                    if (useDefenseFrames)
                    {
                        useDefenseFrames = false;
                    }
                    if (frameUsed > 3)
                    {
                        frameUsed = 0;
                    }
                }
      
            }
        }

        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (npc.type == ModContent.NPCType<Providence>() && !Main.dayTime)
            {
                return false;
            }
            return true;
        }
        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (npc.type == ModContent.NPCType<Providence>() && (!Main.dayTime || BossRushEvent.BossRushActive))
            {
                float lerpValue = Utils.GetLerpValue(0f, 45f, ModContent.GetInstance<Providence>().DeathAnimationTimer, true);
                int lerps = (int)MathHelper.Lerp(1f, 30f, lerpValue);
                for (int i = 0; i < lerps; i++)
                {
                    float lerps2 = MathF.PI * 2f * i * 2f / lerps;
                    Vector2 drawOffset = lerps2.ToRotationVector2() * (float)Math.Sin(lerps2 * 6f + Main.GlobalTimeWrappedHourly * MathF.PI) * ((float)Math.Pow(lerpValue, 3.0) * 50f);
                    Color value = Color.Lerp(Color.White, Color.White * (MathHelper.Lerp(0.4f, 0.8f, lerpValue) / lerps * 1.5f), lerpValue);
                    value.A = 0;
                    drawProvidenceInstance(drawOffset, lerps==1 ? null : new Color?(value));
                }
                void drawProvidenceInstance(Vector2 drawOffset, Color? colorOverride)
                {
                    string textBase = "CalRemix/Resprites/Providence/Providence";
                    string textWings = "CalRemix/Resprites/Providence/Glowmasks/Providence";
                    string textSpike = "CalRemix/Resprites/Providence/Glowmasks/Providence";
                    if (npc.ai[0] == 2f || npc.ai[0] == 5f)
                    {
                        if (useDefenseFrames)
                        {
                            textBase += "DefenseNight";
                            textWings += "DefenseGlowNight";
                            textSpike += "DefenseGlow2Night";
                        }
                        else
                        {
                            textBase += "DefenseAltNight";
                            textWings += "DefenseAltGlowNight";
                            textSpike += "DefenseAltGlow2Night";
                        }
                    }
                    else if (frameUsed == 0)
                    {
                        textWings += "GlowNight";
                        textSpike += "Glow2Night";
                    }
                    else if (frameUsed == 1)
                    {
                        textBase += "AltNight";
                        textWings += "AltGlowNight";
                        textSpike += "AltGlow2Night";
                    }
                    else if (frameUsed == 2)
                    {
                        textBase += "AttackNight";
                        textWings += "AttackGlowNight";
                        textSpike += "AttackGlow2Night";
                    }
                    else
                    {
                        textBase += "AttackAltNight";
                        textWings += "AttackAltGlowNight";
                        textSpike += "AttackAltGlow2Night";
                    }
                    Texture2D body = ModContent.Request<Texture2D>(textBase).Value;
                    Texture2D wings = ModContent.Request<Texture2D>(textWings).Value;
                    Texture2D spikes = ModContent.Request<Texture2D>(textSpike).Value;
                    SpriteEffects effects = npc.spriteDirection==1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

                    Vector2 origin = new Vector2(TextureAssets.Npc[npc.type].Value.Width / 2, TextureAssets.Npc[npc.type].Value.Height / Main.npcFrameCount[npc.type] / 2);
                    Vector2 textPos = (npc.Center - screenPos) - (new Vector2(body.Width, body.Height / Main.npcFrameCount[npc.type]) * npc.scale / 2f) + (origin * npc.scale + new Vector2(0f, npc.gfxOffY) + drawOffset);
                    spriteBatch.Draw(body, textPos, npc.frame, colorOverride ?? npc.GetAlpha(drawColor), npc.rotation, origin, npc.scale, effects, 0f);

                    Color color = Color.Lerp(Color.White, Color.Purple, 0.5f) * npc.Opacity;
                    Color color2 = Color.Lerp(Color.White, Color.LightGreen, 0.5f) * npc.Opacity;
                    if (colorOverride.HasValue)
                    {
                        color = colorOverride.Value;
                        color2 = colorOverride.Value;
                    }
                    if (CalamityConfig.Instance.Afterimages)
                    {
                        for (int k = 1; k < 5; k++)
                        {
                            Color color3 = npc.GetAlpha(Color.Lerp(color, Color.White, 0.5f)) * ((5 - k) / 15f);
                            Color color4 = npc.GetAlpha(Color.Lerp(color2, Color.White, 0.5f)) * ((5 - k) / 15f);
                            if (colorOverride.HasValue)
                            {
                                color3 = colorOverride.Value;
                                color4 = colorOverride.Value;
                            }
                            Vector2 position3 = (npc.oldPos[k] + new Vector2(npc.width, npc.height) / 2f - screenPos) - (new Vector2(wings.Width, wings.Height / Main.npcFrameCount[npc.type]) * npc.scale / 2f) + (origin * npc.scale + new Vector2(0f, npc.gfxOffY) + drawOffset);
                            spriteBatch.Draw(wings, position3, npc.frame, color3, npc.rotation, origin, npc.scale, effects, 0f);
                            spriteBatch.Draw(spikes, position3, npc.frame, color4, npc.rotation, origin, npc.scale, effects, 0f);
                        }
                    }
                    spriteBatch.Draw(wings, textPos, npc.frame, color, npc.rotation, origin, npc.scale, effects, 0f);
                    spriteBatch.Draw(spikes, textPos, npc.frame, color2, npc.rotation, origin, npc.scale, effects, 0f);
                }
            }
        }
        public override bool PreKill(NPC npc)
        {
            if (!CalamityMod.DownedBossSystem.downedRavager && npc.type == ModContent.NPCType<RavagerBody>())
            {
                CalamityUtils.SpawnOre(ModContent.TileType<LifeOreTile>(), 0.25E-05, 0.45f, 0.65f, 30, 40);

                Color messageColor = Color.Lime;
                CalamityUtils.DisplayLocalizedText("Vitality sprawls throughout the underground.", messageColor);
            }
            if (npc.type == NPCID.WallofFlesh && !Main.hardMode)
            {
                CalRemixWorld.ShrineTimer = 3000;
            }
            return true;
        }
        public override void ResetEffects(NPC npc)
        {
            vBurn = false;
        }
        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (vBurn)
            {
                if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
                }
                npc.lifeRegen -= 200;
                if (damage < 40)
                {
                    damage = 40;
                }
            }
        }
        public override void HitEffect(NPC npc, NPC.HitInfo hit)
        {
            if (npc.type == ModContent.NPCType<Crabulon>() && say <= 0)
            {
                Talk("No? Please do be careful with that weapon, though, it looks kinda dangerous. Honestly, you seem quite... crabby. Get it?!", Color.LightSkyBlue);
                say = 1;
            }
            if (npc.type == ModContent.NPCType<Crabulon>() && npc.life <= 0 && say == 3)
            {
                Talk("AAAAAAAAAh", Color.LightSkyBlue);
                say = 4;
            }

        }
        public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
        {
            if (vBurn)
            {
                modifiers.SourceDamage *= 0.95f;
            }
        }
        private static void Talk(string text, Color color)
        {
            if (Main.netMode == NetmodeID.Server)
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(text), color);
            else if (Main.netMode == NetmodeID.SinglePlayer)
                Main.NewText(text, color);
        }
    }
}
