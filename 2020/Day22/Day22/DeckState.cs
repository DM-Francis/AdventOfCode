namespace Day22;

public class DeckState : IEquatable<DeckState>
{
    public bool Equals(DeckState? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;

        if (Player1Deck.Equals(other.Player1Deck) && Player2Deck.Equals(other.Player2Deck))
            return true;

        if (Player1Deck.Count != other.Player1Deck.Count || Player2Deck.Count != other.Player2Deck.Count)
            return false;

        for (int i = 0; i < Player1Deck.Count; i++)
        {
            if (Player1Deck[i] != other.Player1Deck[i])
                return false;
        }
        for (int i = 0; i < Player2Deck.Count; i++)
        {
            if (Player2Deck[i] != other.Player2Deck[i])
                return false;
        }

        return true;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((DeckState)obj);
    }

    public override int GetHashCode()
    {
        int hashCode = 2345809;
        foreach (int card in Player1Deck)
        {
            hashCode = HashCode.Combine(hashCode, card);
        }

        hashCode = HashCode.Combine(100); // Mark end of 1st deck, start of 2nd
        
        foreach (int card in Player2Deck)
        {
            hashCode = HashCode.Combine(hashCode, card);
        }

        return hashCode;
    }

    public List<int> Player1Deck { get; }
    public List<int> Player2Deck { get; }
    
    public DeckState(Queue<int> player1Deck, Queue<int> player2Deck)
    {
        Player1Deck = new List<int>(player1Deck);
        Player2Deck = new List<int>(player2Deck);
    }
}