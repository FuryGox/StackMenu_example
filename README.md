# StackMenu Example Mod

This is an example mod demonstrating how to use `StackMenuAPI` from another Stacklands mod.

### Features Demonstrated:
1. **Interactive Keybind Config UI**: `StackMenuAPI.SetupKeybindConfig`
2. **Interactive Color Picker Config UI**: `StackMenuAPI.SetupColorConfig`
3. **Conditions to show menu items**: `condition: (card) => ...`
4. **Shortcut Keys**: static `Key` and config-bound `Func<Key?>`
5. **Separators**: static separators and conditional separators with `priority`
6. **Dynamic Menu Providers**: `StackMenuAPI.RegisterProvider(...)`

See [Mod.cs](Mod.cs) for full code.