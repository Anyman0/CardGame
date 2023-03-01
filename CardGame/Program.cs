// See https://aka.ms/new-console-template for more information

using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace CardGame;

class Program
{
    static void Main(string[] args)
    {
        // Class instantiations
        Deck deck = new Deck(new List<Card>());
        Game game = new Game(new Player[0]);
        Card playCard = new Card(0);
        Hand hand = new Hand("", new List<Card>());
        Table table = new Table(new List<Card>());  
        
        // Booleans
        bool playerTurn = true;
        bool AITurn = false;
        bool canPlay = false;

        // Create a deck and shuffle it       
        var newDeck = deck.CreateDeck(); // Creating a deck of 52 cards
        var shuffledDeck = deck.ShuffleDeck(newDeck); // Shuffling the deck
                
        // Create a game with players 
        Console.WriteLine("Hello! What is your name? ");
        string namePrompt = Console.ReadLine();                
        var newGame = game.CreateGame(new Hand(namePrompt, new List<Card>()));
        Console.WriteLine(game.Greeting(namePrompt));        

        // Below gives 5 cards to each player in the beginning.
        for(int i = 0; i < game.Players.Length; i++)
        {   
            for(int j = 0; j < hand.MaxDrawValue; j++)
            {
                deck.DrawCard(shuffledDeck.FirstOrDefault(), game.Players[i]);
                shuffledDeck.Remove(shuffledDeck.FirstOrDefault());
            }            
        }

        while (!game.CheckGameState(game.Players))
        {
            Console.WriteLine(deck.DisplayDeck(shuffledDeck.Count));
            Console.WriteLine(table.DisplayTable(table.TableCards.Count, playCard));            
            DisplayHandCounts(game.Players);
            DisplayPlayerCards(game.Players.Last());
            canPlay = PlayBool(game.Players.Last(), playCard);
            
            if (canPlay)
            {
                // Request user input
                Console.WriteLine("\nWhat card do you wish to play? ");
                var playCardString = Console.ReadLine();

                // Loop while user inputs a valid card
                while(playCardString == string.Empty || TryToPlay(playCardString, game.Players.Last().PlayerHand, table).Value == 0)
                {
                    if (table.TableCards.Count > 0)
                    {
                        if (playCardString != string.Empty && int.Parse(playCardString) >= table.TableCards.Last().Value)
                        {
                            Console.WriteLine("This card is NOT in your hand.");
                            playCardString = Console.ReadLine();
                        }
                        else if (playCardString == string.Empty)
                        {
                            Console.WriteLine("You need to input a card.");
                            playCardString = Console.ReadLine();
                        }
                        else if (playCardString != string.Empty && int.Parse(playCardString) < table.TableCards.Last().Value)
                        {
                            Console.WriteLine("You can't play a card thats smaller than the top card on the table..");
                            playCardString = Console.ReadLine();
                        }
                    }
                    else
                    {
                        if (playCardString != string.Empty)
                        {
                            Console.WriteLine("This card is NOT in your hand.");
                            playCardString = Console.ReadLine();
                        }
                        else if (playCardString == string.Empty)
                        {
                            Console.WriteLine("You need to input a card.");
                            playCardString = Console.ReadLine();
                        }
                        else if (playCardString != string.Empty)
                        {
                            Console.WriteLine("You can't play a card thats smaller than the top card on the table..");
                            playCardString = Console.ReadLine();
                        }
                    }
                                       
                }      
                
                playCard = TryToPlay(playCardString, game.Players.Last().PlayerHand, table);

                if (playerTurn)
                {                   
                    PlayerPlayCard(game.Players.Last(), table, shuffledDeck, playCard, deck, hand);
                    AITurn = true;
                    playerTurn = false;
                }                    
            }
            else
            {                
                PlayerDrawTable(game.Players.Last(), table);
                AITurn = true;
                playerTurn = false;
            }

            if (AITurn)
            {               
                playCard = AIPlayCard(game.Players, table, shuffledDeck, playCard, deck, hand);                
                AITurn = false;
                playerTurn = true;

            }
        }  
           

    }
    // This checks for AI-players hand and returns the smallest possible card. If none can be played, draws all cards from the table.
    public static Card CheckHands(List<Card> hand, Table table, Card playCard)
    {
        Card cardToPlay = new Card(0);
        List<int> valueList = new List<int>();
        foreach(Card c in hand)
        {
            valueList.Add(c.Value);
        }
        valueList.Sort(); // Sort the cards to ascending order
        for(int v = 0; v < valueList.Count; v++)
        {
            if (valueList[v] >= playCard.Value)
            {
                // If AI-player has a card that can be played, play it
                cardToPlay = hand.Find(x => x.Value == valueList[v]);
                table.TableCards.Add(cardToPlay);
                break;
            }            
        }
        if (cardToPlay.Value == 0) // If AI-player couldn't play any card, draw the table
        {
            hand.AddRange(table.TableCards);
            table.TableCards.Clear();            
        }
        
        return cardToPlay;
    }

