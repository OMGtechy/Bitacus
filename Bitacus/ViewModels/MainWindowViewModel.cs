using Bitacus.AST;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Bitacus.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
            EvaluateCommand = new EvaluateCommandType(this);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private string expressionText;
        public string ExpressionText
        {
            get { return expressionText; }
            set
            {
                expressionText = value;
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs("ExpressionText"));
            }
        }

        public class EvaluateCommandType : ICommand
        {
            public EvaluateCommandType(MainWindowViewModel parent)
            {
                this.parent = parent;
            }

            // it can always be executed, this never changes
            // 67 is the "never used" warning
            #pragma warning disable 67
            public event EventHandler CanExecuteChanged;
            #pragma warning restore 67

            private MainWindowViewModel parent;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                var result = Parser.Parse(Lexer.Lex(parameter as string))?.Evaluate().Value.ToString();

                if(result == null)
                {
                    MessageBox.Show
                    (
                        "\"" + parent.ExpressionText + "\" is not a valid expression",
                        "Invalid expression",
                        MessageBoxButton.OK,
                        MessageBoxImage.Exclamation
                    );
                    
                    return;
                }

                parent.ExpressionText = result;
            }
        }

        public EvaluateCommandType EvaluateCommand { get; }
    }
}
