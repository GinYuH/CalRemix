using CalamityMod;
using CalamityMod.Projectiles.Typeless;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using System;
using Terraria.ModLoader;
using CalamityMod.Projectiles.Magic;
using CalRemix.Content.Items.Placeables.Relics;
using CalRemix.Content.NPCs.Bosses.RajahBoss;
using CalRemix.Content.Items.Potions;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class BagofCarrotsProj : ModProjectile
    {
        public override string Texture => "CalRemix/Content/Items/RajahItems/BagofCarrots";
        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 600;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] > 3)
            {
                Projectile.tileCollide = true;
            }
            if (Projectile.ai[0] > 10f)
            {
                Projectile.ai[0] = 10f;
                if (Projectile.velocity.Y == 0f && Projectile.velocity.X != 0f)
                {
                    Projectile.velocity.X = Projectile.velocity.X * 0.97f;
                    if (Projectile.velocity.X > -0.01f && Projectile.velocity.X < 0.01f)
                    {
                        Projectile.velocity.X = 0f;
                        Projectile.netUpdate = true;
                    }
                }
                Projectile.velocity.Y += 0.2f;
            }
            Projectile.rotation += Projectile.velocity.X * 0.05f;
        }


        public override void OnKill(int timeLeft)
        {
            int rajaType = Projectile.Calamity().stealthStrike ? ModContent.NPCType<SupremeRajah>() : ModContent.NPCType<Rajah>();
            int n = NPC.NewNPC(Projectile.GetSource_Death(), (int)Projectile.Center.X, (int)Projectile.Center.Y - 200, rajaType);
            Main.npc[n].friendly = true;

            int carAmt = Main.rand.Next(3, 8);
            for (int i = 0; i < carAmt; i++)
            {
                Item car = new Item(ModContent.ItemType<Carrot>());
                Rectangle r = Projectile.Hitbox;
                r.Inflate(20, 20);
                int carrot = Item.NewItem(Projectile.GetSource_Death(), Main.rand.NextVector2FromRectangle(r), car);
                //Main.item[carrot].velocity = Vector2.UnitY.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(10, 20);
            }
        }
    }
}