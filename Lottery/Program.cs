using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lottery
{
    class Buyer
    {
        public string FirstName { set; get; }
        public int TicketNo { set; get; }
        public int Dollars { set; get; } = 0;

        public override string ToString()
        {
            return FirstName + ": " + Dollars + "$";
        }
    }

    class Program
    {
        const int PRICE = 10;
        const int MAXNO = 50;
        const double FIRST_RATE = 0.75;
        const double SECOND_RATE = 0.15;
        const double THIRD_RATE = 0.10;

        static int pot = 200;
        static List<Buyer> buyersList = new List<Buyer>();
        static Buyer[] winners = new Buyer[3];
        static int current = 0;

        static int left = 200;      // amount added to next run

        static void Main(string[] args)
        {
            while (true)
            {
                Console.Write("Please select commands: p (Purchase), d (Draw), w (Winners), e (Exit): ");
                string cmd = Console.ReadLine().ToLower();

                if (cmd == "p" || cmd == "purchase")
                    Purchase();
                else if (cmd == "d" || cmd == "draw")
                    Draw();
                else if (cmd == "w" || cmd == "winners")
                    Winners();
                else if (cmd == "e" || cmd == "exit")
                    Environment.Exit(0);
            }
        }

        static void Purchase()
        {
            if (current > 50)
            {
                Console.WriteLine("50 tickets are sold out. Please draw the winners before next roll of lotter.\n");
                return;
            }

            Console.Write("Please enter your first name: ");
            string name = Console.ReadLine();

            current++;
            //Console.Write("Current number is " + current + ". Please confirm purchasing (y/n)? ");
            //string confirm = Console.ReadLine();
            //if (confirm.ToLower() != "y")
            //{
            //    Console.WriteLine("Purchase cancelled!");
            //    current--;
            //    return;
            //}

            buyersList.Add(new Buyer()
            {
                FirstName = name,
                TicketNo = current
            });
            Console.WriteLine("Purchased ball number is: " + current);

            // Add the money into pot
            pot = pot + 10;
        }
        static void Draw()
        {
            left = pot;

            // Minimum 3 tickets sold required.
            if (buyersList.Count < 3)
            {
                Console.WriteLine("Ticket not sold enough for a draw.\n");
                return;
            }

            // Confirm to run the draw
            //Console.Write("Are you sure to run the draw (y/n)? ");
            //string confirm = Console.ReadLine();
            //if (confirm.ToLower() != "y")
            //{
            //    Console.WriteLine("Cancel running the draw!\n");
            //    return;
            //}

            // Randomly pick 3 numbers from sold tickets (not from all 50 ball numbers)
            List<int> randoms = new List<int>();
            for (int i = 0; i < 3; i++)
            {
                int tmp = GetRandom3(buyersList.Count, randoms);
                randoms.Add(tmp);
                winners[i] = buyersList.Single(o => o.TicketNo == tmp);
            }

            // Set the winning prize into the 3 selected buyers
            int prize = pot / 2;
            winners[0].Dollars = (int)Math.Round(FIRST_RATE * prize, 0);
            winners[1].Dollars = (int)Math.Round(SECOND_RATE * prize, 0);
            winners[2].Dollars = (int)Math.Round(THIRD_RATE * prize, 0);

            // Reset the lottery
            current = 0;
            for (int i = 0; i < 3; i++)
            {
                pot = pot - winners[i].Dollars;
            }
            buyersList.Clear();
            buyersList = new List<Buyer>();
        }
        static void Winners()
        {
            Console.WriteLine("Total amount in pool is $" + left);
            Console.WriteLine(" 1st ball\t\t\t 2nd ball\t\t\t 3rd ball");
            //Console.WriteLine(" Ball " + winners[0].TicketNo + "\t\t\t Ball " + winners[1].TicketNo + "\t\t\t Ball " + winners[2].TicketNo);
            Console.WriteLine("---------\t\t\t----------\t\t\t--------");
            Console.WriteLine(winners[0].ToString() + "\t\t\t" + winners[1] + "\t\t\t" + winners[2]);
        }

        static int GetRandom3(int count, List<int> list)
        {
            var nums = Enumerable.Range(1, count).ToList();
            foreach(int n in list)
            {
                nums.Remove(n);
            }
            
            Random random = new Random();
            int index = random.Next(nums.Count);

            return nums[index];
        }
    }
}
