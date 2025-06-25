using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles
{
    public class AgentPoon : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_23";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Agent's Harpoon");
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.aiStyle = ProjAIStyleID.Arrow;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
        }
    }
}