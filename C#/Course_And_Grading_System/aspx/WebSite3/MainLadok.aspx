<%@ Page Language="C#" AutoEventWireup="true" Inherits="MainLadok" CodeFile="~/MainLadok.aspx.cs" MasterPageFile="~/MasterPage.master" EnableEventValidation="true" %>
<script runat="server">
  protected override void OnLoad(EventArgs e)
  {
      MainLadokView();
  }
</script>
<asp:Content id="myHeader" runat="server" ContentPlaceholderId="header_content">
    <asp:PlaceHolder ID="headerInfo" runat="server"></asp:PlaceHolder>
    <form id="tmpform" runat="server" name="logoutform"  class="input" >
                <div class="headerLogOut"><asp:Button id="submit" UseSubmitBehavior="false" runat="server" Text="Logout" ToolTip="Logout user" OnClick="Logout" EnableViewState="false" /></div>
    </form>
</asp:Content>
<asp:Content id="myContent" runat="server" ContentPlaceholderId="main_content">
    <div class="center">
        <form action="update.aspx" method="post" >
            <div class="coursesList"> 
                <div id="latest">
                     <asp:PlaceHolder ID="latestChanges" runat="server"></asp:PlaceHolder>
                </div>
                <div id="maintitle">
                  <h2>All courses</h2>
                </div>
                <div id="search">
                     <input type="text" size="40" placeholder="Search..." />
                </div> 
                <asp:PlaceHolder ID="courses" runat="server"></asp:PlaceHolder>
            </div>
            <div class="update">
                <input type="submit" value="Perform changes"/>
            </div>
        </form>
    </div>
</asp:Content>
