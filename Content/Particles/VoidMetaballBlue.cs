using CalamityMod.Enums;
using CalamityMod.Graphics.Metaballs;
using CalRemix.Content.NPCs.Subworlds.Sealed;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Particles
{
    public class VoidMetaballBlue : Metaball
    {
        public class VoidParticleBlue
        {
            public float Size;

            public Vector2 Velocity;

            public Vector2 Center;

            public int BoundNPC;

            public VoidParticleBlue(Vector2 center, Vector2 velocity, float size, int boundNPC = -1)
            {
                Center = center;
                Velocity = velocity;
                Size = size;
                BoundNPC = boundNPC;
            }

            public void Update()
            {
                Size *= 0.94f;
                Center += Velocity;
                Velocity *= 0.96f;
                if (BoundNPC != -1)
                {
                    NPC n = Main.npc[BoundNPC];
                    if (!n.active || n.life <= 0)
                    {
                        Size *= 0.8f;
                    }
                    else if (n.Distance(Center) > 300)
                    {
                        Size *= 0.8f;
                    }
                }
            }
        }

        private static List<Asset<Texture2D>> layerAssets;

        public static List<VoidParticleBlue> Particles
        {
            get;
            private set;
        } = new();

        // Check if there are any extraneous particles or if the Void Eminence projectile is present when deciding if this particle should be drawn.
        public override bool AnythingToDraw => Particles.Any() || NPC.AnyNPCs(ModContent.NPCType<ShadeBlue>());

        public override IEnumerable<Texture2D> Layers
        {
            get
            {
                for (int i = 0; i < layerAssets.Count; i++)
                    yield return layerAssets[i].Value;
            }
        }

        public override GeneralDrawLayer DrawLayer => GeneralDrawLayer.BeforeNPCs;

        public override Color EdgeColor => new(0, 0, 255);

        public override void Load()
        {
            if (Main.netMode == NetmodeID.Server)
                return;

            // Load layer assets.
            layerAssets = new() { ModContent.Request<Texture2D>("CalRemix/Assets/ExtraTextures/VoidMetaballBlue", AssetRequestMode.ImmediateLoad) };
        }

        public override void ClearInstances() => Particles.Clear();

        public override void Update()
        {
            // Update all particle instances.
            // Once sufficiently small, they vanish.
            for (int i = 0; i < Particles.Count; i++)
                Particles[i].Update();
            Particles.RemoveAll(p => p.Size <= 2.5f);
        }

        public static void SpawnParticle(Vector2 position, Vector2 velocity, float size, int index = -1) =>
            Particles.Add(new(position, velocity, size, index));

        public override Vector2 CalculateManualOffsetForLayer(int layerIndex)
        {
            return ShadeBlue.texOffsetBlue;
        }

        public override void DrawInstances()
        {
            Texture2D tex = ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/BasicCircle").Value;

            // Draw all particles.
            foreach (VoidParticleBlue particle in Particles)
            {
                Vector2 drawPosition = particle.Center - Main.screenPosition;
                Vector2 origin = tex.Size() * 0.5f;
                Vector2 scale = Vector2.One * particle.Size / tex.Size();
                Main.spriteBatch.Draw(tex, drawPosition, null, Color.White, 0f, origin, scale, 0, 0f);
            }

            int voidType = ModContent.NPCType<ShadeBlue>();
            foreach (NPC n in Main.ActiveNPCs)
            {
                if (n.type == voidType)
                {
                    Main.spriteBatch.Draw(tex, n.Center - Main.screenPosition, null, Color.White * n.Opacity, 0f, tex.Size() / 2, 2, 0, 0);
                }
            }
        }
    }
}
