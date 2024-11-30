using CalamityMod.Projectiles.BaseProjectiles;
using CalRemix.Content.Items.Weapons;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons;
public class WindTurbine : BaseSpearProjectile
{
    public override void SetDefaults()
    {
        Projectile.width = 12;  //The width of the .png file in pixels divided by 2.
        Projectile.height = 12;  //The height of the .png file in pixels divided by 2.
        Projectile.DamageType = Terraria.ModLoader.DamageClass.Melee;
        Projectile.timeLeft = 10;
        Projectile.friendly = true;
        Projectile.hostile = false;
        Projectile.tileCollide = false;
        Projectile.ignoreWater = true;
        Projectile.penetrate = -1;
        Projectile.ownerHitCheck = true;
    }
    public override float InitialSpeed => 6f;
    public override float ReelbackSpeed => 5.1f;
    public override float ForwardSpeed => 2.95f;
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (Main.player[Projectile.owner] is null)
            return;
        Player player = Main.player[Projectile.owner];
        if (Main.myPlayer == player.whoAmI && Main.mouseItem != null && Main.mouseItem.type == ModContent.ItemType<WindTurbineBlade>())
        {
            WindTurbineBlade item = Main.mouseItem.ModItem as WindTurbineBlade;
            item.hitCounter++;
        }
        else if (player.HeldItem != null && player.HeldItem.type == ModContent.ItemType<WindTurbineBlade>())
        {
            WindTurbineBlade item = player.HeldItem.ModItem as WindTurbineBlade;
            item.hitCounter++;
        }
    }
}
