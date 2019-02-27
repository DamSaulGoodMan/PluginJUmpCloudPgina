using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using pGina.Shared.Interfaces;
using pGina.Shared.Types;
using System.Reflection;
using System.Runtime.Serialization;
using System.DirectoryServices;
using System.Management;

namespace PluginJUmpCloudPgina
{
    public class JUmpCloudUserAuthentication : pGina.Shared.Interfaces.IPluginAuthentication,pGina.Shared.Interfaces.IPluginAuthenticationGateway
    {
        #region Members
        
        
        public Guid Uuid => new Guid("711B34DE-8404-4C9E-AB1B-56EA62A8FEF0");
        public BooleanResult AuthenticatedUserGateway(SessionProperties properties)
        {
            try
            {
                UserInformation userInfo = properties.GetTrackedSingle<UserInformation>();
                SelectQuery query = new SelectQuery("Win32_UserAccount");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
                foreach (ManagementObject envVar in searcher.Get())
                {
                    if (envVar["Name"] == userInfo.Username)
                    {
                        return new BooleanResult() {Success = true};
                    }
                }
                DirectoryEntry AD = new DirectoryEntry("WinNT://" +  
                                                       Environment.MachineName + ",computer");  
                DirectoryEntry NewUser = AD.Children.Add(userInfo.Username, "user");  
                NewUser.Invoke("SetPassword", new object[] { userInfo.Password });  
                NewUser.Invoke("Put", new object[] { "Description", "User created by JumpCloud plugin" });  
                NewUser.CommitChanges();  
                return new BooleanResult(){Success = true};
            }
            catch(Exception e)
            {
                return  new BooleanResult(){Success = false,Message =  e.Message};
            }
        }

        public string Name
        {
            get
            {
                return "JumpCloud connection";
            }
        }

        public string Description
        {
            get
            {
                return "Log in with JumpCloud LDAP";
            }
        }

        public string Version
        {
            get
            {
                return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        #endregion
        
        #region Methods
        
        public BooleanResult AuthenticateUser(SessionProperties properties)
        {
            UserInformation userInfo = properties.GetTrackedSingle<UserInformation>();
            string result = SearchUserLDAP(userInfo.Username, userInfo.Password);
            if (result == "Ok")
                return new BooleanResult() { Success = true ,Message = "ouai" };
            else
                return new BooleanResult() { Success = false };
        }

        public void Starting()
        {}

        public void Stopping()
        {}

        private DirectoryEntry createDirectoryEntry(string account,string password)  
        {   
           //On crée la connection LDAP qui va bien 
           string directory = "5c01746b29f03755c3cfa75e";
            DirectoryEntry ldapConnection =
     new DirectoryEntry(
         String.Format("LDAP://" +
                       "ldap.jumpcloud.com:389/ou=Users,o={0},dc=jumpcloud,dc=com", directory));
            ldapConnection.Username = String.Format("uid={0},ou=Users,o={1},dc=jumpcloud,dc=com",account,directory);
            ldapConnection.Password = password;
            ldapConnection.AuthenticationType = AuthenticationTypes.ReadonlyServer;
            return ldapConnection;  
        }
        
        private string SearchUserLDAP(string username,string password)
        {
            DirectoryEntry myLdapConnection = createDirectoryEntry(username, password);
            try
            {
                
                DirectorySearcher search = new DirectorySearcher(myLdapConnection);
                //On lance la requête
                SearchResult result = search.FindOne();
                //Ici , si on n'est pas tombé dans le catch, c'est que le user/psswd sont Ok
                if (result != null)
                    return "Ok";
                Console.Write("kokokokoko");
                return "Ko";
            }
            catch (Exception e)
            {
                //Authentification KO
                Console.Write(e.Message);
                return e.Message + "Username = "+username + " Password = " + password + " Connection = "+myLdapConnection.Path+" User = " +myLdapConnection.Username;
            }

        }
        
        #endregion
        
    }
}
