using System.Collections.Generic;
using CalamityMod;
using CalamityMod.Enums;
using CalamityMod.Graphics.Metaballs;
using CalRemix.Content.NPCs.Subworlds.Sealed;
using CalRemix.Content.Projectiles.Hostile;
using CalRemix.Core.Subworlds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SubworldLibrary;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Content.Particles;

public class DoUMetaball : Metaball
{
    public static List<StreamGougeMetaball.CosmicParticle> Particles { get; private set; } = new List<StreamGougeMetaball.CosmicParticle>();

    public int Variant = 0;


    public override bool AnythingToDraw => SubworldSystem.IsActive<SealedSubworld>();

    public override IEnumerable<Texture2D> Layers
    {
        get
        {
            yield return StreamGougeMetaball.LayerAsset.Value;
        }
    }

    public override GeneralDrawLayer DrawLayer => GeneralDrawLayer.AfterNPCs;

    public override Color EdgeColor => Color.Lerp(Color.Purple, Color.Black, 0.75f);

    public override void Update()
    {
        for (int i = 0; i < Particles.Count; i++)
        {
            Particles[i].Update();
        }

        Particles.RemoveAll((StreamGougeMetaball.CosmicParticle p) => p.Size <= 2f);
    }

    public static StreamGougeMetaball.CosmicParticle SpawnParticle(Vector2 position, Vector2 velocity, float size)
    {
        StreamGougeMetaball.CosmicParticle cosmicParticle = new StreamGougeMetaball.CosmicParticle(position, velocity, size);
        Particles.Add(cosmicParticle);
        return cosmicParticle;
    }

    public override Vector2 CalculateManualOffsetForLayer(int layerIndex)
    {
        return Vector2.UnitX * Main.GlobalTimeWrappedHourly * 0.037f;
    }

    public override void DrawInstances()
    {
        Texture2D value = ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/BasicCircle").Value;
        Texture2D smok = ModContent.Request<Texture2D>("CalRemix/Content/Particles/RemixSmoke").Value;
        foreach (StreamGougeMetaball.CosmicParticle particle in Particles)
        {
            Vector2 position = particle.Center - Main.screenPosition;
            Vector2 origin = value.Size() * 0.5f;
            Vector2 scale = Vector2.One * particle.Size / value.Size();
            Main.spriteBatch.Draw(value, position, null, Color.White, 0f, origin, scale, SpriteEffects.None, 0f);
        }

        foreach (Projectile p in Main.ActiveProjectiles)
        {
            if (p.type == ModContent.ProjectileType<DoUSmoke>())
            {
                Main.spriteBatch.Draw(smok, p.Center - Main.screenPosition, smok.Frame(1, 6, 0, p.frame), Color.White, p.rotation, new Vector2(smok.Width / 2, smok.Height / 12), p.scale, SpriteEffects.None, 0f);
            }
        }

        foreach (NPC n in Main.ActiveNPCs)
        {
            if (n.type == ModContent.NPCType<DoUHead>())
            {
                n.ModNPC<DoUHead>().DrawDoT(Main.spriteBatch, Main.screenPosition);
            }
        }
    }
}