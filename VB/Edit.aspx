<%@ Page Title="" Language="vb" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeFile="Edit.aspx.vb" Inherits="Edit" %>

<%@ Register Assembly="DevExpress.Web.ASPxHtmlEditor.v14.1, Version=14.1.15.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxHtmlEditor" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.15.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxSpellChecker.v14.1, Version=14.1.15.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxSpellChecker" TagPrefix="dx" %>















<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

	<input name="_H_TITLE" id="_H_TITLE" type="hidden" />
	<input name="_H_BOOK" id="_H_BOOK" type="hidden" />
	<input name="_H_AUTHOR" id="_H_AUTHOR" type="hidden" />
	<input name="_H_CHAPTER" id="_H_CHAPTER" type="hidden" />
	<input name="_H_YEAR" id="_H_YEAR" type="hidden" />
	<input name="_H_HTML" id="_H_HTML" type="hidden" />

	<script type="text/javascript">

		function OnSelectAuthor(author) {
			var url = 'Default.aspx/?Author=' + encodeURI(author) + '&Book=';

			window.location = url;
		}

		function OnTopMenuItemClick(s, e) {

			switch (e.item.name) {
				case "CMD_CANCEL":

					window.location = "Default.aspx";

					break;

				case "CMD_SAVE":

					window.alert("This command is not avaible in the online demo");
					window.location = "Default.aspx";

					break;

/*******************************************************************************************************

				This command is not avaible in the online demo

				case "CMD_SAVE":

					ASPxComboBox_Author.Validate();
					ASPxTextBox_Book.Validate();
					ASPxTextBox_Title.Validate();

					if (ASPxComboBox_Author.GetIsValid() && ASPxTextBox_Book.GetIsValid() && ASPxTextBox_Title.GetIsValid()) {

						document.getElementById("_H_AUTHOR").value = ASPxComboBox_Author.GetText();
						document.getElementById("_H_BOOK").value = ASPxTextBox_Book.GetText();
						document.getElementById("_H_CHAPTER").value = ASPxTextBox_Chapter.GetText();
						document.getElementById("_H_TITLE").value = ASPxTextBox_Title.GetText();
						document.getElementById("_H_YEAR").value = ASPxTextBox_Year.GetText();
						document.getElementById("_H_HTML").value = ASPxHtmlEditor.GetHtml();

						$("#aspnetForm").submit();

					}

					break;

*******************************************************************************************************/

			}

		}

	</script>

	<dx:ASPxSplitter ID="MainSplitter" runat="server" Width="100%" Height="100%" ClientInstanceName="MainSplitter" ClientIDMode="Static">

		<ClientSideEvents PaneResized="function(s, e) { 

					 if(e.pane.name == 'ContentCenter') {                 

						var clientHeight = e.pane.GetClientHeight();                     

						ASPxHtmlEditor.SetHeight(clientHeight - 100);

					}

}" />

		<Styles>
			<Pane>
				<Paddings Padding="0px" />
				<Border BorderWidth="0px" />
			</Pane>
		</Styles>
		<Panes>
			<dx:SplitterPane Name="ContentLeft" ShowCollapseBackwardButton="True" Size="321px">
				<PaneStyle BackColor="#F3F3F3">
					<Paddings Padding="0px" />
				</PaneStyle>
				<PaneStyle CssClass="leftPane">
					<Paddings Padding="0px"></Paddings>
				</PaneStyle>
				<ContentCollection>
					<dx:SplitterContentControl>

						<div class="leftPanel">

							<table style="width: 100%;">
								<tr>
									<td>
										<div style="padding: 22px; padding-top: 30px;">

											<div class="BlueTitle">
												Author:
											</div>

											<script type="text/javascript">

												function OnValidateAuthor(s, e) {

													var t = s.GetText();

													switch (t) {

														case "Shakespeare":
														case "Poe":
														case "Dickinson": 
															e.isValid = true;
															break;

														default:
															e.isValid = false;
															break;

													}

												}

											</script>

											<dx:ASPxComboBox ID="ASPxComboBox_Author" runat="server"
												ValueType="System.String"
												ClientInstanceName="ASPxComboBox_Author"
												Font-Size="20px"
												Font-Names="'Segoe UI Light','Segoe UI Web Light','Segoe UI Web Regular','Segoe UI','Segoe UI Symbol','HelveticaNeue-Light','Helvetica Neue',Arial,sans-serif"
												Width="100%"
												DropDownStyle="DropDown">

												<ClientSideEvents Validation="function(s, e) { OnValidateAuthor(s, e); }" />

												<ValidationSettings ErrorDisplayMode="None">
													<RequiredField ErrorText="" IsRequired="True" />
													<RegularExpression ErrorText=""  />
												</ValidationSettings>


											</dx:ASPxComboBox>

											<div class="BlueTitle">
												Book:
											</div>

											<dx:ASPxTextBox
												Width="100%"
												Font-Size="20px"
												Font-Names="'Segoe UI Light','Segoe UI Web Light','Segoe UI Web Regular','Segoe UI','Segoe UI Symbol','HelveticaNeue-Light','Helvetica Neue',Arial,sans-serif"
												ID="ASPxTextBox_Book" runat="server"
												Height="30px"
												ClientInstanceName="ASPxTextBox_Book">

												<ValidationSettings ErrorDisplayMode="None">
													<RequiredField ErrorText="" IsRequired="True" />
												</ValidationSettings>

											</dx:ASPxTextBox>


											<div class="BlueTitle">
												Year:
											</div>

											<dx:ASPxTextBox
												Width="100%"
												Font-Size="20px"
												Font-Names="'Segoe UI Light','Segoe UI Web Light','Segoe UI Web Regular','Segoe UI','Segoe UI Symbol','HelveticaNeue-Light','Helvetica Neue',Arial,sans-serif"
												ID="ASPxTextBox_Year" runat="server"
												Height="30px"
												ClientInstanceName="ASPxTextBox_Year">

												<ValidationSettings ErrorDisplayMode="None">
													<RequiredField ErrorText="" IsRequired="False" />
												</ValidationSettings>

											</dx:ASPxTextBox>

										</div>

									</td>
								</tr>
							</table>

						</div>

					</dx:SplitterContentControl>
				</ContentCollection>
			</dx:SplitterPane>
			<dx:SplitterPane Name="ContentCenter" ScrollBars="None">
				<PaneStyle CssClass="ContentPane">
				</PaneStyle>
				<Separator Visible="True" Size="1px" SeparatorStyle-BackColor="Silver">
					<SeparatorStyle>
						<Border BorderWidth="0px" />
						<BorderTop BorderWidth="0px" />
					</SeparatorStyle>
				</Separator>
				<ContentCollection>
					<dx:SplitterContentControl>

						<div style="padding: 10px; overflow: hidden;" id="ContentDiv">

							<table>
								<tr>
									<td style="width: 80%;">
										<dx:ASPxTextBox
											NullText="(Title)"
											Border-BorderColor="Transparent"
											Width="100%"
											Font-Size="23px"
											Font-Names="'Segoe UI Light','Segoe UI Web Light','Segoe UI Web Regular','Segoe UI','Segoe UI Symbol','HelveticaNeue-Light','Helvetica Neue',Arial,sans-serif"
											ID="ASPxTextBox_Title" runat="server"
											Height="30px"
											ClientInstanceName="ASPxTextBox_Title">

											<ValidationSettings ErrorDisplayMode="None">
												<RequiredField ErrorText="" IsRequired="True" />
											</ValidationSettings>

										</dx:ASPxTextBox>
									</td>
									<td style="width: 20%;">
										<dx:ASPxTextBox
											NullText="(Chapter)"
											Border-BorderColor="Transparent"
											Width="100%"
											Font-Size="23px"
											Font-Names="'Segoe UI Light','Segoe UI Web Light','Segoe UI Web Regular','Segoe UI','Segoe UI Symbol','HelveticaNeue-Light','Helvetica Neue',Arial,sans-serif"
											ID="ASPxTextBox_Chapter" runat="server"
											Height="30px"
											HorizontalAlign="Right"
											ClientInstanceName="ASPxTextBox_Chapter">

											<ValidationSettings ErrorDisplayMode="None">
												<RequiredField ErrorText="" IsRequired="False" />
											</ValidationSettings>

										</dx:ASPxTextBox>
									</td>
								</tr>
							</table>

							<div style="border-top: 1px solid silver; margin-top: 10px;">

								<dx:ASPxHtmlEditor
									BorderBottom-BorderStyle="None"
									Width="100%"
									ID="ASPxHtmlEditor"
									runat="server"
									Font-Names="Calibri, sans-serif"
									SettingsHtmlEditing-EnterMode="BR">

									<Settings AllowDesignView="True" AllowHtmlView="False" AllowPreview="False" />

									<SettingsHtmlEditing EnterMode="BR"></SettingsHtmlEditing>



									<Border BorderWidth="0px" />
									<Styles>
										<ContentArea>
											<Paddings Padding="0px" />
										</ContentArea>
										<PreviewArea BackColor="White"></PreviewArea>
									</Styles>

									<CssFiles>
										<dx:HtmlEditorCssFile FilePath="Editor.css" />
									</CssFiles>

									<Toolbars>
										<dx:HtmlEditorToolbar Name="CustomToolbar">
											<Items>

												<dx:ToolbarBoldButton BeginGroup="True">
												</dx:ToolbarBoldButton>
												<dx:ToolbarItalicButton>
												</dx:ToolbarItalicButton>
												<dx:ToolbarUnderlineButton>
												</dx:ToolbarUnderlineButton>
												<dx:ToolbarStrikethroughButton>
												</dx:ToolbarStrikethroughButton>
												<dx:ToolbarJustifyLeftButton BeginGroup="True">
												</dx:ToolbarJustifyLeftButton>
												<dx:ToolbarJustifyCenterButton>
												</dx:ToolbarJustifyCenterButton>
												<dx:ToolbarJustifyRightButton>
												</dx:ToolbarJustifyRightButton>
												<dx:ToolbarBackColorButton BeginGroup="True">
												</dx:ToolbarBackColorButton>
												<dx:ToolbarFontColorButton>
												</dx:ToolbarFontColorButton>
												<dx:ToolbarRemoveFormatButton BeginGroup="True">
												</dx:ToolbarRemoveFormatButton>

											</Items>
										</dx:HtmlEditorToolbar>
									</Toolbars>

								</dx:ASPxHtmlEditor>

							</div>

						</div>

						<div>

							<script type="text/javascript">
								$('#ASPxHtmlEditor_DesignViewCell').css("border-bottom", "0");
							</script>

						</div>

					</dx:SplitterContentControl>
				</ContentCollection>
			</dx:SplitterPane>

		</Panes>
	</dx:ASPxSplitter>

</asp:Content>