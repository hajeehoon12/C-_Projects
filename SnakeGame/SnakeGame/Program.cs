public class Point
{
    public int x { get; set; }
    public int y { get; set; }
    public char sym { get; set; }

    // Point 클래스 생성자
    public Point(int _x, int _y, char _sym)
    {
        x = _x;
        y = _y;
        sym = _sym;
    }

    // 점을 그리는 메서드
    public void Draw()
    {
        Console.SetCursorPosition(x, y);
        Console.Write(sym);
    }

    // 점을 지우는 메서드
    public void Clear()
    {
        sym = ' ';
        Draw();
    }

    // 두 점이 같은지 비교하는 메서드
    public bool IsHit(Point p)
    {
        return p.x == x && p.y == y;
    }
}
// 방향을 표현하는 열거형입니다.
public enum Direction
{
    LEFT,
    RIGHT,
    UP,
    DOWN
}

public class Snake
{
    public List<Point> body; // 뱀의 몸통을 리스트로 표현합니다.
    public Direction direction; // 뱀의 현재 방향을 저장합니다.

    public Snake(Point tail, int length, Direction _direction) // 초기 뱀 선언
    {
        direction = _direction;
        body = new List<Point>();
        for (int i = 0; i < length; i++)
        {
            Point p = new Point(tail.x, tail.y, '*'); // 몸통 그리기
            body.Add(p);
            tail.x += 1; // X 축으로는 두칸씩 이동하기 때문
        }
    }

    // 뱀을 그리는 메서드입니다.
    public void Draw()
    {
        foreach (Point p in body)
        {
            p.Draw();
        }
    }

    // 뱀이 음식을 먹었는지 판단하는 메서드입니다.
    public bool Eat(Point food)
    {
        Point head = GetNextPoint();
        if (head.IsHit(food))
        {
            food.sym = '*';      // 먹이가 뱀몸체위에 계속해서 화면에 표시되는걸 방지한다. 덮어쓰기
            body.Add(food);      // 이하 동일
            return true;
        }
        else
        {
            return false;
        }
    }

    // 뱀이 이동하는 메서드입니다.
    public void Move() 
    {
        Point tail = body[0]; // First[0]에 비해 매개변수의 유형을 확인한다음 캐스팅하려고 시도하는 과정을 생략하므로 매 프레임마다 2개의 추가과정 생략가능, First()->[0]
        body.Remove(tail);
        Point head = GetNextPoint(); // 커서방향의 좌표값을 통해 다음 좌표를 head로 저장
        body.Add(head);     // 뱀의 몸체에 추가

        tail.Clear(); // 꼬리 부분 삭제
        head.Draw(); // 포인트값 재드로우
    }

    // 다음에 이동할 위치를 반환하는 메서드입니다.
    public Point GetNextPoint()
    {
        Point head = body.Last();
        Point nextPoint = new Point(head.x, head.y, head.sym);
        switch (direction)
        {
            case Direction.LEFT:
                nextPoint.x -= 2; // X축의 이동범위가 화면상으로는 Y축에 비해 짧기에 두칸씩 이동
                break;
            case Direction.RIGHT:
                nextPoint.x += 2; // 이하동문
                break;
            case Direction.UP:
                nextPoint.y -= 1;
                break;
            case Direction.DOWN:
                nextPoint.y += 1;
                break;
        }
        return nextPoint;
    }

    // 뱀이 자신의 몸에 부딪혔는지 확인하는 메서드입니다.
    public bool IsHitTail()
    {
        var head = body.Last(); // body[0] 이 tail , body[body.length-1] 이 head 기에 Last()로 표현
        for (int i = 0; i < body.Count - 1; i++) // 계산량이 많아 최적화를 위해 body.Count -2여도 대부분의 경우 감지하지만, tail과 head과 끝지점에서 교차로 만나는 경우 잡아내질못함 
        {
            if (head.IsHit(body[i]))
                return true;
        }
        return false;
    }

    // 뱀이 벽에 부딪혔는지 확인하는 메서드입니다.
    public bool IsHitWall()
    {
        var head = body.Last();
        if (head.x <= 1 || head.x >= 79 || head.y <= 1 || head.y >= 19) // 0, 80, 0, 20으로 하면 벽을 관통함 미관상으로 좋지 않음
            return true;
        return false;
    }
}

public class FoodCreator
{
    int mapWidth;
    int mapHeight;
    char sym;

    Random random = new Random();

    public FoodCreator(int mapWidth, int mapHeight, char sym)
    {
        this.mapWidth = mapWidth;
        this.mapHeight = mapHeight;
        this.sym = sym;
    }

