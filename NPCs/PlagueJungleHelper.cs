using Terraria;

namespace CalRemix.NPCs
{
    public sealed class PlagueJungleHelper
    {
        private bool _appliedAtAll;
        private bool[] _active = new bool[Main.maxPlayers];
        private bool[] _jungle = new bool[Main.maxPlayers];

        public bool CurrentlyActive()
        {
            return _appliedAtAll;
        }

        public bool CurrentlyApplied(Player player)
        {
            return _active[player.whoAmI];
        }

        public void ApplyJunglePlagueSwappies(Player player)
        {
            if (player.GetModPlayer<CalRemixPlayer>().ZonePlague)
            {
                _appliedAtAll = true;
                _active[player.whoAmI] = true;
                _jungle[player.whoAmI] = player.ZoneJungle;
                player.ZoneJungle = true;
            }
        }

        public void ClearAllJunglePlagueSwappies()
        {
            _appliedAtAll = false;
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                _active[i] = false;
                Main.player[i].ZoneJungle = _jungle[i];
            }
        }
    }
}
