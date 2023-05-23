using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items.Weapons;

public class Exosphear : ModItem
{

    public override void SetStaticDefaults()
    {

        DisplayName.SetDefault("Gravitonomy Pike");
        Tooltip.SetDefault("Fires homing exo pike beams that split into more beams. \n" + "Ignores immunity frames");
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        
    }

    public override void SetDefaults()
    {

        Item.damage = 257;
        Item.crit = 4;
        Item.knockBack = 9.5f;
        Item.useStyle = ItemUseStyleID.Rapier; // Makes the player do the proper arm motion
        Item.DamageType = DamageClass.Melee;
        Item.useAnimation = 18;
        Item.useTime = 18;
        Item.width = 32;
        Item.height = 32;
        Item.UseSound = SoundID.Item1;
        Item.rare = ItemRarityID.Purple;
        Item.autoReuse = true;
        Item.noUseGraphic = true; // The sword is actually a "projectile", so the item should not be visible when used
        Item.noMelee = false; // The projectile will do the damage and not the item
        Item.rare = ItemRarityID.Purple;
        Item.value = Item.buyPrice(0, 80, 0, 0);
        Item.shoot = Mod.Find<ModProjectile>("PikeSpear").Type; // The projectile is what makes a shortsword work
        Item.shootSpeed = 3.5f; // This value bleeds into the behavior of the projectile as velocity, keep that in mind when tweaking values
        
    }
    public override void AddRecipes()
    {
        Mod calamityMod = ModLoader.GetMod("CalamityMod");
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient(calamityMod.Find<ModItem>("StreamGouge").Type);
        recipe.AddIngredient(calamityMod.Find<ModItem>("Nadir").Type);
        recipe.AddIngredient(calamityMod.Find<ModItem>("BansheeHook").Type);
        recipe.AddIngredient(calamityMod.Find<ModItem>("InsidiousImpaler").Type);
        recipe.AddIngredient(calamityMod.Find<ModItem>("MiracleMatter").Type);
       
        recipe.AddTile(calamityMod.Find<ModTile>("DraedonsForge").Type);
        recipe.Register();
    }

   
    public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
    {

        Mod calamityMod = ModLoader.GetMod("CalamityMod");

        if (Item.active == true)
        
            
        {
            int a = Projectile.NewProjectile(spawnSource: null, position, velocity *= 1f, Mod.Find<ModProjectile>("ExoPike").Type, damage *= 1, knockback, player.whoAmI);
            Main.projectile[a].velocity *= 5f;
        }
      
    }


    

    public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
    {


        Mod calamityMod = ModLoader.GetMod("CalamityMod");
        target.AddBuff(calamityMod.Find<ModBuff>("ExoFreeze").Type, 150);
        target.AddBuff(calamityMod.Find<ModBuff>("HolyFlames").Type, 150);
        target.AddBuff(BuffID.Frostburn, 150);
        target.AddBuff(BuffID.OnFire, 150);
    }






}

