<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PMS_UserRulesMapping_List.aspx.cs"
    Inherits="RFD.LMS.WebUI.Permission.PMS_UserRulesMapping_List" MasterPageFile="~/Frame/Frame.Master" %>

<%@ Register Src="../UserControls/Pager.ascx" TagName="Pager" TagPrefix="uc1" %>
<asp:Content ID="headcontent" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/AeroWindow.css" rel="stylesheet" type="text/css" />

    <script src="../Scripts/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Scripts/jquery-ui-1.8.1.custom.min.js" type="text/javascript"></script>

    <script src="../Scripts/jquery.easing.1.3.js" type="text/javascript"></script>

    <script src="../Scripts/jquery-AeroWindow.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function() {
            var openWin = function(event) {
                var go = $(this);
                $('#mywindows').attr('src', go.attr('href'))
                $('#openWindow').AeroWindow({
                    WindowTitle: go.attr('title') + $("#pageTitle").text(),
                    WindowPositionTop: 'center',
                    WindowPositionLeft: 'center',
                    WindowWidth: 800,
                    WindowHeight: 400,
                    WindowMinimize: false,
                    OnClosed: function() {
                        $("#<%=BtnQuery.ClientID %>").click();
                    }
                });
                $('.AeroWindow .title nobr').html(go.attr('title') + $("#pageTitle").text());
                $('html,body').animate({ scrollTop: '0px' }, 300);
                event.preventDefault();
            };
            $('.editLink').bind('click', openWin);
            $("#btnAdd").bind('click', openWin);
            $('.deleteLink').bind('click', function(event) {
                if(!confirm("是否删除该记录？")) {
                    event.preventDefault();
                }
            });
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
	                <asp:TextBox ID="TB_UserCode" runat="server"></asp:TextBox>
	            </td>
						            <td>
	                规则ID
	                <asp:TextBox ID="TB_RuleId" runat="server"></asp:TextBox>
	            </td>
						            <td>
	                IsDelete
	                <asp:TextBox ID="TB_IsDelete" runat="server"></asp:TextBox>
	            </td>
						            <td>
	                CreateBy
	                <asp:TextBox ID="TB_CreateBy" runat="server"></asp:TextBox>
	            </td>
						            <td>
	                CreateTime
	                <asp:TextBox ID="TB_CreateTime" runat="server"></asp:TextBox>
	            </td>
						       </tr>
			<tr>
				            <td>
	                UpdateBy
	                <asp:TextBox ID="TB_UpdateBy" runat="server"></asp:TextBox>
	            </td>
						            <td>
	                UpdateTime
	                <asp:TextBox ID="TB_UpdateTime" runat="server"></asp:TextBox>
	            </td>
						<td>
	    </td>
				<td>
	    </td>
				<td>
	    </td>
			    </tr>
		<tr>
            <td align="right"  colspan="5">
                <asp:Button ID="BtnQuery" runat="server" Text="查询(S)" OnClick="BtnQuery_Click" />
                <input type="button" id="btnAdd" href="PMS_UserRulesMapping_Edit.aspx" title="新增" value="新增(A)" />
            </td>
        </tr>
        <tr>
            <td colspan="5">
                <asp:GridView ID="GridView1" runat="server" DataKeyNames="Id" AutoGenerateColumns="False"
                    OnRowDataBound="GridView1_RowDataBound" Width="100%" OnRowCommand="GridView1_RowCommand">
                    <HeaderStyle Wrap="false" />
                    <RowStyle Wrap="false" />
                    <Columns>
					<asp:BoundField DataField="Id" HeaderText="主键" />
										<asp:BoundField DataField="UserCode" HeaderText="用户编号" />
										<asp:BoundField DataField="RuleId" HeaderText="规则ID" />
										<asp:BoundField DataField="IsDelete" HeaderText="IsDelete" />
										<asp:BoundField DataField="CreateBy" HeaderText="CreateBy" />
										<asp:BoundField DataField="CreateTime" HeaderText="CreateTime" />
										<asp:BoundField DataField="UpdateBy" HeaderText="UpdateBy" />
										<asp:BoundField DataField="UpdateTime" HeaderText="UpdateTime" />
					                        <asp:TemplateField>
                            <HeaderTemplate>
                                编辑
                            </HeaderTemplate>
                            <ItemTemplate>
                                <a class="editLink" title='编辑【PMS_UserRulesMapping】'
                                    href='PMS_UserRulesMapping_Edit.aspx?RuleId=<%# DataBinder.Eval(Container.DataItem, "Id") %>'>
                                    编辑</a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="删除" ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton class="deleteLink" ID="LB_Delete" runat="server" CausesValidation="false"
                                    CommandName="DeleteById" Text="删除" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <uc1:Pager ID="Pager1" runat="server" />
            </td>
        </tr>
    </table>
    <div id="openWindow" style="display: none;">
        <iframe style="border-right-width: 0px; border-top-width: 0px; border-bottom-width: 0px;
            border-left-width: 0px" id="mywindows" height="99.5%" src="" frameborder="0"
            width="100%"></iframe>
    </div>
</asp:Content>
