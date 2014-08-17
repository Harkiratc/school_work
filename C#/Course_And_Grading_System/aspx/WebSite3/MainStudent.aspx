<%@ Page Language="C#" AutoEventWireup="true" Inherits="MainStudent" CodeFile="~/MainStudent.aspx.cs" MasterPageFile="~/MasterPage.master" %>
<script runat="server">
  protected override void OnLoad(EventArgs e)
  {
      MainStudentView();
  }
</script>
<asp:Content id="myHeader" runat="server" ContentPlaceholderId="header_content">
    <asp:PlaceHolder ID="headerInfo" runat="server"></asp:PlaceHolder>
    <form id="Form1" runat="server" name="logoutform"  class="input" >
                <div class="headerLogOut"><asp:Button id="submit" UseSubmitBehavior="false" runat="server" Text="Logout" ToolTip="Logout user" OnClick="Logout" EnableViewState="false" /></div>
    </form>
</asp:Content>
<asp:Content id="myContent" runat="server" ContentPlaceholderId="main_content">
    <div class="center">
          <div class="coursesList">
        	<h2>Your courses</h2>
            <asp:PlaceHolder ID="courses" runat="server"></asp:PlaceHolder>
        </div>
    </div>
</asp:Content>
