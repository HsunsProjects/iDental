using iDental.Class;

namespace iDental.ViewModels
{
    public class AnswerDialogOneViewModel : ViewModelBase.PropertyChangedBase
    {
        private string Mode { get; set; }

        private string answer;
        public string Answer
        {
            get { return answer; }
            set
            {
                answer = value;
                OnPropertyChanged("Answer");
                switch (Mode)
                {
                    case "IP":
                        if (ValidatorHelper.IsIP(answer))
                        {
                            IsValid = true;
                        }
                        else
                        {
                            IsValid = false;
                        }
                        break;
                    case "Verify":
                        IsValid = true;
                        break;
                }
            }
        }

        private bool isValid = false;
        public bool IsValid
        {
            get { return isValid; }
            set
            {
                isValid = value;
                OnPropertyChanged("IsValid");
            }
        }

        public AnswerDialogOneViewModel(string mode)
        {
            Mode = mode;
        }
    }
}
