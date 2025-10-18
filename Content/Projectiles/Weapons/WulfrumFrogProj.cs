using CalRemix.Content.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class WulfrumFrogProj : ModProjectile
    {
        public override string Texture => "CalRemix/Content/NPCs/WulfrumFrog";

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 13;
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.timeLeft = 300;
            Projectile.DamageType = DamageClass.Ranged;
        }
        public override void AI()
        {
            Projectile.rotation += 0.08f;
        }
        public override void OnKill(int timeLeft)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.NewNPC(Projectile.GetSource_FromThis(), (int)Projectile.Center.X, (int)Projectile.Center.Y, ModContent.NPCType<WulfrumFrog>());
            }
        }
    }
}