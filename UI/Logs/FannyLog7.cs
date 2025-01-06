namespace CalRemix.UI.Logs
{
    public class FannyLog7 : TerminalUI
    {
        public override int TotalPages => 3;
        public override string GetTextByPage()
        {
            return Page switch
            {
                0 => CalRemixHelper.LocalText("Lore.FannyLog7.0").Value,
                1 => CalRemixHelper.LocalText("Lore.FannyLog7.1").Value,
                _ => CalRemixHelper.LocalText("Lore.FannyLog7.2").Value,
            };
        }
    }
}
