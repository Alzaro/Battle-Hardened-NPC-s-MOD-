using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace BattleHardened
{
	public class BattleHardened : ModPlayer, ILoadable
	{
        private bool immortal;
        private uint timer;
        bool isInitialized = false;
        public static ModKeybind keybind { get; private set; }

        public override void Load()
        {
            base.Load();
            keybind = KeybindLoader.RegisterKeybind(Mod, "Immortal", Microsoft.Xna.Framework.Input.Keys.X);
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            base.ProcessTriggers(triggersSet);
            

            if(keybind == default || !keybind.JustPressed || timer != 0)
            {
                return;
            }

            immortal = !immortal;
            timer = 10;
            ImmortalNpc();
        }

        public override void PostUpdate()
        {
            base.PostUpdate();
            Countdown();
            if(!isInitialized)
            {
                immortal = true;
                timer = 0;
                ImmortalNpc();
                isInitialized = true;
            }
        }

        public void Countdown()
        {
            if (timer == 0)
                return;
            --timer;
        }

        public void ImmortalNpc()
        {
            foreach(NPC npc in Main.npc)
            {
                if(npc.townNPC)
                {
                    npc.immortal = immortal;
                }
            }

            if (immortal && Main.netMode != NetmodeID.Server)
            {
                Main.NewText("Your NPC's are now immortal!", byte.MaxValue, byte.MaxValue, byte.MaxValue);
            }
            else
            {
                Main.NewText("Your NPC's are no longer immortal!", byte.MaxValue, byte.MaxValue, byte.MaxValue);
            }
        }

        public override void Unload()
        {
            keybind = null;
        }
    }
}