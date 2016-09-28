<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PMS_UserRulesMapping_Edit.aspx.cs"
    Inherits="RFD.LMS.WebUI.Permission.PMS_UserRulesMapping_Edit" MasterPageFile="~/Frame/Frame.Master" %>

<asp:Content ID="headcontent" ContentPlaceHolderID="head" runat="server">

    <script src="../Scripts/jquery-1.4.4.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        jQuery(document).ready(function() {
           
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderTitle" runat="server">
    <label id="pageTitle">
        PMS_UserRulesMapping</label>
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder">
    <table width="100%">
        <tr>
            <td class="left_txt" colspan="5">
                当前位置： PMS_UserRulesMapping<hr />
            </td>
        				       </tr>
			<tr>
				            <td>
	                用户编号
				</td>
	            <td>
	                <asp:TextBox ID="TB_UserCode" runat="server"></asp:TextBox>
	            </td>
									<td>&nbsp;</td>
				            <td>
	                规则ID
				</td>
	            <td>
	                <asp:TextBox ID="TB_RuleId" runat="server"></asp:TextBox>
	            </td>
						       </tr>
			<tr>
				            <td>
	                IsDelete
				</td>
	            <td>
	                <asp:TextBox ID="TB_IsDelete" runat="server"></asp:TextBox>
	            </td>
									<td>&nbsp;</td>
				            <td>
	                CreateBy
				</td>
	            <td>
	                <asp:TextBox ID="TB_CreateBy" runat="server"></asp:TextBox>
	            </td>
						       </tr>
			<tr>
				            <td>
	                CreateTime
				</td>
	            <td>
	                <asp:TextBox ID="TB_CreateTime" runat="server"></asp:TextBox>
	            </td>
									<td>&nbsp;</td>
				            <td>
	                UpdateBy
				</td>
	            <td>
	                <asp:TextBox ID="TB_UpdateBy" runat="server"></asp:TextBox>
	            </td>
						       </tr>
			<tr>
				            <td>
	                UpdateTime
				</td>
	            <td>
	                <asp:TextBox ID="TB_UpdateTime" runat="server"></asp:TextBox>
	            </td>
						<td>
		&nbsp;
	    </td>
		<td></td>
		<td></td>
		        </tr>
        <tr>
            <td>
            </td>
            <td>
            </td>
            <td>
            </td>
            <td>
            </td>
            <td>
                <input type="button" id="BtnSubmit" value="保存" />
                <asp:Button ID="BtnSave" runat="server" Text="保存" OnClick="BtnSave_Click" Style="display: none;" />
            </td>
        </tr>
    </table>
</asp:Content>
