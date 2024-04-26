namespace BlackJack
{
    using System;
    using System.Collections.Generic;

    public enum Suit { Hearts, Diamonds, Clubs, Spades } //문양
    public enum Rank { Two = 2, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace } //등급

    // 카드 한 장을 표현하는 클래스
    public class Card
    {
        public Suit Suit { get; private set; }
        public Rank Rank { get; private set; }

        public Card(Suit s, Rank r)
        {
            Suit = s;
            Rank = r;
        }

        public int GetValue() // Rank의 10이하는 숫자 그대로 11이상은 10으로 처리
        {
            if ((int)Rank <= 10)
            {
                return (int)Rank;
            }
            else if ((int)Rank <= 13) // Jack,Queen,King 은 10으로 처리
            {
                return 10;
            }
            else // Ace 일 떄만 11로 처리 -> 이후 손패의 합이 21이 넘으면 1로 처리하게 바꿈
            {
                return 11;
            }
        }

        public override string ToString()
        {
            return $"{Suit} {Rank}";
        }
    }

    // 덱을 표현하는 클래스
    public class Deck
    {
        private List<Card> cards;

        public Deck() // Deck의 구성 s: 문양 r: 카드의 숫자(값) , 덱초기화
        {
            cards = new List<Card>();

            foreach (Suit s in Enum.GetValues(typeof(Suit)))
            {
                foreach (Rank r in Enum.GetValues(typeof(Rank)))
                {
                    cards.Add(new Card(s, r));
                }
            }

            Shuffle();
        }

        public void Shuffle() // 덱 셔플 
        {
            Random rand = new Random();

            for (int i = 0; i < cards.Count; i++)
            {
                int j = rand.Next(i, cards.Count);
                Card temp = cards[i];
                cards[i] = cards[j];
                cards[j] = temp;
            }
        }

        public Card DrawCard() // 덱에서 카드 뽑기
        {
            Card card = cards[0];
            cards.RemoveAt(0);
            return card;
        }
    }

    // 패를 표현하는 클래스
    public class Hand // 손패 표현
    {
        private List<Card> cards;

        public Hand()
        {
            cards = new List<Card>();
        }

        public void AddCard(Card card) // 손패 카드 추가
        {
            cards.Add(card);
        }

        public int GetTotalValue() // 손패의 값 계산
        {
            int total = 0;
            int aceCount = 0;

            foreach (Card card in cards)
            {
                if (card.Rank == Rank.Ace)
                {
                    aceCount++;
                }
                total += card.GetValue();
            }

            while (total > 21 && aceCount > 0)
            {
                total -= 10;
                aceCount--;
            }

            return total;
        }
    }

    // 플레이어를 표현하는 클래스
    public class Player
    {
        public Hand hand { get; private set; }

        public Player()
        {
            hand = new Hand();
        }

        public Card DrawCardFromDeck(Deck deck) // 손패 + 덱에서 뽑은 카드 -> 손패
        {
            Card drawnCard = deck.DrawCard();
            hand.AddCard(drawnCard);
            return drawnCard;
        }
    }

    // 여기부터는 학습자가 작성
    // 딜러 클래스를 작성하고, 딜러의 행동 로직을 구현하세요.
    public class Dealer : Player
    {
        public bool DealerDraw(Deck deck, Player player)
        {
            if (hand.GetTotalValue() < 17 || player.hand.GetTotalValue()>hand.GetTotalValue())
            {
                Card drawnCard = DrawCardFromDeck(deck);
                Console.WriteLine("딜러가 Hit 합니다.");
                Console.WriteLine($"\n딜러는 '{drawnCard}' 을(를) 뽑았습니다. 현재 딜러의 손패의 합은 {hand.GetTotalValue()} 입니다.");
                return false;
            }
            else
            {
                Console.WriteLine("딜러가 Stay 했습니다.");
                return true;
            }
        }
    }



    // 블랙잭 게임을 구현하세요. 
    public class Blackjack
    {
        private Player player;
        private Dealer dealer;
        private Deck deck;
        private bool playerStop = false;
        private bool dealerStop = false;


        public void WinDisc(Player player, Dealer dealer)
        {
            if (player.hand.GetTotalValue() == 21)
            {
                Console.WriteLine("플레이어의 BlackJack!! \n플레이어의 승리입니다!!");
            }
            else if (dealer.hand.GetTotalValue() == 21)
            {
                Console.WriteLine("딜러의 BlackJack!! \n딜러의 승리입니다!!");
            }
            else if (player.hand.GetTotalValue() > 21)
            {
                Console.WriteLine("플레이어 카드의 합이 21을 초과하여 Burst~ \n딜러의 승리입니다.");
            }
            else if (dealer.hand.GetTotalValue() > 21)
            {
                Console.WriteLine("딜러의 카드의 합이 21점을 초과하여 Burst~ \n플레이어의 승리입니다.");
            }
            else if (player.hand.GetTotalValue() > dealer.hand.GetTotalValue())
            {
                Console.WriteLine("플레이어의 카드 합이 더 높습니다. \n플레이어의 승리입니다.");
            }
            else if (player.hand.GetTotalValue() < dealer.hand.GetTotalValue())
            {
                Console.WriteLine("딜러의 카드 합이 더 높습니다. \n딜러의 승리입니다.");
            }
            else
            {
                Console.WriteLine("딜러와 플레이어의 카드의 합이 같습니다. \n무승부입니다!");
            }
        }

        public void PlayGame()
        {
            deck = new Deck();
            player = new Player();
            dealer = new Dealer();


            Console.WriteLine("=========== 블랙잭 게임을 시작합니다!!! ============\n\n");

            for (int i = 0; i < 2; i++)
            { 
                player.DrawCardFromDeck(deck);
                dealer.DrawCardFromDeck(deck);
            }

            Console.WriteLine($"플레이어의 초기 손패의 합: { player.hand.GetTotalValue() }");
            Console.WriteLine($"딜러의 초기 손패의 합: { dealer.hand.GetTotalValue() }");


            while (player.hand.GetTotalValue() < 21 && dealer.hand.GetTotalValue() < 21)
            {
                playerStop = false;
                if (!playerStop)
                {
                    Console.WriteLine("\n플레이어의 차례입니다. ");
                    Console.Write("Hit 하시겠습니까? (Y/N) : ");
                    string input = Console.ReadLine();

                    if (input == "y" || input == "Y")
                    {
                        Card curCard = player.DrawCardFromDeck(deck);
                        Console.WriteLine($"\n플레이어는 '{curCard}' 을(를) 뽑았습니다. 현재 플레이어의 손패의 합은 {player.hand.GetTotalValue()} 입니다.");
                    }
                    else
                    {
                        playerStop = true; 
                        Console.WriteLine("플레이어가 Stay 했습니다.");
                    }

                }
                if (player.hand.GetTotalValue() >= 21) break;
                
                Console.WriteLine("\n딜러의 차례입니다.");
                if (dealer.DealerDraw(deck, player) && playerStop) break;
            }

            WinDisc(player, dealer);

        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Blackjack game = new Blackjack();
            game.PlayGame();
        }
    }
}
