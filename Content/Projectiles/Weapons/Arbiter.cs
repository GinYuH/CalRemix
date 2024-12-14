using CalamityMod;
using CalamityMod.Buffs.StatDebuffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class Arbiter : ModProjectile
    {
        public Player Owner => Main.player[Projectile.owner];
        public ref float Attack => ref Projectile.ai[0];
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Arbiter of Judgement");
            ProjectileID.Sets.MinionTargettingFeature[Type] = true;
            Main.projFrames[Type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.width = 110;
            Projectile.height = 88;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 18000;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft *= 5;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.minion = true;
            Projectile.minionSlots = 0;
            Projectile.scale = 0.65f;
            Projectile.aiStyle = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }
        public override void OnSpawn(IEntitySource source)
        {
            if (Projectile.owner == Main.myPlayer)
                SoundEngine.PlaySound(SoundID.DD2_BetsySummon);
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width + 2, Projectile.height + 2, DustID.Flare, Main.rand.Next(-3, 4), Main.rand.Next(-3, 4));
            }
        }
        public override void AI()
        {
            if (Projectile.type != ModContent.ProjectileType<Arbiter>())
                return;
            if (Owner.dead || Owner.HeldItem.DamageType != DamageClass.Summon && Owner.HeldItem.DamageType != DamageClass.SummonMeleeSpeed)
                Projectile.Kill();
            else
                Projectile.timeLeft = 2;

            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 4)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
            }
            if (Projectile.frame > 3)
            {
                Projectile.frame = 0;
            }
            NPC npc = (Projectile.OwnerMinionAttackTargetNPC is null) ? Projectile.FindTargetWithinRange(1200) : Projectile.OwnerMinionAttackTargetNPC;
            if (npc != null)
            {
                Projectile.direction = (Projectile.velocity.X < 0) ? -1 : 1;
                if (npc.CanBeChasedBy())
                {
                    Attack++;
                    if (Attack >= 60 || Projectile.Center.Distance(npc.Center) > 320)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            Dust.NewDust(Projectile.position, Projectile.width + 2, Projectile.height + 2, DustID.Flare, Main.rand.Next(-3, 4), Main.rand.Next(-3, 4));
                        }
                        float x = Main.rand.NextBool() ? Main.rand.NextFloat(-200, -180) : Main.rand.NextFloat(180, 200);
                        float y = Main.rand.NextBool() ? Main.rand.NextFloat(-200, -180) : Main.rand.NextFloat(180, 200);
                        Projectile.Center = npc.Center + new Vector2(x, y);
                        Projectile.velocity = Projectile.Center.DirectionTo(npc.Center) * 30f;
                        Attack = 0;
                    }
                }
            }
            else
            {
                Projectile.FloatingPetAI(Owner.direction < 0, 0);
            }
            Projectile.spriteDirection = -Projectile.direction;
        }
        public override void OnKill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
                SoundEngine.PlaySound(SoundID.DD2_BetsySummon);
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width + 2, Projectile.height + 2, DustID.Flare, Main.rand.Next(-3, 4), Main.rand.Next(-3, 4));
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<WhisperingDeath>(), 120);
            target.AddBuff(BuffID.OnFire3, 120);
        }
    }
}
