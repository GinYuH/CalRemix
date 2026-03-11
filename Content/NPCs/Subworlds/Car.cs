using CalamityMod;
using CalamityMod.Graphics.Primitives;
using CalamityMod.Sounds;
using CalRemix.Content.Items.Accessories;
using CalRemix.Core.Subworlds;
using CalRemix.Core.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SubworldLibrary;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.NPCs.Subworlds
{
    public class Car : ModNPC
    {
        public override void SetStaticDefaults()
        {
            this.HideFromBestiary();
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 0;
            NPC.width = 200;
            NPC.height = 80;
            NPC.defense = 0;
            NPC.lifeMax = 2500000;
            NPC.value = 0;
            NPC.lavaImmune = true;
            NPC.knockBackResist = 0;
            NPC.HitSound = CommonCalamitySounds.ExoHitSound;
            NPC.DeathSound = CommonCalamitySounds.ExoDeathSound;
            NPC.GravityIgnoresLiquid = true;
            NPC.npcSlots = 0;
            NPC.dontCountMe = true;
            NPC.waterMovementSpeed = 1f;
            NPC.dontTakeDamage = true;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
        }

        public override void AI()
        {
            if (NPC.ai[0] == 0)
            {
                foreach (Player p in Main.ActivePlayers)
                {
                    if (p.Distance(NPC.Center) < 100)
                    {
                        NPC.ai[0] = 1;
                        NPC.ai[1] = p.whoAmI;
                        break;
                    }
                }
            }
            else if (NPC.ai[0] == 1)
            {
                Player person = Main.player[(int)NPC.ai[1]];
                if (!person.active)
                {
                    NPC.ai[0] = 0;
                    return;
                }
                int maxSpeed = 10;
                if (person.controlRight)
                {
                    NPC.velocity.X = maxSpeed;
                    NPC.localAI[0] += 0.25f;
                }
                else
                {
                    NPC.velocity.X = 0;
                }
                person.Center = NPC.Center + new Vector2(20, -20) + NPC.velocity;
                person.velocity = Vector2.Zero;
                person.direction = 1;
                person.bodyFrame.Y = person.bodyFrame.Height * 3;
                NPC.rotation += NPC.velocity.X * 0.03f;

                Lighting.AddLight(NPC.Left + Vector2.UnitX * 200, 1f, 1f, 0.8f);

                if (!SubworldSystem.AnyActive() && person.controlUseItem)
                {
                    NPC.ai[0] = 0;
                }
                if (NPC.Center.X > (16 * Main.maxTilesX) * 0.93f)
                {
                    NPC.ai[0] = 2;
                    NPC.velocity.X = 0;
                    Main.spawnTileX = (int)(NPC.Center.X / 16);
                }
            }
        }

        public override bool NeedSaving()
        {
            return true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = TextureAssets.Npc[Type].Value;
            Texture2D wheel = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Subworlds/Car_Wheel").Value;
            float offY = 8;
            spriteBatch.Draw(tex, NPC.Bottom - screenPos - Vector2.UnitY * (offY + 2 * MathF.Sin(NPC.localAI[0])), null, NPC.GetAlpha(drawColor), 0, new Vector2(tex.Width / 2, tex.Height), NPC.scale, 0, 0);
            float wheelRotation = NPC.rotation;
            spriteBatch.Draw(wheel, NPC.Bottom - screenPos + new Vector2(-60, -10 - offY), null, NPC.GetAlpha(drawColor), wheelRotation, new Vector2(wheel.Width / 2, wheel.Height / 2), NPC.scale, 0, 0);
            spriteBatch.Draw(wheel, NPC.Bottom - screenPos + new Vector2(80, -10 - offY), null, NPC.GetAlpha(drawColor), wheelRotation, new Vector2(wheel.Width / 2, wheel.Height / 2), NPC.scale, 0, 0);

            if (NPC.ai[0] == 1)
            {
                List<Vector2> points = new List<Vector2>();
                Vector2 start = NPC.Left + Vector2.UnitX * 160;
                for (int i = 0; i < 30; i++)
                {
                    points.Add(Vector2.Lerp(start, start + Vector2.UnitX * Main.screenWidth / 2, i / 29f));
                }

                PrimitiveRenderer.RenderTrail(points, new PrimitiveSettings(new PrimitiveSettings.VertexWidthFunction((float f, Vector2 v) => MathHelper.Lerp(4, 50, f)), new((float f, Vector2 v) => Color.Lerp(Color.Tan * 0.5f, default, CalamityUtils.SineOutEasing(f, 1)) * (1 + MathF.Sin(Main.GlobalTimeWrappedHourly * 5) * 0.15f))));
            }

            return false;
        }
    }

    public class CarDrawLayer : PlayerDrawLayer
    {
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            if (!SubworldSystem.IsActive<NightlineSubworld>())
            {
                return false;
            }
            return NPC.AnyNPCs(ModContent.NPCType<Car>());
        }
        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.LastVanillaLayer);
        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            if (drawInfo.shadow != 0f)
                return;

            foreach (NPC n in Main.ActiveNPCs)
            {
                if (n.type == ModContent.NPCType<Car>())
                {
                    Texture2D tex = TextureAssets.Npc[n.type].Value;
                    Texture2D wheel = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Subworlds/Car_Wheel").Value;
                    float offY = 8;
                    Color lightColor = n.GetAlpha(Lighting.GetColor(n.Center.ToTileCoordinates()));
                    drawInfo.DrawDataCache.Add(new DrawData(tex, n.Bottom - Main.screenPosition - Vector2.UnitY * (offY + 2 * MathF.Sin(n.localAI[0])), null, lightColor, 0, new Vector2(tex.Width / 2, tex.Height), n.scale, 0, 0));
                    float wheelRotation = n.rotation;
                    drawInfo.DrawDataCache.Add(new DrawData(wheel, n.Bottom - Main.screenPosition + new Vector2(-60, -10 - offY), null, lightColor, wheelRotation, new Vector2(wheel.Width / 2, wheel.Height / 2), n.scale, 0, 0));
                    drawInfo.DrawDataCache.Add(new DrawData(wheel, n.Bottom - Main.screenPosition + new Vector2(80, -10 - offY), null, lightColor, wheelRotation, new Vector2(wheel.Width / 2, wheel.Height / 2), n.scale, 0, 0));

                    return;
                }
            }
        }
    }
}