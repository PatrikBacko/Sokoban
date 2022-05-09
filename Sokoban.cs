using System;

namespace prog_2_sokoban_2
{
    class Program
    {
        enum Direction {TOP, DOWN, LEFT, RIGHT}

        static void state_rating(bool[,] warehouse, Coordinates box, Coordinates storekeeper, Queue queue, int rank, Direction direction)
        {
            if (warehouse[storekeeper.x,storekeeper.y])
            {
                if ((storekeeper.x == box.x) & (storekeeper.y == box.y))
                {
                    switch (direction)
                    {
                        case Direction.TOP:
                            if (warehouse[box.x, box.y + 1])
                                queue.enque(new State(storekeeper, new Coordinates(box.x, box.y + 1), rank + 1));
                            break;
                        case Direction.DOWN:
                            if (warehouse[box.x, box.y - 1])
                                queue.enque(new State(storekeeper, new Coordinates(box.x, box.y - 1), rank + 1));
                            break;
                        case Direction.LEFT:
                            if (warehouse[box.x + 1, box.y])
                                queue.enque(new State(storekeeper, new Coordinates(box.x + 1, box.y), rank + 1));
                            break;
                        case Direction.RIGHT:
                            if (warehouse[box.x - 1, box.y])
                                queue.enque(new State(storekeeper, new Coordinates(box.x - 1, box.y), rank + 1));
                            break;
                    }
                }
                else
                    queue.enque(new State(storekeeper, new Coordinates(box.x, box.y), rank + 1));
            }
        }

        static int bfs(bool[,] warehouse, Coordinates start_storekeeper, Coordinates start_box, Coordinates finish)
        {
            Queue queue = new Queue();
            bool[,,,] visited_states = new bool[12, 12, 12, 12];
            State start_state = new State(start_storekeeper, start_box, 0);
            queue.enque(start_state);

            State curr_state;

            Coordinates curr_box;
            Coordinates curr_storekeeper;

            while (!queue.is_empty())
            {

                curr_state = queue.deque();
                curr_box = curr_state.box;
                curr_storekeeper = curr_state.storekeeper;
                if (!visited_states[curr_storekeeper.x, curr_storekeeper.y, curr_box.x, curr_box.y])
                {
                    if ((curr_box.x == finish.x) & (curr_box.y == finish.y))
                        return curr_state.rank;

                    visited_states[curr_storekeeper.x, curr_storekeeper.y, curr_box.x, curr_box.y] = true;

                    state_rating(warehouse, curr_box, new Coordinates(curr_storekeeper.x, curr_storekeeper.y + 1), queue, curr_state.rank, Direction.TOP);
                    state_rating(warehouse, curr_box, new Coordinates(curr_storekeeper.x, curr_storekeeper.y - 1), queue, curr_state.rank, Direction.DOWN);
                    state_rating(warehouse, curr_box, new Coordinates(curr_storekeeper.x + 1, curr_storekeeper.y), queue, curr_state.rank, Direction.LEFT);
                    state_rating(warehouse, curr_box, new Coordinates(curr_storekeeper.x - 1, curr_storekeeper.y), queue, curr_state.rank, Direction.RIGHT);
                }
            }
            return -1;
        }
        static void Main(string[] args)
        {
            (bool[,], Coordinates[]) tupple = Input.create_warehouse();
            bool [,] warehouse = tupple.Item1;
            Coordinates[] figures = tupple.Item2;
            Console.WriteLine(bfs(warehouse, figures[0], figures[1], figures[2]));


        }
    }

    class State
    {
        public Coordinates storekeeper;
        public Coordinates box;
        public int rank;

        public State next = null;
        public State prev = null;

        public State(Coordinates storekeeper, Coordinates box, int rank)
        {
            this.storekeeper = storekeeper;
            this.box = box;
            this.rank = rank;
        }
    }

    class Queue
    {
        State head = new State(new Coordinates(-1, -1), new Coordinates(-1, -1), -1);
        State tail = new State(new Coordinates(-1, -1), new Coordinates(-1, -1), -1);

        public Queue()
        {
            head.next = tail;
            tail.prev = head;
        }

        public void enque(State state)
        {
            State last_state = head.next;
            head.next = state;
            state.next = last_state;
            last_state.prev = state;
            state.prev = head;
        }
        public State deque()
        {
            State state = tail.prev;
            State staying_state = state.prev;
            staying_state.next = tail;
            tail.prev = staying_state;
            return state;
        }
        public bool is_empty()
        {
            if (head.next == tail)
                return true;
            return false;
        }
    }

    struct Coordinates
    {
        public int x;
        public int y;

        public Coordinates(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
    class Input
    {
        public static (bool[,], Coordinates[]) create_warehouse()
        {
            bool[,] warehouse = new bool[12, 12];
            Coordinates storekeeper = new Coordinates(-1, -1);
            Coordinates box = new Coordinates(-1, -1);
            Coordinates finish = new Coordinates(-1, -1);
            string line = "";
            for (int i = 0; i <= 11; i++)
            {
                if (i > 0 & i < 11)
                    line = Console.ReadLine();
                for (int j = 0; j <= 11; j++)
                {
                    if (i == 0 | i == 11 | j == 0 | j == 11)
                        warehouse[i, j] = false;
                    else
                    {
                        switch (line[j - 1])
                        {
                            case '.':
                                warehouse[j, i] = true;
                                break;
                            case 'X':
                                warehouse[j, i] = false;
                                break;
                            case 'S':
                                warehouse[j, i] = true;
                                storekeeper = new Coordinates(j, i);
                                break;
                            case 'B':
                                warehouse[j, i] = true;
                                box = new Coordinates(j, i);
                                break;
                            case 'C':
                                warehouse[j, i] = true;
                                finish = new Coordinates(j, i);
                                break;
                        }
                    }

                }
            }
            return (warehouse, new Coordinates[3] { storekeeper, box, finish });
        }
    }
}
