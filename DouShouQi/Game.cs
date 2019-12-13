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
                Close();
            }
            else if(den2.animal != null)
            {
                MessageBox.Show("Computer Menang");
                Close();
            }
        }

        public void changeGiliran()
        {
            giliran = giliran == 1 ? 2 : 1;
            updateUI();
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
}

