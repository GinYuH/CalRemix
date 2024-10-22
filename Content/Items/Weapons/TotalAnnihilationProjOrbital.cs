using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    // Example Advanced Flail is a complete adaption of Ball O' Hurt projectile. The code has been rewritten a bit to make it easier to follow. Compare this code against the decompiled Terraria code for an example of adapting vanilla code. A few comments and extra code snippets show features from other vanilla flails as well.
    // Example Advanced Flail shows a plethora of advanced AI and collision topics.
    // See ExampleFlail for a simpler but less customizable flail projectile example.
    public class TotalAnnihilationProjOrbital : ModProjectile
    {
        private const string ChainTexturePath = "CalRemix/Content/Projectiles/Weapons/TotalAnnihilationChain"; // The folder path to the flail chain sprite
        private const string ChainTextureExtraPath = "CalRemix/Content/Projectiles/Weapons/TotalAnnihilationChain2";  // This texture and related code is optional and used for a unique effect

        private static Asset<Texture2D> chainTexture;

        public ref float ParentIndex => ref Projectile.ai[0];
        public ref float SpeedModifier => ref Projectile.localAI[2];

        public override void Load()
        {
            chainTexture = ModContent.Request<Texture2D>(ChainTexturePath);
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.ai[2] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true; // This ensures that the projectile is synced when other players join the world.
            Projectile.width = 44; // The width of your projectile
            Projectile.height = 40; // The height of your projectile
            Projectile.friendly = true; // Deals damage to enemies
            Projectile.penetrate = -1; // Infinite pierce
            Projectile.DamageType = DamageClass.Melee; // Deals melee damage
            Projectile.usesLocalNPCImmunity = true; // Used for hit cooldown changes in the ai hook
            Projectile.localNPCHitCooldown = 10; // This facilitates custom hit cooldown logic
            Projectile.tileCollide = false;

            Main.projFrames[ModContent.ProjectileType<TotalAnnihilationProjOrbital>()] = 5;

            // Vanilla flails all use aiStyle 15, but the code isn't customizable so an adaption of that aiStyle is used in the AI method
        }

        // This AI code was adapted from vanilla code: Terraria.Projectile.AI_015_Flails() 
        public override void AI()
        {
            // onspawn is one of the seven wonders of the world so we cant use it for this
            if (Projectile.ai[2] == 0)
            {
                switch (Projectile.frame)
                {
                    case 0: // melee
                        int ranged = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<TotalAnnihilationProjOrbital>(), 0, 0, -1, Projectile.whoAmI);
                        Main.projectile[ranged].frame = 1;
                        Main.projectile[ranged].localAI[2] = 0.6f;
                        break;
                    case 1: // ranged
                        int mage = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<TotalAnnihilationProjOrbital>(), 0, 0, -1, Projectile.whoAmI);
                        Main.projectile[mage].frame = 2;
                        Main.projectile[mage].localAI[2] = 1.2f;
                        break;
                    case 2: // mage
                        int summoner = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<TotalAnnihilationProjOrbital>(), 0, 0, -1, Projectile.whoAmI);
                        Main.projectile[summoner].frame = 3;
                        Main.projectile[summoner].localAI[2] = 0.75f;
                        break;
                    case 3: // summoner
                        int rogue = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<TotalAnnihilationProjOrbital>(), 0, 0, -1, Projectile.whoAmI);
                        Main.projectile[rogue].frame = 4;
                        Main.projectile[rogue].localAI[2] = 1.4f;
                        break;
                    case 4: // dogshit
                        // do nothing bcuz STUPID
                        break;
                }
                Projectile.ai[2] = 1;
            }
            
            Projectile parent = Main.projectile[(int)ParentIndex];
            // Kill
            if (!parent.active)
            {
                Projectile.Kill();
                return;
            }

            // the following code is ripped from goozmaga to give this item maximum spiritual dogshit strength
            Projectile.velocity = Vector2.Zero;
            Projectile.localAI[1]++;
            float distance = 50;
            double deg = Main.GlobalTimeWrappedHourly * 360 * (1 + Math.Clamp(Projectile.localAI[0], 0, 0.5f)) + Projectile.localAI[1] * SpeedModifier;
            double rad = deg * (Math.PI / 180);
            float hyposx = parent.Center.X - (int)(Math.Cos(rad) * distance) - Projectile.width / 2;
            float hyposy = parent.Center.Y - (int)(Math.Sin(rad) * distance) - Projectile.height / 2;

            Projectile.position = new Vector2(hyposx, hyposy);
        }

        // PreDraw is used to draw a chain and trail before the projectile is drawn normally.
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile parent = Main.projectile[(int)ParentIndex];
            Vector2 playerArmPosition = parent.Center;

            Rectangle? chainSourceRectangle = null;
            // Drippler Crippler customizes sourceRectangle to cycle through sprite frames: sourceRectangle = asset.Frame(1, 6);
            float chainHeightAdjustment = 0f; // Use this to adjust the chain overlap. 

            Vector2 chainOrigin = chainSourceRectangle.HasValue ? (chainSourceRectangle.Value.Size() / 2f) : (chainTexture.Size() / 2f);
            Vector2 chainDrawPosition = Projectile.Center;
            Vector2 vectorFromProjectileToPlayerArms = playerArmPosition.MoveTowards(chainDrawPosition, 4f) - chainDrawPosition;
            Vector2 unitVectorFromProjectileToPlayerArms = vectorFromProjectileToPlayerArms.SafeNormalize(Vector2.Zero);
            float chainSegmentLength = (chainSourceRectangle.HasValue ? chainSourceRectangle.Value.Height : chainTexture.Height()) + chainHeightAdjustment;
            if (chainSegmentLength == 0)
            {
                chainSegmentLength = 10; // When the chain texture is being loaded, the height is 0 which would cause infinite loops.
            }
            float chainRotation = unitVectorFromProjectileToPlayerArms.ToRotation() + MathHelper.PiOver2;
            int chainCount = 0;
            float chainLengthRemainingToDraw = vectorFromProjectileToPlayerArms.Length() + chainSegmentLength / 2f;

            // This while loop draws the chain texture from the projectile to the player, looping to draw the chain texture along the path
            while (chainLengthRemainingToDraw > 0f)
            {
                // This code gets the lighting at the current tile coordinates
                Color chainDrawColor = Lighting.GetColor((int)chainDrawPosition.X / 16, (int)(chainDrawPosition.Y / 16f));

                // Flaming Mace and Drippler Crippler use code here to draw custom sprite frames with custom lighting.
                // Cycling through frames: sourceRectangle = asset.Frame(1, 6, 0, chainCount % 6);
                // This example shows how Flaming Mace works. It checks chainCount and changes chainTexture and draw color at different values

                var chainTextureToDraw = chainTexture;

                // Here, we draw the chain texture at the coordinates
                Main.spriteBatch.Draw(chainTextureToDraw.Value, chainDrawPosition - Main.screenPosition, chainSourceRectangle, chainDrawColor, chainRotation, chainOrigin, 1f, SpriteEffects.None, 0f);

                // chainDrawPosition is advanced along the vector back to the player by the chainSegmentLength
                chainDrawPosition += unitVectorFromProjectileToPlayerArms * chainSegmentLength;
                chainCount++;
                chainLengthRemainingToDraw -= chainSegmentLength;
            }
            return true;
        }
    }
}