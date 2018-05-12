using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;

namespace KiniApp.ViewModel
{
    
    public class LoginViewModel : ViewModelBase
    {

        private string usuarioLogin;
        private string passwordLogin;
        private string passwordReg;
        private string passwordReg2;
        private string email;
        private bool recibirNotf;
        private string equipoFav;
        private bool inicioLogin;

        private int indexFlip;
        private RelayCommand registroCommand;
        public ICommand RegistroCommand
        {
            get
            {
                return registroCommand ?? (registroCommand = new RelayCommand(()=> { indexFlip = 1; }));
            }
        }
        public int IndexFlip
        {
            get { return indexFlip; }
            set
            {
                indexFlip = value;
                RaisePropertyChanged();
            }
        }
        public string UsuarioLogin
        {
            get { return usuarioLogin; }
            set
            {
                usuarioLogin = value;
                RaisePropertyChanged();
            }
        }
        public string PasswordLogin
        {
            get { return passwordLogin; }
            set
            {
                passwordLogin = value;
                RaisePropertyChanged();
            }
        }
        public LoginViewModel()
        {
            General.GetListaEquipos();
        }

        
    }
}