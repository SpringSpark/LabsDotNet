using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml;
using TelephoneBook.Model;
using TelephoneBook.Utils;

namespace TelephoneBook.ViewModel
{
    class ContactViewModel
    {
        private ObservableCollection<Contact> _contacts;
        public ICollectionView _contactView;
        private string _filterString;
        private bool _birthdaysFilter;

        public ICollectionView Contacts
        {
            get { return _contactView; }
            set
            {
                _contactView = value;
                OnPropertyChanged("Contacts");
            }
        }

        public ContactViewModel()
        {
            _contacts = new ObservableCollection<Contact>();
            _alphabetButtons = new ObservableCollection<AlphabetButton>();
            _alphabetButtons.Add(new AlphabetButton("*", AlphabetSortCommand));
            for (int i = 0; i < 32; i++)
                _alphabetButtons.Add(new AlphabetButton(((char)('а' + i)).ToString(), AlphabetSortCommand));
            LoadContacts();
            _contactView = CollectionViewSource.GetDefaultView(_contacts);
            _contactView.Filter = ContactFilter;
            _birthdaysFilter = false;
        }

        private bool ContactFilter(object item)
        {
            Contact contact = item as Contact;
            bool stringCond, dateCond;
            DateTime birthday = new DateTime(DateTime.Now.Year, contact.Birthday.Month, contact.Birthday.Day);
            stringCond = _filterString == null || _filterString == "*" || contact.Name.First().ToString().ToLower().Equals(_filterString);
            dateCond = !_birthdaysFilter || ((birthday - DateTime.Now).TotalDays >= -1 && (birthday - DateTime.Now).TotalDays <= 7);
            return (stringCond && dateCond);
        }

        public string FilterString
        {
            get { return _filterString; }
            set
            {
                _filterString = value;
                OnPropertyChanged("FilterString");
                _contactView.Refresh();
            }
        }

        public bool BirthdaysFilter
        {
            get { return _birthdaysFilter; }
            set
            {
                _birthdaysFilter = value;
                OnPropertyChanged("BirthdaysFilter");
                _contactView.Refresh();
            }
        }


