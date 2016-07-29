using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace shareshare.Business
{
    public class ConstantV
    {

        public const string ACCOUNT_TYPE_REGULAR = "Regular";
        public const string ACCOUNT_TYPE_SHORT = "Short";
        public const string ACCOUNT_TYPE_CASH = "Cash";


        public const string ORDER_STATUS_CREATE = "Create";
        public const string ORDER_STATUS_PENDING_SUBMIT = "PendingSubmit";
        public const string ORDER_STATUS_PENDING_CANCEL = "PendingCancel";
        public const string ORDER_STATUS_PRESUBMITTED = "PreSubmitted";
        public const string ORDER_STATUS_SUBMITTED = "Submitted";
        public const string ORDER_STATUS_CANCELLED = "Cancelled";
        public const string ORDER_STATUS_FILLED = "Filled";
        public const string ORDER_STATUS_CLOSED = "Closed";
        public const string ORDER_STATUS_INACTIVE = "Inactive";
        public const string ORDER_STATUS_DEAD = "Dead";


        public const string ORDER_TYPE_MARKET = "market";
        public const string ORDER_TYPE_LIMIT = "limit";

        public const string ORDER_EXPIRY_DAY_ = "day";
        public const string ORDER_EXPIRY_GTC = "gtc";
        public const string ORDER_EXPIRY_GTD = "gtd";




    }
}
