using CalamityMod.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Content.NPCs.Bosses.RajahBoss
{
    public class BunnySummon1 : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Bunny Summon");
        }

        public override void SetDefaults()
        {
            Projectile.width = 98;
            Projectile.height = 98;
            Projectile.penetrate = -1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 255;
        }

        public override void AI()
        {
            for (int num468 = 0; num468 < 10; num468++)
            {
                int num469 = Dust.NewDust(Projectile.Center, 0, 0, ModContent.DustType<ShadowspecBarDust>(), 0f, 0f, 0, NPC.AnyNPCs(ModContent.NPCType<SupremeRajah>()) ? Main.DiscoColor : new Color(107, 137, 179), 1f);
                Main.dust[num469].noGravity = true;
            }
            Projectile.damage = 0;
            Projectile.knockBack = 0;
            Move(new Vector2(Projectile.ai[0], Projectile.ai[1]));
            if (Vector2.Distance(Projectile.Center, new Vector2(Projectile.ai[0], Projectile.ai[1])) < 10)
            {
                OnKill(Projectile.timeLeft);
            }
        }

        public override void OnKill(int timeLeft)
        {
            int MinionType = ModContent.NPCType<RabbitcopterSoldier>();
            if (NPC.AnyNPCs(ModContent.NPCType<SupremeRajah>()))
            {
                MinionType = ModContent.NPCType<RabbitcopterSoldier2>();
            }


            int Minion = NPC.NewNPC(Projectile.GetSource_FromThis(), (int)Projectile.Center.X, (int)Projectile.Center.Y, MinionType, 0);
            Main.npc[Minion].netUpdate2 = true;
            Projectile.active = false;
            Projectile.netUpdate2 = true;
        }

        public void Move(Vector2 point)
        {
            float Speed = 13;

            float velMultiplier = 1f;
            Vector2 dist = point - Projectile.Center;
            float length = dist == Vector2.Zero ? 0f : dist.Length();
            if (length < Speed)
            {
                velMultiplier = MathHelper.Lerp(0f, 1f, length / Speed);
            }
            if (length < 200f)
            {
                Speed *= 0.5f;
            }
            if (length < 100f)
            {
                Speed *= 0.5f;
            }
            if (length < 50f)
            {
                Speed *= 0.5f;
            }
            Projectile.velocity = length == 0f ? Vector2.Zero : Vector2.Normalize(dist);
            Projectile.velocity *= Speed;
            Projectile.velocity *= velMultiplier;
        }
    }

    public class BunnySummon2 : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Bunny Summon");
        }

        public override void SetDefaults()
        {
            Projectile.width = 98;
            Projectile.height = 98;
            Projectile.penetrate = -1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 255;
        }

        public override void AI()
        {
            for (int num468 = 0; num468 < 10; num468++)
            {
                int num469 = Dust.NewDust(Projectile.Center, 0, 0, ModContent.DustType<ShadowspecBarDust>(), 0f, 0f, 0, NPC.AnyNPCs(ModContent.NPCType<SupremeRajah>()) ? Main.DiscoColor : new Color(107, 137, 179), 1f);
                Main.dust[num469].noGravity = true;
            }
            Projectile.damage = 0;
            Projectile.knockBack = 0;
            Move(new Vector2(Projectile.ai[0], Projectile.ai[1]));
            if (Vector2.Distance(Projectile.Center, new Vector2(Projectile.ai[0], Projectile.ai[1])) < 10)
            {
                OnKill(Projectile.timeLeft);
            }
        }

        public override void OnKill(int timeLeft)
        {
            int MinionType = ModContent.NPCType<BunnyBrawler>();
            if (NPC.AnyNPCs(ModContent.NPCType<SupremeRajah>()))
            {
                MinionType = ModContent.NPCType<BunnyBrawler2>();
            }


            int Minion = NPC.NewNPC(Projectile.GetSource_FromThis(), (int)Projectile.Center.X, (int)Projectile.Center.Y, MinionType, 0);
            Main.npc[Minion].netUpdate2 = true;
            Projectile.active = false;
            Projectile.netUpdate2 = true;
        }

        public void Move(Vector2 point)
        {
            float Speed = 13;

            float velMultiplier = 1f;
            Vector2 dist = point - Projectile.Center;
            float length = dist == Vector2.Zero ? 0f : dist.Length();
            if (length < Speed)
            {
                velMultiplier = MathHelper.Lerp(0f, 1f, length / Speed);
            }
            if (length < 200f)
            {
                Speed *= 0.5f;
            }
            if (length < 100f)
            {
                Speed *= 0.5f;
            }
            if (length < 50f)
            {
                Speed *= 0.5f;
            }
            Projectile.velocity = length == 0f ? Vector2.Zero : Vector2.Normalize(dist);
            Projectile.velocity *= Speed;
            Projectile.velocity *= velMultiplier;
        }
    }

    public class BunnySummon3 : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Bunny Summon");
        }

        public override void SetDefaults()
        {
            Projectile.width = 98;
            Projectile.height = 98;
            Projectile.penetrate = -1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 255;
        }

        public override void AI()
        {
            for (int num468 = 0; num468 < 10; num468++)
            {
                int num469 = Dust.NewDust(Projectile.Center, 0, 0, ModContent.DustType<ShadowspecBarDust>(), 0f, 0f, 0, NPC.AnyNPCs(ModContent.NPCType<SupremeRajah>()) ? Main.DiscoColor : new Color(107, 137, 179), 1f);
                Main.dust[num469].noGravity = true;
            }
            Projectile.damage = 0;
            Projectile.knockBack = 0;
            Move(new Vector2(Projectile.ai[0], Projectile.ai[1]));
            if (Vector2.Distance(Projectile.Center, new Vector2(Projectile.ai[0], Projectile.ai[1])) < 10)
            {
                OnKill(Projectile.timeLeft);
            }
        }

        public override void OnKill(int timeLeft)
        {
            int MinionType = ModContent.NPCType<BunnyBattler>();
            if (NPC.AnyNPCs(ModContent.NPCType<SupremeRajah>()))
            {
                MinionType = ModContent.NPCType<BunnyBattler2>();
            }


            int Minion = NPC.NewNPC(Projectile.GetSource_FromThis(), (int)Projectile.Center.X, (int)Projectile.Center.Y, MinionType, 0);
            Main.npc[Minion].netUpdate2 = true;
            Projectile.active = false;
            Projectile.netUpdate2 = true;
        }

        public void Move(Vector2 point)
        {
            float Speed = 13;

            float velMultiplier = 1f;
            Vector2 dist = point - Projectile.Center;
            float length = dist == Vector2.Zero ? 0f : dist.Length();
            if (length < Speed)
            {
                velMultiplier = MathHelper.Lerp(0f, 1f, length / Speed);
            }
            if (length < 200f)
            {
                Speed *= 0.5f;
            }
            if (length < 100f)
            {
                Speed *= 0.5f;
            }
            if (length < 50f)
            {
                Speed *= 0.5f;
            }
            Projectile.velocity = length == 0f ? Vector2.Zero : Vector2.Normalize(dist);
            Projectile.velocity *= Speed;
            Projectile.velocity *= velMultiplier;
        }
    }
}
