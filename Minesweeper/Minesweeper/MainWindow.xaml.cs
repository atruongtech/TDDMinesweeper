using Minesweeper.Library;
using Prism.Mvvm;
using System.Windows;
using System.ComponentModel;

namespace Minesweeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            PromptForNewGame();
        }

        public void PromptForNewGame()
        {
            if (!ShowNewGameDialog())
                Application.Current.Shutdown();
        }

        public bool ShowNewGameDialog()
        {
            DifficultyModal modal = new DifficultyModal();
            modal.ShowDialog();

            if (modal.DialogResult == true)
            {
                StartNewGame(modal.SelectedDifficulty);
                return true;
            }                

            return false;
        }

        public void StartNewGame(DifficultyLevel level)
        {
            switch(level)
            {
                case DifficultyLevel.Easy:
                    this.Height = 475;
                    this.Width = 550;
                    break;
                case DifficultyLevel.Medium:
                    this.Height = 830;
                    this.Width = 1000;
                    break;
                case DifficultyLevel.Hard:
                    this.Height = 900;
                    this.Width = 1300;
                    break;
            }
            this.Hide();

            var board = new Gameboard(level, new NeighboringTileFinder());
            board.InitializeGameBoard();
            board.StartGame();
            this.DataContext = board;

            ((BindableBase)this.DataContext).PropertyChanged += GameEndDelegate;
            this.Show();
        }

       private void GameEndDelegate(object sender, PropertyChangedEventArgs e)
       {
             if (e.PropertyName == "GameOver")
                ShowGameOverScreen();
             else if (e.PropertyName == "Win")
                ShowWinScreen();
       }

       public void ShowMessageBox(string message, string caption)
        {
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Question;

            MessageBoxResult results = MessageBox.Show(message, caption, buttons, icon);
            switch (results)
            {
                case MessageBoxResult.Yes:
                    PromptForNewGame();
                    break;
                case MessageBoxResult.No:
                    Application.Current.Shutdown();
                    break;
            }
        }

        public void ShowGameOverScreen()
        {
            ShowMessageBox("Game Over. Would you like to play again?", "Game Over");
        }

        public void ShowWinScreen()
        {
            ShowMessageBox("Congratulations! You win! Would you like to play again?", "Congratulations!");
        }
        
    }
}
