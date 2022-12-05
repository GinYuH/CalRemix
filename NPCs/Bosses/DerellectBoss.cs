using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using CalamityMod.Particles;
using Microsoft.Xna.Framework.Graphics;
using CalamityMod;
using static CalamityMod.World.CalamityWorld;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Armor.Vanity;
using CalamityMod.Items.LoreItems;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Mounts;
using CalamityMod.Items.Placeables.Furniture.BossRelics;
using CalamityMod.Items.Placeables.Furniture.Trophies;
using CalamityMod.Items.Potions;
using CalamityMod.Items.TreasureBags;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items.Weapons.DraedonsArsenal;
using CalRemix.Items;
using CalRemix.Projectiles;
using CalRemix.Items.Placeables;

namespace CalRemix.NPCs.Bosses
{
    [AutoloadBossHead]
    internal class DerellectBoss : ModNPC
    {
        public bool p2 = false;

        public Particle aura;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Derellect");
            Main.npcFrameCount[NPC.type] = 5;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
            NPCID.Sets.MustAlwaysDraw[NPC.type] = true;
            NPCID.Sets.BossBestiaryPriority.Add(Type);
        }

        public override void SetDefaults()
        {
            NPC.noGravity = true;
            NPC.lavaImmune = true;
            NPC.aiStyle = -1;
            NPC.LifeMaxNERB(42000, 55000);
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.Item14;
            NPC.knockBackResist = 0f;
            NPC.noTileCollide = true;
            NPC.width = 240;
            NPC.height = 200;
            NPC.boss = true;
            NPC.defense = 90;
            Music = MusicLoader.GetMusicSlot("CalRemix/Sounds/Music/signal interruption");
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                new FlavorTextBestiaryInfoElement("A mother computer designed as a simulacrum of a grotesque creature. Though scraped for parts, it still barely operates.")
            });
        }

        public override void AI()
        {
            if (Main.getGoodWorld)
            {
                NPC.scale = 1.75f;
            }
            //Boss zen
            Main.player[Main.myPlayer].Calamity().isNearbyBoss = true;
            Main.player[Main.myPlayer].AddBuff(ModContent.BuffType<CalamityMod.Buffs.StatBuffs.BossEffects>(), 10, true);
            //Handle transitioning to phase 2
            if (NPC.CountNPCS(ModContent.NPCType<SignalDrone>()) <= 0 && NPC.ai[0] > 1)
            {
                p2 = true;
            }

            switch (NPC.ai[0])
            {
                case 0: //spawn drones
                    {
                        NPC.ai[1]++;
                        if (NPC.ai[1] == 1)
                        {
                            for (int i = 0; i < 12; i++)
                            {
                                NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<SignalDrone>(), 0, NPC.whoAmI, i);
                            }
                        ChangePhase(1);
                        }
                    }
                    break;
                case 1:
                    {
                        NPC.ai[1]++;
                        if (NPC.ai[1] == 120)
                        {
                             
                        }
                    }
                    break;
            }
        }

       public void ChangePhase(int phasenum, bool reset1 = true, bool reset2 = true, bool reset3 = true)
        {
            NPC.ai[0] = phasenum;
            if (reset1)
            {
                NPC.ai[1] = 0;
            }
            if (reset2)
            {
                NPC.ai[2] = 0;
            }
            if (reset3)
            {
                NPC.Calamity().newAI[3] = 0;
            }
            NPC.ai[3] = 0;
            NPC.damage = 0;
            NPC.Calamity().canBreakPlayerDefense = false;
            NPC.TargetClosest();
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 0.15f;
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.IsABestiaryIconDummy)
            {
                SpriteEffects spriteEffects = SpriteEffects.None;
                if (NPC.spriteDirection == 1)
                    spriteEffects = SpriteEffects.FlipHorizontally;

                Texture2D texture = TextureAssets.Npc[NPC.type].Value;
                Texture2D glowmask = Request<Texture2D>("CalRemix/NPCs/Bosses/DerellectGlow").Value;
                Vector2 origin = new Vector2((float)(texture.Width / 2), (float)(texture.Height / Main.npcFrameCount[NPC.type] / 2));
                Color white = Color.White;
                float colorLerpAmt = 0.5f;
                int afterimageAmt = 7;

                Vector2 npcOffset = NPC.Center - screenPos;
                npcOffset -= new Vector2((float)texture.Width, (float)(texture.Height / Main.npcFrameCount[NPC.type])) * NPC.scale / 2f;
                npcOffset += origin * NPC.scale + new Vector2(0f, NPC.gfxOffY);
                spriteBatch.Draw(texture, npcOffset, NPC.frame, Color.White, NPC.rotation, origin, NPC.scale, spriteEffects, 0f);
                spriteBatch.Draw(glowmask, npcOffset, NPC.frame, Color.White, NPC.rotation, origin, NPC.scale, spriteEffects, 0f);
            }
            return true;
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.8f * bossLifeScale);
            NPC.damage = (int)(NPC.damage * NPC.GetExpertDamageMultiplier());
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.GreaterHealingPotion;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            LeadingConditionRule normalOnly = new LeadingConditionRule(new Conditions.NotExpert());
            npcLoot.Add(normalOnly);

            // Trophies
            npcLoot.Add(ModContent.ItemType<DerellectTrophyInv>());
            npcLoot.Add(ModContent.ItemType<DerellectMask>(), 3);

            // Lore item
            npcLoot.Add(ItemDropRule.ByCondition(DropHelper.If(() => !CalRemixWorld.downedDerellect), ModContent.ItemType<KnowledgeDerellect>()));
        }

        public override void OnKill()
        {
            CalRemixWorld.downedDerellect = true;

            if (NPC.life <= 0)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Derellect1").Type, 2f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Derellect2").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Derellect3").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Derellect4").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Derellect5").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Derellect6").Type, 2f);
                }
            }
        }

        public override bool CheckActive()
        {
            return false;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(p2);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            p2 = reader.ReadBoolean();
        }
    }
}