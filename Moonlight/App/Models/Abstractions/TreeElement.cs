namespace Moonlight.App.Models.Abstractions;

public class TreeElement
{
    public TreeElement()
    {}

    public TreeElement(string name)
    {
        Name = name;
    }
    
    public string Name { get; set; }
    public string Icon { get; set; } = "bx bx-sm bxs-square";
    public bool IsExpanded { get; set; } = false;
    public Func<Task>? OnClick { get; set; }
    public List<TreeElement> Elements { get; set; } = new();
}