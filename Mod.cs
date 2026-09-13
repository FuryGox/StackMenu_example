using HarmonyLib;
using StackMenu;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ExampleModNS
{
    public class ExampleMod : Mod
    {
        public static ConfigEntry<string>? ExampleKeyConfig;
        public static ConfigEntry<string>? ExampleColorConfig;

        public static Key ExampleKey => StackMenuAPI.ParseKey(ExampleKeyConfig, Key.H);
        public static Color ExampleColor => StackMenuAPI.ParseColor(ExampleColorConfig, Color.green);

        public override void Ready()
        {
            Logger.Log("ExampleMod Ready!");

            // Use StackMenuAPI.SetupKeybindConfig to create an interactive Keybind button in Mod Options
            ExampleKeyConfig = StackMenuAPI.SetupKeybindConfig(
                mod: this,
                name: "example_custom_key",
                displayName: "Example Action Key",
                tooltip: "Shortcut key for example action in StackMenu",
                defaultKey: Key.H,
                onKeyChanged: (newKey) => Logger.Log($"Example keybind changed to: {newKey}")
            );

            // Use StackMenuAPI.SetupColorConfig to create an interactive Color palette picker in Mod Options
            ExampleColorConfig = StackMenuAPI.SetupColorConfig(
                mod: this,
                name: "example_custom_color",
                displayName: "Example Highlight Color",
                tooltip: "Custom color chosen from the palette",
                defaultColor: new Color(0.298f, 0.686f, 0.314f, 1f),
                onColorChanged: (newColor) => Logger.Log($"Example color changed to: {StackMenuAPI.ColorToHex(newColor)}")
            );

            // 1. Add a custom separator (Priority determines ordering, higher = appears higher up)
            StackMenuAPI.RegisterSeparator(
                condition: (card) => card != null && card.CardData != null,
                priority: 10
            );

            // 2. Add an action with:
            //    - Condition to show menu (e.g., only on Combatable cards)
            //    - Dynamic shortcut key linked to the keybind config created above
            StackMenuAPI.RegisterAction(
                label: "Heal Card (+10 HP)",
                onClick: (card) =>
                {
                    if (card.CardData is Combatable combatable)
                    {
                        combatable.HealthPoints += 10;
                        Logger.Log($"Healed {card.CardData.Name} by 10 HP!");
                    }
                    else
                    {
                        Logger.Log($"Card {card.CardData.Name} is not combatable.");
                    }
                },
                condition: (card) => card != null && card.CardData is Combatable,
                dynamicShortcutKey: () => ExampleKey,
                dynamicShortcutLabel: () => ExampleKey.ToString(),
                priority: 9
            );

            // 3. Add an action with:
            //    - Dynamic label showing card name
            //    - Condition (only when stack has more than 1 card)
            StackMenuAPI.RegisterAction(
                dynamicLabel: (card) => $"Log Stack Info ({card.GetAllCardsInStack().Count} cards)",
                onClick: (card) =>
                {
                    var stack = card.GetAllCardsInStack();
                    Logger.Log($"Stack contains {stack.Count} cards starting with {card.CardData.Name}. Color config is {ExampleColor}");
                },
                condition: (card) => card != null && card.GetAllCardsInStack().Count > 1,
                shortcutKey: Key.L,
                shortcutLabel: "L",
                priority: 8
            );

            // 4. Add a dynamic provider (generates multiple custom items & separators on-the-fly)
            StackMenuAPI.RegisterProvider((card) =>
            {
                var list = new List<CustomMenuItem>();

                // Only add extra options if hovering a coin
                if (card != null && card.CardData != null && card.CardData.Id == "coin")
                {
                    list.Add(CustomMenuItem.CreateSeparator());
                    list.Add(CustomMenuItem.CreateAction(
                        label: "Coin Special Action",
                        onClick: (c) => Logger.Log("Interacted with Coin!"),
                        shortcutKey: Key.K,
                        shortcutLabel: "K"
                    ));
                }

                return list;
            });
        }
    }
}