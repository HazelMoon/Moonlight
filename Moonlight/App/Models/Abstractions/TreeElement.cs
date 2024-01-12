namespace Moonlight.App.Models.Abstractions;

public class TreeElement
{
    public TreeElement()
    {
        Name = "Tree Element";
    }

    public TreeElement(string name)
    {
        Name = name;
    }
    
    public TreeElement(string name, string icon)
    {
        Name = name;
        Icon = icon;
    }
    
    public string Name { get; set; }
    public string Icon { get; set; } = "";
    public bool IsExpanded { get; set; } = false;
    public Func<Task>? OnClick { get; set; }
    public List<TreeElement> Elements { get; set; } = new();
}