using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using pGina.Shared.Interfaces;
using pGina.Shared.Types;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Serialization;
using System.DirectoryServices;

namespace PluginJUmpCloudPgina
{
    public class JUmpCloudUserAuthentication : pGina.Shared.Interfaces.IPluginAuthentication
    {
        private string _directory = "5c01746b29f03755c3cfa75e";
        public string Name
        {
            get
            {
                return "User";
            }
        }

        public string Description
        {
            get
            {
                return "Log user test \"User User\"";
            }
        }

        public string Version
        {
            get
            {
                return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        //public Guid Uuid => throw new NotImplementedException();

        public Guid Uuid => new Guid("CED8D126-9121-4CD2-86DE-3D84E4A2625E");

        public BooleanResult AuthenticateUser(SessionProperties properties)
        {
            throw new NotImplementedException();
        }

        public void Starting()
        {
            throw new NotImplementedException();
        }

        public void Stopping()
        {
            throw new NotImplementedException();
        }

        private DirectoryEntry createDirectoryEntry(string account,string password)  
        {   
           //On crée la connection LDAP qui va bien 
            DirectoryEntry ldapConnection = 
                new DirectoryEntry(
                    String.Format(@"LDAP://ldap.jumpcloud.com:389/ou=Users,o={0},dc=jumpcloud,dc=com",_directory));
            ldapConnection.Username = String.Format("uid={0},ou=Users,o={1},dc=jumpcloud,dc=com",account,_directory);
            ldapConnection.Password = password;
            ldapConnection.AuthenticationType = AuthenticationTypes.Secure;
            return ldapConnection;  
        }
        
        private bool SearchUserLDAP(string username,string password)
        {
            try
            {
                DirectoryEntry myLdapConnection = createDirectoryEntry(username, password);
                DirectorySearcher search = new DirectorySearcher(myLdapConnection);
                //On lance la requête
                SearchResult result = search.FindOne();
                //Ici , si on n'est pas tombé dans le catch, c'est que le user/psswd sont Ok
                if(result !=null)
                    return true;
                return false;
            }
            catch (Exception e)
            {
                //Authentification KO
                Console.Write(e.Message);
                return false;
            }

        }
    }
}