        public void LoadContacts()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("PhoneBook.xml");
            XmlElement xRoot = xDoc.DocumentElement;
            foreach (XmlNode xnode in xRoot)
            {
                Contact new_contact = new Contact();
                if (xnode.Attributes.Count > 0)
                {
                    XmlNode attr = xnode.Attributes.GetNamedItem("name");
                    if (attr != null)
                        new_contact.Name = (attr.Value);                    
                }
                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    if (childnode.Name == "worknumber")                    
                        new_contact.WorkNumber = childnode.InnerText;                    
                    if (childnode.Name == "homenumber")                    
                        new_contact.HomeNumber = childnode.InnerText;
                    if (childnode.Name == "email")
                        new_contact.Email = childnode.InnerText;
                    if (childnode.Name == "skype")
                        new_contact.Skype = childnode.InnerText;
                    if (childnode.Name == "birthday")
                        new_contact.Birthday = DateTime.ParseExact(childnode.InnerText, "yyyy-MM-dd HH:mm:ss,fff",
                                       System.Globalization.CultureInfo.InvariantCulture);
                    if (childnode.Name == "commentary")
                        new_contact.Commentary = childnode.InnerText;
                }
                _contacts.Add(new_contact);
            }
        }


        private Contact _selectedContact;
        public Contact SelectedContact
        {
            get { return _selectedContact; }
            set
            {
                _selectedContact = value;
                OnPropertyChanged("SelectedContact");
                RemoveContactCommand.RaiseCanExecuteChanged();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyChanged) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyChanged));

        public class AlphabetButton
        {
            public string Content { get; set; }

            public ICommand Command { get; set; }

            public AlphabetButton(string content, ICommand command)
            {
                Content = content;
                Command = command;
            }
        }
        private readonly ObservableCollection<AlphabetButton> _alphabetButtons = new ObservableCollection<AlphabetButton>();
        public ObservableCollection<AlphabetButton> AlphabetButtons { get { return _alphabetButtons; } }

        private DelegateCommand _alphabetSortCommand;
        public DelegateCommand AlphabetSortCommand => _alphabetSortCommand ?? (_alphabetSortCommand = new DelegateCommand(AlphabetSort));
        private void AlphabetSort(object arg)
        {
            FilterString = arg.ToString();
        }

        private DelegateCommand _showBirthdaysCommand;
        public DelegateCommand ShowBirthdaysCommand => _showBirthdaysCommand ?? (_showBirthdaysCommand = new DelegateCommand(ShowBirthdays));
        private void ShowBirthdays(object arg) => BirthdaysFilter = (BirthdaysFilter == true ? false : true);

        private DelegateCommand _addContactCommand;
        public DelegateCommand AddContactCommand => _addContactCommand ?? (_addContactCommand = new DelegateCommand(AddNewContact));
        private void AddNewContact(object arg)
        {
            _contacts.Add(new Contact() { Name = "Новый контакт", Birthday = DateTime.Now, Commentary = "", Email = "", Skype = "", WorkNumber = "", HomeNumber = "" });
        }


        private DelegateCommand _removeContactCommand;
        public DelegateCommand RemoveContactCommand => _removeContactCommand ?? (_removeContactCommand = new DelegateCommand(RemoveContact, CanRemoveContact));
        private void RemoveContact(object args)
        {
            _contacts.Remove(SelectedContact);
        }


        private bool CanRemoveContact(object args)
        {
            if (SelectedContact == null)
                return false;
            var contact = FindContact(SelectedContact);
            if (contact == null)
                return false;
            return true;
        }

        private Contact FindContact(Contact findContact)
        {
            if (findContact == null)
                return null;

            return _contacts.FirstOrDefault(contact => contact.Name == findContact.Name
                                       && contact.HomeNumber == findContact.HomeNumber
                                       && contact.Email == findContact.Email
                                       && contact.Birthday == findContact.Birthday
                                       && contact.Commentary == findContact.Commentary
                                       && contact.WorkNumber == findContact.WorkNumber);
        }

        private DelegateCommand _saveContactsCommand;
        public DelegateCommand SaveContactsCommand => _saveContactsCommand ?? (_saveContactsCommand = new DelegateCommand(SaveContacts));

        private void SaveContacts(object args)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("PhoneBook.xml");
            XmlElement xRoot = xDoc.DocumentElement;

            xRoot.RemoveAll();

            /*foreach (XmlNode xnode in xRoot)
            {
                xRoot.RemoveChild(xnode);
            }*/

            foreach (Contact contact in _contacts)
            {
                XmlElement contactElem = xDoc.CreateElement("contact");

                XmlAttribute nameAttr = xDoc.CreateAttribute("name");

                XmlElement worknumberElem = xDoc.CreateElement("worknumber");
                XmlElement homenumberElem = xDoc.CreateElement("homenumber");
                XmlElement emailElem = xDoc.CreateElement("email");
                XmlElement skypeElem = xDoc.CreateElement("skype");
                XmlElement birthdayElem = xDoc.CreateElement("birthday");
                XmlElement commentaryElem = xDoc.CreateElement("commentary");

                XmlText nameText = xDoc.CreateTextNode(contact.Name);
                XmlText worknumberText = xDoc.CreateTextNode(contact.WorkNumber);
                XmlText homenumberText = xDoc.CreateTextNode(contact.HomeNumber);
                XmlText emailText = xDoc.CreateTextNode(contact.Email);
                XmlText skypeText = xDoc.CreateTextNode(contact.Skype);
                XmlText birthdayText = xDoc.CreateTextNode(contact.Birthday.ToString("yyyy-MM-dd HH:mm:ss,fff"));
                XmlText commentaryText = xDoc.CreateTextNode(contact.Commentary);

                //добавляем узлы
                nameAttr.AppendChild(nameText);
                worknumberElem.AppendChild(worknumberText);
                homenumberElem.AppendChild(homenumberText);
                emailElem.AppendChild(emailText);
                skypeElem.AppendChild(skypeText);
                birthdayElem.AppendChild(birthdayText);
                commentaryElem.AppendChild(commentaryText);

                contactElem.Attributes.Append(nameAttr);
                contactElem.AppendChild(worknumberElem);
                contactElem.AppendChild(homenumberElem);
                contactElem.AppendChild(emailElem);
                contactElem.AppendChild(skypeElem);
                contactElem.AppendChild(birthdayElem);
                contactElem.AppendChild(commentaryElem);

                xRoot.AppendChild(contactElem);
            }
            xDoc.Save("PhoneBook.xml");
        }
    }
}

