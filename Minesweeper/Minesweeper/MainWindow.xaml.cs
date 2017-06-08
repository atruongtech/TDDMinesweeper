using Minesweeper.Library;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
                    this.Width = 425;
                    break;
                case DifficultyLevel.Medium:
                    this.Height = 830;
                    this.Width = 860;
                    break;
                case DifficultyLevel.Hard:
                    this.Height = 900;
                    this.Width = 1000;
                    break;
                default:
                    break;
            }
            this.Hide();
            this.DataContext = new Gameboard(level);
            ((BindableBase)this.DataContext).PropertyChanged +=
                new PropertyChangedEventHandler(
                    (s, e) =>
                    {
                        if (e.PropertyName == "GameOver")
                        {
                            ShowGameOverScreen();
                        }
                        else if (e.PropertyName == "Win")
                        {
                            ShowWinScreen();
                        }
                    });
            this.Show();
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
