using System;
using System.Linq;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;

namespace FirstStardewMod
{
    public class ModEntry : Mod
    {
        /*********
        ** Private properties
        *********/
        private int movementSpeed = 5;

        private readonly int uniqueRunningBuffID = 69420360;

        /*********
        ** Public methods
        *********/
        public override void Entry(IModHelper helper)
        {
            helper.Events.Input.ButtonPressed += this.OnButtonPressed;
        }

        /*********
        ** Private methods
        *********/
        // A hook for the button pressed event
        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            SpeedHack(e.Button);
        }

        // Does all the necessary checks to add the speed buff
        private void SpeedHack(SButton button)
        {
            CheckSpeedChange(button);

            if (IsRunning())
                AddSpeedBuff();
        }

        // Check if the player is in running state
        private bool IsRunning()
        {
            return Game1.player.running || Game1.options.autoRun != Game1.isOneOfTheseKeysDown(
                Game1.GetKeyboardState(),
                Game1.options.runButton
            );
        }

        // Checking if O or P were pressed to increase or decrease player speed
        private void CheckSpeedChange(SButton button)
        {
            switch (button)
            {
                case SButton.O:
                    this.movementSpeed -= 1;
                    Game1.buffsDisplay.removeOtherBuff(this.uniqueRunningBuffID);
                    break;

                case SButton.P:
                    this.movementSpeed += 1;
                    Game1.buffsDisplay.removeOtherBuff(this.uniqueRunningBuffID);
                    break;
            }
        }

        // Adding the speed buff to the player with the wanted movement speed
        private void AddSpeedBuff()
        {
            // Get the first buff with that unique buff ID
            Buff buff = Game1.buffsDisplay.otherBuffs.FirstOrDefault(p => p.which == this.uniqueRunningBuffID);

            // If the buff doesn't exist, add that buff to the player
            if (buff == null)
            {
                Game1.buffsDisplay.addOtherBuff(
                    buff = new Buff(0, 0, 0, 0, 0, 0, 0, 0, 0, speed: this.movementSpeed, 0, 0, minutesDuration: 1, source: "FirstStardewMod", displaySource: "FirstStardewMod")
                    { which = this.uniqueRunningBuffID }
                );
            }
            buff.millisecondsDuration = 1000 * 60;
        }
    }
}
