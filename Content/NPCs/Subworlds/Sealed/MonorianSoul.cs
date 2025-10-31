using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Terraria.GameContent.Bestiary;
using CalRemix.Core.Biomes;
using CalRemix.Content.Items.Armor;
using CalRemix.Content.Items.Potions;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.GameContent;
using System;
using CalamityMod.Graphics.Primitives;
using CalRemix.UI;
using Terraria.UI;
using static Terraria.Graphics.Effects.Filters;
using static Terraria.ModLoader.ModContent;
using Terraria.ModLoader.IO;
using CalamityMod.NPCs.Cryogen;
using Terraria.Graphics.Shaders;

namespace CalRemix.Content.NPCs.Subworlds.Sealed
{
    public class MonorianSoul : ModNPC
    {
        public Player Target => Main.player[NPC.target];
        public ref float Timer => ref NPC.ai[0];
        public ref float State => ref NPC.ai[1];

        public ref float ExtraVar => ref NPC.ai[2];

        public ref float ExtraVar2 => ref NPC.ai[3];
        public Vector2 SavePosition
        {
            get => new Vector2(NPC.Calamity().newAI[2], NPC.Calamity().newAI[1]);
            set
            {
                NPC.Calamity().newAI[2] = value.X;
                NPC.Calamity().newAI[1] = value.Y;
            }
        }
        public Vector2 OldPosition
        {
            get => new Vector2(NPC.localAI[2], NPC.localAI[1]);
            set
            {
                NPC.localAI[2] = value.X;
                NPC.localAI[1] = value.Y;
            }
        }

        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 80;
            NPC.height = 80;
            NPC.lifeMax = 100000;
            NPC.damage = 270;
            NPC.defense = 40;
            NPC.noGravity = true;
            NPC.HitSound = Cryogen.HitSound with { Pitch = -1 };
            NPC.DeathSound = Cryogen.DeathSound with { Pitch = 1 };
            NPC.knockBackResist = 0f;
            NPC.noTileCollide = true;
            NPC.boss = true;
            NPC.Calamity().canBreakPlayerDefense = true;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<VoidForestBiome>().Type };
            Music = CalRemixMusic.TheCalamity;
        }
        public override void AI()
        {
            NPC.TargetClosest(false);
            switch (State)
            {
                case 0:
                    {

                    }
                    break;
                case 1:
                    {

                    }
                    break;
                case 2:
                    {

                    }
                    break;
                case 3:
                    {

                    }
                    break;
                case 4:
                    {

                    }
                    break;
            }
            Timer++;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Microsoft.Xna.Framework.Color drawColor)
        {
            spriteBatch.ExitShaderRegion();
            Texture2D radians = TextureAssets.Npc[ModContent.NPCType<WalkingBird>()].Value;
            Texture2D spark = Request<Texture2D>("CalamityMod/Particles/HollowCircleSoftEdge").Value;
            Texture2D star = Request<Texture2D>("CalamityMod/Particles/Sparkle").Value;

            float sizeMod = 0.5f;

            var shader = GameShaders.Misc[$"{Mod.Name}:Onesoul"];
            Color c = Color.Cyan;
            shader.UseColor(c * NPC.Opacity);
            shader.Apply();
            spriteBatch.EnterShaderRegion(BlendState.Additive, shader.Shader);
            spriteBatch.Draw(radians, NPC.Center - screenPos, null, Color.White, 5 * Main.GlobalTimeWrappedHourly, radians.Size() / 2, NPC.scale * new Vector2(12f, 1f) * sizeMod, SpriteEffects.FlipHorizontally, 0);

            spriteBatch.ExitShaderRegion();
            spriteBatch.EnterShaderRegion(BlendState.Additive);

            //Main.spriteBatch.Draw(spark, NPC.Center - Main.screenPosition, null, Color.Cyan, Main.GlobalTimeWrappedHourly, spark.Size() / 2, NPC.scale * 1.8f * sizeMod, SpriteEffects.FlipHorizontally, 0);
            spriteBatch.Draw(star, NPC.Center - screenPos, null, Color.White, 0, star.Size() / 2, NPC.scale * 2.4f * sizeMod + (1 + 0.5f * MathF.Sin(Main.GlobalTimeWrappedHourly * 2f)), SpriteEffects.FlipHorizontally, 0);

            spriteBatch.ExitShaderRegion();

            return false;
        }
    }
}
