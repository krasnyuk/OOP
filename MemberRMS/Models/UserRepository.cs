using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MemberRMS.Models
{
    //public class UserRepository : Controller
    interface IUserRepository
    {
        IEnumerable<UserViewModel> GetAll();
        UserViewModel Get(int id);
        UserViewModel Add(UserViewModel item);
        void Remove(int id);
        bool Update(UserViewModel item);
    }
    public class UserRepository : IUserRepository
    {
        private readonly List<UserViewModel> _users;
        private int _nextId = 1;

        public UserRepository()
        {
            using (var db = new RecipeManagmentSystemEntities())
            {
                var result = (from p in db.User.OrderBy(x => x.FirstName)
                              select new UserViewModel
                              {
                                  UserID = p.UserID,
                                  FirstName = p.FirstName,
                                  LastName = p.LastName,
                              
                                  Birthday = p.Birthday,
                                  Telephone = p.Telephone,
                                  Mail = p.Mail

                              }).ToList();
                _users = result;
            }
        }


        public IEnumerable<UserViewModel> GetAll()
        {

            return _users;
        }

        public UserViewModel Get(int id)
        {
            return _users.Find(p => p.UserID == id);
        }

        public UserViewModel Add(UserViewModel item)
        {
            item.UserID = _nextId++;
            _users.Add(item);
            return item;
        }

        public void Remove(int id)
        {
            _users.RemoveAll(p => p.UserID == id);
        }

        public bool Update(UserViewModel item)
        {
            int index = _users.FindIndex(p => p.UserID == item.UserID);
            if (index == -1)
            {
                return false;
            }
            _users.RemoveAt(index);
            _users.Add(item);
            return true;
        }
    }
}