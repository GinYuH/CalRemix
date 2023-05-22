using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles.Weapons;

// Shortsword projectiles are handled in a special way with how they draw and damage things
// The "hitbox" itself is closer to the player, the sprite is centered on it
// However the interactions with the world will occur offset from this hitbox, closer to the sword's tip (CutTiles, Colliding)
// Values chosen mostly correspond to Iron Shortword
public class ExoPikePhantom : ModProjectile
{
    public override string Texture => "CalRemix/Projectiles/Weapons/ExoPike";
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
        // DisplayName.SetDefault("Gravitonomy Beam");
      
    }

    public override void SetDefaults()
    {
        Projectile.width = 10;
        Projectile.height = 10;
       
        Projectile.friendly = true;
        Projectile.penetrate = -1;
        Projectile.tileCollide = false;


      
        Projectile.timeLeft = 360; // This value does not matter since we manually kill it earlier, it just has to be higher than the duration we use in AI
      
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = 0;
        Projectile.aiStyle = 0;
        Projectile.DamageType = DamageClass.Melee;
    }
    public override bool PreDraw(ref Color lightColor)
    {
        Mod calamityMod = ModLoader.GetMod("CalamityMod");
        GameShaders.Armor.GetShaderFromItemId(calamityMod.Find<ModItem>("ExoDye").Type);
        return true;
    }
    public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
    {


        SoundEngine.PlaySound(SoundID.Item109, Projectile.Center);
        Mod calamityMod = ModLoader.GetMod("CalamityMod");

    
       
    }
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        Mod calamityMod = ModLoader.GetMod("CalamityMod");
        target.AddBuff(calamityMod.Find<ModBuff>("ExoFreeze").Type, 150);
        target.AddBuff(calamityMod.Find<ModBuff>("HolyFlames").Type, 150);
        target.AddBuff(BuffID.Frostburn, 150);
        target.AddBuff(BuffID.OnFire, 150);
    }
    public override void AI()
    {

        Player player = Main.player[Projectile.owner];
        Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        Lighting.AddLight(Projectile.Center, TorchID.Rainbow);
        for (int i = 0; i < 1; i++)
        {
            int DustID = Dust.NewDust(Projectile.position, Projectile.width + 2, Projectile.height + 2, 43, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 255, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 1f);
            Main.dust[DustID].noGravity = true;
            Main.dust[DustID].scale *= 1.00f;
            Main.dust[DustID].noLight = false;
        }
   
        }
    }


