using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Moonlight.App.Helpers;

public static class ComponentHelper
{
    public static RenderFragment FromType(Type type, Action<Dictionary<string, object>>? buildAttributes = null) => builder =>
    {
        builder.OpenComponent(0, type);

        if (buildAttributes != null)
        {
            Dictionary<string, object> parameters = new();
            buildAttributes.Invoke(parameters);

            int i = 1;

            foreach (var parameter in parameters)
            {
                if (type.GetProperties().All(x => x.Name != parameter.Key))
                    continue;
                
                builder.AddAttribute(i, parameter.Key, parameter.Value);
                
                i++;
            }
        }
        
        builder.CloseComponent();
    };

    public static RenderFragment FromType<T>(Action<Dictionary<string, object>>? buildAttributes = null) where T : ComponentBase =>
        FromType(typeof(T), buildAttributes);
    
    public static RenderFragment FromTypeCascading(Type type, Action<Dictionary<string, object>> buildAttributes) => builder =>
    {
        Dictionary<string, object> parameters = new();
        buildAttributes.Invoke(parameters);

        var items = parameters.ToList();
        
        var fragment = BuildCascadingParameter(items, 0, type);
        
        builder.AddContent(0, fragment);
    };

    private static RenderFragment BuildCascadingParameter(List<KeyValuePair<string, object>> parameters, int skip, Type childContent) => builder =>
    {
        var isEndOfItems = parameters.Skip(0).Any();

        if (isEndOfItems)
        {
            builder.OpenComponent(0, childContent);
            builder.CloseComponent();
        }
        else
        {
            var itemToUse = parameters.Skip(0).First();
            
            var cascadingParameterType = typeof(CascadingValue<>);
            cascadingParameterType = cascadingParameterType.MakeGenericType(itemToUse.Value.GetType());
        
            builder.OpenComponent(0, cascadingParameterType);
            builder.AddAttribute(1, "Value", itemToUse.Value);
            builder.AddAttribute(2, "Name", itemToUse.Key);
            builder.AddAttribute(3, "ChildContent", BuildCascadingParameter(parameters, skip + 1, childContent));
            builder.CloseComponent();
        }
    };
}