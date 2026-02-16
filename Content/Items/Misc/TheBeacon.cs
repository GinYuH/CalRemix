using Terraria.ModLoader;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace CalRemix.Content.Items.Misc
{
    public class TheBeacon : ModItem
    {
        private bool hasPlayedSound = false;
        public static readonly SoundStyle Beacon = new("CalRemix/Assets/Sounds/Beacon")
        {
            MaxInstances = 0
        };
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 10;
            Item.useAnimation = 30;
            Item.rare = ItemRarityID.Yellow;
        }
        public override void UpdateInventory(Player player)
        {
            if (!hasPlayedSound)
            {
                SoundEngine.PlaySound(Beacon, player.position);
                hasPlayedSound = true;
            }
        }
    }
}
