using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DouShouQi
{
    public partial class Game : Form
    {
        // int giliran. apabila 1 maka player duluan, apabila 2 maka Ai duluan
        int giliran = 1;

        // Object Player
        Player player = new Player("player", 2);
        Player computer = new Player("computer", 1);

        // Object untuk papan
        private SquareNode[,] board;
        private Button[,] boardButtons;

        // Object untuk first click dan last click
        private SquareNode first_click = null;
        private SquareNode second_click = null;

        // Object untuk check DEN
        private SquareNode den1 = null;
        private SquareNode den2 = null;

        // Settings untuk buttons
        const int BUTTON_SIZE = 75;
        const int OFFSET_BUTTON = 20;
        const int MARGIN_BUTTON = 30;

        // BUTTON COLOR AND FONT SETTINGS
        Color bgcolor = Color.LimeGreen;
        Color fontColor = Color.White;
        Color waterColor = Color.Aqua;
        Font textFont = new Font("Calibri", 10, FontStyle.Bold, GraphicsUnit.Point);

        Image[] images = new Image[]
        {
            Image.FromFile("./resources/0.png"),
            Image.FromFile("./resources/1.png"),
            Image.FromFile("./resources/2.png"),
            Image.FromFile("./resources/3.png"),
            Image.FromFile("./resources/4.png"),
            Image.FromFile("./resources/5.png"),
            Image.FromFile("./resources/6.png"),
            Image.FromFile("./resources/7.png"),
            Image.FromFile("./resources/den.png"),
            Image.FromFile("./resources/trap.png"),
        };

        Random rand = new Random();
        bool AI_first_move = true;

        int[,] mouse_magic_number = new int[,]
            {
                { 8, 8, 8, 0, 8, 8, 8 },
                { 8, 8, 8, 9, 9, 9, 9 },
                { 8, 8, 8, 9, 10, 10, 10 },
                { 8, 9, 9, 10, 12, 12, 11},
                { 8, 9, 9, 11, 12, 12, 12 },
                { 8, 9, 9, 11, 12, 12, 13 },
                { 10, 11, 11, 13, 13, 13, 13 },
                { 11, 12, 13, 50, 13, 13, 13 },
                { 11, 13, 50, int.MaxValue, 50,13,13 }
            };

        int[,] cat_magic_number = new int[,]
        {
                { 8, 8, 8, 0, 8, 8, 8 },
                { 13, 10, 8, 8, 8, 8, 8 },
                { 10, 10, 10, 8, 8, 8, 8 },
                { 10, 0, 0, 8, 0, 0, 8 },
                { 10, 0, 0, 8, 0, 0, 8 },
                { 10, 0, 0, 10, 0, 0, 8 },
                { 10, 11, 11, 15, 11, 11, 10 },
                { 11, 11, 15, 50, 15, 11, 11 },
                { 11, 15, 50, int.MaxValue, 50, 15, 11}
        };

        int[,] wolf_magic_number = new int[,]
        {
                {8, 12, 12, 0, 8, 8, 8},
                {8, 12, 13, 8, 8, 8, 8},
                {8, 8, 10, 8, 8, 8, 8},
                {8, 0, 0, 8, 0, 0, 8},
                {8, 0, 0, 8, 0, 0, 8},
                {9, 0, 0, 10, 0, 0, 9},
                {9, 10, 11, 15, 11, 10, 9},
                {10, 11, 15, 50, 15, 11, 10},
                {11, 15, 50, int.MaxValue, 50, 15, 11}
        };

        int[,] dog_magic_number = new int[,]
        {
                {8, 8, 8, 0, 12, 12, 8},
                {8, 8, 8, 8, 13, 10, 8},
                {8, 8, 8, 8, 8, 8, 8},
                {8, 0, 0, 8, 0, 0, 8},
                {8, 0, 0, 8, 0, 0, 8},
                {9, 0, 0, 10, 0, 0, 9},
                {9, 10, 11, 15, 11, 10, 9},
                {10, 11, 15, 50, 15, 11, 10},
                {11, 15, 50, int.MaxValue, 50, 15, 11}
        };

        int[,] leopard_magic_number = new int[,]
        {
                {9, 9, 9, 0, 9, 9, 9},
                {9, 9, 9, 9, 9, 9, 9},
                {9, 9, 9, 10, 10, 9, 9},
                {10, 0, 0, 13, 0, 0, 10},
                {11, 0, 0, 14, 0, 0, 11},
                {12, 0, 0, 15, 0, 0, 12},
                {13, 13, 14, 15, 14, 13, 13},
                {13, 14, 15, 50, 15, 14, 13},
                {14, 15, 50, int.MaxValue, 50, 15, 14}
        };

        int[,] tiger_magic_number = new int[,]
        {
                {10, 12, 12, 0, 12, 12, 10},
                {12, 14, 12, 12, 12, 12, 12},
                {14, 16, 16, 14, 16, 16, 14},
                {15, 0, 0, 15, 0, 0, 15},
                {15, 0, 0, 15, 0, 0, 15},
                {15, 0, 0, 15, 0, 0, 15},
                {18, 20, 20, 30, 20, 20, 18},
                {25, 25, 30, 50, 30, 25, 25},
                {25, 30, 50, int.MaxValue, 50, 30, 25}
        };

        int[,] lion_magic_number = new int[,]
        {
                {10, 12, 12, 0, 12, 12, 10},
                {12, 12, 12, 12, 12, 14, 12},
                {14, 16, 16, 14, 16, 16, 14},
                {15, 0, 0, 15, 0, 0, 15},
                {15, 0, 0, 15, 0, 0, 15},
                {15, 0, 0, 15, 0, 0, 15},
                {18, 20, 20, 30, 20, 20, 18},
                {25, 25, 30, 50, 30, 25, 25},
                {25, 30, 50, int.MaxValue, 50, 30, 25}

        };

        int[,] elephant_magic_number = new int[,]
        {
                {11, 11, 11, 0, 11, 11, 11},
                {11, 11, 11, 11, 11, 11, 11},
                {10, 15, 14, 14, 14, 14, 12},
                {12, 0, 0, 12, 0, 0, 12},
                {14, 0, 0, 14, 0, 0, 14},
                {16, 0, 0, 16, 0, 0, 16},
                {18, 20, 20, 30, 20, 20, 18},
                {25, 25, 30, 50, 30, 25, 25},
                {25, 30, 50, int.MaxValue, 50, 30, 25}
        };

        List<int[,]> magic_numbers;

        public Game(int giliran)
        {
            InitializeComponent();
            //set Giliran
            this.giliran = giliran;

            // Initialize SquareNodes dan boardButtons
            board = new SquareNode[7, 9];
            boardButtons = new Button[7, 9];

            // Initialize Papan
            initPapan();
            updateUI();

            if(giliran == 1)
            {
                //random move
            }

        }

        public void updateUI()
        {
            //set toolstrip giliran
            giliranToolStripMenuItem.Text = $"Giliran: {giliran}";

            //set toolstrip selected animal
            selectedNullToolStripMenuItem.Text = $"Selected: null [null]";
            if (first_click != null)
            {
                selectedNullToolStripMenuItem.Text = $"Selected: {first_click.animal.name} [{first_click.animal.strength}]";
            }

            //set button images
            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 7; x++)
                {
                    boardButtons[x, y].BackColor = bgcolor;

                    if (board[x, y].animal == null)
                    {
                        boardButtons[x, y].Text = "";
                        boardButtons[x, y].BackgroundImage = null;
                    }
                    
                    if (board[x, y].animal != null)
                    {
                        boardButtons[x, y].Text = $"{board[x,y].animal.name}\n\nPlayer {board[x, y].animal.player}";
                        boardButtons[x, y].BackgroundImage = images[board[x, y].animal.strength];
                    }
                    else if (board[x, y].isDen)
                    {
                        boardButtons[x, y].Text = $"Den\n\nPlayer {board[x, y].denOwner}";
                        boardButtons[x, y].BackgroundImage = images[8];
                    }
                    else if (board[x, y].isTrap)
                    {
                        boardButtons[x, y].Text = $"Trap\n\nPlayer{board[x, y].trapOwner}";
                        boardButtons[x, y].BackgroundImage = images[9];
                    }
                    else if (board[x, y].isWater)
                    {
                        boardButtons[x, y].BackColor = waterColor;
                    }
                    
                    boardButtons[x, y].BackgroundImageLayout = ImageLayout.Zoom;
                }
            }
        }

        public void initPapan()
        {
            // SET WINDOWS SIZE
            this.Size = new Size((MARGIN_BUTTON * 2 + OFFSET_BUTTON) + (OFFSET_BUTTON + 7 * BUTTON_SIZE), (MARGIN_BUTTON * 2 + OFFSET_BUTTON) + (OFFSET_BUTTON + 9 * BUTTON_SIZE));
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;

            //generate buttons dan SquareNode
            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 7; x++)
                {
                    //bikin button baru lalu add ke controls
                    boardButtons[x, y] = new Button {
                        Size = new Size(BUTTON_SIZE, BUTTON_SIZE),
                        Location = new Point(MARGIN_BUTTON + OFFSET_BUTTON + x * BUTTON_SIZE, MARGIN_BUTTON + OFFSET_BUTTON + y * BUTTON_SIZE),
                        Name = $"{x},{y}",
                        Font = textFont,
                        ForeColor = fontColor
                    };
                    //tambahkan listener button
                    boardButtons[x, y].Click += buttonClickHandler;
                    this.Controls.Add(boardButtons[x, y]);

                    // bikin SquareNodeBaru
                    SquareNode node = new SquareNode();
                    if (y == 8 || y == 0)
                    {
                        if (x == 2 || x == 4)
                        {
                            node.isTrap = true;
                        }
                        if (x == 3)
                        {
                            node.isDen = true;
                        }
                    }
                    else if (y == 1 || y == 7)
                    {
                        if (x == 3)
                        {
                            node.isTrap = true;
                        }
                    }

                    if (y >= 3 && y <= 5)
                    {
                        if (x == 1 || x == 2 || x == 4 || x == 5)
                        {
                            node.isWater = true;
                        }
                    }
                    board[x, y] = node;
                }
            }

            // INIT DEN DAN TRAPS OWNER
            board[3, 0].denOwner = 1;
            board[3, 8].denOwner = 2;
            board[2, 0].trapOwner = 1;
            board[4, 0].trapOwner = 1;
            board[3, 1].trapOwner = 1;
            board[2, 8].trapOwner = 2;
            board[4, 8].trapOwner = 2;
            board[3, 7].trapOwner = 2;

            // INIT DEN UNTUK PENGECHECKAN
            den1 = board[3, 0]; /* DEN ATAS */
            den2 = board[3, 8]; /* DEN BAWAH */

            // INIT ANIMAL PIECES
            board[0, 2].animal = new Piece(0, 1);
            board[5, 1].animal = new Piece(1, 1);
            board[4, 2].animal = new Piece(2, 1);
            board[1, 1].animal = new Piece(3, 1);

            board[2, 2].animal = new Piece(4, 1);
            board[6, 0].animal = new Piece(5, 1);
            board[0, 0].animal = new Piece(6, 1);
            board[6, 2].animal = new Piece(7, 1);

            board[0, 6].animal = new Piece(7, 2);
            board[5, 7].animal = new Piece(3, 2);
            board[4, 6].animal = new Piece(4, 2);
            board[1, 7].animal = new Piece(1, 2);

            board[2, 6].animal = new Piece(2, 2);
            board[6, 8].animal = new Piece(6, 2);
            board[0, 8].animal = new Piece(5, 2);
            board[6, 6].animal = new Piece(0, 2);

            // Init Animal Position
            board[0, 2].animal.position = new int[] { 0, 2 };
            board[5, 1].animal.position = new int[] { 5, 1 };
            board[4, 2].animal.position = new int[] { 4, 2 };
            board[1, 1].animal.position = new int[] { 1, 1 };
            board[2, 2].animal.position = new int[] { 2, 2 };
            board[6, 0].animal.position = new int[] { 6, 0 };

            board[0, 0].animal.position = new int[] { 0, 0 };

            board[6, 2].animal.position = new int[] { 6, 2 };
            board[0, 6].animal.position = new int[] { 0, 6 };
            board[5, 7].animal.position = new int[] { 5, 7 };
            board[4, 6].animal.position = new int[] { 4, 6 };
            board[1, 7].animal.position = new int[] { 1, 7 };
            board[2, 6].animal.position = new int[] { 2, 6 };
            board[6, 8].animal.position = new int[] { 6, 8 };
            board[0, 8].animal.position = new int[] { 0, 8 };
            board[6, 6].animal.position = new int[] { 6, 6 };

            magic_numbers = new List<int[,]>()
            {
                mouse_magic_number,
                cat_magic_number,
                wolf_magic_number,
                dog_magic_number,
                leopard_magic_number,
                tiger_magic_number,
                lion_magic_number,
                elephant_magic_number
            };
        }

        public SquareNode[,] getBoardClone(SquareNode[,] toCopy)
        {
            SquareNode[,] cloned = new SquareNode[7, 9];
            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 7; x++)
                {
                    cloned[x, y] = (SquareNode)toCopy[x, y].DeepClone();
                }
            }
            return cloned;
        }

        public void buttonClickHandler(object sender, EventArgs e)
        {
            Button clicked = (Button)sender;
            int[] coords = getButtonCoords(clicked);

            // check player click
            if (first_click == null)
            {
                //kalau first click maka firstclick = current button, First Click harus animal
                if (board[coords[0], coords[1]].animal != null && board[coords[0], coords[1]].animal.player == giliran)
                {
                    first_click = board[coords[0], coords[1]];
                }
            }
            else
            {
                //kalau nggak first click
                second_click = board[coords[0], coords[1]];
                
                //get around untuk check
                Dictionary<string, SquareNode> around_first_click = getAround(first_click);
                // check apakah second click itu valid
                if (around_first_click.ContainsValue(second_click))
                {
                    //if valid then check apabila ada animal
                    if (second_click.isDen && second_click.denOwner == giliran)
                    {
                        // tidak bisa masuk den sendiri
                        MessageBox.Show("Tidak Bisa Masuk Den Sendiri");
                    }
                    else
                    {
                        int str = first_click.animal.strength;
                        //check str untuk check special move
                        if(str == 0)
                        {
                            // rat
                            if (checkMoveRat(second_click, around_first_click, coords))
                            {
                                changeGiliran();
                            }
                        }
                        else if(str == 5 || str == 6)
                        {
                            // lion / tiger
                            //check gerakanLion/Tiger
                            if(checkMoveLionTiger(second_click, around_first_click, coords))
                            {
                                changeGiliran();
                            }
                        }
                        else if(str == 7)
                        {
                            if(checkMoveElephant(second_click, around_first_click, coords))
                            {
                                changeGiliran();
                            }
                        }
                        else
                        {
                            //move biasa (Hewan tanpa special move)
                            if(checkRegularMove(second_click, around_first_click, coords))
                            {
                                changeGiliran();
                            }
                        }
                    }
                }
                else if (first_click == second_click)
                {
                    //cancel move
                    first_click = null;
                    second_click = null;
                }
                else
                {
                    MessageBox.Show("Move Tidak Valid!");
                }
            }
            updateUI();
        }

        public void moveAnimal(SquareNode second, int[] coordinates)
        {
            second.removeAnimal();
            second.animal = first_click.animal;
            second.animal.position = coordinates;
            first_click.animal = null;
            first_click = null;
            second = null;
        }

        public bool crossRiverDown(SquareNode second, int[] coordinates)
        {
            return second.animal == null && board[coordinates[0], coordinates[1] + 1].animal == null && board[coordinates[0], coordinates[1] + 2].animal == null && (board[coordinates[0], coordinates[1] + 3].animal == null || (board[coordinates[0], coordinates[1] + 3].animal.strength <= first_click.animal.strength && first_click.animal.player != board[coordinates[0], coordinates[1] + 3].animal.player));
        }

        public bool crossRiverUp(SquareNode second, int[] coordinates)
        {
            return second.animal == null && board[coordinates[0], coordinates[1] - 1].animal == null && board[coordinates[0], coordinates[1] - 2].animal == null && (board[coordinates[0], coordinates[1] - 3].animal == null || (board[coordinates[0], coordinates[1] - 3].animal.strength <= first_click.animal.strength && first_click.animal.player != board[coordinates[0], coordinates[1] - 3].animal.player));
        }

        public bool crossRiverLeft(SquareNode second, int[] coordinates)
        {
            return second.animal == null && board[coordinates[0] - 1, coordinates[1]].animal == null && (board[coordinates[0] - 2, coordinates[1]].animal == null || (board[coordinates[0] - 2, coordinates[1]].animal.strength <= first_click.animal.strength && first_click.animal.player != board[coordinates[0] - 2, coordinates[1]].animal.player));
        }

        public bool crossRiverRight(SquareNode second, int[] coordinates)
        {
            return second.animal == null && board[coordinates[0] + 1, coordinates[1]].animal == null && (board[coordinates[0] + 2, coordinates[1]].animal == null || (board[coordinates[0] + 2, coordinates[1]].animal.strength <= first_click.animal.strength && first_click.animal.player != board[coordinates[0] + 2, coordinates[1]].animal.player));
        }

        public bool checkRegularMove(SquareNode second, Dictionary<string, SquareNode> around, int[] coordinates)
        {
            if (!second.isWater)
            {
                if (second.animal == null || (second.animal.strength <= first_click.animal.strength && second.animal.player != first_click.animal.player) || second.isTrap)
                {
                    moveAnimal(second, coordinates);
                    return true;
                }
            }
            return false;
        }

        public bool checkMoveLionTiger(SquareNode second, Dictionary<string, SquareNode> around, int[] coordinates)
        {
            //check secondclick
            if(second.isWater)
            {
                //check lompat 
                string key = around.FirstOrDefault(x => x.Value == second).Key;
                if (key == "atas")
                {
                    //check atas kosong
                    if (crossRiverUp(second, coordinates))
                    {
                        coordinates[1] -= 3;
                        second = board[coordinates[0], coordinates[1]];
                        moveAnimal(second, coordinates);
                        return true;
                    }
                }
                else if (key == "bawah")
                {
                    //check bawah kosong
                    if (crossRiverDown(second, coordinates))
                    {
                        coordinates[1] += 3;
                        second = board[coordinates[0], coordinates[1]];
                        moveAnimal(second, coordinates);
                        return true;
                    }
                }
                else if (key == "kiri")
                {
                    //check kiri kosong
                    if (crossRiverLeft(second, coordinates))
                    {
                        coordinates[0] -= 2;
                        second = board[coordinates[0], coordinates[1]];
                        moveAnimal(second, coordinates);
                        return true;
                    }
                }
                else if (key == "kanan")
                {
                    // check kanan kosong
                    if (crossRiverRight(second, coordinates))
                    {
                        coordinates[0] += 2;
                        second = board[coordinates[0], coordinates[1]];
                        moveAnimal(second, coordinates);
                        return true;
                    }
                }
            }
            else
            {
                //jika tidak lompat maka check biasa
                return checkRegularMove(second, around, coordinates);
            }
            return false;
        }

        public bool checkMoveRat(SquareNode second, Dictionary<string, SquareNode> around, int[] coordinates)
        {
            // check secondclick
            if(first_click.isWater && second.isWater)
            {
                // gerak dalam air
                // check apakah ada hewan, kalau ada tikus lain gak ngurus karena tikus bisa makan tikus dan karena pergerakan air ke air
                moveAnimal(second, coordinates);
                return true;
            }
            else
            {
                // gerak masuk air / gerak keluar air / gerak biasa
                // check apakah animal pada second click null atau tidak, kalau nggak null maka check strength
                if(second.animal != null)
                {
                    // check str, tikus makan tikus / tikus makan gajah dan check makan diri sendiri
                    if(second.isWater != first_click.isWater)
                    {
                        //masuk dalem air / keluar air (KALAU NGGAK NULL GAK BISA MAKAN)
                        return false;
                    }
                    else if(second.animal.player != giliran && (second.animal.strength == 0 || second.animal.strength == 7 || second.isTrap))
                    {
                        // kalau musuh merupakan tikus / gajah / atau apapun yang menempati trap, makan
                        moveAnimal(second, coordinates);
                        return true;
                    }
                }
                else
                {
                    //is null 
                    moveAnimal(second, coordinates);
                    return true;
                }
            }
            return false;
        }

        public bool checkMoveElephant(SquareNode second, Dictionary<string, SquareNode> around, int[] coordinates)
        {
            // check secondclick water?
            if(!second.isWater)
            {
                if (second.animal == null || second.animal.strength != 0)
                {
                    moveAnimal(second, coordinates);
                    return true;
                }
            }
            return false;
        }

        public Dictionary<string, SquareNode> getAround(SquareNode first_click)
        {
            Dictionary<string, SquareNode> around = new Dictionary<string, SquareNode>();
            int x = first_click.animal.position[0];
            int y = first_click.animal.position[1];

            if (y - 1 >= 0)
            {
                around.Add("atas", board[x, y - 1]);
            }
            if (y + 1 < 9)
            {
                around.Add("bawah", board[x, y + 1]);
            }
            if (x - 1 >= 0)
            {
                around.Add("kiri", board[x - 1, y]);
            }
            if (x + 1 < 7)
            {
                around.Add("kanan", board[x + 1, y]);
            }

            return around;
        }

        public int[] getButtonCoords(Button b)
        {
            string[] str = b.Name.Split(',');
            return new int[] { Convert.ToInt32(str[0]), Convert.ToInt32(str[1]) };
        }

        public void checkWin()
        { 
            if(den1.animal != null)
            {
                MessageBox.Show("Player Menang");
                this.Close();
            }
            else if(den2.animal != null)
            {
                MessageBox.Show("Computer Menang");
                this.Close();
            }
        }

        public void changeGiliran()
        {
            giliran = giliran == 1 ? 2 : 1;
            updateUI();
            checkWin();

            //gilirancomputer
            if (giliran == 1)
            {
                //MessageBox.Show("Giliran Computer");
                SquareNode[,] clone = getBoardClone(board);

                // init depth
                int depth = 3;

                List<object> m = MiniMax(clone, depth, 1, new Move(clone), int.MinValue, int.MaxValue);
                Move t = (Move) m[0];
                
                while(t.nextMove.nextMove != null)
                {
                    if(t.nextMove != null)
                    {
                        t = t.nextMove;
                    }
                    else
                    {
                        break;
                    }
                }

                if(t != null)
                {
                    board = t.currentBoard;
                    if (t.currentBoard[3, 0].animal != null)
                    {
                        den1.animal = t.currentBoard[3, 0].animal;
                    }
                    else if (t.currentBoard[3, 8].animal != null)
                    {
                        den2.animal = t.currentBoard[3, 8].animal;
                    }
                    changeGiliran();
                }
                AI_first_move = false;
            }
            updateUI();
        }
        
        private int getShaktiValue(int str, int[] coords)
        {
            try
            {
                if(AI_first_move == true)
                {
                    return magic_numbers[rand.Next(7)][coords[1], coords[0]];
                }
                return magic_numbers[str][coords[1], coords[0]];
            }
            catch
            {
                return 0;
            }
        }
        
        private List<object> MiniMax(SquareNode[,] board, int depth, int giliran_now, Move last_move, int alpha, int beta)
        {
            if (depth == 0)
            {
                // kalau depth sudah 3 (sudah masuk node depth ke 3 ) Maka Hitung SBE
                // initial value
                int value = 0;
                SquareNode[,] copy = last_move.currentBoard;

                // initialize myPieces and enemyPieces
                List<Piece> myPieces, enemyPieces;
                myPieces = new List<Piece>();
                enemyPieces = new List<Piece>();

                
                // Ambil myPieces dan enemyPieces
                for (int y = 0; y < 9; y++)
                {
                    for (int x = 0; x < 7; x++)
                    {
                        SquareNode currentPiece = copy[x, y];
                        if (currentPiece.animal != null)
                        {
                            if (currentPiece.animal.player == giliran)
                            {
                                myPieces.Add(currentPiece.animal);
                            }
                            else
                            {
                                enemyPieces.Add(currentPiece.animal);
                            }
                        }
                    }
                }
                enemyPieces.Shuffle();
                myPieces.Shuffle();

                value += ((myPieces.Count - enemyPieces.Count) * 100);

                // set jarak
                int jarakX = int.MinValue, jarakY = int.MinValue;
                int dx = int.MaxValue, dy = int.MaxValue;

                foreach (Piece item in myPieces)
                {
                    jarakX = 3 - item.position[0];
                    jarakX = Math.Abs(jarakX);
                    value += getShaktiValue(item.strength, item.position);
                    if (giliran == 1)
                    {
                        jarakY = Math.Abs(8 - item.position[1]);
                    }
                    else
                    {
                        jarakY = Math.Abs(0 - item.position[1]);
                    }
                    if (jarakY == jarakX && jarakX == 0)
                    {
                        value = int.MaxValue;
                        return new List<object>()
                        {
                            last_move,
                            value
                        };
                    }
                    else
                    {
                        if (dx > jarakX) dx = jarakX;
                        if (dy > jarakY) dy = jarakY;
                    }
                }
                value += (((jarakX + jarakY) / 2) * -1);
                return new List<object>()
                {
                    last_move,
                    value
                };
            }
            else
            {
                // Kalau belum Depth 3 maka lanjut rekur
                List<Piece> willMove = new List<Piece>();
                int ctr = 0;
                
                // Ambil semua piece yang akan gerak pada turn ini
                for(int y = 0; y < 9; y++)
                {
                    for (int x = 0; x < 7; x++)
                    {
                        SquareNode node = board[x, y];
                        if(node.animal != null)
                        {
                            if(node.animal.isAlive && node.animal.player == giliran_now)
                            {
                                willMove.Add(node.animal);
                                ++ctr;
                            }
                        }
                    }
                }

                // Initialize Boards untuk simpan tree(?) papan. 
                List<SquareNode[,]> boards = new List<SquareNode[,]>();

                // Loop setiap piece yang akan gerak
                foreach(Piece myAnimal in willMove)
                {
                    //check possible moves (Atas, bawah, kiri, kanan) tidak outof bound
                    var possibleMoves = findPossibleMoves(myAnimal.position, board);
                    foreach (var move in possibleMoves)
                    {
                        //untuk setiap possible moves copy board lalu coba gerak 
                        SquareNode[,] temp = getBoardClone(board);
                        int x = myAnimal.position[0];
                        int y = myAnimal.position[1];

                        //check if move keynya atas / bawah / kiri / kanan
                        if(move.Key == "atas")
                        {
                            tryMoveUp(boards, myAnimal, temp, x, y);
                        }
                        else if(move.Key == "bawah")
                        {
                            tryMoveDown(boards, myAnimal, temp, x, y);
                        }
                        else if (move.Key == "kiri")
                        {
                            tryMoveLeft(boards, myAnimal, temp, x, y);
                        }
                        else if (move.Key == "kanan")
                        {
                            tryMoveRight(boards, myAnimal, temp, x, y);
                        }
                    }
                }

                // siapkann return
                List<object> kembalian = new List<object>();
                foreach (SquareNode[,] item in boards)
                {
                    SquareNode[,] clonedBoard = getBoardClone(item);
                    Move now = new Move(clonedBoard);
                    now.nextMove = last_move;
                    List<object> tampung = MiniMax(clonedBoard, depth - 1, giliran_now == 1 ? 2 : 1, now, alpha, beta);
                    if(kembalian.Count == 0)
                    {
                        kembalian = tampung;
                    }

                    //if (giliran_now == giliran)
                    //{
                    //    tampung[1] = (int)tampung[1] * -1;

                    //    if (alpha < (int)tampung[1])
                    //    {
                    //        alpha = (int)tampung[1];
                    //        kembalian = tampung;
                    //    }
                    //}
                    //else
                    //{
                        
                    //    int temp_beta = beta;
                    //    beta = alpha * -1;
                    //    alpha = temp_beta * -1;
                    //}

                    if (giliran_now == giliran)
                    {
                        // get Max
                        if (alpha < (int)tampung[1])
                        {
                            // swap
                            alpha = (int)tampung[1];
                            kembalian = tampung;
                        }
                    }
                    else
                    {
                        // get Min
                        if (beta > (int)tampung[1])
                        {
                            beta = (int)tampung[1];
                            kembalian = tampung;
                        }
                    }

                    // PRUNE
                    if (alpha >= beta)
                    {
                        break;
                    }
                }
                return kembalian;
            }
        }

        private void tryMoveUp(List<SquareNode[,]> boards, Piece myAnimal, SquareNode[,] temp_board, int x, int y)
        {
            // Ambil Node atas dari myAnimal
            SquareNode check_node = temp_board[x, y - 1];
            if(check_node.isDen && check_node.denOwner == myAnimal.player)
            {
                // Tidak bisa masuk den sendiri
            }
            else if(check_node.isDen && check_node.denOwner != myAnimal.player)
            {
                // masuk o den e musuh
                check_node.animal = temp_board[x, y].animal;
                check_node.animal.position = new int[] { x, y - 1 };
                temp_board[x, y].animal = null;
                boards.Add(temp_board);
            }
            else
            {
                // tidak masuk den, maka check apakah petak yang akan dituju kosong dan bukan water?
                if(check_node.animal == null && !check_node.isWater)
                {
                    //kalau iya langsung tancap gas
                    check_node.animal = temp_board[x, y].animal;
                    check_node.animal.position = new int[] { x, y-1 };
                    temp_board[x, y].animal = null;
                    boards.Add(temp_board);
                }
                else
                {
                    //kalau nggak null atau nggak air check dulu str supaya bisa check special move (?)
                    if (myAnimal.strength == 0)
                    {
                        // check special move rat
                        // check gerak dalam air
                        if(check_node.isWater &&  temp_board[x,y].isWater)
                        {
                            check_node.animal = temp_board[x, y].animal;
                            check_node.animal.position = new int[] { x, y - 1 };
                            temp_board[x, y].animal = null;
                            boards.Add(temp_board);
                        }
                        else
                        {
                            // gerak masuk air / gerak keluar air / gerak biasa
                            // check apakah animal pada checknode  null atau tidak, kalau nggak null maka check strength
                            if(check_node.animal != null)
                            {
                                if(check_node.isWater != temp_board[x,y].isWater)
                                {
                                    //masuk dalem air / keluar air (KALAU NGGAK NULL GAK BISA MAKAN)
                                }
                                else if(check_node.animal.player != myAnimal.player && (check_node.animal.strength == 0 || check_node.animal.strength == 7 || check_node.isTrap))
                                {
                                    // kalau musuh merupakan tikus / gajah / atau apapun yang menempati trap, makan
                                    check_node.animal = temp_board[x, y].animal;
                                    check_node.animal.position = new int[] { x, y - 1 };
                                    temp_board[x, y].animal = null;
                                    boards.Add(temp_board);
                                }
                            }
                            else
                            {
                                // is null gaskan
                                check_node.animal = temp_board[x, y].animal;
                                check_node.animal.position = new int[] { x, y - 1 };
                                temp_board[x, y].animal = null;
                                boards.Add(temp_board);
                            }
                        }
                    }
                    else if (myAnimal.strength == 5 || myAnimal.strength == 6)
                    {
                        // check special move jump (Lion and Tiger)
                        // check depannya air?
                        if(check_node.isWater)
                        {
                            // check lompat, cek depannya null dan kotak ke2 dan kotak 3 null
                            if(check_node.animal == null && temp_board[x, y-2].animal == null && temp_board[x,y-3].animal == null)
                            {
                                // bisa lompat tapi check tujuan lompat apabila ada animal diseberang?
                                if (temp_board[x, y - 4].animal == null)
                                {
                                    // tidak ada animal maka lansung tancap gas
                                    temp_board[x, y - 4].animal = temp_board[x, y].animal;
                                    temp_board[x, y - 4].animal.position = new int[] { x, y - 4 };
                                    temp_board[x, y].animal = null;
                                    boards.Add(temp_board);
                                }
                                else if(temp_board[x, y - 4].animal.player != myAnimal.player)
                                {
                                    // ada animal maka check str
                                    if(myAnimal.strength >= temp_board[x,y-4].animal.strength)
                                    {
                                        temp_board[x, y - 4].animal = temp_board[x, y].animal;
                                        temp_board[x, y - 4].animal.position = new int[] { x, y - 4 };
                                        temp_board[x, y].animal = null;
                                        boards.Add(temp_board);
                                    }
                                }
                            }
                        }
                        else
                        {
                            // bukan air then check depannya null gak, kalau null langsung tapi kalau nggak brarti check str
                            if(check_node.animal == null)
                            {
                                check_node.animal = temp_board[x, y].animal;
                                check_node.animal.position = new int[] { x, y - 1 };
                                temp_board[x, y].animal = null;
                                boards.Add(temp_board);
                            }
                            else
                            {
                                if (myAnimal.strength >= check_node.animal.strength && myAnimal.player != check_node.animal.player)
                                {
                                    // kalau str dan tidak allies gaskan
                                    check_node.animal = temp_board[x, y].animal;
                                    check_node.animal.position = new int[] { x, y - 1 };
                                    temp_board[x, y].animal = null;
                                    boards.Add(temp_board);
                                }
                            }
                        }
                    }
                    else if (myAnimal.strength == 7)
                    {
                        //check gajah tidak bisa makan tikus
                        if(!check_node.isWater)
                        {
                            if(check_node.animal != null)
                            {
                                //check apakah itu tikus dan bukan punyaku?
                                if (check_node.animal.strength != 0 && check_node.animal.player != myAnimal.player)
                                {
                                    check_node.animal = temp_board[x, y].animal;
                                    check_node.animal.position = new int[] { x, y - 1 };
                                    temp_board[x, y].animal = null;
                                    boards.Add(temp_board);
                                }
                            }
                            else
                            {
                                check_node.animal = temp_board[x, y].animal;
                                check_node.animal.position = new int[] { x, y - 1 };
                                temp_board[x, y].animal = null;
                                boards.Add(temp_board);
                            }
                        }
                    }
                    else
                    {
                        // check gerak biasa
                        if(!check_node.isWater)
                        {
                            // kalau bukan water
                            if( check_node.animal.player != myAnimal.player && myAnimal.strength >= check_node.animal.strength)
                            {
                                check_node.animal = temp_board[x, y].animal;
                                check_node.animal.position = new int[] { x, y - 1 };
                                temp_board[x, y].animal = null;
                                boards.Add(temp_board);
                            }
                        }
                    }
                }
            }
        }

        private void tryMoveDown(List<SquareNode[,]> boards, Piece myAnimal, SquareNode[,] temp_board, int x, int y)
        {
            // Ambil Node atas dari myAnimal
            SquareNode check_node = temp_board[x, y + 1];
            if (check_node.isDen && check_node.denOwner == myAnimal.player)
            {
                // Tidak bisa masuk den sendiri
            }
            else if (check_node.isDen && check_node.denOwner != myAnimal.player)
            {
                // masuk o den e musuh
                check_node.animal = temp_board[x, y].animal;
                check_node.animal.position = new int[] { x, y + 1 };
                temp_board[x, y].animal = null;
                boards.Add(temp_board);
            }
            else
            {
                // tidak masuk den, maka check apakah petak yang akan dituju kosong dan bukan water?
                if (check_node.animal == null && !check_node.isWater)
                {
                    //kalau iya langsung tancap gas
                    check_node.animal = temp_board[x, y].animal;
                    check_node.animal.position = new int[] { x, y + 1 };
                    temp_board[x, y].animal = null;
                    boards.Add(temp_board);
                }
                else
                {
                    //kalau nggak null atau nggak air check dulu str supaya bisa check special move (?)
                    if (myAnimal.strength == 0)
                    {
                        // check special move rat
                        // check gerak dalam air
                        if (check_node.isWater && temp_board[x, y].isWater)
                        {
                            check_node.animal = temp_board[x, y].animal;
                            check_node.animal.position = new int[] { x, y + 1 };
                            temp_board[x, y].animal = null;
                            boards.Add(temp_board);
                        }
                        else
                        {
                            // gerak masuk air / gerak keluar air / gerak biasa
                            // check apakah animal pada checknode  null atau tidak, kalau nggak null maka check strength
                            if (check_node.animal != null)
                            {
                                if (check_node.isWater != temp_board[x, y].isWater)
                                {
                                    //masuk dalem air / keluar air (KALAU NGGAK NULL GAK BISA MAKAN)
                                }
                                else if (check_node.animal.player != myAnimal.player && (check_node.animal.strength == 0 || check_node.animal.strength == 7 || check_node.isTrap))
                                {
                                    // kalau musuh merupakan tikus / gajah / atau apapun yang menempati trap, makan
                                    check_node.animal = temp_board[x, y].animal;
                                    check_node.animal.position = new int[] { x, y + 1 };
                                    temp_board[x, y].animal = null;
                                    boards.Add(temp_board);
                                }
                            }
                            else
                            {
                                // is null gaskan
                                check_node.animal = temp_board[x, y].animal;
                                check_node.animal.position = new int[] { x, y + 1 };
                                temp_board[x, y].animal = null;
                                boards.Add(temp_board);
                            }
                        }
                    }
                    else if (myAnimal.strength == 5 || myAnimal.strength == 6)
                    {
                        // check special move jump (Lion and Tiger)
                        // check depannya air?
                        if (check_node.isWater)
                        {
                            // check lompat, cek depannya null dan kotak ke2 dan kotak 3 null
                            if (check_node.animal == null && temp_board[x, y + 2].animal == null && temp_board[x, y + 3].animal == null)
                            {
                                // bisa lompat tapi check tujuan lompat apabila ada animal diseberang?
                                if (temp_board[x, y + 4].animal == null)
                                {
                                    // tidak ada animal maka lansung tancap gas
                                    temp_board[x, y + 4].animal = temp_board[x, y].animal;
                                    temp_board[x, y + 4].animal.position = new int[] { x, y + 4 };
                                    temp_board[x, y].animal = null;
                                    boards.Add(temp_board);
                                }
                                else if (temp_board[x, y + 4].animal.player != myAnimal.player)
                                {
                                    // ada animal maka check str
                                    if (myAnimal.strength >= temp_board[x, y + 4].animal.strength)
                                    {
                                        temp_board[x, y + 4].animal = temp_board[x, y].animal;
                                        temp_board[x, y + 4].animal.position = new int[] { x, y + 4 };
                                        temp_board[x, y].animal = null;
                                        boards.Add(temp_board);
                                    }
                                }
                            }
                        }
                        else
                        {
                            // bukan air then check depannya null gak, kalau null langsung tapi kalau nggak brarti check str
                            if (check_node.animal == null)
                            {
                                check_node.animal = temp_board[x, y].animal;
                                check_node.animal.position = new int[] { x, y + 1 };
                                temp_board[x, y].animal = null;
                                boards.Add(temp_board);
                            }
                            else
                            {
                                if (myAnimal.strength >= check_node.animal.strength && myAnimal.player != check_node.animal.player)
                                {
                                    // kalau str dan tidak allies gaskan
                                    check_node.animal = temp_board[x, y].animal;
                                    check_node.animal.position = new int[] { x, y + 1 };
                                    temp_board[x, y].animal = null;
                                    boards.Add(temp_board);
                                }
                            }
                        }
                    }
                    else if (myAnimal.strength == 7)
                    {
                        //check gajah tidak bisa makan tikus
                        if (!check_node.isWater)
                        {
                            if (check_node.animal != null)
                            {
                                //check apakah itu tikus dan bukan punyaku?
                                if (check_node.animal.strength != 0 && check_node.animal.player != myAnimal.player)
                                {
                                    check_node.animal = temp_board[x, y].animal;
                                    check_node.animal.position = new int[] { x, y + 1 };
                                    temp_board[x, y].animal = null;
                                    boards.Add(temp_board);
                                }
                            }
                            else
                            {
                                check_node.animal = temp_board[x, y].animal;
                                check_node.animal.position = new int[] { x, y + 1 };
                                temp_board[x, y].animal = null;
                                boards.Add(temp_board);
                            }
                        }
                    }
                    else
                    {
                        // check gerak biasa
                        if (!check_node.isWater)
                        {
                            // kalau bukan water
                            if (check_node.animal.player != myAnimal.player && myAnimal.strength >= check_node.animal.strength)
                            {
                                check_node.animal = temp_board[x, y].animal;
                                check_node.animal.position = new int[] { x, y + 1 };
                                temp_board[x, y].animal = null;
                                boards.Add(temp_board);
                            }
                        }
                    }
                }
            }
        }

        private void tryMoveLeft(List<SquareNode[,]> boards, Piece myAnimal, SquareNode[,] temp_board, int x, int y)
        {
            // Ambil Node kiri dari MyAnimal
            SquareNode check_node = temp_board[x - 1, y];
            if (check_node.isDen && check_node.denOwner == myAnimal.player)
            {
                // Tidak bisa masuk den sendiri
            }
            else if (check_node.isDen && check_node.denOwner != myAnimal.player)
            {
                // masuk o den e musuh
                check_node.animal = temp_board[x, y].animal;
                check_node.animal.position = new int[] { x - 1, y };
                temp_board[x, y].animal = null;
                boards.Add(temp_board);
            }
            else
            {
                // tidak masuk den, maka check apakah petak yang akan dituju kosong dan bukan water?
                if (check_node.animal == null && !check_node.isWater)
                {
                    //kalau iya langsung tancap gas
                    check_node.animal = temp_board[x, y].animal;
                    check_node.animal.position = new int[] { x - 1, y };
                    temp_board[x, y].animal = null;
                    boards.Add(temp_board);
                }
                else
                {
                    //kalau nggak null atau nggak air check dulu str supaya bisa check special move (?)
                    if (myAnimal.strength == 0)
                    {
                        // check special move rat
                        // check gerak dalam air
                        if (check_node.isWater && temp_board[x, y].isWater)
                        {
                            check_node.animal = temp_board[x, y].animal;
                            check_node.animal.position = new int[] { x - 1, y };
                            temp_board[x, y].animal = null;
                            boards.Add(temp_board);
                        }
                        else
                        {
                            // gerak masuk air / gerak keluar air / gerak biasa
                            // check apakah animal pada checknode  null atau tidak, kalau nggak null maka check strength
                            if (check_node.animal != null)
                            {
                                if (check_node.isWater != temp_board[x, y].isWater)
                                {
                                    //masuk dalem air / keluar air (KALAU NGGAK NULL GAK BISA MAKAN)
                                }
                                else if (check_node.animal.player != myAnimal.player && (check_node.animal.strength == 0 || check_node.animal.strength == 7 || check_node.isTrap))
                                {
                                    // kalau musuh merupakan tikus / gajah / atau apapun yang menempati trap, makan
                                    check_node.animal = temp_board[x, y].animal;
                                    check_node.animal.position = new int[] { x - 1, y };
                                    temp_board[x, y].animal = null;
                                    boards.Add(temp_board);
                                }
                            }
                            else
                            {
                                // is null gaskan
                                check_node.animal = temp_board[x, y].animal;
                                check_node.animal.position = new int[] { x - 1, y };
                                temp_board[x, y].animal = null;
                                boards.Add(temp_board);
                            }
                        }
                    }
                    else if (myAnimal.strength == 5 || myAnimal.strength == 6)
                    {
                        // check special move jump (Lion and Tiger)
                        // check kirinya air?
                        if (check_node.isWater)
                        {
                            // check lompat, cek kirinya null dan kotak ke2
                            if (check_node.animal == null && temp_board[x - 2, y].animal == null)
                            {
                                // bisa lompat tapi check tujuan lompat apabila ada animal diseberang?
                                if (temp_board[x - 3, y ].animal == null)
                                {
                                    // tidak ada animal maka lansung tancap gas
                                    temp_board[x - 3, y ].animal = temp_board[x, y].animal;
                                    temp_board[x - 3, y].animal.position = new int[] { x - 3, y };
                                    temp_board[x, y].animal = null;
                                    boards.Add(temp_board);
                                }
                                else if (temp_board[x - 3, y].animal.player != myAnimal.player)
                                {
                                    // ada animal maka check str
                                    if (myAnimal.strength >= temp_board[x - 3, y].animal.strength)
                                    {
                                        // tidak ada animal maka lansung tancap gas
                                        temp_board[x - 3, y].animal = temp_board[x, y].animal;
                                        temp_board[x - 3, y].animal.position = new int[] { x - 3, y };
                                        temp_board[x, y].animal = null;
                                        boards.Add(temp_board);
                                    }
                                }
                            }
                        }
                        else
                        {
                            // bukan air then check depannya null gak, kalau null langsung tapi kalau nggak brarti check str
                            if (check_node.animal == null)
                            {
                                check_node.animal = temp_board[x, y].animal;
                                check_node.animal.position = new int[] { x - 1, y };
                                temp_board[x, y].animal = null;
                                boards.Add(temp_board);
                            }
                            else
                            {
                                if (myAnimal.strength >= check_node.animal.strength && myAnimal.player != check_node.animal.player)
                                {
                                    // kalau str dan tidak allies gaskan
                                    check_node.animal = temp_board[x, y].animal;
                                    check_node.animal.position = new int[] { x - 1, y };
                                    temp_board[x, y].animal = null;
                                    boards.Add(temp_board);
                                }
                            }
                        }
                    }
                    else if (myAnimal.strength == 7)
                    {
                        //check gajah tidak bisa makan tikus
                        if (!check_node.isWater)
                        {
                            if (check_node.animal != null)
                            {
                                //check apakah itu tikus dan bukan punyaku?
                                if (check_node.animal.strength != 0 && check_node.animal.player != myAnimal.player)
                                {
                                    check_node.animal = temp_board[x, y].animal;
                                    check_node.animal.position = new int[] { x - 1, y };
                                    temp_board[x, y].animal = null;
                                    boards.Add(temp_board);
                                }
                            }
                            else
                            {
                                check_node.animal = temp_board[x, y].animal;
                                check_node.animal.position = new int[] { x - 1, y };
                                temp_board[x, y].animal = null;
                                boards.Add(temp_board);
                            }
                        }
                    }
                    else
                    {
                        // check gerak biasa
                        if (!check_node.isWater)
                        {
                            // kalau bukan water
                            if (check_node.animal.player != myAnimal.player && myAnimal.strength >= check_node.animal.strength)
                            {
                                check_node.animal = temp_board[x, y].animal;
                                check_node.animal.position = new int[] { x - 1, y };
                                temp_board[x, y].animal = null;
                                boards.Add(temp_board);
                            }
                        }
                    }
                }
            }
        }

        private void tryMoveRight(List<SquareNode[,]> boards, Piece myAnimal, SquareNode[,] temp_board, int x, int y)
        {
            // Ambil Node kiri dari MyAnimal
            SquareNode check_node = temp_board[x + 1, y];
            if (check_node.isDen && check_node.denOwner == myAnimal.player)
            {
                // Tidak bisa masuk den sendiri
            }
            else if (check_node.isDen && check_node.denOwner != myAnimal.player)
            {
                // masuk o den e musuh
                check_node.animal = temp_board[x, y].animal;
                check_node.animal.position = new int[] { x + 1, y };
                temp_board[x, y].animal = null;
                boards.Add(temp_board);
            }
            else
            {
                // tidak masuk den, maka check apakah petak yang akan dituju kosong dan bukan water?
                if (check_node.animal == null && !check_node.isWater)
                {
                    //kalau iya langsung tancap gas
                    check_node.animal = temp_board[x, y].animal;
                    check_node.animal.position = new int[] { x + 1, y };
                    temp_board[x, y].animal = null;
                    boards.Add(temp_board);
                }
                else
                {
                    //kalau nggak null atau nggak air check dulu str supaya bisa check special move (?)
                    if (myAnimal.strength == 0)
                    {
                        // check special move rat
                        // check gerak dalam air
                        if (check_node.isWater && temp_board[x, y].isWater)
                        {
                            check_node.animal = temp_board[x, y].animal;
                            check_node.animal.position = new int[] { x + 1, y };
                            temp_board[x, y].animal = null;
                            boards.Add(temp_board);
                        }
                        else
                        {
                            // gerak masuk air / gerak keluar air / gerak biasa
                            // check apakah animal pada checknode  null atau tidak, kalau nggak null maka check strength
                            if (check_node.animal != null)
                            {
                                if (check_node.isWater != temp_board[x, y].isWater)
                                {
                                    //masuk dalem air / keluar air (KALAU NGGAK NULL GAK BISA MAKAN)
                                }
                                else if (check_node.animal.player != myAnimal.player && (check_node.animal.strength == 0 || check_node.animal.strength == 7 || check_node.isTrap))
                                {
                                    // kalau musuh merupakan tikus / gajah / atau apapun yang menempati trap, makan
                                    check_node.animal = temp_board[x, y].animal;
                                    check_node.animal.position = new int[] { x + 1, y };
                                    temp_board[x, y].animal = null;
                                    boards.Add(temp_board);
                                }
                            }
                            else
                            {
                                // is null gaskan
                                check_node.animal = temp_board[x, y].animal;
                                check_node.animal.position = new int[] { x + 1, y };
                                temp_board[x, y].animal = null;
                                boards.Add(temp_board);
                            }
                        }
                    }
                    else if (myAnimal.strength == 5 || myAnimal.strength == 6)
                    {
                        // check special move jump (Lion and Tiger)
                        // check kirinya air?
                        if (check_node.isWater)
                        {
                            // check lompat, cek kirinya null dan kotak ke2
                            if (check_node.animal == null && temp_board[x + 2, y].animal == null)
                            {
                                // bisa lompat tapi check tujuan lompat apabila ada animal diseberang?
                                if (temp_board[x + 3, y].animal == null)
                                {
                                    // tidak ada animal maka lansung tancap gas
                                    temp_board[x + 3, y].animal = temp_board[x, y].animal;
                                    temp_board[x + 3, y].animal.position = new int[] { x + 3, y };
                                    temp_board[x, y].animal = null;
                                    boards.Add(temp_board);
                                }
                                else if (temp_board[x + 3, y].animal.player != myAnimal.player)
                                {
                                    // ada animal maka check str
                                    if (myAnimal.strength >= temp_board[x + 3, y].animal.strength)
                                    {
                                        // tidak ada animal maka lansung tancap gas
                                        temp_board[x + 3, y].animal = temp_board[x, y].animal;
                                        temp_board[x + 3, y].animal.position = new int[] { x + 3, y };
                                        temp_board[x, y].animal = null;
                                        boards.Add(temp_board);
                                    }
                                }
                            }
                        }
                        else
                        {
                            // bukan air then check depannya null gak, kalau null langsung tapi kalau nggak brarti check str
                            if (check_node.animal == null)
                            {
                                check_node.animal = temp_board[x, y].animal;
                                check_node.animal.position = new int[] { x + 1, y };
                                temp_board[x, y].animal = null;
                                boards.Add(temp_board);
                            }
                            else
                            {
                                if (myAnimal.strength >= check_node.animal.strength && myAnimal.player != check_node.animal.player)
                                {
                                    // kalau str dan tidak allies gaskan
                                    check_node.animal = temp_board[x, y].animal;
                                    check_node.animal.position = new int[] { x + 1, y };
                                    temp_board[x, y].animal = null;
                                    boards.Add(temp_board);
                                }
                            }
                        }
                    }
                    else if (myAnimal.strength == 7)
                    {
                        //check gajah tidak bisa makan tikus
                        if (!check_node.isWater)
                        {
                            if (check_node.animal != null)
                            {
                                //check apakah itu tikus dan bukan punyaku?
                                if (check_node.animal.strength != 0 && check_node.animal.player != myAnimal.player)
                                {
                                    check_node.animal = temp_board[x, y].animal;
                                    check_node.animal.position = new int[] { x + 1, y };
                                    temp_board[x, y].animal = null;
                                    boards.Add(temp_board);
                                }
                            }
                            else
                            {
                                check_node.animal = temp_board[x, y].animal;
                                check_node.animal.position = new int[] { x + 1, y };
                                temp_board[x, y].animal = null;
                                boards.Add(temp_board);
                            }
                        }
                    }
                    else
                    {
                        // check gerak biasa
                        if (!check_node.isWater)
                        {
                            // kalau bukan water
                            if (check_node.animal.player != myAnimal.player && myAnimal.strength >= check_node.animal.strength)
                            {
                                check_node.animal = temp_board[x, y].animal;
                                check_node.animal.position = new int[] { x + 1, y };
                                temp_board[x, y].animal = null;
                                boards.Add(temp_board);
                            }
                        }
                    }
                }
            }
        }

        private Dictionary<string, int[]> findPossibleMoves(int[] coordinates, SquareNode[,] board)
        {
            Dictionary<string, int[]> around = new Dictionary<string, int[]>();
            if (coordinates[1] - 1 >= 0)
            {
                around.Add("atas", new int[] { coordinates[0], coordinates[1] - 1 });
            }
            if (coordinates[1] + 1 < 9)
            {
                around.Add("bawah", new int[] { coordinates[0], coordinates[1] + 1 });
            }
            if (coordinates[0] - 1 >= 0)
            {
                around.Add("kiri", new int[] { coordinates[0] - 1, coordinates[1] });
            }
            if (coordinates[0] + 1 < 7)
            {
                around.Add("kanan", new int[] { coordinates[0] + 1, coordinates[1] });
            }
            return around;
        }
    }

    class Player
    {
        public string name;
        public int giliran;

        public Player(string name, int giliran)
        {
            this.name = name;
            this.giliran = giliran;
        }
    }

    class Move
    {
        public SquareNode[,] currentBoard;
        public Move nextMove;
        public Move(SquareNode[,] board)
        {
            this.currentBoard = board;
        }
    }

}

