using CalamityMod;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalRemix.Content.Cooldowns;
using CalRemix.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Weapons;

public class GearworkShield : ModItem
{
    private bool recorded = false;
    private bool stealth = false;
    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("Gearwork Shield");
        Item.ResearchUnlockCount = 1;
    }
    public override void SetDefaults()
    {
        Item.damage = 167;
        Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
        Item.width = 1;
        Item.height = 1;
        Item.useTime = 23;
        Item.useAnimation = 23;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.noMelee = true;
        Item.noUseGraphic = true;
        Item.knockBack = 4.25f;
        Item.rare = ModContent.RarityType<PureGreen>();
        Item.value = Item.sellPrice(gold: 20);
        Item.UseSound = SoundID.Item1;
        Item.autoReuse = true;
        Item.shoot = ModContent.ProjectileType<GearShield>();
        Item.shootSpeed = 13f;
    }
    public override bool AltFunctionUse(Player player)
    {
        return true;
    }
    public override bool CanUseItem(Player player)
    {
        if (player.altFunctionUse == 2)
        {
            if (!player.HasCooldown(GearworkCooldown.ID) && !recorded)
            {
                stealth = false;
                Item.useStyle = ItemUseStyleID.Swing;
                Item.noMelee = true;
                Item.noUseGraphic = true;
                Item.UseSound = SoundID.DoorOpen;
                CombatText.NewText(player.getRect(), Color.Olive, Language.GetOrRegister("Mods.CalRemix.StatusText.GearworkRecord").Value, true);
                foreach (NPC npc in Main.npc)
                {
                    if (npc is null)
                        continue;
                    if (!npc.IsAnEnemy(true) || player.Distance(npc.Center) > 3200)
                        continue;
                    for (int i = 0; i < 4; i++)
                        npc.GetGlobalNPC<CalRemixNPC>().storedAI[i] = npc.ai[i];
                    for (int i = 0; i < 4; i++)
                        npc.GetGlobalNPC<CalRemixNPC>().storedLocalAI[i] = npc.localAI[i];
                    for (int i = 0; i < 4; i++)
                        npc.GetGlobalNPC<CalRemixNPC>().storedCalAI[i] = npc.Calamity().newAI[i];
                    for (int i = 0; i < 22; i++)
                        npc.GetGlobalNPC<CalRemixNPC>().storedGreenAI[i] = npc.Remix().GreenAI[i];
                }
                recorded = true;
            }
            else if (player.HasCooldown(GearworkCooldown.ID))
                CombatText.NewText(player.getRect(), Color.LightSalmon, Language.GetOrRegister("Mods.CalRemix.StatusText.GearworkCooldown").Value, true);
        }
        else
        {
            if (!player.HasCooldown(GearworkCooldown.ID) && recorded)
            {
                Item.useStyle = ItemUseStyleID.Shoot;
                Item.noUseGraphic = false;
                Item.UseSound = SoundID.DoorClosed;
                CombatText.NewText(player.getRect(), Color.Green, Language.GetOrRegister("Mods.CalRemix.StatusText.GearworkApply").Value, true);
                foreach (NPC npc in Main.npc)
                {
                    if (npc is null)
                        continue;
                    if (!npc.IsAnEnemy(true) || player.Distance(npc.Center) > 3200)
                        continue;
                    for (int i = 0; i < 4; i++)
                        npc.ai[i] = npc.GetGlobalNPC<CalRemixNPC>().storedAI[i];
                    for (int i = 0; i < 4; i++)
                        npc.localAI[i] = npc.GetGlobalNPC<CalRemixNPC>().storedLocalAI[i];
                    for (int i = 0; i < 4; i++)
                        npc.Calamity().newAI[i] = npc.GetGlobalNPC<CalRemixNPC>().storedCalAI[i];
                    for (int i = 0; i < 22; i++)
                        npc.Remix().storedGreenAI[i] = npc.GetGlobalNPC<CalRemixNPC>().storedGreenAI[i];
                    if (player.Calamity().StealthStrikeAvailable())
                    {
                        npc.StrikeNPC(new NPC.HitInfo { Damage = Item.damage * 20 });
                        stealth = true;
                    }
                }
                recorded = false;
                player.AddCooldown(GearworkCooldown.ID, CalamityUtils.SecondsToFrames(60));
            }
            else if (!player.HasCooldown(GearworkCooldown.ID) && !recorded)
                CombatText.NewText(player.getRect(), Color.LightSalmon, Language.GetOrRegister("Mods.CalRemix.StatusText.GearworkNA").Value, true);
            else if (player.HasCooldown(GearworkCooldown.ID))
            {
                Item.useStyle = ItemUseStyleID.Swing;
                Item.noUseGraphic = true;
                Item.UseSound = SoundID.Item1;
            }
        }
        return true;
    }
    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        if (Main.LocalPlayer.HasCooldown(GearworkCooldown.ID))
        {
            if (!stealth)
                return true;
            Projectile proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
            proj.Calamity().stealthStrike = true;
        }
        return false;
    }
    public override void AddRecipes()
    {
        CreateRecipe().
            AddIngredient<OrnateShield>(1).
            AddIngredient(ItemID.EoCShield).
            AddIngredient<RuinousSoul>(5).
            AddTile(TileID.LunarCraftingStation).
            Register();
    }

}

