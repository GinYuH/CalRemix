using Terraria;
using Terraria.ModLoader;
using CalamityMod.NPCs.TownNPCs;
using CalRemix.Items.Materials;
using CalRemix.Tiles;
using Microsoft.Xna.Framework;
using CalamityMod.NPCs.DesertScourge;
using CalamityMod;
using CalamityMod.NPCs;
using CalamityMod.NPCs.Abyss;
using CalamityMod.NPCs.AcidRain;
using CalamityMod.NPCs.AdultEidolonWyrm;
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
using CalRemix.Projectiles;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework.Graphics;
using CalamityMod.NPCs.Providence;
using CalamityMod.Events;
using System;
using Terraria.GameContent;
using System.IO;
using CalamityMod.NPCs.DevourerofGods;
using CalRemix.Items.Placeables;

namespace CalRemix
{
    public class CalRemixGlobalNPC : GlobalNPC
    {
        bool SlimeBoost = false;
        public int bossKillcount = 0;
        public float shadowHit = 1;
        private bool useDefenseFrames;
        private int frameUsed;
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
            ModContent.NPCType<EbonianSlimeGod>(),
            ModContent.NPCType<CrimulanSlimeGod>(),
            ModContent.NPCType<SplitEbonianSlimeGod>(),
            ModContent.NPCType<SplitCrimulanSlimeGod>(),
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
            ModContent.NPCType<CalamityMod.NPCs.Crags.CharredSlime>(),
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

