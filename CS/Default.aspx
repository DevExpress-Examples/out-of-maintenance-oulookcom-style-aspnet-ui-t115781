<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Root.master" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.15.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxCallback" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.15.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.15.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.15.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxGridView.Export" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.15.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.15.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.15.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxSplitter" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.15.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.15.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.15.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.15.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxNavBar" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.15.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxSplitter" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.15.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.15.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

    <script type="text/javascript">

        function OnSelectAuthor(author, info) {

            if (window.NavBar) {
                var selectedItem = NavBar.GetSelectedItem();

                if (selectedItem) {
                    if (selectedItem.group.GetText() == author) {
                        return;
                    }
                }
            }

            focusedId = undefined;

            $('#title').text("");

            $('#info').text(info);
            $('#avatar').attr("src", author + ".jpg");

            var url = 'http://tempuri.org/?Author=' + encodeURI(author) + '&Book=';

            NavBarCallback.PerformCallback(url);

        }

        function SetSelectedBook() {
            if (!window.NavBar) {
                return;
            }

            var selectedItem = NavBar.GetSelectedItem();
            var title;

            if (selectedItem) {
                title = selectedItem.group.GetText() + ': ' + selectedItem.GetText();
            } else {
                title = "";
            }

            window.title = title;
            document.title = title;

            $('#title').text(title);
        }

        function OnTopMenuItemClick(s, e) {
            switch (e.item.name) {
                case "CMD_NEW":

                    window.location = "Edit.aspx";
                    break;

                case "CMD_EDIT":

                    HandleRowDblClick();
                    break;
            }
        }

        var focusedId = undefined;

        function HandleFocusedRowChanged() {

            SetSelectedBook();

            var tmp = GridControl.GetRowKey(GridControl.GetFocusedRowIndex());

            if (tmp === null || tmp === undefined) {

                $("#documentContainer").hide();
            }
            else {

                if (focusedId !== tmp) {

                    focusedId = tmp;

                    $("#documentContainer").show();

                    $("#documentIframe").attr("src", "Document.aspx?Id=" + focusedId);

                }

            }

            var CMD_EDIT = MainMenu.GetItemByName("CMD_EDIT");

            if (CMD_EDIT) {
                if (tmp) {
                    CMD_EDIT.SetVisible(true);
                } else {
                    CMD_EDIT.SetVisible(false);
                }
            }

        }

        function HandleRowDblClick() {
            var tmp = GridControl.GetRowKey(GridControl.GetFocusedRowIndex());

            if (focusedId === tmp) {

                window.location = "Edit.aspx?Id=" + tmp.toString();

            }
        }

        function HandleGridEndCallback() {
            HandleFocusedRowChanged();
        }

        function DoSortClick(s, e) {
            switch (e.item.name) {
                case "CMD_SORT_BY_TITLE":
                case "CMD_SORT_BY_CHAPTER":
                case "CMD_SORT_ASC":
                case "CMD_SORT_DESC":
                    break;
                default:
                    return;
            }

            var bSortDesc = e.item.menu.GetItemByName("CMD_SORT_DESC").GetChecked();
            var bSortByTitle = e.item.menu.GetItemByName("CMD_SORT_BY_TITLE").GetChecked();

            var d = bSortDesc ? "desc" : "asc";
            var s = bSortByTitle ? "&Sort=title&Mode=" + d : "&Sort=chapter&Mode=" + d;


            var bSortTopMenu = e.item.menu.GetItemByName("SortTopMenu");
            if (bSortTopMenu) {
                if (bSortByTitle) {
                    bSortTopMenu.SetText("Sort by: Title");
                } else {
                    bSortTopMenu.SetText("Sort by: Chapter");
                }
            }

            if (s) {

                if (!window.NavBar) {
                    return;
                }

                var item = NavBar.GetSelectedItem();

                var url = 'http://tempuri.org/?Author=' + encodeURI(item.group.GetText()) + '&Book=' + encodeURI(item.GetText()) + s;

                if (focusedId) {

                    url += "&Id=" + focusedId.toString();

                }

                focusedId = undefined;

                SetSelectedBook();

                GridControl.PerformCallback(url);

            }

        }

    </script>

    <dx:ASPxSplitter ID="DefaultContentSplitter" runat="server" Width="100%" Height="100%" ClientInstanceName="DefaultSplitter"
        ClientIDMode="Static">
        <ClientSideEvents PaneResized="function(s, e) { 
            }" />
        <Styles>
            <Pane>
                <Paddings Padding="0px" />
                <Border BorderWidth="0px" />
            </Pane>
        </Styles>
        <Panes>
            <dx:SplitterPane Name="DefaultContentLeftPane" ShowCollapseBackwardButton="True" Size="321px">
                <PaneStyle BackColor="#F3F3F3">
                    <Paddings Padding="0px" />
                </PaneStyle>
                <PaneStyle CssClass="DefaultContentLeftPane">
                    <Paddings Padding="0px"></Paddings>

                </PaneStyle>
                <ContentCollection>
                    <dx:SplitterContentControl>
                        <div style="position: relative; height: 100%;">

                            <div class="DefaultContentLeftPanel">
                                <dx:ASPxCallbackPanel ID="NavBarCallback" runat="server" Width="100%" ClientIDMode="Static" ClientInstanceName="NavBarCallback">

                                    <ClientSideEvents EndCallback="function() { 
                                  
    focusedId = undefined;

    var url = 'http://tempuri.org/?Author=' +  encodeURI(NavBar.groups[0].GetText()) + '&Book=' + encodeURI(NavBar.groups[0].items[0].GetText());
                                   
    SetSelectedBook();                                    

    GridControl.PerformCallback(url);

}" />
                                    <PanelCollection>
                                        <dx:PanelContent runat="server">

                                            <dx:ASPxNavBar ID="NavBar" runat="server" Width="100%" Border-BorderStyle="None"
                                                AllowSelectItem="true" ClientIDMode="Static" ClientInstanceName="NavBar" EnableCallBacks="True"
                                                AutoPostBack="False"
                                                EnableClientSideAPI="True">

                                                <ClientSideEvents
                                                    ItemClick="function(s, e) {  
                                                           
    var url = 'http://tempuri.org/?Author=' +  encodeURI(e.item.group.GetText()) + '&Book=' + encodeURI(e.item.GetText());

    if (focusedId) {
        url += '&Id=' + focusedId.toString();
    }
                                  
    focusedId = undefined;                                                                                                                                  
                                                                                    
    SetSelectedBook();                                   

    GridControl.PerformCallback(url);
                                                  
}" />


                                                <Border BorderStyle="None"></Border>
                                            </dx:ASPxNavBar>

                                        </dx:PanelContent>
                                    </PanelCollection>

                                </dx:ASPxCallbackPanel>
                            </div>

                            <div style="width: 100%; bottom: 20px; position: absolute;">
                                <div style="">
                                    <div style="margin:20px; border-top:1px solid silver; padding-top:20px;">
                                         <img src="<%=this.Author%>.jpg" style="width: 120px; float:left; margin-right:10px; margin-bottom:10px;" id="avatar" />
                                         <span id="info"><%=WebDbProvider.GetAuthorDescription(this.Author)%></span>
                                    </div>
                                </div>                                             
                            </div>

                        </div>

                    </dx:SplitterContentControl>
                </ContentCollection>
            </dx:SplitterPane>
            <dx:SplitterPane Name="DefaultContentCenterPane" ScrollBars="None">
                <PaneStyle CssClass="DefaultContentCenterPane">
                </PaneStyle>
                <Separator Visible="True" Size="1px" SeparatorStyle-BackColor="Silver">
                    <SeparatorStyle>
                        <Border BorderWidth="0px" />
                        <BorderTop BorderWidth="0px" />
                    </SeparatorStyle>
                </Separator>
                <ContentCollection>
                    <dx:SplitterContentControl>

                        <style type="text/css">
                            .BodyPane {
                                overflow: hidden;
                            }

                            .TopPanel {
                                border-bottom: 0px solid silver !important;
                                overflow-y: scroll;
                            }
                        </style>

                        <dx:ASPxSplitter ID="ContentSplitter" runat="server" Width="100%" Height="100%" Orientation="Vertical"
                            ClientInstanceName="ContentSplitter"
                            ClientIDMode="Static">

                            <ClientSideEvents PaneResized="function(s, e) { 
                          
    if(e.pane.name == 'ContentTop') {
       $('#ASPxGridViewPanel1').height(e.pane.GetClientHeight() -  $('#TopHeader').height());
    }

    if(e.pane.name == 'ContentBody') {
        $('#documentIframe').height(e.pane.GetClientHeight() - 27);
    }
        
}" />
                            <Styles>
                                <Pane>
                                    <Paddings Padding="0px" />
                                    <Border BorderWidth="0px" />
                                </Pane>
                            </Styles>

                            <Panes>
                                <dx:SplitterPane Name="ContentTop" ShowCollapseBackwardButton="True" Size="48%">
                                    <PaneStyle CssClass="TopPane">
                                        <Paddings Padding="0px"></Paddings>
                                    </PaneStyle>
                                    <PaneStyle>
                                        <Paddings Padding="0px" />
                                    </PaneStyle>
                                    <ContentCollection>
                                        <dx:SplitterContentControl>

                                            <div id="TopHeader">

                                                <div style="border-bottom: 1px solid silver; padding-top: 2px; padding-bottom: 2px;">

                                                    <div style="float: left;">
                                                        <div class="title" id="title"></div>
                                                    </div>
                                                    <div style="float: right; margin-right: 20px;" class="dropDownMenu">
                                                        <dx:ASPxMenu ID="SortMenu" runat="server" ItemAutoWidth="False" >

                                                            <ClientSideEvents ItemClick="function(s, e) {
                                                                
    DoSortClick(s, e); 

}" />
                                                            <ItemStyle>
                                                                <Paddings PaddingTop="10" PaddingBottom="10" />
                                                            </ItemStyle>

                                                            <Items>
                                                                <dx:MenuItem Text="Sort by: Chapter" Name="SortTopMenu" Image-Url="DownSmall.png" ItemStyle-CssClass="dropDownMenu_SortBy">
                                                                    <Items>
                                                                        <dx:MenuItem Text="Title" Checked="true" GroupName="Sort" Name="CMD_SORT_BY_TITLE"></dx:MenuItem>
                                                                        <dx:MenuItem Text="Chapter" GroupName="Sort" Name="CMD_SORT_BY_CHAPTER"></dx:MenuItem>
                                                                        <dx:MenuItem Text="Ascending" Checked="true" BeginGroup="true" GroupName="Direction" Name="CMD_SORT_ASC"></dx:MenuItem>
                                                                        <dx:MenuItem Text="Descending" GroupName="Direction" Name="CMD_SORT_DESC"></dx:MenuItem>
                                                                    </Items>

                                                                    <Image Url="DownSmall.png" Height="12px" Width="8px"></Image>
                                                                </dx:MenuItem>
                                                            </Items>

                                                            <Border BorderWidth="0px" />
                                                            <BorderTop BorderWidth="1px" />

                                                        </dx:ASPxMenu>
                                                    </div>

                                                    <div style="clear: both;"></div>

                                                </div>

                                            </div>

                                            <div class="TopPanel" id="ASPxGridViewPanel1" style="position: relative;">

                                                <dx:ASPxGridView ID="GridControl" runat="server" AutoGenerateColumns="False"
                                                    ClientInstanceName="GridControl"
                                                    Width="100%" KeyFieldName="Id">

                                                    <Columns>
                                                        <dx:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="40px">
                                                        </dx:GridViewCommandColumn>
                                                        <dx:GridViewDataTextColumn FieldName="Author" VisibleIndex="2" >
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="Book" VisibleIndex="3" >
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="Title" VisibleIndex="4">
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="Year" VisibleIndex="6" >
                                                             <CellStyle HorizontalAlign="Right">
                                                            </CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="Chapter" VisibleIndex="7">
                                                            <CellStyle HorizontalAlign="Right">
                                                            </CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                    </Columns>

                                                    <ClientSideEvents FocusedRowChanged="HandleFocusedRowChanged" />
                                                    <ClientSideEvents RowDblClick="HandleRowDblClick" />
                                                    <ClientSideEvents EndCallback="HandleGridEndCallback" />

                                                    <Settings GridLines="None" ShowColumnHeaders="False" />
                                                    <SettingsBehavior AllowFocusedRow="True" />
                                                    <SettingsPager Visible="True" PageSize="2" Mode="ShowAllRecords" />
                                                    <SettingsText EmptyDataRow="(No Documents)" />
                                                    <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
                                                    <Paddings Padding="0px" />
                                                    <Border BorderWidth="0px" />

                                                </dx:ASPxGridView>

                                            </div>

                                        </dx:SplitterContentControl>
                                    </ContentCollection>
                                </dx:SplitterPane>

                                <dx:SplitterPane Name="ContentBody" ScrollBars="Auto">

                                    <PaneStyle CssClass="BodyPane">
                                    </PaneStyle>

                                    <Separator Visible="True" Size="1px" SeparatorStyle-BackColor="Silver">
                                        <SeparatorStyle>
                                            <Border BorderWidth="0px" />
                                            <BorderTop BorderWidth="0px" />
                                        </SeparatorStyle>
                                    </Separator>

                                    <ContentCollection>
                                        <dx:SplitterContentControl>
                                            <div style="border-top: 0px solid gray; overflow: hidden;">

                                                <div style="padding: 0px; overflow: hidden; display: none;" id="documentContainer">

                                                    <iframe src="Document.aspx"
                                                        style="width: 100%; overflow: hidden; border: 0; margin: 0; padding: 0; border-bottom: 1px solid silver; border-top: 0px solid silver;"
                                                        id="documentIframe"></iframe>

                                                    <div style="height: 20px; padding-left: 6px;" class="copy">
                                                        (c) 2014 Developer Express, Inc.
                                                    </div>

                                                </div>

                                            </div>
                                        </dx:SplitterContentControl>
                                    </ContentCollection>

                                </dx:SplitterPane>

                            </Panes>
                        </dx:ASPxSplitter>

                    </dx:SplitterContentControl>
                </ContentCollection>
            </dx:SplitterPane>

        </Panes>
    </dx:ASPxSplitter>

</asp:Content> 