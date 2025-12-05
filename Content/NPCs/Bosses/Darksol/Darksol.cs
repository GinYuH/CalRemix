using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace CalRemix.Content.NPCs.Bosses.Darksol;

public class Darksol : ModNPC
{
    public override string Texture => "CalRemix/Core/Graphics/Metaballs/BasicCircle";

    private float time;
    private float breakAmount;
    private Vector3[] fragmentOffsets;

    public override void SetDefaults()
    {
        NPC.width = 64;
        NPC.height = 64;
        NPC.scale = 4f;
        NPC.damage = 20;
        NPC.lifeMax = 1;
        NPC.knockBackResist = 0f;
        NPC.noGravity = true;
        NPC.noTileCollide = true;
        NPC.boss = true;
        NPC.dontTakeDamage = true;
        NPC.npcSlots = 10f;
        NPC.netAlways = true;
        //Music = -1;
        //SceneEffectPriority = SceneEffectPriority.None;

        fragmentOffsets = new Vector3[5];

        for (int i = 0; i < fragmentOffsets.Length; i++)
        {
            float angle = (i / 5f) * MathHelper.TwoPi;
            fragmentOffsets[i] = new Vector3(
                (float)Math.Cos(angle) * 0.5f,  // X direction
                (float)Math.Sin(angle) * 0.5f,  // Y direction
                i * 0.2f                         // Fade timing (0, 0.2, 0.4, 0.6, 0.8)
            );
        }
    }

    public override void AI()
    {
        NPC.velocity = (Main.MouseWorld - NPC.Center) / 30f;
        NPC.rotation *= 0.9f;
        NPC.rotation += NPC.velocity.X / 128f;
        time++;
    }

    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        Texture2D bossTexture = TextureAssets.Npc[Type].Value;
        var ozmaShader = GameShaders.Misc[$"{Mod.Name}:Ozma"];

        Vector2 worldPos = Vector2.Transform(NPC.Center - Main.screenPosition, Main.GameViewMatrix.ZoomMatrix) / Main.ScreenSize.ToVector2();
        ozmaShader.Shader.Parameters["time"]?.SetValue(time);
        ozmaShader.Shader.Parameters["worldPosition"]?.SetValue(NPC.Center);
        ozmaShader.Shader.Parameters["scale"]?.SetValue(new Vector2(bossTexture.Width, bossTexture.Height) * NPC.scale);
        ozmaShader.Shader.Parameters["rotation"]?.SetValue(NPC.rotation);
        ozmaShader.Shader.Parameters["zoom"]?.SetValue(Main.GameViewMatrix.Zoom);

        spriteBatch.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/GreyscaleGradients/MeltyNoise").Value;
        spriteBatch.GraphicsDevice.SamplerStates[1] = SamplerState.LinearWrap;

        // Optional: Customize colors (uncomment to change)
        // ozmaShader.Parameters["color1"]?.SetValue(new Vector4(1.0f, 0.2f, 0.8f, 1.0f));
        // ozmaShader.Parameters["color2"]?.SetValue(new Vector4(0.2f, 0.5f, 1.0f, 1.0f));
        // ozmaShader.Parameters["color3"]?.SetValue(new Vector4(1.0f, 0.8f, 0.2f, 1.0f));
        // ozmaShader.Parameters["color4"]?.SetValue(new Vector4(0.3f, 1.0f, 0.4f, 1.0f));

        // Apply the shader and draw
        spriteBatch.End();

        spriteBatch.Begin(
            SpriteSortMode.Immediate,
            BlendState.AlphaBlend,
            SamplerState.LinearClamp,
            DepthStencilState.None,
            RasterizerState.CullNone,
            ozmaShader.Shader,
            Main.GameViewMatrix.TransformationMatrix
        );

        Vector2 origin = new Vector2(bossTexture.Width / 2f, bossTexture.Height / 2f);
        spriteBatch.Draw(bossTexture, NPC.Center - Main.screenPosition, null, Color.White, 0f, origin, NPC.scale, SpriteEffects.None, 0);

        spriteBatch.End();

        spriteBatch.Begin(
            SpriteSortMode.Deferred,
            BlendState.AlphaBlend,
            SamplerState.LinearClamp,
            DepthStencilState.None,
            RasterizerState.CullNone,
            null,
            Main.GameViewMatrix.TransformationMatrix
        );
        return false;
    }
}
