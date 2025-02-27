using System.Collections.Generic;
using CalamityMod.Items;
using CalamityMod.Rarities;
using Microsoft.Xna.Framework;
using CalRemix.Content.NPCs.Bosses.Noxus;
using CalRemix.Content.NPCs;
using CalRemix.Content.NPCs.Minibosses;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Projectiles;

namespace CalRemix.Content.Items.Tools
{
    public class NoxusSprayer : ModItem
    {
        public static List<int> NPCsToNotDelete => new()
        {
            NPCID.CultistTablet,
            NPCID.DD2LanePortal,
            NPCID.DD2EterniaCrystal,
            NPCID.TargetDummy,
            ModContent.NPCType<LaRuga>(),
            ModContent.NPCType<NoxusEgg>(),
            ModContent.NPCType<NoxusEggCutscene>(),
            ModContent.NPCType<EntropicGod>(),
        };

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 34;
            Item.useAnimation = 2;
            Item.useTime = 2;
            Item.autoReuse = true;
            Item.noMelee = true;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item20 with { MaxInstances = 50, Volume = 0.3f };

            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
            Item.rare = ModContent.RarityType<Violet>();

            Item.shoot = ModContent.ProjectileType<NoxusSprayerGas>();
            Item.shootSpeed = 7f;
        }

        public override Vector2? HoldoutOffset()
        {
            return new(0f, 4f);
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position -= velocity * 4f;
        }
    }
}