    // This checks if the card player inputs is in their hand and can be played. If not, return 0. 
    public static Card TryToPlay(string c, List<Card> hand, Table table)
    {
        Card cardToPlay = new Card(0);
        if(table.TableCards.Count <= 0)
        {
            foreach (Card card in hand)
            {
                if (card.Value == int.Parse(c))
                {
                    cardToPlay = card;
                }
            }
        }
        else
        {
            foreach (Card card in hand)
            {
                if (card.Value == int.Parse(c) && int.Parse(c) >= table.TableCards.Last().Value)
                {
                    cardToPlay = card;
                }
            }
        }
         
        return cardToPlay;
    }

    // Play a card as as player
    public static void PlayerPlayCard(Player player, Table table, List<Card> shuffledDeck, Card card, Deck deck, Hand hand)
    {
        Console.WriteLine($"{player.Name} played {card.Value}");
        player.PlayerHand.Remove(card);
        table.TableCards.Add(card);
        if(shuffledDeck.Count > 0 && player.PlayerHand.Count < hand.MaxDrawValue)
        {
            deck.DrawCard(shuffledDeck.FirstOrDefault(), player);
            shuffledDeck.Remove(shuffledDeck.FirstOrDefault());
        }       
    }

    // Player draws the table
    public static void PlayerDrawTable(Player player, Table table)
    {
        Console.WriteLine("You cannot play any cards. You draw the table.");
        Console.WriteLine($"{player.Name} drew table.");
        player.PlayerHand.AddRange(table.TableCards);
        table.TableCards.Clear();
    }

    // Play a card as AI
    public static Card AIPlayCard(Player[] players, Table table, List<Card> shuffledDeck, Card card, Deck deck, Hand hand)
    {
        for(int i = 0; i < players.Length - 1; i++)
        {
            Thread.Sleep(2000);
            card = CheckHands(players[i].PlayerHand, table, card);
            if (card.Value != 0)
            {
                Console.WriteLine($"{players[i].Name} played {card.Value}");
                players[i].PlayerHand.Remove(card);
                if (shuffledDeck.Count > 0 && players[i].PlayerHand.Count < hand.MaxDrawValue)
                {
                    deck.DrawCard(shuffledDeck.FirstOrDefault(), players[i]);
                    shuffledDeck.Remove(shuffledDeck.FirstOrDefault());
                }
            }
            else Console.WriteLine($"{players[i].Name} drew the table.");
        }
        return card;
    }

    // Displays how many cards each player has in their hand
    public static void DisplayHandCounts(Player[] players)
    {
        for(int i = 0; i < players.Length; i++)
        {
            Console.Write($"{players[i].Name}: {players[i].PlayerHand.Count} cards. | ");
        }
    }

    // Display each card in players hand
    public static void DisplayPlayerCards(Player player)
    {
        Console.WriteLine("\nIn your hand are the following cards: ");
        foreach(Card card in player.PlayerHand)
        {
            Console.Write($"{card.Value} \t");
        }
    }

    // Check if player has an option to play, if so, return true, else false.
    public static bool PlayBool(Player player, Card playCard)
    {
        bool result = false;
        foreach(Card card in player.PlayerHand)
        {
            result = (card.Value >= playCard.Value) ? true : false;
            if (result) break;
        }
        return result;
        /*foreach (var item in game.Players.Last().PlayerHand)
            {                
                if (item.Value >= playCard.Value)
                {
                    canPlay = true;
                    break;
                }
                else canPlay = false;
       }*/
    }




    // Game is "Paskahousu"
    // Rules: A player who runs out of cards first wins. Until the deck is empty, each player draws until they have 5 cards.
    // The played card must be bigger or equal to the card on the table. If player does not have a valid card, they draw all the cards on the table. 

