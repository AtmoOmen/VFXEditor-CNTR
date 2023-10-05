using Dalamud.Interface;
using ImGuiNET;
using OtterGui.Raii;

namespace OtterGui.Widgets;

public sealed class FilterComboColors : FilterComboCache<KeyValuePair<byte, (string Name, uint Color, bool Gloss)>>
{
    private readonly float        _comboWidth;
    private readonly ImRaii.Color _color = new();
    private          Vector2      _buttonSize;
    private          uint         _currentColor = 0;
    private          bool         _currentGloss = false;

    protected override int UpdateCurrentSelected(int currentSelected)
    {
        if (CurrentSelection.Key != _currentColor)
        {
            CurrentSelectionIdx = Items.IndexOf(c => c.Value.Color == _currentColor);
            CurrentSelection    = CurrentSelectionIdx >= 0 ? Items[CurrentSelectionIdx] : default;
            return base.UpdateCurrentSelected(CurrentSelectionIdx);
        }

        return currentSelected;
    }

    public FilterComboColors(float comboWidth, IEnumerable<KeyValuePair<byte, (string Name, uint Color, bool Gloss)>> colors)
        : base(colors)
    {
        _comboWidth   = comboWidth;
        SearchByParts = true;
    }

    protected override float GetFilterWidth()
    {
        // Hack to not color the filter frame.
        _color.Pop();
        return _buttonSize.X + ImGui.GetStyle().ScrollbarSize;
    }

    protected override void DrawList(float width, float itemHeight)
    {
        using var style = ImRaii.PushStyle(ImGuiStyleVar.ItemSpacing, Vector2.Zero)
            .Push(ImGuiStyleVar.WindowPadding, Vector2.Zero)
            .Push(ImGuiStyleVar.FrameRounding, 0);
        _buttonSize = new Vector2(_comboWidth * ImGuiHelpers.GlobalScale, 0);
        if (ImGui.GetScrollMaxY() > 0)
            _buttonSize.X += ImGui.GetStyle().ScrollbarSize;
        base.DrawList(width, itemHeight);
    }

    protected override string ToString(KeyValuePair<byte, (string Name, uint Color, bool Gloss)> obj)
        => obj.Value.Name;

    protected override bool DrawSelectable(int globalIdx, bool selected)
    {
        var (_, (name, color, gloss)) = Items[globalIdx];
        // Push the stain color to type and if it is too bright, turn the text color black.
        var intensity = ImGuiUtil.ColorIntensity(color);
        using var colors = ImRaii.PushColor(ImGuiCol.Button, color, color != 0)
            .Push(ImGuiCol.Text,   0xFF101010, intensity > 127)
            .Push(ImGuiCol.Border, 0xFF2020D0, selected);
        using var style = ImRaii.PushStyle(ImGuiStyleVar.FrameBorderSize, 2f * ImGuiHelpers.GlobalScale, selected);
        var       ret   = ImGui.Button(name, _buttonSize);

        if (gloss)
            ImGui.GetWindowDrawList().AddRectFilledMultiColor(ImGui.GetItemRectMin(), ImGui.GetItemRectMax(), 0x50FFFFFF, 0x50000000,
                0x50FFFFFF, 0x50000000);

        return ret;
    }

    public bool Draw(string label, uint color, string name, bool found, bool gloss, float previewWidth)
    {
        _currentColor = color;
        _currentGloss = gloss;
        var preview = found && ImGui.CalcTextSize(name).X <= previewWidth ? name : string.Empty;

        _color.Push(ImGuiCol.FrameBg, color, found && color != 0)
            .Push(ImGuiCol.Text, 0xFF101010, ImGuiUtil.ColorIntensity(color) > 127 && preview.Length > 0);
        var change = Draw(label, preview, found ? name : string.Empty, previewWidth, ImGui.GetFrameHeight(), ImGuiComboFlags.NoArrowButton);
        return change;
    }

    protected override void PostCombo(float previewWidth)
    {
        _color.Dispose();
        if (_currentGloss)
        {
            var min = ImGui.GetItemRectMin();
            ImGui.GetWindowDrawList().AddRectFilledMultiColor(min, new Vector2(min.X + previewWidth, ImGui.GetItemRectMax().Y), 0x50FFFFFF,
                0x50000000,
                0x50FFFFFF, 0x50000000);
        }
    }

    public bool Draw(string label, uint color, string name, bool found, bool gloss)
        => Draw(label, color, name, found, gloss, ImGui.GetFrameHeight());
}
