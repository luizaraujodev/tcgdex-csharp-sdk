using System.Text.Json;

namespace TCGDex.Models;

/// <summary>
/// Base for TCGDex models; holds SDK reference and supports Build/Fill from JSON.
/// </summary>
public abstract class ModelBase
{
    protected TCGDexClient Sdk { get; }

    protected ModelBase(TCGDexClient sdk)
    {
        Sdk = sdk ?? throw new ArgumentNullException(nameof(sdk));
    }

    /// <summary>
    /// Builds a model instance by deserializing JSON into the given model type and returning it.
    /// </summary>
    public static T Build<T>(T model, JsonElement data) where T : ModelBase
    {
        if (model == null) throw new ArgumentNullException(nameof(model));
        model.Fill(data);
        return model;
    }

    /// <summary>
    /// Override to customize how JSON is applied to the model (e.g. nested list deserialization).
    /// </summary>
    protected virtual void Fill(JsonElement data)
    {
        // Default: use reflection-free approach; subclasses override for nested types.
        foreach (var prop in data.EnumerateObject())
        {
            var pi = GetType().GetProperty(ToPascalCase(prop.Name));
            if (pi == null || !pi.CanWrite) continue;
            var value = prop.Value;
            if (value.ValueKind == JsonValueKind.Null || value.ValueKind == JsonValueKind.Undefined)
                continue;
            try
            {
                var converted = JsonValueToObject(value, pi.PropertyType);
                if (converted != null)
                    pi.SetValue(this, converted);
            }
            catch
            {
                // Skip properties we can't set
            }
        }
    }

    private static string ToPascalCase(string name)
    {
        if (string.IsNullOrEmpty(name)) return name;
        return char.ToUpperInvariant(name[0]) + name[1..];
    }

    private static object? JsonValueToObject(JsonElement el, Type targetType)
    {
        switch (el.ValueKind)
        {
            case JsonValueKind.String:
                return el.GetString();
            case JsonValueKind.Number:
                if (targetType == typeof(int) || targetType == typeof(int?))
                    return el.TryGetInt32(out var i) ? i : null;
                if (targetType == typeof(long) || targetType == typeof(long?))
                    return el.TryGetInt64(out var l) ? l : null;
                if (targetType == typeof(double) || targetType == typeof(double?))
                    return el.TryGetDouble(out var d) ? d : null;
                return el.TryGetInt32(out var i2) ? (object)i2 : el.GetDouble();
            case JsonValueKind.True:
                return true;
            case JsonValueKind.False:
                return false;
            case JsonValueKind.Object:
                return el; // Caller may deserialize to specific type
            case JsonValueKind.Array:
                return el;
            default:
                return null;
        }
    }
}
