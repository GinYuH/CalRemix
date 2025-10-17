using CalRemix.Core.Biomes;
using System;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalRemix.Content.Items.Materials;
using CalamityMod;
using CalRemix.Content.Items.Placeables.Subworlds.Sealed;
using CalamityMod.Graphics.Metaballs;
using CalRemix.Content.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using CalRemix.Core.World;

namespace CalRemix.Content.NPCs.Subworlds.Sealed
{
    //[AutoloadBossHead]
    public class VoidBoss : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.MustAlwaysDraw[Type] = true;
        }

        public static Vector2 texOffset = new Vector2();

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 100;
            NPC.height = 100;
            NPC.lifeMax = 100000;
            NPC.damage = 240;
            NPC.defense = 10;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.HitSound = null;
            NPC.DeathSound = null;
            NPC.noTileCollide = true;
            NPC.value = Item.buyPrice(gold: 20);
            NPC.boss = true;
            NPC.alpha = 255;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<VoidForestBiome>().Type };
            Music = CalRemixMusic.TheCalamity;
        }

        public override void AI()
        {
            //NPC.velocity = Main.MouseWorld - NPC.Center;
            VoidMetaball.SpawnParticle(NPC.Center + Main.rand.NextVector2Circular(40, 40), Main.rand.NextVector2Circular(2, 2), Main.rand.NextFloat(100, 150));
            VoidMetaball.SpawnParticle(NPC.Center + Main.rand.NextVector2Circular(40, 40), Main.rand.NextVector2Circular(8, 8), Main.rand.NextFloat(30, 90));
            if (NPC.velocity == Vector2.Zero)
            {
                for (int i = 0; i < 10; i++)
                {

                    VoidMetaball.SpawnParticle(NPC.Center + Vector2.UnitY * i * 20 + Main.rand.NextVector2Circular(40, 40), Main.rand.NextVector2Circular(2, 2), Main.rand.NextFloat(100, 150) * MathHelper.Lerp(1, 0.01f, i / (float)(10 - 1)));
                    if (Main.rand.NextBool(10))
                    {

                        VoidMetaball.SpawnParticle(NPC.Center + Vector2.UnitY * i * 20 + Main.rand.NextVector2Circular(40, 40), Main.rand.NextVector2Circular(8, 8), Main.rand.NextFloat(10, 30));
                    }
                }
            }
            if (NPC.frameCounter++ % 8 == 0)
            {
                texOffset = Main.rand.NextVector2Circular(100, 100);
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<VoidSingularity>(), 1, 5, 13);
            npcLoot.Add(ModContent.ItemType<VoidInfusedStone>(), 1, 150, 250);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = TextureAssets.Npc[Type].Value;
            Vector2 offset = Vector2.Zero;
            spriteBatch.Draw(tex, NPC.Center - screenPos + offset, null, Color.Red, 0, tex.Size() / 2, NPC.scale, 0, 0);
            return false;
        }

        public override void OnKill()
        {
            RemixDowned.downedVoid = true;
            CalRemixWorld.UpdateWorldBool();
        }
    }
}