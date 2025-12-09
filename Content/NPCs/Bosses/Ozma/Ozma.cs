using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace CalRemix.Content.NPCs.Bosses.Ozma;

public class Ozma : ModNPC
{
    float Time = 0;
    public override void SetDefaults()
    {
        NPC.width = 256;
        NPC.height = 256;
        NPC.damage = 200;
        NPC.LifeMaxNERB(55535, 5553500, 55535);
        if (Main.expertMode)
            NPC.lifeMax /= 2;
        NPC.DR_NERD(0.9f, 0.9f, 0.9f, 0.9f);
        NPC.knockBackResist = 0f;
        NPC.noGravity = true;
        NPC.noTileCollide = true;
        NPC.boss = true;
        NPC.npcSlots = 10f;
        NPC.netAlways = true;
        //Music = -1;
        //SceneEffectPriority = SceneEffectPriority.None;
    }

    public override void OnSpawn(IEntitySource source)
    {
        if (NPC.lifeMax != 55535)
            NPC.lifeMax = 55535;
        NPC.life = NPC.lifeMax;
    }

    public override void AI()
    {
        NPC.velocity = (Main.MouseWorld - NPC.Center) / 30f;
        NPC.rotation *= 0.9f;
        NPC.rotation -= NPC.velocity.X / 128f;

        Time++;
    }

    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        Texture2D bossTexture = TextureAssets.Npc[Type].Value;
        Vector2 origin = bossTexture.Size() * 0.5f;
        Vector2 stretchScale = Vector2.One;// + new Vector2((float)Math.Cos(Time / 15f) / 12f, (float)Math.Cos(Time / 15f) / -12f);

        var ozmaShader = GameShaders.Misc[$"{Mod.Name}:Ozma"];

        Vector2 worldPos = Vector2.Transform(NPC.Center - Main.screenPosition, Main.GameViewMatrix.ZoomMatrix) / Main.ScreenSize.ToVector2();
        ozmaShader.Shader.Parameters["uTime"]?.SetValue(Time);
        ozmaShader.Shader.Parameters["uRotSpeed"]?.SetValue(0.01f);
        ozmaShader.Shader.Parameters["uSwirlStrength"]?.SetValue(3f);
        ozmaShader.Shader.Parameters["uSaturation"]?.SetValue(1.5f);
        ozmaShader.Shader.Parameters["uPixelSize"]?.SetValue(0);
        ozmaShader.Shader.Parameters["uOutlineColor"]?.SetValue(Color.DarkBlue.ToVector3());

        ozmaShader.Shader.Parameters["uRotation"]?.SetValue(new Vector3(-Time / 100f, 0, Time / 100f));

        spriteBatch.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Bosses/Ozma/OzmaLight").Value;
        spriteBatch.GraphicsDevice.SamplerStates[1] = SamplerState.LinearWrap;

        spriteBatch.GraphicsDevice.Textures[2] = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Bosses/Ozma/OzmaDark").Value;
        spriteBatch.GraphicsDevice.SamplerStates[2] = SamplerState.LinearWrap;

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

        spriteBatch.Draw(bossTexture, NPC.Center - Main.screenPosition, null, Color.Transparent, 0f, origin, stretchScale * NPC.scale, SpriteEffects.None, 0);
        
        return false;
    }
}