    // 무작위 위치에 음식을 생성하는 메서드입니다.
    public Point CreateFood()
    {
        int x = random.Next(2, mapWidth - 2); // x 좌표를 플레이어가 먹을 수 있게 2단위로 맞추기 위해 짝수로 만듭니다.
        x = x % 2 == 1 ? x : x + 1; // 강제짝수화
        int y = random.Next(2, mapHeight - 2);
        return new Point(x, y, sym);
    }
}

class Program
{
    

    static void WriteGameOver()
    {
        int xOffset = 25;
        int yOffset = 22;
        Console.SetCursorPosition(xOffset, yOffset++);
        WriteText("============================", xOffset, yOffset++);
        WriteText("         GAME OVER          ", xOffset, yOffset++);
        WriteText("============================", xOffset, yOffset++);

    }
    
    static void WriteText(string text, int xOffset, int yOffset)
    {
        Console.SetCursorPosition(xOffset, yOffset);
        Console.WriteLine(text);
    }

    // 벽 그리는 메서드
    static void DrawWalls()
    {
        // 상하 벽 그리기
        for (int i = 0; i < 80; i++)
        {
            Console.SetCursorPosition(i, 0);
            Console.Write("▒");
            Console.SetCursorPosition(i, 20);
            Console.Write("▒");
        }

        // 좌우 벽 그리기
        for (int i = 0; i < 20; i++)
        {
            Console.SetCursorPosition(0, i);
            Console.Write("▒");
            Console.SetCursorPosition(80, i);
            Console.Write("▒");
        }
    }

    static void Main(string[] args)
    {
        // 게임 속도를 조정하기 위한 변수입니다. 숫자가 클수록 게임이 느려집니다.
        int gameSpeed = 100; // 0.1초마다 화면갱신 1000에 1초
        int foodCount = 0; // 먹은 음식 수

        // 게임을 시작할 때 벽을 그립니다.
        DrawWalls();

        // 뱀의 초기 위치와 방향을 설정하고, 그립니다.
        Point p = new Point(4, 5, '*');
        Snake snake = new Snake(p, 4, Direction.RIGHT);
        snake.Draw();

        // 음식의 위치를 무작위로 생성하고, 그립니다.
        
        FoodCreator foodCreator = new FoodCreator(80, 20, '#'); // 맵 너비, 높이, 음식 표시 문자
        Point food = foodCreator.CreateFood();
        food.Draw(); 
        
        

        // 게임 루프: 이 루프는 게임이 끝날 때까지 계속 실행됩니다.
        while (true)
        {
            if (Console.KeyAvailable) // 키 입력 감지
            {
                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        snake.direction = Direction.UP;
                        break;
                    case ConsoleKey.DownArrow:
                        snake.direction = Direction.DOWN;
                        break;
                    case ConsoleKey.LeftArrow:
                        snake.direction = Direction.LEFT;
                        break;
                    case ConsoleKey.RightArrow:
                        snake.direction = Direction.RIGHT;
                        break;
                }
            }

            // 뱀이 음식을 먹었는지 확인합니다.
            if (snake.Eat(food))
            {
                foodCount++; // 먹은 음식 수를 증가
                food.Draw();

                // 뱀이 음식을 먹었다면, 새로운 음식을 만들고 그립니다.
                food = foodCreator.CreateFood(); // 스네이크 몸통 부분에서 랜덤젠하게 되면 화면 표기상 오류가 발생하지만 귀찮기도 하고 최적화 문제로 인해 생략
                food.Draw();
                if (gameSpeed > 10) // 게임이 점점 빠르게
                {
                    gameSpeed -= 5; // 화면이 갱신되는 간격이 0.005초씩 줄어들음
                }
            }
            else
            {
                // 뱀이 음식을 먹지 않았다면, 그냥 이동합니다.
                snake.Move();
            }

            Thread.Sleep(gameSpeed); // Frame 의 개념과 같음

            // 벽이나 자신의 몸에 부딪히면 게임을 끝냅니다.
            if (snake.IsHitTail() || snake.IsHitWall())
            {
                break;
            }

            Console.SetCursorPosition(100, 25); // 하드웨어 적인 문제로 인해 Cursor가 While문으로 인해 반복적으로 찍히는 위치를 양쪽 사이드에서감쌈
            Console.SetCursorPosition(90, 11); // 커서 위치 설정
            Console.WriteLine($"Score: {foodCount}"); // 먹은 음식 수 출력
            Console.SetCursorPosition(100, 25); // 하드웨어 적인 문제로 인해 Cursor가 While문으로 인해 반복적으로 찍히는 위치를 양쪽 사이드에서감쌈




        }

        WriteGameOver();  // 게임 오버 메시지를 출력합니다.
        Console.ReadLine();
        
    }
}



