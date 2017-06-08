using Minesweeper.Library;
using Prism.Commands;
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
using System.Windows.Shapes;

namespace Minesweeper
{
    /// <summary>
    /// Interaction logic for DifficultyModal.xaml
    /// </summary>
    public partial class DifficultyModal : Window
    {
        public DifficultyLevel SelectedDifficulty;

        public DelegateCommand<DifficultyLevel?> SetDifficultyCommand
        {
            get;
            private set;
        }

        public DifficultyModal()
        {
            InitializeComponent();
            this.SetDifficultyCommand = new DelegateCommand<DifficultyLevel?>(SetDifficulty);
        }

        public void SetDifficulty(DifficultyLevel? level)
        {
            this.SelectedDifficulty = (DifficultyLevel)level;
            this.DialogResult = true;
        }
    }
}
