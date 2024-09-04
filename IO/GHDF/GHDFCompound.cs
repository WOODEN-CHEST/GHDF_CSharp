using System.Collections;

namespace GHEngine.IO.GHDF;

public class GHDFCompound : IEnumerable<KeyValuePair<ulong, object>>
{
    // Fields.
    public int Count => _entries.Count;

    public object this[ulong id]
    {
        get => _entries[id];
        set => Add(id, value);
    }

    public IEnumerable<ulong> Keys => _entries.Keys;
    public IEnumerable<object> Values => _entries.Values;


    // Private fields.
    private readonly Dictionary<ulong, object> _entries = new();


    // Methods.
    public void Add(ulong id, object value)
    {
        if (!IsValidType(value))
        {
            throw new GHDFEntryException("Invalid type for entry in compound.");
        }
        _entries[id] = value;
    }

    public void Remove(ulong id)
    {
        _entries.Remove(id);
    }

    public bool Get<T>(ulong id, out T? value)
    {
        return GetEntry(id, false, true, default, out value);
    }

    public bool GetOrDefault<T>(ulong id, T defaultValue, out T? value)
    {
        return GetEntry(id, false, true, defaultValue, out value);
    }

    public T GetVerified<T>(ulong id)
    {
        GetEntry(id, true, false, default, out T? Value);
        return Value!;
    }

    public bool GetOptionalVerified<T>(ulong id, out T? value)
    {
        return GetEntry(id, true, true, default, out value);
    }

    public T GetVerifiedOrDefault<T>(ulong id, T defaultValue)
    {
        GetEntry(id, true, true, defaultValue, out T? Value);
        return Value!;
    }


    // Private methods.
    private static bool IsValidType(object value)
    {
        return value is byte or sbyte or short or ushort or int or uint or long or ulong or float or double
            or string or bool or GHDFEncodedInt or GHDFCompound or byte[] or sbyte[] or short[] or ushort[]
            or int[] or uint[] or long[] or ulong[] or float[] or double[] or string[] or bool[]
            or GHDFEncodedInt[] or GHDFCompound[];
    }

    private bool GetEntry<T>(ulong id, bool isVerified, bool isOptional, T? defaultValue, out T? result)
    {
        result = defaultValue;
        if (!_entries.ContainsKey(id))
        {
            if (isVerified && !isOptional)
            {
                throw new GHDFEntryException($"Entry with ID {id} not found");
            }
            return false;
        }

        object Value = _entries[id];
        try
        {
            result = (T)Value;
            return true;
        }
        catch (InvalidCastException)
        {
            if (isVerified)
            {
                throw new GHDFEntryException($"Entry with ID {id} is of type {Value.GetType().FullName}, expected {typeof(T).FullName}");
            }
            return false;
        }
    }

    public IEnumerator<KeyValuePair<ulong, object>> GetEnumerator()
    {
        return _entries.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}