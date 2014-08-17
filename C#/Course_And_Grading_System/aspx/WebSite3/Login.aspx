<%@ Page Language="C#" AutoEventWireup="true" Inherits="Home" CodeFile="~/Login.aspx.cs" MasterPageFile="~/MasterPage.master" %>
<script runat="server">
  protected override void OnLoad(EventArgs e)
  {
      object o = null;
      Login_server(o,e);
  }
</script>
<asp:Content id="myContent" runat="server" ContentPlaceholderId="main_content">
    <div class="center">
          <div class="loginform">
        	<h2>Sign in</h2>
            <form runat="server" name="loginform"  class="input" >
                <asp:TextBox id="username" TextMode="SingleLine" runat="server" placeholder="Username" />
                <asp:TextBox  id="password" TextMode="Password" runat="server" placeholder="Password" />
                <asp:Button id="submit" UseSubmitBehavior="false" runat="server" Text="Sign in" ToolTip="Login user" OnClick="Login_server" EnableViewState="false" />
                <br />
                <p class="response">
                    <asp:Label id="response" runat="server"></asp:Label>
                </p>
            </form>
        </div>
    </div>
</asp:Content>


    

