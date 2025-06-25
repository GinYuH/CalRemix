using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class DeicideFist : ModProjectile
    {
        public ref float State => ref Projectile.ai[0];
        public ref float Found => ref Projectile.ai[1];
        public override string Texture => "CalamityMod/Projectiles/Boss/SupremeCataclysmFist";
        public Player Owner => Main.player[Projectile.owner];
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Sacrilegious Fist");
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 83;
            Projectile.height = 83;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 42;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.aiStyle = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 42;
        }
        public override void OnSpawn(IEntitySource source)
        {
            if (State > 0)
                Projectile.timeLeft = 600;
        }
        public override void AI()
        {
            int frameGate = 6;
            Projectile.frameCounter++;
            if (Projectile.frameCounter > frameGate)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= 4)
                Projectile.frame = 0;
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (State > 0 && Found == 0)
            {
                int index = Projectile.FindTargetWithLineOfSight(640);
                if (!index.WithinBounds(Main.maxNPCs))
                    return;
                NPC npc = Main.npc[index];
                if (Projectile.owner == Main.myPlayer)
                    Projectile.velocity = Projectile.SafeDirectionTo(npc.Center) * 16f;
                Found++;
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 255, 255, 150);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            SpriteEffects spriteEffects = Projectile.direction > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically;
            Rectangle rect = new(0, Projectile.frame * texture.Height / 4, texture.Width, texture.Height / 4);
            Vector2 centered = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
            Main.EntitySpriteDraw(texture, centered, rect, new Color(255, 255, 255, Projectile.alpha), Projectile.rotation, new Vector2(texture.Width, texture.Height / 4) / 2, Projectile.scale, spriteEffects, 0);
            return false;
        }
    }
}
