using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace PrinterMonitorLibrary
{
    public class MailHelper : ConfigurationSection
    {
        ConfigurationProperty _mailElement;
        ConfigurationProperty _smtpClientElement;

        public MailHelper()
        {
            _mailElement = new ConfigurationProperty("mail", typeof(MailElement), null);
            _smtpClientElement = new ConfigurationProperty("smtpClient", typeof(SmtpClientElement), null);

            this.Properties.Add(_mailElement);
            this.Properties.Add(_smtpClientElement);
        }

        public MailElement Mail
        {
            get
            {
                return this[_mailElement] as MailElement;
            }
        }

        public SmtpClientElement Smtp
        {
            get
            {
                return this[_smtpClientElement] as SmtpClientElement;
            }
        }
    }

    public class MailElement : ConfigurationElement
    {
        ConfigurationProperty _fromEmailAddress;
        ConfigurationProperty _username;
        ConfigurationProperty _password;

        public MailElement()
        {
            _fromEmailAddress = new ConfigurationProperty("fromEmailAddress", typeof(string), "");
            _username = new ConfigurationProperty("username", typeof(string), "");
            _password = new ConfigurationProperty("password", typeof(string), "");

            this.Properties.Add(_fromEmailAddress);
            this.Properties.Add(_username);
            this.Properties.Add(_password);
        }

        public string FromEmailAddress
        {
            get
            {
                return (String)this[_fromEmailAddress];
            }
            set
            {
                this[_fromEmailAddress] = value;
            }
        }

        public string Username
        {
            get
            {
                return (String)this[_username];
            }
            set
            {
                this[_username] = value;
            }
        }

        public string Password
        {
            get
            {
                return (String)this[_password];
            }
            set
            {
                this[_password] = value;
            }
        }
    }

    public class SmtpClientElement : ConfigurationElement
    {
        ConfigurationProperty _host;
        ConfigurationProperty _port;
        ConfigurationProperty _useDefaultCredentials;
        ConfigurationProperty _enableSsl;

        public SmtpClientElement()
        {
            _host = new ConfigurationProperty("host", typeof(string), "");
            _port = new ConfigurationProperty("port", typeof(string), "");
            _useDefaultCredentials = new ConfigurationProperty("useDefaultCredentials", typeof(string), "");
            _enableSsl = new ConfigurationProperty("enableSsl", typeof(string), "");

            this.Properties.Add(_host);
            this.Properties.Add(_port);
            this.Properties.Add(_useDefaultCredentials);
            this.Properties.Add(_enableSsl);
        }

        public string Host
        {
            get
            {
                return (String)this[_host];
            }
            set
            {
                this[_host] = value;
            }
        }

        public string Port
        {
            get
            {
                return (String)this[_port];
            }
            set
            {
                this[_port] = value;
            }
        }

        public string UseDefaultCredentials
        {
            get
            {
                return (String)this[_useDefaultCredentials];
            }
            set
            {
                this[_useDefaultCredentials] = value;
            }
        }

        public string EnableSsl
        {
            get
            {
                return (String)this[_enableSsl];
            }

            set
            {
                this[_enableSsl] = value;
            }
        }
    }
}
