using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml;

namespace SecuroteckWebApplication.Models
{
    public class User
    {
         //Task2
        // TODO: Create a User Class for use with Entity Framework
        // Note that you can use the [key] attribute to set your ApiKey Guid as the primary key 
        
        public User() { }
        [Key]
        public string ApiKey { get; set; }

        public string UserName { get; set; }

        public string Role { get; set; }
    }

    #region Task13?
    // TODO: You may find it useful to add code here for Logging
    #endregion

    public class UserDatabaseAccess
    {
        //Task3 
        // TODO: Make methods which allow us to read from/write to the database 
       // User user;

        public string NewUser(string UserName)
        {
            
            using (var ctx = new UserContext())
            {
                User user = new User()
                {
                    ApiKey = Guid.NewGuid().ToString(),
                    UserName = UserName
                };
                if(ctx.Users.Count() > 0)
                {
                    user.Role = "User";
                }
                else
                {
                    user.Role = "Admin";
                }
                ctx.Users.Add(user);
                ctx.SaveChanges();
                return user.ApiKey;
                  
            }
        }

        public bool CheckUserKey(string Key)
        {
            using (var ctx = new UserContext())
            {
                foreach (User user in ctx.Users)
                {
                    if (user.ApiKey.Equals(Key))
                    {
                        return true;
                    }
                    
                }
            return false;
            }
        }

        public bool CheckUserKeyAndName(string Key, string Name)
        {
            using (var ctx = new UserContext())
            {
                foreach (User user in ctx.Users)
                {
                    if (user.UserName.Equals(Name) && user.ApiKey.Equals(Key))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public User UserCheckKey(string Key)
        {
            using (var ctx = new UserContext())
            {
                foreach (User user in ctx.Users)
                {
                    if (user.ApiKey.Equals(Key))
                    {
                        return user;
                    }
                    
                }
                return null;
            }
        }

       public bool DeleteUser(string Name, string Key)
        {
            using (var ctx = new UserContext())
            {
                bool userFound = false;
                foreach (User user in ctx.Users)
                { 
                    if (user.UserName.Equals(Name) && user.ApiKey.Equals(Key))
                    {
                        ctx.Users.Remove(user);
                        userFound = true;
                        break;
                    }
                   
                }
                if (userFound)
                {
                    ctx.SaveChanges();
                    return true;
                }
                return false;
                
            }
        }

        

        public bool CheckUserName(string Name)
        {
            using (var ctx = new UserContext())
            {
                foreach(User user in ctx.Users)
                {
                    if(user.UserName.Equals(Name))
                    {
                        return true;
                    }
                }

                return false;
               
            }
        }

        public bool ChangeRole(string Name, string Role)
        {
            using (var ctx = new UserContext())
            {
                bool roleChange = false;
                foreach(User user in ctx.Users)
                {
                    if(user.UserName.Equals(Name))
                    {
                        user.Role = Role;
                        roleChange = true;
                        break;
                    }
                }
                if (roleChange)
                {
                    ctx.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public string KeyReturnUsername(string Key)
        {
            using (var ctx = new UserContext())
            {
                foreach (User user in ctx.Users)
                {
                    if (user.ApiKey.Equals(Key))
                    {
                        return user.UserName;
                    }

                }
                return null;
            }
        }
    }


}