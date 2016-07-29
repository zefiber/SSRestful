using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Database;

namespace SSService
{
    public partial class SSService
    {
        /*
        public Dictionary<string, HashSet<int>> GetWatchList()
        {

            //Get current SecurityContext to inspect below for authorizing
            ServiceSecurityContext securityCtx;
            securityCtx = OperationContext.Current.ServiceSecurityContext;
            string userName = securityCtx.PrimaryIdentity.Name;
            //This code is a bit primitive and ideally you would call off to another method here that would
            //perform the logic and probably just return a bool value as in commented out line below:
            //if (CheckIfAuthorized(securityCtx) != true)
           // if ((securityCtx.PrimaryIdentity.IsAuthenticated != true))
           // {
             //   throw new UnauthorizedAccessException("You are  permitted to call this method. Access Denied.");
           // }

            CustomerWatchList ca = _db.GetCustomerWatchLisstByUser(userName);
            if (ca == null || ca.watch_list == null)
            {
                return null;
            }
            return ca.watch_list;
        }


        public GeneralMessage CreateWatchList(string name)
        {
            ServiceSecurityContext securityCtx;
            securityCtx = OperationContext.Current.ServiceSecurityContext;
            string userName = securityCtx.PrimaryIdentity.Name;
            GeneralMessage gm = new GeneralMessage();
            Customer cs = _db.GetCustomerByUser(userName);
            if (cs != null)
            {
                
                if (cs.watch_list == null)
                {
                    cs.watch_list = new Dictionary<string, HashSet<int>>();
                    cs.watch_list.Add(name,null);
                    _db.SaveCustomer(cs);
                    gm.success = true;
                }
                else
                {
                    if (!cs.watch_list.ContainsKey(name))
                    {
                        cs.watch_list.Add(name,null);
                        _db.SaveCustomer(cs);
                        gm.success = true;
                        return gm;
                    }
                }
            }

            gm.success = false;
            gm.error_message = string.Format("Failed to create watchlist {0}, server error!", name);
            return gm;
        }

        public GeneralMessage DeleteWatchList(string name)
        {
            ServiceSecurityContext securityCtx;
            securityCtx = OperationContext.Current.ServiceSecurityContext;
            string userName = securityCtx.PrimaryIdentity.Name;
            GeneralMessage gm = new GeneralMessage();

            Customer cs = _db.GetCustomerByUser(userName);
            if (cs != null)
            {
                if (cs.watch_list != null)
                {
                    if (cs.watch_list.Remove(name))
                    {
                        _db.SaveCustomer(cs);
                        gm.success = true;
                        return gm;
                    }
                }
            }

            gm.success = false;
            gm.error_message = string.Format("Failed to delete watchlist {0}, server error!", name);
            return gm;
        }

        public GeneralMessage CreateWatchListItem(string lname, int sid)
        {
            ServiceSecurityContext securityCtx;
            securityCtx = OperationContext.Current.ServiceSecurityContext;
            string userName = securityCtx.PrimaryIdentity.Name;
            GeneralMessage gm = new GeneralMessage();
            Customer cs = _db.GetCustomerByUser(userName);
            if (cs != null && cs.watch_list != null)
            {
                WatchList wl = cs.watch_list.FirstOrDefault(e => e.name == lname);
                if (wl != null)
                {
                    if (wl.equitylist == null)
                    {
                        wl.equitylist = new HashSet<int>();
                    }
                    if (wl.equitylist.Add(sid))
                    {
                        _db.SaveCustomer(cs);
                        gm.success = true;
                        return gm;
                    }

                }
                
            }

            gm.success = false;
            gm.error_message = string.Format("Failed to create watchlistitem, server error!");
            return gm;
        }


        public GeneralMessage DeleteWatchListItem(string lname, int sid)
        {
            ServiceSecurityContext securityCtx;
            securityCtx = OperationContext.Current.ServiceSecurityContext;
            string userName = securityCtx.PrimaryIdentity.Name;
            GeneralMessage gm = new GeneralMessage();
            Customer cs = _db.GetCustomerByUser(userName);
            if (cs != null && cs.watch_list != null)
            {
                WatchList wl = cs.watch_list.FirstOrDefault(e => e.name == lname);
                if (wl != null)
                {
                    if (wl.equitylist.Remove(sid))
                    {
                        _db.SaveCustomer(cs);
                        gm.success = true;
                        return gm;
                    }

                }
                
            }

            gm.success = false;
            gm.error_message = string.Format("Failed to create watchlistitem, server error!");
            return gm;

        }

        */



