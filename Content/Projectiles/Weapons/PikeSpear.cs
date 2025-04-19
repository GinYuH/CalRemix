using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;


namespace CalRemix.Content.Projectiles.Weapons;

// Shortsword projectiles are handled in a special way with how they draw and damage things
// The "hitbox" itself is closer to the player, the sprite is centered on it
// However the interactions with the world will occur offset from this hitbox, closer to the sword's tip (CutTiles, Colliding)
// Values chosen mostly correspond to Iron Shortword
public class PikeSpear : ModProjectile
{
    public const int FadeInDuration = 7;
    public const int FadeOutDuration = 4;

    public const int TotalDuration = 16;

    // The "width" of the blade
    public float CollisionWidth => 10f * Projectile.scale;

    public int Timer
    {
        get => (int)Projectile.ai[0];
        set => Projectile.ai[0] = value;
    }

    public override void SetStaticDefaults()
    {
        // DisplayName.SetDefault("Exo shiv");
    }

    public override void SetDefaults()
    {
        Projectile.width = 38;
        Projectile.height = 38;
        
        Projectile.aiStyle = -1; // Use our own AI to customize how it behaves, if you don't want that, keep this at ProjAIStyleID.ShortSword. You would still need to use the code in SetVisualOffsets() though
        Projectile.friendly = true;
        Projectile.penetrate = -1;
        Projectile.tileCollide = false;
        Projectile.scale = 1.5f;
        Projectile.ownerHitCheck = true; // Prevents hits through tiles. Most melee weapons that use projectiles have this
        Projectile.extraUpdates = 1; // Update 1+extraUpdates times per tick
        Projectile.timeLeft = 30; // This value does not matter since we manually kill it earlier, it just has to be higher than the duration we use in AI
        Projectile.hide = true; // Important when used alongside player.heldProj. "Hidden" projectiles have special draw conditions
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = 10;
        Projectile.DamageType = DamageClass.Melee;
    }
    public float movementFactor // Change this value to alter how fast the spear moves
    {
        get => Projectile.ai[0];
        set => Projectile.ai[0] = value;
    }
    public override void AI()
    {


        // Since we access the owner player instance so much, it's useful to create a helper local variable for this
        // Sadly, Projectile/ModProjectile does not have its own
        Player projOwner = Main.player[Projectile.owner];
        // Here we set some of the projectile's owner properties, such as held item and itemtime, along with projectile direction and position based on the player
        Vector2 ownerMountedCenter = projOwner.RotatedRelativePoint(projOwner.MountedCenter, true);
        Projectile.direction = projOwner.direction;
        projOwner.heldProj = Projectile.whoAmI;
        projOwner.itemTime = projOwner.itemAnimation;
        Projectile.position.X = ownerMountedCenter.X - (float)(Projectile.width / 2);
        Projectile.position.Y = ownerMountedCenter.Y - (float)(Projectile.height / 2);
        // As long as the player isn't frozen, the spear can move
        if (!projOwner.frozen)
        {
            if (movementFactor == 0f) // When initially thrown out, the ai0 will be 0f
            {
                movementFactor = 3f; // Make sure the spear moves forward when initially thrown out
                Projectile.netUpdate = true; // Make sure to netUpdate this spear
            }
            if (projOwner.itemAnimation < projOwner.itemAnimationMax / 3) // Somewhere along the item animation, make sure the spear moves back
            {
                movementFactor -= 2.4f;
            }
            else // Otherwise, increase the movement factor
            {
                movementFactor += 2.1f;
            }
        }
        // Change the spear position based off of the velocity and the movementFactor
        Projectile.position += Projectile.velocity * movementFactor;
        // When we reach the end of the animation, we can kill the spear projectile
        if (projOwner.itemAnimation == 0)
        {
            Projectile.Kill();
        }
        // Apply proper rotation, with an offset of 135 degrees due to the sprite's rotation, notice the usage of MathHelper, use this class!
        // MathHelper.ToRadians(xx degrees here)
      
        Projectile.rotation = Projectile.velocity.ToRotation() + 3 * MathHelper.PiOver4;

    }
      

    public override void CutTiles()
    {
        // "cutting tiles" refers to breaking pots, grass, queen bee larva, etc.
        DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
        Vector2 start = Projectile.Center;
        Vector2 end = start + Projectile.velocity.SafeNormalize(-Vector2.UnitY) * 10f;
        Utils.PlotTileLine(start, end, CollisionWidth, DelegateMethods.CutTiles);
    }

    public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
    {
        // "Hit anything between the player and the tip of the sword"
        // shootSpeed is 2.1f for reference, so this is basically plotting 12 pixels ahead from the center
        Vector2 start = Projectile.Center;
        Vector2 end = start + Projectile.velocity * 6f;
        float collisionPoint = 0f; // Don't need that variable, but required as parameter
        return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, CollisionWidth, ref collisionPoint);
    }



    


    public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
    {
        for (int i = 0; i < 200; i++)
        {
            Player player = Main.player[i];




        }

        Mod calamityMod = CalRemix.CalMod;
        target.AddBuff(calamityMod.Find<ModBuff>("HolyFlames").Type, 150);
        target.AddBuff(BuffID.Frostburn, 150);
        target.AddBuff(BuffID.OnFire, 150);


        SoundEngine.PlaySound(SoundID.Item116);
      
        

    }

}

