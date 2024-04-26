namespace TikTakTo
{
    internal class Program
    {
        static char[] map = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        static int player = 1;
        static int choice;
        static int flag = 0;

        static bool insert = true;

        static void Main(string[] args)
        {
            do
            {
                
                Console.WriteLine("플레이어 1의 돌 : O --------- 플레이어 2의 돌 : X");
                Console.WriteLine();
                Console.WriteLine("\n");

                if (player % 2 == 1)
                {
                    Console.WriteLine("플레이어 1의 차례");
                }
                else
                {
                    Console.WriteLine("플레이어 2의 차례");
                }

                Console.WriteLine("\n");
                Board();

                string line = Console.ReadLine();
                bool res = int.TryParse(line, out choice);

                


                insert = (res == true && choice < 10 && 0 < choice);
                if (insert)
                {
                    if (map[choice] != 'O' && map[choice] != 'X')
                    {
                        if (player % 2 == 1)
                        {
                            map[choice] = 'O';
                        }
                        else
                        {
                            map[choice] = 'X';
                        }

                        player++;
                        Console.Clear();
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("==========입력 오류 발생!!=============== \n\n 죄송합니다. {0} 행은 이미 {1}로 표시되어 있습니다.\n", choice, map[choice]);
                        
                        
                        
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("==========입력 오류 발생!!=============== \n\n 0~9 사이의 숫자를 입력해주세요.\n");
                    Board();
                    continue;
                }

                flag = CheckWin();
            }while (flag != 'O' && flag != 'X' && flag != 'D');



            if (flag == 'O' || flag == 'X')
            {

                Board();


                if (flag == 'O') Console.WriteLine("플레이어 1 승리!! \n\n");
                else Console.WriteLine("플레이어 2 승리!! \n\n");

                

                Console.WriteLine("\n게임을 종료합니다!!.\n");
                return;
            }
            else
            {

                Board();
                Console.WriteLine("무승부");
                Console.WriteLine("\n게임을 종료합니다!!.\n");

            }

            Console.ReadLine();
        }

        static void Board()
        {
            Console.WriteLine("     |     |     ");
            Console.WriteLine($"  {map[1]}  |  {map[2]}  |  {map[3]}  ");
            Console.WriteLine("_____|_____|_____");
            Console.WriteLine("     |     |     ");
            Console.WriteLine($"  {map[4]}  |  {map[5]}  |  {map[6]}  ");
            Console.WriteLine("_____|_____|_____");
            Console.WriteLine("     |     |     ");
            Console.WriteLine($"  {map[7]}  |  {map[8]}  |  {map[9]}  ");
            Console.WriteLine("     |     |     ");
        }

        static char CheckWin()
        {
            // 가로 승리 조건
            if (map[1] == map[2] && map[2] == map[3])
            {

                return map[1];
            }
            else if (map[4] == map[5] && map[5] == map[6])
            {
                return map[4];
            }
            else if (map[7] == map[8] && map[8] == map[9])
            {
                return map[7];
            }

            // 세로 승리 조건
            else if (map[1] == map[4] && map[4] == map[7])
            {
                return map[1];
            }
            else if (map[2] == map[5] && map[5] == map[8])
            {
                return map[2];
            }
            else if (map[3] == map[6] && map[6] == map[9])
            {
                return map[3];
            }

            // 대각선 승리조건
            else if (map[1] == map[5] && map[5] == map[9])
            {
                return map[1];
            }
            else if (map[3] == map[5] && map[5] == map[7])
            {
                return map[3];
            }

            // 무승부
            else if (map[1] != '1' && map[2] != '2' && map[3] != '3' && map[4] != '4' && map[5] != '5' && map[6] != '6' &&
                map[7] != '7' && map[8] != '8' && map[9] != '9')
            {
                return 'D';
            }
            else { return 'E'; }

        }
    }
}
