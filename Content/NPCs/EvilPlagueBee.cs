using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Buffs.DamageOverTime;
using System;
using Terraria.DataStructures;
using Terraria.Enums;
using Microsoft.Xna.Framework;

namespace CalRemix.Content.NPCs
{
    public class EvilPlagueBee : ModNPC
    {
        public Player Target => Main.player[NPC.target];
        public ref float Death => ref NPC.ai[2];
        public override void SetStaticDefaults()
        {
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][ModContent.BuffType<PearlAura>()] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][ModContent.BuffType<Plague>()] = true;
            Main.npcFrameCount[Type] = 4;
        }
        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.Bee);
            AIType = NPCID.Bee;
            AnimationType = NPCID.Bee;
            NPC.lifeMax = 100;
            NPC.damage = 100;
            NPC.defense = 0;
            NPC.knockBackResist = 0f;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath4;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] 
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle,
		        new FlavorTextBestiaryInfoElement("This bee has been overtaken by the Plague to find more hosts to infect.")
            });
        }
        public override void AI()
        {
            Death++;
            if (Death >= 900)
                NPC.life = 0;
            NPC.TargetClosest();
            NPCAimedTarget targetData = NPC.GetTargetData();
            bool flag = false;
            if (targetData.Type == NPCTargetType.Player)
            {
                flag = Target.dead;
            }
            float num = 6f;
            float num2 = 0.05f;
            NPC.ai[1] += 1f;
            float num3 = (NPC.ai[1] - 60f) / 60f;
            if (num3 > 1f)
            {
                num3 = 1f;
            }
            else
            {
                if (NPC.velocity.X > 6f)
                {
                    NPC.velocity.X = 6f;
                }
                if (NPC.velocity.X < -6f)
                {
                    NPC.velocity.X = -6f;
                }
                if (NPC.velocity.Y > 6f)
                {
                    NPC.velocity.Y = 6f;
                }
                if (NPC.velocity.Y < -6f)
                {
                    NPC.velocity.Y = -6f;
                }
            }
            num = 5f;
            num2 = 0.1f;
            num2 *= num3;
            Vector2 vector = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
            float num4 = targetData.Position.X + (float)(targetData.Width / 2);
            float num5 = targetData.Position.Y + (float)(targetData.Height / 2);
            num4 = (int)(num4 / 8f) * 8;
            num5 = (int)(num5 / 8f) * 8;
            vector.X = (int)(vector.X / 8f) * 8;
            vector.Y = (int)(vector.Y / 8f) * 8;
            num4 -= vector.X;
            num5 -= vector.Y;
            float num6 = (float)Math.Sqrt(num4 * num4 + num5 * num5);
            float num7 = num6;
            bool flag2 = false;
            if (num6 > 600f)
            {
                flag2 = true;
            }
            if (num6 == 0f)
            {
                num4 = NPC.velocity.X;
                num5 = NPC.velocity.Y;
            }
            else
            {
                num6 = num / num6;
                num4 *= num6;
                num5 *= num6;
            }
            NPC.ai[0] += 1f;
            if (NPC.ai[0] > 0f)
            {
                NPC.velocity.Y += 0.023f;
            }
            else
            {
                NPC.velocity.Y -= 0.023f;
            }
            if (NPC.ai[0] < -100f || NPC.ai[0] > 100f)
            {
                NPC.velocity.X += 0.023f;
            }
            else
            {
                NPC.velocity.X -= 0.023f;
            }
            if (NPC.ai[0] > 200f)
            {
                NPC.ai[0] = -200f;
            }
            if (flag)
            {
                num4 = (float)NPC.direction * num / 2f;
                num5 = (0f - num) / 2f;
            }
            if (NPC.velocity.X < num4)
            {
                NPC.velocity.X += num2;
            }
            else if (NPC.velocity.X > num4)
            {
                NPC.velocity.X -= num2;
            }
            if (NPC.velocity.Y < num5)
            {
                NPC.velocity.Y += num2;
            }
            else if (NPC.velocity.Y > num5)
            {
                NPC.velocity.Y -= num2;
            }
            NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X) - 1.57f;
            float num12 = 0.7f;
            if (NPC.collideX)
            {
                NPC.netUpdate = true;
                NPC.velocity.X = NPC.oldVelocity.X * (0f - num12);
                if (NPC.direction == -1 && NPC.velocity.X > 0f && NPC.velocity.X < 2f)
                {
                    NPC.velocity.X = 2f;
                }
                if (NPC.direction == 1 && NPC.velocity.X < 0f && NPC.velocity.X > -2f)
                {
                    NPC.velocity.X = -2f;
                }
            }
            if (NPC.collideY)
            {
                NPC.netUpdate = true;
                NPC.velocity.Y = NPC.oldVelocity.Y * (0f - num12);
                if (NPC.velocity.Y > 0f && (double)NPC.velocity.Y < 1.5)
                {
                    NPC.velocity.Y = 2f;
                }
                if (NPC.velocity.Y < 0f && (double)NPC.velocity.Y > -1.5)
                {
                    NPC.velocity.Y = -2f;
                }
            }
            if (((NPC.velocity.X > 0f && NPC.oldVelocity.X < 0f) || (NPC.velocity.X < 0f && NPC.oldVelocity.X > 0f) || (NPC.velocity.Y > 0f && NPC.oldVelocity.Y < 0f) || (NPC.velocity.Y < 0f && NPC.oldVelocity.Y > 0f)) && !NPC.justHit)
            {
                NPC.netUpdate = true;
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<Plague>(), 360);
            NPC.life = 0;
            NPC.netUpdate = true;
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Dust dust = Dust.NewDustDirect(NPC.Center, 0, 0, DustID.GemEmerald);
                        dust.velocity = Vector2.One.RotatedByRandom(MathHelper.ToRadians(360));
                    }
                }
            }
        }
    }
}
