using CalamityMod;
using CalRemix.Content.Buffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class SealedLashesProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // This makes the projectile use whip collision detection and allows flasks to be applied to it.
            ProjectileID.Sets.IsAWhip[Type] = true;
        }
        public override void SetDefaults()
        {
            // This method quickly sets the whip's properties.
            Projectile.DefaultToWhip();
            Projectile.aiStyle = -1;

            // use these to change from the vanilla defaults
            Projectile.WhipSettings.Segments = 30;
            Projectile.WhipSettings.RangeMultiplier = 0.6f;
        }

        private float Timer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        // This example uses PreAI to implement a charging mechanic.
        // If you remove this, also remove Item.channel = true from the item's SetDefaults.
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.rotation = Projectile.velocity.ToRotation() + (float)Math.PI / 2f;
            Projectile.ai[0] += 1f;
            Projectile.GetWhipSettings(Projectile, out var timeToFlyOut, out var _, out var _);
            Projectile.Center = Main.GetPlayerArmPosition(Projectile) + Projectile.velocity * (Projectile.ai[0] - 1f);
            Projectile.spriteDirection = ((!(Vector2.Dot(Projectile.velocity, Vector2.UnitX) < 0f)) ? 1 : (-1));
            timeToFlyOut = 60;
            if (Projectile.ai[0] >= timeToFlyOut)
            {
                Projectile.Kill();
                return false;
            }
            //player.heldProj = Projectile.whoAmI;
            //player.MatchItemTimeToItemAnimation();
            if (Projectile.ai[0] == (float)(int)(timeToFlyOut / 2f))
            {
                Projectile.WhipPointsForCollision.Clear();
                FillWhipControlPoints(Projectile, Projectile.WhipPointsForCollision);
                Vector2 vector = Projectile.WhipPointsForCollision[Projectile.WhipPointsForCollision.Count - 1];
                SoundEngine.PlaySound(in SoundID.Item153, vector);
            }
            if (player == null || !player.active || player.dead)
                return false;
            return false;
        }

        public static void FillWhipControlPoints(Projectile proj, List<Vector2> controlPoints)
        {
            Projectile.GetWhipSettings(proj, out var timeToFlyOut, out var segments, out var rangeMultiplier);
            timeToFlyOut = 60;
            float num = proj.ai[0] / timeToFlyOut;
            float num10 = 0.5f;
            float num11 = 1f + num10;
            float num12 = (float)Math.PI * 10f * (1f - num * num11) * (float)(-proj.spriteDirection) / (float)segments;
            float num13 = num * num11;
            float num14 = 0f;
            if (num13 > 1f)
            {
                num14 = (num13 - 1f) / num10;
                num13 = MathHelper.Lerp(1f, 0f, num14);
            }
            float num15 = proj.ai[0] - 1f;
            Player player = Main.player[proj.owner];
            Item heldItem = Main.player[proj.owner].HeldItem;
            num15 = (float)(player.HeldItem.useTime * 2) * num * player.whipRangeMultiplier;
            float num16 = proj.velocity.Length() * num15 * num13 * rangeMultiplier / (float)segments;
            float num17 = 1f;
            Vector2 playerArmPosition = Main.GetPlayerArmPosition(proj);
            Vector2 vector = playerArmPosition;
            float num2 = -(float)Math.PI / 2f;
            Vector2 vector2 = vector;
            float num3 = (float)Math.PI / 2f + (float)Math.PI / 2f * (float)proj.spriteDirection;
            Vector2 vector3 = vector;
            float num4 = (float)Math.PI / 2f;
            controlPoints.Add(playerArmPosition);
            for (int i = 0; i < segments; i++)
            {
                float num5 = (float)i / (float)segments;
                float num6 = num12 * num5 * num17;
                Vector2 vector4 = vector + num2.ToRotationVector2() * num16;
                Vector2 vector5 = vector3 + num4.ToRotationVector2() * (num16 * 2f);
                Vector2 vector7 = vector2 + num3.ToRotationVector2() * (num16 * 2f);
                float num7 = 1f - num13;
                float num8 = 1f - num7 * num7;
                Vector2 value = Vector2.Lerp(vector5, vector4, num8 * 0.9f + 0.1f);
                Vector2 vector6 = Vector2.Lerp(vector7, value, num8 * 0.7f + 0.3f);
                Vector2 spinningpoint = playerArmPosition + (vector6 - playerArmPosition) * new Vector2(1f, num11);
                float num9 = num14;
                num9 *= num9;
                Vector2 item = spinningpoint.RotatedBy(proj.rotation + 4.712389f * num9 * (float)proj.spriteDirection, playerArmPosition);
                controlPoints.Add(item);
                num2 += num6;
                num4 += num6;
                num3 += num6;
                vector = vector4;
                vector3 = vector5;
                vector2 = vector7;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
            Projectile.damage = (int)(Projectile.damage * 0.5f); // Multihit penalty. Decrease the damage the more enemies the whip hits.
        }

        // This method draws a line between all points of the whip, in case there's empty space between the sprites.
        private void DrawLine(List<Vector2> list)
        {
        }

        public override bool PreDraw(ref Color lightColor)
        {
            List<Vector2> list = new List<Vector2>();
            FillWhipControlPoints(Projectile, list);

            DrawLine(list);

            //Main.DrawWhip_WhipBland(Projectile, list);
            // The code below is for custom drawing.
            // If you don't want that, you can remove it all and instead call one of vanilla's DrawWhip methods, like above.
            // However, you must adhere to how they draw if you do.

            SpriteEffects flip = Projectile.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            Texture2D texture = TextureAssets.Projectile[Type].Value;

            Vector2 pos = list[0];

            for (int i = 0; i < list.Count - 1; i++)
            {
                // These two values are set to suit this projectile's sprite, but won't necessarily work for your own.
                // You can change them if they don't!
                float scale = 1;
                Rectangle frame = new Rectangle(0, 0, 10, 14);

                // These statements determine what part of the spritesheet to draw for the current segment.
                // They can also be changed to suit your sprite.
                if (i == list.Count - 2)
                {
                    // For a more impactful look, this scales the tip of the whip up when fully extended, and down when curled up.
                    Projectile.GetWhipSettings(Projectile, out float timeToFlyOut, out int _, out float _);
                    float t = Timer / timeToFlyOut;
                    //scale = MathHelper.Lerp(0.5f, 1.5f, Utils.GetLerpValue(0.1f, 0.7f, t, true) * Utils.GetLerpValue(0.9f, 0.7f, t, true));
                    frame = new Rectangle(0, 34, 10, 14);
                }
                else if (i == list.Count - 3)
                {
                    frame = new Rectangle(0, 17, 10, 14);
                }

                Vector2 element = list[i];
                Vector2 diff = list[i + 1] - element;

                float rotation = diff.ToRotation() - MathHelper.PiOver2; // This projectile's sprite faces down, so PiOver2 is used to correct rotation.
                Color color = Lighting.GetColor(element.ToTileCoordinates());

                Main.EntitySpriteDraw(texture, pos - Main.screenPosition, frame, color, rotation, texture.Size() / 2, scale, flip, 0);

                pos += diff;
            }
            return false;
        }
    }
}