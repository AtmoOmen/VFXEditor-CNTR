using Dalamud.Interface.Internal.Notifications;
using ImGuiNET;
using OtterGui.Classes;
using OtterGui.Raii;

namespace OtterGui.Custom;

public static class CustomGui
{
    public const uint DiscordColor     = 0xFFDA8972;
    public const uint ReniColorButton  = 0xFFCC648D;
    public const uint ReniColorHovered = 0xFFB070B0;
    public const uint ReniColorActive  = 0xFF9070E0;

    /// <summary> Draw a button to open the official discord server. </summary>
    public static void DrawDiscordButton(ChatService chat, float width)
    {
        const string address = @"https://discord.gg/kVva7DHV4r";
        using var    color   = ImRaii.PushColor(ImGuiCol.Button, DiscordColor);
        if (ImGui.Button("Join Discord for Support", new Vector2(width, 0)))
            try
            {
                var process = new ProcessStartInfo(address)
                {
                    UseShellExecute = true,
                };
                Process.Start(process);
            }
            catch
            {
                chat.NotificationMessage($"Unable to open Discord at {address}.", "Error", NotificationType.Error);
            }

        ImGuiUtil.HoverTooltip($"Open {address}");
    }

    /// <summary> Draw the button that opens the ReniGuide. </summary>
    public static void DrawGuideButton(ChatService chat, float width)
    {
        const string address = @"https://reniguide.info/";
        using var color = ImRaii.PushColor(ImGuiCol.Button, ReniColorButton)
            .Push(ImGuiCol.ButtonHovered, ReniColorHovered)
            .Push(ImGuiCol.ButtonActive,  ReniColorActive);
        if (ImGui.Button("Beginner's Guides", new Vector2(width, 0)))
            try
            {
                var process = new ProcessStartInfo(address)
                {
                    UseShellExecute = true,
                };
                Process.Start(process);
            }
            catch
            {
                chat.NotificationMessage($"Could not open guide at {address} in external browser.", "Error",
                    NotificationType.Error);
            }

        ImGuiUtil.HoverTooltip(
            $"Open {address}\nImage and text based guides for most functionality of Penumbra made by Serenity.\n"
          + "Not directly affiliated and potentially, but not usually out of date.");
    }
}
