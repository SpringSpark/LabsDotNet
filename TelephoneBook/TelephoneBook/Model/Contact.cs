using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelephoneBook.Model
{
    class Contact : INotifyPropertyChanged
    {
        public string _name;
        public string _workNumber;
        public string _homeNumber;
        public string _email;
        public string _skype;
        public DateTime _birthday;
        public string _commentary;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }
        public string WorkNumber
        {
            get { return _workNumber; }
            set
            {
                _workNumber = value;
                OnPropertyChanged("WorkNumber");
            }
        }
        public string HomeNumber
        {
            get { return _homeNumber; }
            set
            {
                _homeNumber = value;
                OnPropertyChanged("HomeNumber");
            }
        }
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged("Email");
            }
        }
        public string Skype
        {
            get { return _skype; }
            set
            {
                _skype = value;
                OnPropertyChanged("Skype");
            }
        }
        public DateTime Birthday
        {
            get { return _birthday; }
            set
            {
                _birthday = value;
                OnPropertyChanged("Birthday");
            }
        }
        public string Commentary
        {
            get { return _commentary; }
            set
            {
                _commentary = value;
                OnPropertyChanged("Commentary");
            }
        }

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
