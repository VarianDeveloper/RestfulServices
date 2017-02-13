<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Untitled Page</title>
    <script type="text/javascript">
    
         function LookupCourses()
         {          
            tempuri.org.ICourseService.GetCourseList(OnLookupComplete, OnError);
         }
            
         function OnLookupComplete(result, state)
         {
           var sel = $get("Select1");
           sel.length = 0;
           for (i in result)
                sel.add(new Option(result[i], result[i]));           
         }

         function OnError(result)
         { alert("Error: " + result.get_message()); }

    </script>
    <style type="text/css">
        #Select1 {
            width: 135px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
            <Services>
                <asp:ServiceReference Path="~/courses.svc" />
            </Services>
        </asp:ScriptManager>
    </div>
    </form>
    <select id="Select1" name="D1">
    </select>
    <input id="Button1" type="button" value="Load Courses" onclick="return LookupCourses();"/>
</body>
</html>
