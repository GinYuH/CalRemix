using System;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class ChainSawProjectile : ModProjectile
	{
		// Define the range of the Spear Projectile. These are overrideable properties, in case you'll want to make a class inheriting from this one.
		protected virtual float HoldoutRangeMin => 24f;
		protected virtual float HoldoutRangeMax => 40f;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chain-Saw");
		}

		public override void SetDefaults()
		{
			Projectile.width = 26;
			base.Projectile.height = 26;
			base.Projectile.friendly = true;
			base.Projectile.penetrate = -1;
			base.Projectile.tileCollide = false;
			base.Projectile.hide = true;
			base.Projectile.ownerHitCheck = true;
			base.Projectile.DamageType = DamageClass.Melee;
			Projectile.aiStyle = 20;
            Projectile.usesLocalNPCImmunity= true;
            Projectile.localNPCHitCooldown = 5;
		}

        public override bool PreAI()
        {
            CalRemixPlayer pplayer = Main.player[Projectile.owner].GetModPlayer<CalRemixPlayer>();
            Projectile.CritChance = (int)Math.Pow(pplayer.chainSawCharge + 1, 2) / (8 * pplayer.chainSawChargeCritMax);
            return true;
        }
        public override void AI()
        {
            
            base.Projectile.frameCounter++;
            if ((float)base.Projectile.frameCounter % 4f == 3f)
            {
                base.Projectile.frame++;
                if (base.Projectile.frame >= Main.projFrames[base.Projectile.type])
                {
                    base.Projectile.frame = 0;
                }
            }
        }


        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            CalRemixPlayer pplayer = Main.player[Projectile.owner].GetModPlayer<CalRemixPlayer>();
            pplayer.chainSawCharge += 10;
            if (Main.rand.Next(1,100) <= Math.Pow(pplayer.chainSawCharge + 1, 2) / (8 * pplayer.chainSawChargeCritMax)) {
                hit.Crit = true;
            } else
            {
                hit.Crit = false;
            }
            if (hit.Crit)
            {
                pplayer.chainSawCharge += 10;
            }
            pplayer.chainSawHitCooldown = 60;
            if (pplayer.chainSawCharge > 30 * 20)
            {
                pplayer.chainSawCharge = 30 * 20;
            }
        }

    }

        /*public override void AI()
		{
			Player player = Main.player[base.Projectile.owner];
			player.heldProj = Projectile.whoAmI;
			Projectile.velocity = (Main.MouseWorld - player.MountedCenter).SafeNormalize(Vector2.One);
            if (!player.channel)
			{
				base.Projectile.Kill();
				base.Projectile.active = false;
				return;
			}
			Projectile.timeLeft = 2;
            Projectile.spriteDirection = Projectile.direction = (Projectile.velocity.X > 0).ToDirectionInt();
            Projectile.rotation = Projectile.velocity.ToRotation() + (Projectile.spriteDirection == 1 ? 0f : MathHelper.Pi);
            if (Projectile.spriteDirection == 1)
            {
                DrawOffsetX = 68 - 26;
                DrawOriginOffsetX = 0;
                DrawOriginOffsetY = 0;
            }
            else
            {
                DrawOffsetX = 0;
                DrawOriginOffsetX = 0;
                DrawOriginOffsetY = 0;
            }

        }*/

		
    }