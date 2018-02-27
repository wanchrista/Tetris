/* Chris Wan
 * Tetris
 * Grade 11 Culminating
 * June 2 2014*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Grade_11_Culminating
{
    public partial class Tetris : Form
    {        
        //Store the number of rows in the grid
        const int NUMBER_OF_ROWS = 20;
        //Store the number of columns in the grid
        const int NUMBER_OF_COLUMNS = 10;

        //Store the starting position of the block
        int currentRow = 3;
        int currentColumn = 4;
        //This variable is used to store the piece the user is using
        int currentPiece;
        //Set constants for each of the blocks
        const int VERTICAL_PIECE = 0;
        const int HORIZONTAL_PIECE = 1;

        //Make constants to store tetris block colors
        const int BLOCK_COLOR_0 = 0;
        const int BLOCK_COLOR_1 = 1;
        const int BLOCK_COLOR_2 = 2;
        const int BLOCK_COLOR_3 = 3;
        const int BLOCK_COLOR_4 = 4;

        //Random number generator to generate a random number  
        Random blockColorGenerator = new Random();
        //Create a variable to store the random number
        int colorNumber;

        
        //Constant to store the aniamtion frame rate and frame time
        //25 frames per second
        const int FRAME_RATE = 60;
        //Lemgth of time needed for each frame
        const int FRAME_TIME = 1000 / FRAME_RATE;

        //Keep track of the time that has passed
        //Store the current time
        int currentTime;
        //Store the previous time
        int previousTime;

        //Store the game over text's location
        int text1X = 0;
        int text1Y = 150;
        //Store the font type of the text
        Font textFont;
        
        //Create a 2d array to store the grid points of the rectangles
        Point[,] gridPoints = new Point[NUMBER_OF_COLUMNS,NUMBER_OF_ROWS];
        //Create a 2d array to store the rectangles being created 
        Rectangle[,] rectangles = new Rectangle[NUMBER_OF_COLUMNS, NUMBER_OF_ROWS];
        //Store the rectangle size
        Size rectangleSize = new Size(20, 20);
        //Create a 2d array to store the true/ false data for filled squares
        bool[,] rectanglePosition = new bool[NUMBER_OF_COLUMNS, NUMBER_OF_ROWS];
                

        //This variable determines the speed of the block as it moves down
        int moveDownCounter = 0;
        //Store the users score which starts off at zero
        int score = 0;

        //This determines whether or not the user has lost
        bool gameOver = false;
        //Create a bool to see whether or not the program is running
        bool keepRunning = false;

        public Tetris()
        {
            InitializeComponent();

            //Tell the user what to do if the enter button has not been pressed
            lblPrompt.Text = "Press enter to begin.";

            //Tell the user their score
            lblScore.Text = "Score: " + score;

            //Print out instructions
            lblInfo.Text = "TETRIS \n\nBasic rules: \n\nUse left and right \nkeys to move the \nblock. \nUse up key to \nrotate. \nClear lines by filling up \none row. \n+100 points per line \ncleared. \nDo not spam keys.";
            
            //Print out the credits
            lblCredits.Text = "Programmed by: \nChris Wan";

            //Set the current piece to it's default 
            currentPiece = VERTICAL_PIECE;
            //Store the font and font sizes for the game over text
            textFont = new Font("Comic Sans Serif", 27.0f);

            //Call the create grid subprogram to draw the grid on the screen
            CreateGrid();
            //Refresh the screen
            Refresh();

        }

        //Create a subprogram that store the variable that fills the squares
        void MoveBlock()
        {
            currentRow++;

            if (currentPiece == VERTICAL_PIECE)
            {
                if (currentRow == 19)
                {
                    rectanglePosition[currentColumn, currentRow] = true;
                    rectanglePosition[currentColumn, currentRow - 1] = true;
                    rectanglePosition[currentColumn, currentRow - 2] = true;
                    rectanglePosition[currentColumn, currentRow - 3] = true;

                    //Create a new block after the previous one has been "locked" into place
                    CreateNewPiece();
                    return;                         
                    
                }
                if (currentRow + 1 == 19 && rectanglePosition[currentColumn, 19] == false)
                {                    
                    rectanglePosition[currentColumn, currentRow + 1] = true;
                    rectanglePosition[currentColumn, currentRow] = true;
                    rectanglePosition[currentColumn, currentRow - 1] = true;
                    rectanglePosition[currentColumn, currentRow - 2] = true;

                    //Create a new block after the previous one has been "locked" into place
                    CreateNewPiece();
                    return;                     
                }
                else if (rectanglePosition[currentColumn, Math.Min(currentRow + 1, 19)] == true)
                {
                    rectanglePosition[currentColumn, currentRow] = true;
                    rectanglePosition[currentColumn, currentRow - 1] = true;
                    rectanglePosition[currentColumn, currentRow - 2] = true;
                    rectanglePosition[currentColumn, currentRow - 3] = true;

                    //Create a new block after the previous one has been "locked" into place
                    CreateNewPiece();
                    return;                                                
                }

            }
            else if (currentPiece == HORIZONTAL_PIECE)
            {
                int nextRow = currentRow + 1;
                bool firstSquareAfterMovingDown = rectanglePosition[currentColumn, Math.Min(nextRow, 19)];
                bool secondSquareAfterMovingDown = rectanglePosition[Math.Min(currentColumn + 1, 7), Math.Min(nextRow, 19)];
                bool thirdSquareAfterMovingDown = rectanglePosition[Math.Min(currentColumn + 2, 8), Math.Min(nextRow, 19)];
                bool fourthSquareAfterMovingDown = rectanglePosition[Math.Min(currentColumn + 3, 9), Math.Min(nextRow, 19)];

                if (currentRow == 19)
                {   
                    rectanglePosition[currentColumn, currentRow] = true;
                    rectanglePosition[Math.Min(currentColumn + 1, 7), currentRow] = true;
                    rectanglePosition[Math.Min(currentColumn + 2, 8), currentRow] = true;
                    rectanglePosition[Math.Min(currentColumn + 3, 9), currentRow] = true;
                    
                    //Create a new block after the previous one has been "locked" into place
                    CreateNewPiece();                        
                    return;                    
                }                
                else if (firstSquareAfterMovingDown == true || secondSquareAfterMovingDown == true || thirdSquareAfterMovingDown == true || fourthSquareAfterMovingDown == true)
                {
                    rectanglePosition[currentColumn, currentRow] = true;
                    rectanglePosition[Math.Min(currentColumn + 1, 7), currentRow] = true;
                    rectanglePosition[Math.Min(currentColumn + 2, 8), currentRow] = true;
                    rectanglePosition[Math.Min(currentColumn + 3, 9), currentRow] = true;

                    //Create a new block after the previous one has been "locked" into place
                    CreateNewPiece();
                    return;                     
                }
            }

        }

        //Create a customer timer subprogram that will control how the game runs
        void CustomTimer()
        {          
            //Loops until the escape key has been pressed            
            do
            {
                //Get current time 
                currentTime = Environment.TickCount;

                //Calculate time passed
                int timePassed = currentTime - previousTime;
                
                //only update the program when enough time has passed
                if (timePassed >= FRAME_TIME)
                {
                    //Update time
                    previousTime = currentTime;
                    //Update the move down counter
                    moveDownCounter++;

                    //Set the speed for the block
                    if (moveDownCounter == 5)
                    {
                        //Reset the counter
                        moveDownCounter = 0;

                        //Only allow the user to move the piece if the game over boolean is false
                        if (gameOver == false)
                        {
                            MoveBlock();                            
                        }                        
                    }
                    Refresh();
                }                
                //Prevent the program from freezing
                Application.DoEvents();
            } while (keepRunning == true);
        }

        //Create a subprogram that generates new blocks and checks if it is game over
        void CreateNewPiece()
        {
            ClearLine();
            //Check to see if a new piece can be drawn 
            if (rectanglePosition[4, 0] == false)
            {
                //Generate a random number for a random color
                colorNumber = blockColorGenerator.Next(0, 5);
                //Reset all the pieces to their default position
                currentRow = 3;
                currentColumn = 4;
                currentPiece = VERTICAL_PIECE;
            }
            
            //If a new block cannot be drawn, it is game over
            if (rectanglePosition[4, 4] == true)
            {
                gameOver = true;
                //Tell the user to press the escape key
                lblPrompt.Text = "Please press escape.";
            }
        }

        //Create a subprogram that will generate a grid
        void CreateGrid()
        {
            for (int i = 0; i < NUMBER_OF_ROWS; i++)
            {
                for (int j = 0; j < NUMBER_OF_COLUMNS; j++)
                {
                    //Generate the points in which the rectangles will be located
                    gridPoints[j, i] = new Point(10 + 20 * j, 15 + 20 * i);
                    //Generate the position and size of rectangles
                    rectangles[j, i] = new Rectangle(gridPoints[j, i], rectangleSize);
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            for (int i = 0; i < NUMBER_OF_ROWS; i++)
            {
                for (int j = 0; j < NUMBER_OF_COLUMNS; j++)
                {
                    //Draw the rectangles on the screen to make a grid
                    e.Graphics.DrawRectangle(Pens.Black, rectangles[j, i]);

                    //Check which rectangles are true
                    if (rectanglePosition[j, i] == true)
                    {                       
                        //Fill in the squares that are "true" to indicate they are "filled"
                        e.Graphics.FillRectangle(Brushes.Gainsboro, rectangles[j, i]);                       
                    }
                }
            }
            
            //Check which piece needs to be drawn and which color it is going to be drawn in 
            if (currentPiece == VERTICAL_PIECE)
            {   
                if (colorNumber == BLOCK_COLOR_0)
                {
                    //Draw the long tetris block
                    e.Graphics.FillRectangle(Brushes.Turquoise, rectangles[currentColumn, currentRow]);
                    e.Graphics.FillRectangle(Brushes.Turquoise, rectangles[currentColumn, currentRow - 1]);
                    e.Graphics.FillRectangle(Brushes.Turquoise, rectangles[currentColumn, currentRow - 2]);
                    e.Graphics.FillRectangle(Brushes.Turquoise, rectangles[currentColumn, currentRow - 3]);
                }
                else if (colorNumber == BLOCK_COLOR_1)
                {
                    //Draw the long tetris block
                    e.Graphics.FillRectangle(Brushes.DeepPink, rectangles[currentColumn, currentRow]);
                    e.Graphics.FillRectangle(Brushes.DeepPink, rectangles[currentColumn, currentRow - 1]);
                    e.Graphics.FillRectangle(Brushes.DeepPink, rectangles[currentColumn, currentRow - 2]);
                    e.Graphics.FillRectangle(Brushes.DeepPink, rectangles[currentColumn, currentRow - 3]);
                }
                else if (colorNumber == BLOCK_COLOR_2)
                {
                    //Draw the long tetris block
                    e.Graphics.FillRectangle(Brushes.DarkViolet, rectangles[currentColumn, currentRow]);
                    e.Graphics.FillRectangle(Brushes.DarkViolet, rectangles[currentColumn, currentRow - 1]);
                    e.Graphics.FillRectangle(Brushes.DarkViolet, rectangles[currentColumn, currentRow - 2]);
                    e.Graphics.FillRectangle(Brushes.DarkViolet, rectangles[currentColumn, currentRow - 3]);
                }
                else if (colorNumber == BLOCK_COLOR_3)
                {
                    //Draw the long tetris block
                    e.Graphics.FillRectangle(Brushes.LawnGreen, rectangles[currentColumn, currentRow]);
                    e.Graphics.FillRectangle(Brushes.LawnGreen, rectangles[currentColumn, currentRow - 1]);
                    e.Graphics.FillRectangle(Brushes.LawnGreen, rectangles[currentColumn, currentRow - 2]);
                    e.Graphics.FillRectangle(Brushes.LawnGreen, rectangles[currentColumn, currentRow - 3]);
                }
                else if (colorNumber == BLOCK_COLOR_4)
                {
                    //Draw the long tetris block
                    e.Graphics.FillRectangle(Brushes.LightSkyBlue, rectangles[currentColumn, currentRow]);
                    e.Graphics.FillRectangle(Brushes.LightSkyBlue, rectangles[currentColumn, currentRow - 1]);
                    e.Graphics.FillRectangle(Brushes.LightSkyBlue, rectangles[currentColumn, currentRow - 2]);
                    e.Graphics.FillRectangle(Brushes.LightSkyBlue, rectangles[currentColumn, currentRow - 3]);
                }
            }
            else if (currentPiece == HORIZONTAL_PIECE)
            {
                if (colorNumber == BLOCK_COLOR_0)
                {
                    //Draw the long tetris block
                    e.Graphics.FillRectangle(Brushes.Turquoise, rectangles[currentColumn, currentRow]);
                    e.Graphics.FillRectangle(Brushes.Turquoise, rectangles[currentColumn + 1, currentRow]);
                    e.Graphics.FillRectangle(Brushes.Turquoise, rectangles[currentColumn + 2, currentRow]);
                    e.Graphics.FillRectangle(Brushes.Turquoise, rectangles[currentColumn + 3, currentRow]);
                }
                else if (colorNumber == BLOCK_COLOR_1)
                {
                    //Draw the long tetris block
                    e.Graphics.FillRectangle(Brushes.DeepPink, rectangles[currentColumn, currentRow]);
                    e.Graphics.FillRectangle(Brushes.DeepPink, rectangles[currentColumn + 1, currentRow]);
                    e.Graphics.FillRectangle(Brushes.DeepPink, rectangles[currentColumn + 2, currentRow]);
                    e.Graphics.FillRectangle(Brushes.DeepPink, rectangles[currentColumn + 3, currentRow]);
                }
                else if (colorNumber == BLOCK_COLOR_2)
                {
                    //Draw the long tetris block
                    e.Graphics.FillRectangle(Brushes.DarkViolet, rectangles[currentColumn, currentRow]);
                    e.Graphics.FillRectangle(Brushes.DarkViolet, rectangles[currentColumn + 1, currentRow]);
                    e.Graphics.FillRectangle(Brushes.DarkViolet, rectangles[currentColumn + 2, currentRow]);
                    e.Graphics.FillRectangle(Brushes.DarkViolet, rectangles[currentColumn + 3, currentRow]);
                }
                else if (colorNumber == BLOCK_COLOR_3)
                {
                    //Draw the long tetris block
                    e.Graphics.FillRectangle(Brushes.LawnGreen, rectangles[currentColumn, currentRow]);
                    e.Graphics.FillRectangle(Brushes.LawnGreen, rectangles[currentColumn + 1, currentRow]);
                    e.Graphics.FillRectangle(Brushes.LawnGreen, rectangles[currentColumn + 2, currentRow]);
                    e.Graphics.FillRectangle(Brushes.LawnGreen, rectangles[currentColumn + 3, currentRow]);
                }
                else if (colorNumber == BLOCK_COLOR_4)
                {
                    //Draw the long tetris block
                    e.Graphics.FillRectangle(Brushes.LightSkyBlue, rectangles[currentColumn, currentRow]);
                    e.Graphics.FillRectangle(Brushes.LightSkyBlue, rectangles[currentColumn + 1, currentRow]);
                    e.Graphics.FillRectangle(Brushes.LightSkyBlue, rectangles[currentColumn + 2, currentRow]);
                    e.Graphics.FillRectangle(Brushes.LightSkyBlue, rectangles[currentColumn + 3, currentRow]);
                }
                
            }

            //Print out "Game Over" if the user has lost the game
            if (gameOver == true)
            {
                e.Graphics.DrawString("GAME OVER", textFont, Brushes.Red, text1X, text1Y);
            }

        }

        //make a subprogram that rotates the blocks
        void RotatePiece()
        {
            //Ensure that the game is still running and that there is enough space of the block to rotate
            if (gameOver == false && currentPiece == VERTICAL_PIECE && currentColumn < 7 && rectanglePosition[currentColumn + 1, currentRow] == false && rectanglePosition[currentColumn + 2, currentRow] == false && rectanglePosition[currentColumn + 3, currentRow] == false)
            {
                currentPiece = HORIZONTAL_PIECE;
            }
            //Ensure that the game is still running and that there is enough space of the block to rotate
            else if (gameOver == false && currentPiece == HORIZONTAL_PIECE && currentColumn != 9 && rectanglePosition[currentColumn, currentRow - 1] == false && rectanglePosition[currentColumn, currentRow - 2] == false && rectanglePosition[currentColumn, currentRow - 3] == false)
            {
                currentPiece = VERTICAL_PIECE;
            }
        }

        void ClearLine()
        {
            //Make counter variables to store the rows filled
            int count = 0;

            for (int b = 0; b < NUMBER_OF_ROWS; b++)
            {
                //Restart the counter variables 
                count = 0;

                for (int y = 0; y < NUMBER_OF_COLUMNS; y++)
                {
                    if (rectanglePosition[y, b] == true)
                    {
                        count = count + 1;

                        //Add one hundred points for each line cleared.
                        if (count == 10)
                        {
                            score = score + 100;
                            lblScore.Text = "Score: " + score;
                        }
                    }
                }
                //If an entire row is filled, clear it and move it down
                if (count == NUMBER_OF_COLUMNS)
                {
                    for (int x = 0; x < NUMBER_OF_COLUMNS; x++)
                    {
                        for (int p = b; p >= 1; p--)
                        {
                            //Move all the blocks down 
                            rectanglePosition[x, p] = rectanglePosition[x, p - 1];
                        }
                    }
                }
            }            
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //Program the escape key to close the program when pressed
            if (e.KeyCode == Keys.Escape)
            {
                keepRunning = false;
                Application.Exit();
            }
            //Game starts once enter is pressed
            else if (e.KeyCode == Keys.Enter)
            {
                //Change the prompt to tell the user how to exit the game
                lblPrompt.Text = "Press escape to exit.";
                //Keep running is set to true to start the custom timer
                keepRunning = true;
                //The custom timer is called
                CustomTimer();
            }
            //Rotate piece subprogram is called when the up key is pressed 
            else if (e.KeyCode == Keys.Up)
            {
                RotatePiece();
            }
            //If the user presses the left key
            else if (e.KeyCode == Keys.Left)
            {
                //Make sure that game over is equal to false and that the piece will not go out of bounds
                if (gameOver == false && currentColumn > 0 && rectanglePosition[Math.Max(currentColumn - 1, 0), currentRow] == false)
                {
                    currentColumn = currentColumn - 1;
                }
            }
            //If the user presses the right key
            else if (e.KeyCode == Keys.Right)
            {
                //Make sure that game over is equal to false and that the piece will not go out of bounds
                if (gameOver == false && currentPiece == HORIZONTAL_PIECE && currentColumn < 6 && rectanglePosition[Math.Min(currentColumn + 1, 5), currentRow] == false)
                {
                    currentColumn = currentColumn + 1;
                }
                //Make sure that game over is equal to false and that the piece will not go out of bounds
                else if (gameOver == false && currentPiece == VERTICAL_PIECE && currentColumn < 9 && rectanglePosition[Math.Min(currentColumn + 1, 8), currentRow] == false)
                {
                    currentColumn = currentColumn + 1;
                }
            }

        }

    }
}
