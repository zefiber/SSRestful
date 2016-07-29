using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using shareshare.Message;
using Database;

namespace shareshare.Business
{
    public partial class BusinessUnit
    {

        public WatchListResponseMessage CreateWatchList(CreateWatchListRequest req, string username)
        {
            WatchListResponseMessage resp = new WatchListResponseMessage();
            Customer cs = _dataAccess.GetCustomerByUser(username);
            if (cs != null)
            {
                WatchList wl = new WatchList();
                wl.name = req.watch_list_name;
                if (cs.watch_list == null)
                {
                    cs.watch_list = new HashSet<WatchList>();
                    cs.watch_list.Add(wl);
                    _dataAccess.SaveCustomer(cs);
                    resp.success = true;
                }
                else
                {
                    if (!cs.watch_list.Contains(wl))
                    {
                        cs.watch_list.Add(wl);
                        _dataAccess.SaveCustomer(cs);
                        resp.success = true;
                    }
                    else
                    {
                        resp.success = false;
                        resp.fail_reason = string.Format("Failed to create watchlist {0}, it already exists", req.watch_list_name);
                    }

                }

            }
            else
            {
                resp.success = false;
                resp.fail_reason = string.Format("Failed to create watchlist {0}, server error! can't find customer", req.watch_list_name);
            }
            return resp;
        }


        public WatchListResponseMessage DeleteWatchList(DeleteWatchListRequest req, string username)
        {
            WatchListResponseMessage resp = new WatchListResponseMessage();
            Customer cs = _dataAccess.GetCustomerByUser(username);
            if (cs != null)
            {
                WatchList wl = new WatchList();
                wl.name = req.watch_list_name;
                if (cs.watch_list != null)
                {
                    cs.watch_list.Remove(wl);
                    _dataAccess.SaveCustomer(cs);
                }

            }
            else
            {
                resp.success = false;
                resp.fail_reason = string.Format("Failed to create watchlist {0}, server error! can't find customer", req.watch_list_name);
            }
            resp.success = true;
            return resp;
        }


        public WatchListResponseMessage CreateWatchListItem(CreateWatchListItemRequest req, string username)
        {
            WatchListResponseMessage resp = new WatchListResponseMessage();
            Customer cs = _dataAccess.GetCustomerByUser(username);
            if (cs != null && cs.watch_list != null)
            {
                WatchList wl = cs.watch_list.FirstOrDefault(e => e.name == req.watch_list_name);
                if (wl != null)
                {
                    if (wl.equitylist == null)
                    {
                        wl.equitylist = new HashSet<int>();
                    }
                    wl.equitylist.Add(req.equity_id);

                }
                _dataAccess.SaveCustomer(cs);
                resp.success = true;
                return resp;
            }

            resp.success = false;
            resp.fail_reason = string.Format("Failed to create watchlistitem, server error!");
            return resp;
        }



        public WatchListResponseMessage DeleteWatchListItem(DeleteWatchListItemRequest req, string username)
        {
            WatchListResponseMessage resp = new WatchListResponseMessage();
            Customer cs = _dataAccess.GetCustomerByUser(username);
            if (cs != null && cs.watch_list != null)
            {
                WatchList wl = cs.watch_list.FirstOrDefault(e => e.name == req.watch_list_name);
                if (wl != null)
                {
                    wl.equitylist.Remove(req.equity_id);

                }
                _dataAccess.SaveCustomer(cs);
                resp.success = true;
                return resp;
            }

            resp.success = false;
            resp.fail_reason = string.Format("Failed to create watchlistitem, server error!");
            return resp;
        }

    }
}
