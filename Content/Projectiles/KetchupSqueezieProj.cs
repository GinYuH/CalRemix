using CalRemix.Content.Items.Tools;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles
{
    public class KetchupSqueezieProj : ModProjectile
    {
        public override string Texture => "CalRemix/Content/Items/Tools/KetchupSqueezie";

        public Player Owner => Main.player[Projectile.owner];

        public override void SetDefaults()
        {
            Projectile.width = 42;
            Projectile.height = 50;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
        }


        public override void AI()
        {
            int totalTime = 60;
            if (!Owner.active || Owner.dead || Owner.HeldItem.type != ModContent.ItemType<KetchupSqueezie>())
            {
                Projectile.Kill();
                return;
            }
            Projectile.timeLeft = 2;
            if (Main.myPlayer == Owner.whoAmI)
            {
                Projectile.Center = Owner.Center + Owner.DirectionTo(Main.MouseWorld) * 16f;
                Projectile.spriteDirection = (Main.MouseWorld.X < Owner.Center.X) ? -1 : 1;
                Owner.ChangeDir(Projectile.spriteDirection);
                Projectile.netUpdate = true;
                Owner.heldProj = Projectile.whoAmI;
                Projectile.rotation = Projectile.DirectionTo(Main.MouseWorld).ToRotation();
                Projectile.rotation += Projectile.spriteDirection == -1 ? MathHelper.Pi : 0;
                Projectile.rotation += Projectile.spriteDirection * MathHelper.PiOver4;
                int gate = 22;
                float extraOomf = MathHelper.Lerp(0.8f * Projectile.spriteDirection, 0f, Utils.GetLerpValue(totalTime, gate, Projectile.ai[0], true));
                if (Projectile.ai[0] < gate)
                {
                    extraOomf = MathHelper.Lerp(0f, 0.8f * Projectile.spriteDirection, Utils.GetLerpValue(gate, 0, Projectile.ai[0], true));
                }
                Owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Owner.HandPosition.Value.DirectionTo(Projectile.Center).ToRotation() + (Projectile.spriteDirection == -1 ? MathHelper.Pi + 1.4f : -1.4f) + extraOomf);
                Owner.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, Owner.HandPosition.Value.DirectionTo(Projectile.Center).ToRotation() + (Projectile.spriteDirection == -1 ? MathHelper.Pi + 1.4f : -1.4f) + extraOomf);
            }
            Owner.AddBuff(BuffID.WellFed3, totalTime);
            if (Owner.controlUseItem)
            {
                if (Projectile.ai[0] <= 0)
                {
                    Projectile.ai[0] = totalTime;
                }
                else
                {
                    Owner.SetDummyItemTime(2);
                }
            }
            Projectile.ai[0]--;
            if (Projectile.ai[0] < 0)
                Projectile.ai[0] = 0;

            if (Projectile.ai[0] % 5 == 0 && Projectile.ai[0] > 0)
            {
                SoundEngine.PlaySound(BetterSoundID.ItemPoopSquish with { Pitch = 0.8f }, Projectile.Center);
                int projCount = Main.rand.Next(3, 7);
                int randomness = 5;
                int speed = 11;
                for (int i = 0; i < projCount * 5; i++)
                {
                    Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.Blood, Projectile.DirectionTo(Main.MouseWorld) * speed * 0.75f + Main.rand.NextVector2Circular(randomness, randomness), Scale: Main.rand.NextFloat(1.5f, 3f));
                    d.noGravity = true;
                }
            }
            if (Projectile.ai[0] > 0)
            {
                Owner.velocity = Main.MouseWorld.DirectionTo(Owner.Center) * 22;
            }

        }
        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }
    }
}