    /*---------------------------  Things to add: ---------------------------*/
    // 10 should clear the table of all the cards if the top card is smaller or equal to 9. 
    // 14(Ace) should clear the table of all the cards if the top is bigger or equal to 11. 
    // 10 or 14 played on empty table forces the next player to draw that card.
    // You should be able to play all the cards under or equal to 6 in one turn. 
    // Also when all 4 of the same cards are on the table on top of each other, we should clear the table.
    // AI should sometimes "Choose" to play a card other than smallest possible.
    // A card should have suit as well. (Hearts, Diamonds, Spades & Clubs).




























// Task 1
// Get the maximum value between 2 similar characters from a string (IE. "MMMWEAM" --> Return should be 3, since there are 'WEA' between M's.
//int max = MaxValue("mmerytsm");
public static int MaxValue(string str) 
{
    Dictionary<char, int> differentChars = new Dictionary<char, int>();
    List<int> gaps = new List<int>();

    // Loop through every character in given string        
    for(int i = 0; i < str.Length; i++)
    {
        if (!differentChars.ContainsKey(str[i]))
        {
            differentChars.Add(str[i], 1);
            gaps.Add(CountGap(str[i].ToString(), str)); // Find all gaps between similar characters and return the largest cap
        }            
    }
    foreach(var s in differentChars) 
    { 
        Console.WriteLine("{0} - {1}", s.Key, s.Value);
    }
    Console.WriteLine(gaps.Max());
    return gaps.Max();
}

public static int CountGap(string toCount, string wholeString)
{
    int count = -1;
    List<int> results = new List<int>();
    bool canCount = false;
    for(int i = 0; i < wholeString.Length; i++)
    {
        try
        {
            if (canCount)
            {
                count++;
                if (wholeString[i].ToString() == toCount)
                {
                    results.Add(count);
                    canCount = false;                       
                }
            }
            if (toCount == wholeString[i].ToString() && wholeString[i + 1].ToString() != toCount && !canCount)
            {
                canCount = true;
                count = -1;
            }                

        }
        catch
        {

        }

    }

    if (results.Count > 0)
    {
        return results.Max();
    }
    else return 0;
}


// Task 2
// Find the shortest path from given array. (IE. "String[] {"5", "A", "B", "C", "D", "E", "A-C", "A-B", "B-C", B-D", "C-D", "D-E"}. --> Return A-C-D-E. We can skip B in this case.
//ShortestPath(new string[] { "5", "A", "B", "C", "D", "E", "A-B", "A-D", "B-C", "B-D", "C-D", "D-E" });
public static string[] ShortestPath(string[] strArr)
{
    int startIndex = int.Parse(strArr[0]);
    string endAlphabet = strArr[startIndex];
    List<string> pathList = new List<string>();
    string nextPath = string.Empty;
    List<string> result = new List<string>();
    string stringResult = string.Empty;
    string resultWithoutDublicates = string.Empty;

    // Populate pathList
    for(int i = strArr.Length - 1; i > startIndex; i--)
    {
        pathList.Add(strArr[i]);
    }

    try
    {                    
        pathList.Sort();
        nextPath = pathList[0].Split('-')[0];
        result.Add(NextPath(pathList, nextPath));
        while (result[result.Count - 1].Split('-')[1] != endAlphabet)
        {
            nextPath = result[result.Count - 1].Split('-')[1];
            result.Add(NextPath(pathList, nextPath));
        }

    }
    catch
    {

    }

    stringResult = string.Join('-', result);
    for(int i = 0; i < stringResult.Length; i++)
    {
        try
        {
            if (!resultWithoutDublicates.Contains(stringResult[i]))
            {
                resultWithoutDublicates += stringResult[i];
            }
            else if (resultWithoutDublicates.Contains(stringResult[i]) && stringResult[i] == '-' && resultWithoutDublicates.LastOrDefault() != '-')
            {
                resultWithoutDublicates += stringResult[i];
            }
        }
        catch
        {

        }
    }

    return new string[] { resultWithoutDublicates };
}

public static string NextPath(List<string> pathList, string moveFrom)  
{
    string result = string.Empty;
    List<string> options = new List<string>(); 
    pathList.Sort(); // List sorted alphabetically

    for(int i = 0; i < pathList.Count; i++)
    {
        if (pathList[i].Split('-')[0] == moveFrom)
        {
            options.Add(pathList[i]);
        }
    }
    options.Sort();
    options.Reverse();

    result = options[0];

    return result;
}

}