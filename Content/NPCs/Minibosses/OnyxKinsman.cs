using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalamityMod.World;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Items.Mounts;
using Terraria.GameContent.ItemDropRules;
using CalRemix.Content.Items.Accessories;
using System;
using CalRemix.Content.Projectiles.Hostile;
using Terraria.Audio;
using CalRemix.UI;
using System.Linq;
using CalRemix.Core.World;

namespace CalRemix.Content.NPCs.Minibosses
{
    public class OnyxKinsman : ModNPC
    {
        public Player Target => Main.player[NPC.target];
        public ref float Timer => ref NPC.ai[0];
        public ref float Attack => ref NPC.ai[1];
        public ref float State => ref NPC.ai[2];
        public ref float Eat => ref NPC.ai[3];
        private Vector2 initPos;
        public override void SetStaticDefaults()
        {
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][ModContent.BuffType<ArmorCrunch>()] = true;

            if (Main.dedServ)
                return;
            HelperMessage.New("KinsmenFight",
                "Quick! Get him! He's escaping with a one of a kind item!",
                "FannyAwooga",
                (ScreenHelperSceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == Type));
        }
        public override bool SpecialOnKill()
        {
            RemixDowned.downedOnyxKinsman = true;
            return false;
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 196;
            NPC.height = 174;
            NPC.lifeMax = 2000;
            NPC.damage = 20;
            if (CalamityWorld.death)
                NPC.damage = 46;
            else if (CalamityWorld.revenge)
                NPC.damage = 49;
            NPC.defense = 10;
            NPC.DR_NERD(0.9f);
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(gold: 1, silver: 20);
            NPC.lavaImmune = true;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,
		new FlavorTextBestiaryInfoElement("Husbando gone wild.")
            });
        }
        public override void OnSpawn(IEntitySource source)
        {
            initPos = NPC.position;
        }
        public override void AI()
        {
            NPC.TargetClosest();
            if (Vector2.Distance(Target.Center, NPC.Center) > 2000f && State < 1)
            {
                if (CalamityWorld.CavernLabCenter != Vector2.Zero)
                {
                    NPC.position = CalamityWorld.CavernLabCenter;
                    State = 1;
                }
                else
                {
                    NPC.position = initPos;
                    State = 2;
                }
                NPC.life = NPC.lifeMax;
                NPC.velocity.X = 0;
                NPC.Calamity().ShouldCloseHPBar = true;
            }
            if (State > 0)
            {
                if (State == 1)
                {
                    Eat++;
                    if (Eat > 60)
                    {
                        SoundEngine.PlaySound(SoundID.Item2 with { Pitch = -0.5f }, NPC.position);
                        SoundEngine.PlaySound(SoundID.NPCHit4, NPC.position);
                        Eat = 0;
                    }
                }
                return;
            }
            if (Main.netMode != NetmodeID.Server && !Target.dead && Target.active)
                Target.AddBuff(BuffID.NoBuilding, 2);
            // Dig
            if (NPC.velocity.X == 0f)
            {
                Timer++;
                if (Timer >= 120 && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int posX = (int)NPC.position.X / 16;
                    int posY = (int)NPC.position.Y / 16;
                    int num91 = 20;
                    bool flag4 = false;
                    while (!flag4)
                    {
                        int num93 = Main.rand.Next(posX - num91, posX + num91);
                        int num94 = Main.rand.Next(posY - num91, posY + num91);
                        for (int num95 = num94; num95 < posY + num91; num95++)
                        {
                            if ((num95 < posY - 4 || num95 > posY + 4 || num93 < posX - 4 || num93 > posX + 4) && (num95 < posY - 1 || num95 > posY + 1 || num93 < posX - 1 || num93 > posX + 1) && Main.tile[num93, num95].HasUnactuatedTile)
                            {
                                bool flag5 = true;
                                if (Main.tile[num93, num95 - 1].LiquidType == LiquidID.Lava || Main.tile[num93, num95 - 1].LiquidType == LiquidID.Shimmer)
                                    flag5 = false;
                                if (flag5 && Main.tileSolid[Main.tile[num93, num95].TileType] && !Collision.SolidTiles(num93 - 1, num93 + 1, num95 - 4, num95 - 1))
                                {
                                    NPC.position.X = num93 * 16f - (float)(NPC.width / 2) + 8f;
                                    NPC.position.Y = num95 * 16f - (float)NPC.height;
                                    flag4 = true;
                                    break;
                                }
                            }
                        }
                    }
                    Timer = 0;
                    SoundEngine.PlaySound(SoundID.NPCDeath14, NPC.position);
                }
            }
            else
                Timer = 0;
            if (Math.Abs(NPC.velocity.X) < 10)
                NPC.velocity.X = (Target.Center.X > NPC.Center.X) ? NPC.velocity.X - 0.22f : NPC.velocity.X + 0.22f;
            Attack++;
            if (Attack >= 150f && Main.netMode != NetmodeID.Server)
            {
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.DirectionTo(Target.Center) * 5f, ModContent.ProjectileType<OnyxBullet>(), 1, 0);
                for (int i = 0; i < 3; i++)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.DirectionTo(Target.Center).RotatedByRandom(MathHelper.ToRadians(30)) * 5f, ModContent.ProjectileType<OnyxBullet>(), 1, 0);
                }
                Attack = 0;
            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemID.MudBlock, 1, 30, 30);
            npcLoot.Add(ItemID.StoneBlock, 1, 7, 7);
            npcLoot.Add(ItemID.Cog, 1, 15, 15);
            npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<RockStone>(), 20, 10));
            npcLoot.Add(ModContent.ItemType<OnyxExcavatorKey>());
        }
        public override void DrawEffects(ref Color drawColor)
        {
            if (Main.rand.NextBool(6) && NPC.velocity.X != 0)
            {
                Dust dust = Dust.NewDustDirect(new Vector2(NPC.position.X + 10f, NPC.position.Y + NPC.height - 2f), NPC.width, 0, DustID.Smoke, -NPC.velocity.X, 0f);
                dust.noGravity = true;
            }
            if (Eat >= 60 && State > 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    float x = (NPC.direction == -1) ? -36 : 36;
                    Dust.NewDustDirect(NPC.Center + new Vector2(x, -16), 1, 1, DustID.Web, Main.rand.Next(-10, 11), Main.rand.Next(2, 7));
                }
            }
            if (Timer >= 119)
            {
                for (int i = 0; i < 10; i++)
                {
                    Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Smoke, Main.rand.Next(-5, 6), Main.rand.Next(-9, -4));
                }
            }
            NPC.spriteDirection = (State > 0) ? NPC.direction : -NPC.direction;
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (State > 0)
                State = 0;
        }
        public override bool CheckActive()
        {
            return false;
        }
        public override bool NeedSaving()
        {
            return true;
        }
    }
}
