<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PMS_RuleType_Edit.aspx.cs"
    Inherits="RFD.LMS.WebUI.Permission.PMS_RuleType_Edit" MasterPageFile="~/Frame/Frame.Master" %>

<asp:Content ID="headcontent" ContentPlaceHolderID="head" runat="server">

    <script src="../Scripts/jquery-1.4.4.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        jQuery(document).ready(function() {
           
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderTitle" runat="server">
    <label id="pageTitle">
        规则类型</label>
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder">
    <table width="100%">
        <tr>
            <td class="left_txt" colspan="5">
                当前位置： 规则类型<hr />
            </td>
        				       </tr>
			<tr>
				            <td>
	                规则类型标识
				</td>
	            <td>
	                <asp:TextBox ID="TB_RuleTypeKey" runat="server"></asp:TextBox>
	            </td>
									<td>&nbsp;</td>
				            <td>
	                规则类型名称
				</td>
	            <td>
	                <asp:TextBox ID="TB_RuleTypeName" runat="server"></asp:TextBox>
	            </td>
						       </tr>
			<tr>
				            <td>
	                数据源类型：0，实现IDataPermissionService的同一应用程序集下的类，1，WCF服务地址
				</td>
	            <td>
	                <asp:TextBox ID="TB_SourceType" runat="server"></asp:TextBox>
	            </td>
									<td>&nbsp;</td>
				            <td>
	                数据源类名或WCF地址
				</td>
	            <td>
	                <asp:TextBox ID="TB_SourceName" runat="server"></asp:TextBox>
	            </td>
						       </tr>
			<tr>
				            <td>
	                子系统标识
				</td>
	            <td>
	                <asp:TextBox ID="TB_SubSysKey" runat="server"></asp:TextBox>
	            </td>
									<td>&nbsp;</td>
				            <td>
	                
				</td>
	            <td>
	                <asp:TextBox ID="TB_IsDelete" runat="server"></asp:TextBox>
	            </td>
						       </tr>
			<tr>
				            <td>
	                
				</td>
	            <td>
	                <asp:TextBox ID="TB_CreatBy" runat="server"></asp:TextBox>
	            </td>
									<td>&nbsp;</td>
				            <td>
	                
				</td>
	            <td>
	                <asp:TextBox ID="TB_CreateTime" runat="server"></asp:TextBox>
	            </td>
						       </tr>
			<tr>
				            <td>
	                
				</td>
	            <td>
	                <asp:TextBox ID="TB_UpdateBy" runat="server"></asp:TextBox>
	            </td>
									<td>&nbsp;</td>
				            <td>
	                
				</td>
	            <td>
	                <asp:TextBox ID="TB_UpdateTime" runat="server"></asp:TextBox>
	            </td>
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
