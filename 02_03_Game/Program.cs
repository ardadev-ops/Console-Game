using System.Runtime.CompilerServices;

class Game
{

    // Define the World dimensions
    const int WORLD_WIDTH = 30; //Constant Variable, wenn es einmal bestimmt wurde, kann es nicht mehr geändert werden
    const int WORLD_HEIGHT = 20;

    /* This is our player class which will be used
     * to capsulate the 2-dimensional position
     * and the lifes
     */
    class Player
    {
        public int x;
        public int y;
        public int lifes;

        /* Constructor: This function will be executed
         * when calling "new" player. It is used to 
         * intialize default for members/variables of this class
         */
        public Player(int x, int y, int lifes)
        {
            this.x = x;
            this.y = y;
            this.lifes = lifes;
        }
    }

    class Enemy
    {
        public int x;
        public int y;
        public int dx; //Direction on the x-axis
        public int dy; //Direction on the y-axis


        public Enemy(Random rand)
        {
            this.x = rand.Next(1, WORLD_WIDTH - 1);
            this.y = rand.Next(1, WORLD_HEIGHT - 1);
            this.dx = RandomDirection(rand); // Random direction on the x-axis
            this.dy = RandomDirection(rand); // Random direction on the y-axis
        }

        public void Move()
        {
            this.x += dx;
            this.y += dy;

            if (x <= 1 || x >= WORLD_WIDTH - 2)
            {
                this.dx *= -1;
            }
            if (y <= 1 || y >= WORLD_HEIGHT - 2)
            {
                this.dy *= -1;
            }
        }
    }


    static int RandomDirection(Random rand)
    {
        double randDouble = rand.NextDouble();
        if (randDouble < 0.5)
        {
            return -1; // Move left/up
        }
        return 1; // Move right/down
    }



    /* The "Main" function here is ent entry point, 
     * executed upon starting the program. 
     * This is now needed due to the complexity of our program
     */
    static void Main()
    {
        Console.CursorVisible = false; // Hide the cursor

        Random rand = new Random(); // Create a new random generator

        // Create a new Player via the Constructor at the center of the world
        Player hero = new Player(WORLD_WIDTH / 2, WORLD_HEIGHT / 2, 7);

        // Create a new List of Enemies
        List<Enemy> enemyList = new List<Enemy>();

        for (int i = 0; i < 5; i++)
        {
            Enemy newEnemy = new Enemy(rand);
            enemyList.Add(newEnemy);
        }

        // Draw the Grid
        while (hero.lifes > 0)
        {
            // A measure to control the frame rate
            int framestart = Environment.TickCount;

            DrawGrid(hero, enemyList); // Correctly call the DrawGrid method
            HandleInput(hero); // Correctly call the HandleInput method

            for (int i = 0; i < enemyList.Count(); i++)
            {
                enemyList[i].Move();
            }

            CheckCollision(hero, enemyList); // Check for collisions with enemies

            DrawGrid(hero, enemyList);

            int frameend = Environment.TickCount;
            int frameDelta = frameend - framestart;
            int SleepTime = 32 - frameDelta;

            if (SleepTime < 0)
            {
                SleepTime = 0; // If the frame took longer than 32ms, do not sleep
            }

            Thread.Sleep(SleepTime);
        }

        // Wait for any key
        Console.ReadKey();
    }

    static void CheckCollision(Player hero, List<Enemy> enemies)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (hero.x == enemies[i].x && hero.y == enemies[i].y)
            {
                if (hero.x == enemies[i].x && hero.y == enemies[i].y)
                {
                    hero.lifes--;
                    return; // Exit the method if a collision is detected
                }
            }
        }
    }

    // Move the DrawGrid method outside of the Checkkollision method
    static void DrawGrid(Player hero, List<Enemy> enemies)
    {
        // Iterate over all x-coordinates
        for (int x = 0; x < WORLD_WIDTH; x++)
        {
            // In every x, iterate over all y-coordinates
            for (int y = 0; y < WORLD_HEIGHT; y++)
            {
                // Set the cursor to this position
                Console.SetCursorPosition(x, y);

                bool enemyFound = false;
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (x == enemies[i].x && y == enemies[i].y)
                    {
                        // Enemy
                        enemyFound = true;
                        break; // Break out of the loop to avoid overwriting the enemy
                    }
                }

                // Draw according to logic
                if (x == 0 || x == WORLD_WIDTH - 1 || y == 0 || y == WORLD_HEIGHT - 1)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    // World Border
                    Console.Write("#");
                }
                else if (x == hero.x && y == hero.y)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    // Player
                    Console.Write("P");
                }
                else if (enemyFound == true)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("E");
                }
                else
                {
                    // Empty Space
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(".");
                }
            }
        }
        // "Flush" out the grid
        Console.WriteLine($"\nLifes: {hero.lifes}");
    }
    // Add the missing HandleInput method to fix the CS0103 error
    static void HandleInput(Player hero)
    {
        if (Console.KeyAvailable)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    if (hero.y > 1) hero.y--; // Move up
                    break;
                case ConsoleKey.DownArrow:
                    if (hero.y < WORLD_HEIGHT - 2) hero.y++; // Move down
                    break;
                case ConsoleKey.LeftArrow:
                    if (hero.x > 1) hero.x--; // Move left
                    break;
                case ConsoleKey.RightArrow:
                    if (hero.x < WORLD_WIDTH - 2) hero.x++; // Move right
                    break;
            }
        }
    }
}