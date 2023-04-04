namespace Schneider.Sweeper.Map
{
    public interface IMap
    {
        uint Width { get; }
        uint Height { get; }

        // Returns the visible value - either hidden field (if not visited yet) or the exposed value (if already visited)
        FieldValue this[uint x, uint y] { get; }

        // Exposes field value and if empty, visits recursively all neighbours (8-neighbourhood)
        FieldValue Visit(uint x, uint y);
    }
}
