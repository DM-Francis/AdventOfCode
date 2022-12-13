using System.Text;

namespace Day13;

public interface IPacketItem : IComparable<PacketInt>, IComparable<PacketList>
{
    int CompareTo(IPacketItem? other)
    {
        if (other is null) throw new ArgumentNullException(nameof(other));
        
        return other switch
        {
            PacketInt otherInt => ((IComparable<PacketInt>)this).CompareTo(otherInt),
            PacketList otherList => ((IComparable<PacketList>)this).CompareTo(otherList),
            _ => throw new ArgumentException("Unrecognised packet item type", nameof(other))
        };
    }
}

public class PacketList : IPacketItem
{
    public List<IPacketItem> Items { get; } = new();

    public int CompareTo(PacketInt? other)
    {
        if (other is null) throw new ArgumentNullException(nameof(other));

        var otherList = new PacketList();
        otherList.Items.Add(other);
        return CompareTo(otherList);
    }

    public int CompareTo(PacketList? other)
    {
        if (other is null) throw new ArgumentNullException(nameof(other));
        
        for (int i = 0; i < Items.Count; i++)
        {
            if (i >= other.Items.Count)
                return 1;

            var comparison = Items[i].CompareTo(other.Items[i]);
            if (comparison != 0)
                return comparison;
        }

        if (Items.Count < other.Items.Count)
            return -1;

        return 0;
    }

    public override string ToString()
    {
        var output = new StringBuilder("[");
        for (var i = 0; i < Items.Count; i++)
        {
            var item = Items[i];
            output.Append(item);
            if (i < Items.Count - 1)
                output.Append(',');
        }

        output.Append(']');
        return output.ToString();
    }
}

public class PacketInt : IPacketItem
{
    public int Value { get; }

    public PacketInt(int value)
    {
        Value = value;
    }
    
    public int CompareTo(PacketList? other)
    {
        if (other is null) throw new ArgumentNullException(nameof(other));

        var thisList = new PacketList();
        thisList.Items.Add(this);
        return thisList.CompareTo(other);
    }

    public int CompareTo(PacketInt? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        return Value.CompareTo(other.Value);
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}