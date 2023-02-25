using Terraria;
using Terraria.ID;
using Terraria.GameInput;
using Terraria.Audio;
using Terraria.ModLoader;
using CalRemix;
using CalRemix.Items.Weapons;

namespace CalRemix
{
    // See Common/Systems/KeybindSystem for keybind registration.
    public class CalRemixKeybindPlayer : ModPlayer
    {
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (CalRemixKeybindSystem.VerbotenGunHotkey.JustPressed && Player.HeldItem.type == ModContent.ItemType<TheVerbotenGun>())
            {
                Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().VerbotenMode += 1;
                SoundEngine.PlaySound(new SoundStyle("CalRemix/Sounds/VerbotenGunClick"));
            }
        }
    }
}