        public override bool PreAI(NPC npc)
        {
            if (SlimeBoost && !Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().assortegel && !Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().amalgel && !Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().godfather)
            {
                npc.active = false;
                return false;
            }
            if (Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().godfather && !CalamityMod.Events.BossRushEvent.BossRushActive)
            {
                if ((Slimes.Contains(npc.type) || BossSlimes.Contains(npc.type)))
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
            else
            {
                if (Slimes.Contains(npc.type) && (Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().assortegel || Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().amalgel))
                {
                    if (!npc.GetGlobalNPC<CalRemixGlobalNPC>().SlimeBoost)
                    {
                        if (Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().amalgel)
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
                        if (target.immune[npc.whoAmI] == 0 && thisrect.Intersects(theirrect) && target.whoAmI != npc.whoAmI && npc.active && target.active && !target.dontTakeDamage && !Slimes.Contains(target.type))
                        {
                            if (BossSlimes.Contains(target.type) && Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().amalgel)
                            {

                            }
                            else
                            {
                                target.StrikeNPC(npc.damage, 0, 0);
                                target.immune[npc.whoAmI] = 10;
                                if (target.damage > 0)
                                    npc.StrikeNPC(target.damage, 0, 0);
                            }
                        }

                    }
                }
                if (BossSlimes.Contains(npc.type) && Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().amalgel && !Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().assortegel && !CalamityMod.Events.BossRushEvent.BossRushActive)
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
                        if (target.immune[npc.whoAmI] == 0 && thisrect.Intersects(theirrect) && target.whoAmI != npc.whoAmI && npc.active && target.active && !target.dontTakeDamage && !Slimes.Contains(target.type) && !BossSlimes.Contains(target.type))
                        {
                            target.StrikeNPC(npc.damage, 0, 0);
                            target.immune[npc.whoAmI] = 10;
                            if (target.damage > 0)
                                npc.StrikeNPC(target.damage, 0, 0);
                        }

                    }
                    return false;
                }
                else if (BossSlimes.Contains(npc.type) && Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().assortegel && !Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().amalgel && !CalamityMod.Events.BossRushEvent.BossRushActive)
                {
                    npc.damage = 0;
                    if (npc.type == ModContent.NPCType<SlimeGodCore>() && NPC.CountNPCS(ModContent.NPCType<CrimulanSlimeGod>()) < 1
                        && NPC.CountNPCS(ModContent.NPCType<SplitCrimulanSlimeGod>()) < 1
                        && NPC.CountNPCS(ModContent.NPCType<EbonianSlimeGod>()) < 1
                        && NPC.CountNPCS(ModContent.NPCType<SplitEbonianSlimeGod>()) < 1)
                    {
                        return true;
                    }
                    return false;
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
            if (npc.type == ModContent.NPCType<Bumblefuck>() && Main.LocalPlayer.ZoneDesert)
            {
                npc.localAI[1] = 0;
            }
            if (npc.type == ModContent.NPCType<AureusSpawn>() && modPlayer.nuclegel || modPlayer.assortegel && !CalamityMod.Events.BossRushEvent.BossRushActive)
            {
                npc.active = false;
            }
            if (npc.type == ModContent.NPCType<WulfrumAmplifier>())
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC exc = Main.npc[i];
                    if (npc.Distance(exc.Center) <= npc.ai[1] && exc.ModNPC is WulfrumExcavatorHead)
                    {
                        exc.ModNPC<WulfrumExcavatorHead>().PylonCharged = true;
                    }
                }
            }
        }
        public override void PostAI(NPC npc)
        {
            if (!CalamityMod.CalPlayer.CalamityPlayer.areThereAnyDamnBosses && !CalamityLists.enemyImmunityList.Contains(npc.type))
            {
                if (npc.GetGlobalNPC<CalamityMod.NPCs.CalamityGlobalNPC>().pearlAura > 0)
                    npc.AddBuff(ModContent.BuffType<CalamityMod.Buffs.StatDebuffs.ExoFreeze>(), 60);
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
            else if (npc.type == ModContent.NPCType<Bumblefuck>())
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
            }
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
                //npcLoot.Remove(npcLoot.DefineNormalOnlyDropSet().Add(DropHelper.PerPlayer(ModContent.ItemType<PearlShard>(), 1, 25, 30)));
            }
            else if (npc.type == ModContent.NPCType<Bumblefuck>())
            {
                npcLoot.Add(ModContent.ItemType<DesertFeather>(), 11, 17, 34);
                //npcLoot.Remove(npcLoot.DefineNormalOnlyDropSet().Add(ModContent.ItemType<EffulgentFeather>(), 1, 25, 30));
            }
            else if (npc.type == ModContent.NPCType<AdultEidolonWyrmHead>())
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
            else if (npc.type == ModContent.NPCType<CosmicElemental>())
            {
                npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<CosmicStone>(), 20, 10));
            }
            else if (npc.type == ModContent.NPCType<Horse>())
            {
                npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<RockStone>(), 5, 3));
            }
            if (npc.boss && bossKillcount > 5)
            {
                //npcLoot.Add(ModContent.ItemType<PearlShard>(), 1, 2, 4);
            }
            else if (!npc.SpawnedFromStatue && npc.value > 0 && NPC.killCount[npc.type] >= 25)
            {
                //npcLoot.Add(ModContent.ItemType<PearlShard>(), 5, 1, 1);
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
        }
        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            if (type == NPCID.Painter)
            {
                if (NPC.AnyNPCs(ModContent.NPCType<DevourerofGodsHead>()) && BossRushEvent.BossRushActive)
                {
                    shop.item[nextSlot].SetDefaults(ModContent.ItemType<TrialsEnd>());
                    ++nextSlot;
                }
            }
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
            binaryWriter.Write(useDefenseFrames);
            binaryWriter.Write(frameUsed);
           // binaryWriter.Write(DeathAnimationTimer);
            for (int i = 0; i < 4; i++)
            {
                binaryWriter.Write(npc.Calamity().newAI[i]);
            }
        }
        public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader)
        {
            useDefenseFrames = binaryReader.ReadBoolean();
            frameUsed = binaryReader.ReadInt32();
          //  DeathAnimationTimer = binaryReader.ReadInt32();
            for (int i = 0; i < 4; i++)
            {
                npc.Calamity().newAI[i] = binaryReader.ReadSingle();
            }
        }
        public override void FindFrame(NPC npc, int frameHeight)
        {
            if (!Main.dayTime)
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
            if (npc.type == ModContent.NPCType<Providence>() && !Main.dayTime)
            {
                int DeathAnimationTimer = ModContent.GetInstance<Providence>().DeathAnimationTimer;
                float lerpValue = Utils.GetLerpValue(0f, 45f, DeathAnimationTimer, clamped: true);
                int num = (int)MathHelper.Lerp(1f, 30f, lerpValue);
                for (int i = 0; i < num; i++)
                {
                    float num2 = MathF.PI * 2f * (float)i * 2f / (float)num;
                    float num3 = (float)Math.Sin(num2 * 6f + Main.GlobalTimeWrappedHourly * MathF.PI);
                    num3 *= (float)Math.Pow(lerpValue, 3.0) * 50f;
                    Vector2 drawOffset2 = num2.ToRotationVector2() * num3;
                    Color value = Color.White * (MathHelper.Lerp(0.4f, 0.8f, lerpValue) / (float)num * 1.5f);
                    value.A = 0;
                    value = Color.Lerp(Color.White, value, lerpValue);
                    drawProvidenceInstance(drawOffset2, (num == 1) ? null : new Color?(value));
                }
                void drawProvidenceInstance(Vector2 drawOffset, Color? colorOverride)
                {
                    bool bossRushActive = BossRushEvent.BossRushActive;
                    bool flag = !Main.dayTime || bossRushActive;
                    string text = "CalRemix/Resprites/Providence/";
                    string text2 = text + "Glowmasks/";
                    string text3 = text + "Providence";
                    string text4 = text2 + "ProvidenceGlow";
                    string text5 = text2 + "ProvidenceGlow2";
                    if (npc.ai[0] == 2f || npc.ai[0] == 5f)
                    {
                        if (useDefenseFrames)
                        {
                            text3 = text + "ProvidenceDefense";
                            text4 = text2 + "ProvidenceDefenseGlow";
                            text5 = text2 + "ProvidenceDefenseGlow2";
                        }
                        else
                        {
                            text3 = text + "ProvidenceDefenseAlt";
                            text4 = text2 + "ProvidenceDefenseAltGlow";
                            text5 = text2 + "ProvidenceDefenseAltGlow2";
                        }
                    }
                    else if (frameUsed == 0)
                    {
                        text4 = text2 + "ProvidenceGlow";
                        text5 = text2 + "ProvidenceGlow2";
                    }
                    else if (frameUsed == 1)
                    {
                        text3 = text + "ProvidenceAlt";
                        text4 = text2 + "ProvidenceAltGlow";
                        text5 = text2 + "ProvidenceAltGlow2";
                    }
                    else if (frameUsed == 2)
                    {
                        text3 = text + "ProvidenceAttack";
                        text4 = text2 + "ProvidenceAttackGlow";
                        text5 = text2 + "ProvidenceAttackGlow2";
                    }
                    else
                    {
                        text3 = text + "ProvidenceAttackAlt";
                        text4 = text2 + "ProvidenceAttackAltGlow";
                        text5 = text2 + "ProvidenceAttackAltGlow2";
                    }
                    if (flag)
                    {
                        text3 += "Night";
                        text4 += "Night";
                        text5 += "Night";
                    }
                    Texture2D value2 = ModContent.Request<Texture2D>(text3).Value;
                    Texture2D value3 = ModContent.Request<Texture2D>(text4).Value;
                    Texture2D value4 = ModContent.Request<Texture2D>(text5).Value;
                    SpriteEffects effects = SpriteEffects.None;
                    if (npc.spriteDirection == 1)
                    {
                        effects = SpriteEffects.FlipHorizontally;
                    }

                    Vector2 vector = new Vector2(TextureAssets.Npc[npc.type].Value.Width / 2, TextureAssets.Npc[npc.type].Value.Height / Main.npcFrameCount[npc.type] / 2);
                    Color white = Color.White;
                    float amount = 0.5f;
                    int num4 = 5;
                    Vector2 position2 = npc.Center - screenPos;
                    position2 -= new Vector2(value2.Width, value2.Height / Main.npcFrameCount[npc.type]) * npc.scale / 2f;
                    position2 += vector * npc.scale + new Vector2(0f, npc.gfxOffY) + drawOffset;
                    spriteBatch.Draw(value2, position2, npc.frame, colorOverride ?? npc.GetAlpha(drawColor), npc.rotation, vector, npc.scale, effects, 0f);
                    Color color = Color.Lerp(Color.White, flag ? Color.Purple : Color.Yellow, 0.5f) * npc.Opacity;
                    Color color2 = Color.Lerp(Color.White, flag ? Color.LightGreen : Color.Violet, 0.5f) * npc.Opacity;
                    if (colorOverride.HasValue)
                    {
                        color = colorOverride.Value;
                        color2 = colorOverride.Value;
                    }
                    if (CalamityConfig.Instance.Afterimages)
                    {
                        for (int k = 1; k < num4; k++)
                        {
                            Color value6 = color;
                            value6 = Color.Lerp(value6, white, amount);
                            value6 = npc.GetAlpha(value6);
                            value6 *= (float)(num4 - k) / 15f;
                            if (colorOverride.HasValue)
                            {
                                value6 = colorOverride.Value;
                            }

                            Vector2 position3 = npc.oldPos[k] + new Vector2(npc.width, npc.height) / 2f - screenPos;
                            position3 -= new Vector2(value3.Width, value3.Height / Main.npcFrameCount[npc.type]) * npc.scale / 2f;
                            position3 += vector * npc.scale + new Vector2(0f, npc.gfxOffY) + drawOffset;
                            spriteBatch.Draw(value3, position3, npc.frame, value6, npc.rotation, vector, npc.scale, effects, 0f);
                            Color value7 = color2;
                            value7 = Color.Lerp(value7, white, amount);
                            value7 = npc.GetAlpha(value7);
                            value7 *= (float)(num4 - k) / 15f;
                            if (colorOverride.HasValue)
                            {
                                value7 = colorOverride.Value;
                            }

                            spriteBatch.Draw(value4, position3, npc.frame, value7, npc.rotation, vector, npc.scale, effects, 0f);
                        }
                    }
                    spriteBatch.Draw(value3, position2, npc.frame, color, npc.rotation, vector, npc.scale, effects, 0f);
                    spriteBatch.Draw(value4, position2, npc.frame, color2, npc.rotation, vector, npc.scale, effects, 0f);
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
    }
}
