using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pGina.Shared.Interfaces;
using pGina.Shared.Types;

namespace PluginJUmpCloudPgina
{
    public class JUmpCloudUserAuthentication : pGina.Shared.Interfaces.IPluginAuthentication
    {
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

        public Guid Uuid = new Guid("CED8D126-9121-4CD2-86DE-3D84E4A2625E");

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
    }
}
