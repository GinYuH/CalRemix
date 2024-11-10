using Terraria;
using CalamityMod;
using CalamityMod.Items;
using Terraria.ModLoader;
using CalRemix.Content.Projectiles.Accessories;
using Terraria.ID;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Microsoft.Xna.Framework;

namespace CalRemix.Content.Items.Accessories
{
    public class SoulofCarcinogen : ModItem
    {
        public const int MaxSmokeTime = 600;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Soul of Carcinogen");
            Tooltip.SetDefault("7% increase to all damage\nPuts a cigar in the wearer's mouth which leaves behind smoke that inflicts several debuffs\nDamage of the smoke increases based on how long the player has been wearing the accessory\nIt all stops after 10 minutes...");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 3));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
        }
        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            float lightOffset = (float)Main.rand.Next(90, 111) * 0.01f;
            lightOffset *= Main.essScale;
            Lighting.AddLight((int)((Item.position.X + (float)(Item.width / 2)) / 16f), (int)((Item.position.Y + (float)(Item.height / 2)) / 16f), 0.5f * lightOffset, 0.4f * lightOffset, 0f * lightOffset);
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 22;
            Item.value = CalamityGlobalItem.RarityOrangeBuyPrice;
            Item.accessory = true;
            Item.rare = RarityHelper.Carcinogen;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage<GenericDamageClass>() += 0.07f;
            player.GetModPlayer<CalRemixPlayer>().carcinogenSoul = true;
            player.GetModPlayer<CalRemixPlayer>().timeSmoked++;
            if (player.GetModPlayer<CalRemixPlayer>().timeSmoked > CalamityUtils.SecondsToFrames(MaxSmokeTime))
            {
                player.KillMe(PlayerDeathReason.ByCustomReason(player.name + "'s lungs were replaced by nicotine."), 10142, 0);
                player.GetModPlayer<CalRemixPlayer>().timeSmoked = 0;
            }
            if (!hideVisual)
                player.GetModPlayer<CalRemixPlayer>().carcinogenSoulVanity = true;

            if (player.miscCounter % 12 == 0)
            {
                int damage = (int)MathHelper.Lerp(12, 60, player.GetModPlayer<CalRemixPlayer>().timeSmoked / (float)CalamityUtils.SecondsToFrames(MaxSmokeTime));
                int p = Projectile.NewProjectile(player.GetSource_Accessory(Item), player.Center + new Vector2(player.direction * 30, -10), Vector2.UnitY * -4 + new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1)), ModContent.ProjectileType<CigarSmoke>(), (int)player.GetDamage<AverageDamageClass>().ApplyTo(damage), 0f, player.whoAmI);
            }
        }

        public override void UpdateVanity(Player player)
        {
            player.GetModPlayer<CalRemixPlayer>().carcinogenSoulVanity = true;
            if (player.miscCounter % 12 == 0)
            {
                int p = Projectile.NewProjectile(player.GetSource_Accessory(Item), player.Center + new Vector2(player.direction * 30, -10), Vector2.UnitY * -4 + new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1)), ModContent.ProjectileType<CigarSmoke>(), 0, 0f, player.whoAmI);
                Main.projectile[p].timeLeft = 120;
            }
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            CalamityUtils.DrawInventoryCustomScale(
                spriteBatch,
                texture: TextureAssets.Item[Type].Value,
                position,
                frame,
                drawColor,
                itemColor,
                origin,
                scale,
                wantedScale: 1f,
                drawOffset: new(0f, 0f)
            );
            return false;
        }
    }
    public class CigarDrawLayer : PlayerDrawLayer
    {
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            return drawInfo.drawPlayer.GetModPlayer<CalRemixPlayer>().carcinogenSoulVanity;
        }
        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.FaceAcc);
        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            if (drawInfo.shadow != 0f)
                return;

            Player drawPlayer = drawInfo.drawPlayer;
            float alb = (255 - drawPlayer.immuneAlpha) / 255f;
            int dyeShader = drawPlayer.dye?[1].dye ?? 0;

            for (int n = 0; n < 18 + drawInfo.drawPlayer.extraAccessorySlots; n++)
            {
                Item item = drawInfo.drawPlayer.armor[n];
                if (item.type == ModContent.ItemType<SoulofCarcinogen>())
                {
                    if (n > 9)
                        dyeShader = drawPlayer.dye?[n - 10].dye ?? 0;
                    else
                        dyeShader = drawPlayer.dye?[n].dye ?? 0;
                }
            }

            int gnuflip = -drawPlayer.direction;
            Texture2D texture = ModContent.Request<Texture2D>("CalRemix/Content/Projectiles/Hostile/Cigar").Value;

            int drawX = (int)(drawPlayer.position.X + drawPlayer.width / 2f - Main.screenPosition.X + (-22 * gnuflip));
            int drawY = (int)(drawPlayer.position.Y + drawInfo.drawPlayer.height - Main.screenPosition.Y - 22 + drawPlayer.gfxOffY);

            DrawData dat = new(texture, new Vector2(drawX, drawY), null, Lighting.GetColor((int)drawPlayer.position.X / 16, (int)drawPlayer.position.Y / 16) * alb, drawPlayer.bodyRotation, new Vector2(texture.Width / 2f, texture.Height), 1f,
                drawPlayer.direction != -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            dat.shader = dyeShader;
            drawInfo.DrawDataCache.Add(dat);            
        }
    }
}