        public Dictionary<string, HashSet<int>> GetWatchList(string username)
        {
            
            CustomerWatchList ca = _db.GetCustomerWatchLisstByUser(username);
            if (ca == null || ca.watch_list == null)
            {
                return null;
            }
            return ca.watch_list;
        }


        public GeneralMessage CreateWatchList(string username,string name)
        {
            GeneralMessage gm = new GeneralMessage();
            CustomerWatchList cs = _db.GetCustomerWatchLisstByUser(username);
            if (cs != null)
            {
                if (cs.watch_list == null)
                {
                    cs.watch_list = new Dictionary<string, HashSet<int>>(); ;
                    cs.watch_list.Add(name, null);
                    _db.SaveCustomerWatchLisstByUser(cs);
                    gm.success = true;
                }
                else
                {
                    if (!cs.watch_list.ContainsKey(name))
                    {
                        cs.watch_list.Add(name,null);
                        _db.SaveCustomerWatchLisstByUser(cs);
                        gm.success = true;
                        return gm;
                    }
                }
            }

            gm.success = false;
            gm.error_message = string.Format("Failed to create watchlist {0}, already exist!", name);
            return gm;
        }

        public GeneralMessage DeleteWatchList(string username,string name)
        {
        
            GeneralMessage gm = new GeneralMessage();

            CustomerWatchList cs = _db.GetCustomerWatchLisstByUser(username);
            if (cs != null)
            {
                if (cs.watch_list != null)
                {
                    if (cs.watch_list.Remove(name))
                    {
                        _db.SaveCustomerWatchLisstByUser(cs);
                        gm.success = true;
                        return gm;
                    }
                }
            }

            gm.success = false;
            gm.error_message = string.Format("Failed to delete watchlist {0}, server error!", name);
            return gm;
        }

        public GeneralMessage CreateWatchListItem(string username,string lname, int sid)
        {
            GeneralMessage gm = new GeneralMessage();
            CustomerWatchList cs = _db.GetCustomerWatchLisstByUser(username);
            if (cs != null && cs.watch_list != null)
            {
                HashSet<int> wl = cs.watch_list[lname];
                if (wl != null)
                {
                    wl.Add(sid);

                }
                else
                {
                    cs.watch_list[lname] = new HashSet<int>();
                    wl.Add(sid);
                }
                _db.SaveCustomerWatchLisstByUser(cs);
                gm.success = true;
                return gm;

            }

            gm.success = false;
            gm.error_message = string.Format("Failed to create watchlistitem, server error!");
            return gm;
        }


        public GeneralMessage DeleteWatchListItem(string username, string lname, int sid)
        {

            GeneralMessage gm = new GeneralMessage();
            CustomerWatchList cs = _db.GetCustomerWatchLisstByUser(username);
            if (cs != null && cs.watch_list != null)
            {
                HashSet<int> wl = cs.watch_list[lname];
                if (wl != null)
                {
                    if (wl.Remove(sid))
                    {
                        _db.SaveCustomerWatchLisstByUser(cs);
                        gm.success = true;
                        return gm;
                    }

                }
            }

            gm.success = false;
            gm.error_message = string.Format("Failed to create watchlistitem, server error!");
            return gm;

        }




    }
}
