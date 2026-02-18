using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Terraria.ModLoader.Utilities;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using CalamityMod.Items.Materials;
using Terraria.Audio;
using CalamityMod.Buffs.DamageOverTime;
using CalRemix.Content.Items.Pets;
using Terraria.Graphics.Shaders;
using CalamityMod.Graphics.Primitives;
using System.Collections.Generic;

namespace CalRemix.Content.NPCs.Eclipse
{
    public class Glitch : ModNPC
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Glitch");
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 30;
            NPC.height = 32;
            NPC.lifeMax = 3000;
            NPC.damage = 50;
            NPC.defense = 30;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(gold: 15);
            NPC.HitSound = null;
            NPC.DeathSound = null;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
        }

        public override void AI()
        {
            NPC.ai[0]++;
            if (NPC.ai[0] % 120 == 0)
            {
                if (NPC.CountNPCS(ModContent.NPCType<Corruption>()) < 7)
                {
                    if (Main.rand.NextBool())
                    {
                        SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/Zombie_" + Main.rand.Next(1, 131)) with { PitchRange = (-1, 1) }, NPC.Center);
                    }
                    else
                    {
                        SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/Item_" + Main.rand.Next(1, 179)) with { PitchRange = (-1, 1) }, NPC.Center);
                    }
                    CalRemixHelper.SpawnNewNPC(NPC.GetSource_FromThis(), NPC.Center, ModContent.NPCType<Corruption>(), ai0: Main.rand.Next(0, 4));
                }
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Eclipse,
        new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!DownedBossSystem.downedDoG || NPC.AnyNPCs(Type))
                return 0f;

            return SpawnCondition.SolarEclipse.Chance * 0.1f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<DarksunFragment>(), 1, 4, 6);
            npcLoot.Add(ModContent.ItemType<Everflute>(), 20);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            foreach (NPC n in Main.ActiveNPCs)
            {
                if (n.type == ModContent.NPCType<Corruption>() && n.active)
                {
                    Main.spriteBatch.EnterShaderRegion();

                    // Prepare the flame trail shader with its map texture.
                    GameShaders.Misc["CalamityMod:ImpFlameTrail"].SetShaderTexture(ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/Trails/ScarletDevilStreak"));
                    Vector2 trailOffset = NPC.Size * 0.5f;
                    trailOffset += (NPC.rotation + MathHelper.PiOver2).ToRotationVector2();

                    List<Vector2> points = new List<Vector2>();

                    Vector2 destination = n.Center;
                    Vector2 start = NPC.Center;

                    Vector2 dist = destination - start;

                    points.Add(start);

                    for (int i = 1; i < 30; i++)
                    {
                        points.Add(start + dist / 30f * i);
                    }

                    points.Add(destination);


                    PrimitiveRenderer.RenderTrail(points, new(FlameTrailWidthFunction, FlameTrailColorFunction, (_, _) => trailOffset, shader: GameShaders.Misc["CalamityMod:ImpFlameTrail"]), 61);

                    Main.spriteBatch.ExitShaderRegion();
                }
            }
            Vector2 npcOffset = NPC.Center - screenPos;
            spriteBatch.Draw(TextureAssets.Npc[Type].Value, npcOffset, null, NPC.GetAlpha(Color.White), 0f, TextureAssets.Npc[Type].Size() / 2, 1f, SpriteEffects.None, 1f);
            return false;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            if (Main.rand.NextBool(4))
                target.AddBuff(BuffID.Cursed, CalamityUtils.SecondsToFrames(25));
            target.AddBuff(ModContent.BuffType<Vaporfied>(), CalamityUtils.SecondsToFrames(4));
        }
        public float FlameTrailWidthFunction(float completionRatio, Vector2 v) => MathHelper.SmoothStep(12f * NPC.scale, 8f * NPC.scale, completionRatio);

        public Color FlameTrailColorFunction(float completionRatio, Vector2 v)
        {
            return Main.DiscoColor * completionRatio;
        }
    }
}