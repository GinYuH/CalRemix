using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using CalRemix.Core.Biomes;
using CalamityMod;
using CalRemix.Content.Items.Materials;
using CalamityMod.Tiles.Ores;
using CalRemix.Core.Subworlds;
using CalamityMod.Projectiles.Typeless;
using CalRemix.Content.Items.Weapons;
using System.IO;

namespace CalRemix.Content.NPCs.Subworlds.Sealed
{
    public class ObliteratorHead : ModNPC
    {
        public ref float SegmentType => ref NPC.Calamity().newAI[3];
        public ref float Timer => ref NPC.Calamity().newAI[2];

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 2;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 54;
            NPC.height = 86;
            NPC.LifeMaxNERB(100000, 200000, 150000);
            NPC.damage = 11;
            NPC.defense = 90;
            NPC.noGravity = true;
            NPC.HitSound = AuricOre.MineSound;
            NPC.DeathSound = BetterSoundID.ItemElectricFizzleExplosion;
            NPC.noTileCollide = true;
            NPC.Calamity().canBreakPlayerDefense = true;
            NPC.DR_NERD(0.1f);
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            AIType = -1;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(0, 10, 0, 0);
            NPC.boss = true;
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToSickness = true;
            NPC.Calamity().VulnerableToWater = true;
            NPC.Calamity().VulnerableToElectricity = true;
            NPC.Calamity().VulnerableToCold = true;
            NPC.dontTakeDamage = true;
            SpawnModBiomes = new[] { ModContent.GetInstance<SealedUndergroundBiome>().Type };
        }


        public override void ReceiveExtraAI(BinaryReader reader)
        {
            NPC.Calamity().newAI[1] = reader.ReadSingle();
            NPC.Calamity().newAI[2] = reader.ReadSingle();
            NPC.Calamity().newAI[3] = reader.ReadSingle();
            NPC.Calamity().newAI[0] = reader.ReadSingle();
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(NPC.Calamity().newAI[1]);
            writer.Write(NPC.Calamity().newAI[2]);
            writer.Write(NPC.Calamity().newAI[3]);
            writer.Write(NPC.Calamity().newAI[0]);
        }

        public override void AI()
        {
            NPC.TargetClosest();
            if (SegmentType == 0)
            {
                if (NPC.ai[0] == 0)
                {
                    NPC.ai[3] = NPC.whoAmI;
                    NPC.realLife = NPC.whoAmI;
                    int num4 = 0;
                    int num5 = NPC.whoAmI;
                    int ct = 50;
                    for (int m = 0; m < ct; m++)
                    {
                        num4 = CalRemixHelper.SpawnNewNPC(NPC.GetSource_FromThis(), (int)(NPC.position.X + (float)(NPC.width / 2)), (int)(NPC.position.Y + (float)NPC.height), Type, NPC.whoAmI, npcTasks: (NPC n) =>
                        {
                            n.ai[3] = NPC.whoAmI;
                            n.realLife = NPC.whoAmI;
                            n.ai[1] = num5;
                            n.npcSlots = 0;
                            n.dontCountMe = true;
                            Main.npc[num5].ai[0] = n.whoAmI;
                            n.width = n.width = 40;
                            n.boss = false;
                            n.Calamity().newAI[3] = 1;
                            n.Calamity().newAI[2] = NPC.Calamity().newAI[2];
                            if (m == ct - 1)
                            {
                                n.Calamity().newAI[3] = 2;
                            }
                        }).whoAmI;
                        num5 = num4;
                    }
                }
                CalRemixNPC.WormAI(NPC, 5, 0.12f, null, SealedSubworldData.TentCenter, segmentType: SegmentType == 0 ? 0 : 1, canFlyByDefault: true);
            }
            else
            {
                CalRemixNPC.WormAI(NPC, 5, 0.12f, null, SealedSubworldData.TentCenter, segmentType: 1, canFlyByDefault: true);
            }
            if (SegmentType == 0)
            {
                if (NPC.Distance(SealedSubworldData.TentCenter) < 50)
                {
                    NPC.Calamity().newAI[1] = 1;
                }
                if (NPC.Calamity().newAI[1] == 1)
                {
                    NPC.velocity *= 0.95f;
                    Timer++;
                    if (Timer == 90)
                    {
                        foreach (NPC n in Main.ActiveNPCs)
                        {
                            if (n.type == Type)
                            {
                                n.Calamity().newAI[0] = 1;
                                NPC.netUpdate = true;
                                n.netUpdate = true;
                            }
                        }
                    }
                    if (Timer > 150)
                    {
                        NPC.dontTakeDamage = false;
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<DirectStrike>(), 999999, 1, Main.player[NPC.target].whoAmI, NPC.whoAmI);
                        }
                    }
                    foreach (NPC n in Main.ActiveNPCs)
                    {
                        if (n.type == Type)
                        {
                            n.Calamity().newAI[2] = Timer;
                        }
                    }
                }
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.IsABestiaryIconDummy)
                return true;

            Texture2D texture = SegmentType switch
            {
                1 => ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Subworlds/Sealed/ObliteratorBody").Value,
                2 => ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Subworlds/Sealed/ObliteratorTail").Value,
                _ => ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Subworlds/Sealed/ObliteratorHead").Value
            };

            float shakeStrength = MathHelper.Lerp(2, 4, Utils.GetLerpValue(0, 90, Timer, true));
            Main.EntitySpriteDraw(texture, NPC.Center - screenPos + Main.rand.NextVector2Circular(shakeStrength, shakeStrength), texture.Frame(1, 2, 0, (int)NPC.Calamity().newAI[0]), NPC.GetAlpha(drawColor), NPC.rotation - MathHelper.PiOver2, new Vector2(texture.Width / 2, texture.Height / 4), NPC.scale, SpriteEffects.None);
            return false;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Electric, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Electric, hit.HitDirection, -1f, 0, default, 1f);
                }
                string seg = SegmentType == 0 ? "Head" : SegmentType == 1 ? "Body" : "Tail";
                Gore.NewGoreDirect(NPC.GetSource_Death(), NPC.Center, Main.rand.NextVector2Circular(5, 5), Mod.Find<ModGore>("Obliterator" + seg).Type);
            }
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position) => SegmentType == 0;

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<HeavenReaper>());
            npcLoot.Add(ModContent.ItemType<NauseatingPowder>(), 1, 8, 15);
        }

        public override bool CheckActive()
        {
            bool alive = false;
            foreach (Player p in Main.ActivePlayers)
            {
                if (!p.dead)
                {
                    alive = true;
                }
            }
            return !alive;
        }

        public override void BossLoot(ref int potionType)
        {
            potionType = ItemID.SuperHealingPotion;
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return false;
        }
    }
}
