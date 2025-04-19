using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Projectiles.Weapons;

namespace CalRemix.Content.Items.Weapons
{
    public class Ogscule : ModItem
    {
        int ogsculetick = 0;
        int ogsculetickloop = 0;
        public override void SetStaticDefaults()
        {

            // DisplayName.SetDefault("Ogscule");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 203;
            Item.DamageType = DamageClass.Generic;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 30;
            Item.useAnimation = 1200;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.knockBack = 6;
            Item.value = Item.buyPrice(0, 0, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.autoReuse = true;
            Item.crit = 0;
            Item.shootSpeed = 0;

        }




        public override void MeleeEffects(Player player, Rectangle hitbox)
        {

            if (Item.active == true)

            {
                ogsculetickloop++;
                ogsculetick++;

                if (ogsculetick <= 300)
                {
                    Lighting.AddLight(player.position, 3, 0, 0);

                    if (ogsculetickloop == 30)
                    {
                        SoundEngine.PlaySound(SoundID.Item15, player.position);
                    
                    }
                    if (Main.rand.NextBool(3))
                    {
                        float x2 = hitbox.Center.X;
                        float y2 = hitbox.Center.Y;
                        int num165 = Dust.NewDust(new Vector2(x2, y2), 10, 10, DustID.Clentaminator_Red, 0f, 0f, 0, default, 5f);


                        Main.dust[num165].position.X = x2;
                        Main.dust[num165].position.Y = y2;
                        Main.dust[num165].velocity *= Main.rand.NextFloat(0, 10);
                        Main.dust[num165].noGravity = true;
                        Main.dust[num165].fadeIn *= 0f;
                        Main.dust[num165].scale = 2;
                    }
                
                }

                if (ogsculetick >= 300)
                {
                    for (int num163 = 0; num163 < 2; num163++)
                    {
                        float x2 = hitbox.Center.X;
                        float y2 = hitbox.Center.Y;
                        int num165 = Dust.NewDust(new Vector2(x2, y2), 10, 10, DustID.Clentaminator_Red, 0f, 0f, 0, default, 5f);


                        Main.dust[num165].position.X = x2;
                        Main.dust[num165].position.Y = y2;
                        Main.dust[num165].velocity *= Main.rand.NextFloat(0, 10);
                        Main.dust[num165].noGravity = true;
                        Main.dust[num165].fadeIn *= 0f;
                        Main.dust[num165].scale = 2;
                    }
                    Lighting.AddLight(player.position, 5, 0, 0);

               
                    if (ogsculetickloop == 30)
                    {
                        SoundEngine.PlaySound(SoundID.Item43, player.position);
                        int a = Projectile.NewProjectile(spawnSource: null, player.Center.X, player.Center.Y - 0f, (float)(Main.rand.Next(-8, 8) * 3.14f), Main.rand.Next(-8, 8) * -3.14f, ModContent.ProjectileType<OgsculeBeam>(), Damage: Item.damage, KnockBack: Item.knockBack, player.whoAmI);

                        Main.projectile[a].ai[0] = Main.rand.NextFloat(-0.04f, 0.04f);
                    }
                }
            }

            if (ogsculetick > 1200)
            {
                ogsculetick = 0;
            }
            if (ogsculetickloop >= 30)
            {
                ogsculetickloop = 0;
            }
            // Emit dusts when the sword is swung
        }
    }

}

