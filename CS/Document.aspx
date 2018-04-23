<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Document.aspx.cs" Inherits="Document" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title><%=this.Title%></title>

    <style type="text/css">
        html, body, form, p, div {
            margin: 0;
            padding: 0;
            border: 0;
            font-family: "book antiqua",Calibri, "times new roman",times,sans-serif;
            font-size: 16px;
            color: rgb(68, 68, 68);
            font-family: 'Segoe UI Light', 'Segoe UI Web Light', 'Segoe UI Web Regular', 'Segoe UI', 'Segoe UI Symbol', HelveticaNeue-Light, 'Helvetica Neue', Arial, sans-serif;
            font-kerning: auto;
            font-style: normal;
            font-variant: normal;
            font-variant-ligatures: normal;
            font-weight: normal;
        }
        p {
            margin-bottom: 1em;
        }
    </style>
    <style type="text/css">
        #subject {
            font-size: 21px;
            margin-bottom: 5px;
            height: 39px;
            line-height: 39px;
            padding-left: 21px;
            padding-bottom: 0px;
            float: left;
        }
        #chatper {
            font-size: 21px;
            margin-bottom: 5px;
            height: 39px;
            line-height: 39px;
            padding-right: 21px;
            padding-bottom: 0px;
            float: right;
        }        
    </style>

    <script type="text/javascript" src="jquery-2.1.0.js"></script>

</head>

<body>
    
    <form id="DocumentForm" runat="server">

        <div style="line-height: 39px;">
            <div id="subject"><%=this.DocumentTitle%></div>
            <div id="chatper"><%=this.Chapter%></div>
        </div>

        <div style="clear: both;"></div>

        <div id="body" style="padding: 21px; padding-top: 0px;">
            <div style="border-top: 1px solid silver; padding-top: 10px;">
                <%=this.Body%>
            </div>
        </div>

    </form>

</body>
</html>