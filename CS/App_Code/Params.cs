using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.SessionState;

public static class CallbackParams {
    public static NameValueCollection Get(HttpRequest request) {
        String __CALLBACKPARAM = Convert.ToString(request.Form["__CALLBACKPARAM"]);

        if (__CALLBACKPARAM != null && __CALLBACKPARAM.IndexOf("http://tempuri.org/?") > 0) {

            if (__CALLBACKPARAM.EndsWith(";")) {
                __CALLBACKPARAM = __CALLBACKPARAM.Remove(__CALLBACKPARAM.Length - 1);
            }

            __CALLBACKPARAM = __CALLBACKPARAM.Substring(
                            __CALLBACKPARAM.IndexOf("http://tempuri.org/?"));

            NameValueCollection parameters =
                        System.Web.HttpUtility.ParseQueryString(new Uri(__CALLBACKPARAM).Query);

            return parameters;

        }

        return null;
    }
}

public static class SqlParams {
    public static NameValueCollection Get(NameValueCollection parameters, HttpSessionState session) {

        if (parameters != null) {             

            String sort = parameters["Sort"];
            if (String.IsNullOrWhiteSpace(sort)) {
                sort = Convert.ToString(session["Sort"]);
            }

            if (String.Equals(sort, "title", StringComparison.InvariantCultureIgnoreCase)) {
                sort = "Title";
            } 
            else {
                sort = "Chapter";
            }

            parameters["Sort"] = sort;

            String mode = parameters["Mode"];
            if (String.IsNullOrWhiteSpace(mode)) {
                mode = Convert.ToString(session["Mode"]);
            }

            if (String.Equals(mode, "desc", StringComparison.InvariantCultureIgnoreCase)) {
                mode = "DESC";
            } else {
                mode = "ASC";
            }

            parameters["Mode"] = mode;

        }

        return parameters;
    }
}