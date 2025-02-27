using System.Collections.Generic;
using System.Linq;
using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CalRemix.Content.NPCs.Bosses.Noxus;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;
using CalRemix.Content.Projectiles;

namespace CalRemix.Core.Graphics
{
    public class PitchBlackMetaball : Metaball
    {
        public class GasParticle
        {
            public float Size;

            public Vector2 Velocity;

            public Vector2 Center;
        }

        public static readonly List<GasParticle> GasParticles = new();

        public override MetaballDrawLayerType DrawContext => MetaballDrawLayerType.BeforeBlack;

        public override Color EdgeColor => Color.MediumPurple;

        public override List<Asset<Texture2D>> Layers => new()
        {
            ModContent.Request<Texture2D>("CalRemix/Core/Graphics/Metaballs/PitchBlackLayer")
        };

        public static void CreateParticle(Vector2 spawnPosition, Vector2 velocity, float size)
        {
            GasParticles.Add(new()
            {
                Center = spawnPosition,
                Velocity = velocity,
                Size = size
            });
        }

        public override void Update()
        {
            foreach (GasParticle particle in GasParticles)
            {
                particle.Velocity *= 0.99f;
                particle.Size *= 0.93f;
                particle.Center += particle.Velocity;
            }
            GasParticles.RemoveAll(p => p.Size <= 2f);
        }

        public override void DrawInstances()
        {
            Texture2D circle = ModContent.Request<Texture2D>("CalRemix/Core/Graphics/Metaballs/BasicCircle").Value;
            foreach (GasParticle particle in GasParticles)
                Main.spriteBatch.Draw(circle, particle.Center - Main.screenPosition, null, Color.White, 0f, circle.Size() * 0.5f, new Vector2(particle.Size) / circle.Size(), 0, 0f);

            foreach (NPC noxus in Main.npc.Where(n => n.active && n.type == ModContent.NPCType<EntropicGod>()))
            {
                Vector2 drawPosition = noxus.Center - Main.screenPosition;
                if (noxus.Opacity >= 0.8f)
                    noxus.ModNPC<EntropicGod>().DrawBack(drawPosition, noxus.GetAlpha(noxus.ModNPC<EntropicGod>().GeneralColor), noxus.rotation);
            }
            foreach (Projectile babyNoxus in Main.projectile.Where(p => p.active && p.type == ModContent.ProjectileType<BabyNoxus>()))
            {
                Vector2 drawPosition = babyNoxus.Center - Main.screenPosition;
                if (babyNoxus.Opacity >= 0.8f)
                    babyNoxus.ModProjectile<BabyNoxus>().DrawBack();
            }
        }
    }
}
