using GTANetworkAPI;
using System;

namespace Example
{
    public class KeypadLibrary : Script
    {
        private const string CallBackStr = "keyPad_callback";

        [ServerEvent(Event.ResourceStart)]
        public override void OnScriptStart()
        {
            base.OnScriptStart();
        }

        public static void StartKeypadForPlayer(Player player, string title, string subTitle, bool useAsterisk, Action<Player,int> callback)
        {
            if (player == null)
                return;

            player.SetData<dynamic>(CallBackStr, callback);
            player.TriggerEvent("startKeyPad", title, subTitle, useAsterisk);
        }

        [RemoteEvent("sendKeypadValue")]
        public void Event_SendKeypadValue(Player player, object[] args)
        {
            try
            {
                if (args == null || args.Length < 1)
                    return;
                if (args == null || args[0] == null)
                    return;

                int value; 
                if(!Int32.TryParse((string)args[0], out value))
                    return;

                if (player.HasData(CallBackStr))
                {
                    Action<Player, int> action = player.GetData<dynamic>(CallBackStr);
                    if (action != null)
                        action.Invoke(player, value);

                    player.ResetData(CallBackStr);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
