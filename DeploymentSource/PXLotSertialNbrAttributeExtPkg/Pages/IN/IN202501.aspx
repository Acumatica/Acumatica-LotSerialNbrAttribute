<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormDetail.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="IN202501.aspx.cs" Inherits="Page_IN202501" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormDetail.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" PrimaryView="InventoryLotSerialContainers" SuspendUnloading="False" TypeName="PX.LotSertialNbrAttribute.Ext.InventoryLotSerialContainerMaint">
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" Runat="Server">
    <px:PXFormView ID="form" runat="server" DataSourceID="ds" Style="z-index: 100" 
		Width="100%" DataMember="InventoryLotSerialContainers" TabIndex="5300">
		<Template>
			<px:PXLayoutRule runat="server" StartRow="True"/>
            <px:PXSegmentMask ID="edInventoryID" runat="server" DataField="InventoryID" CommitChanges="true">
            </px:PXSegmentMask>
            <px:PXSelector ID="edLotSerialNbr" runat="server" DataField="LotSerialNbr" AutoRefresh="True" CommitChanges="True" >
            </px:PXSelector>
		    <px:PXSelector ID="edLotSerClassID" runat="server" DataField="LotSerClassID" CommitChanges="True"  AllowEdit="True">
            </px:PXSelector>
		</Template>
	</px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" Runat="Server">
    <px:PXTab ID="tab" runat="server" Width="100%" Height="606px" DataSourceID="ds" DataMember="InventoryLotSerialContainerSettings" FilesIndicator="False" NoteIndicator="False">
        <AutoSize Enabled="True" Container="Window" MinHeight="150" />
        <Items>
            <px:PXTabItem Text="Attributes">
                <Template>
                    <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="S" ControlSize="XM" />
                    <px:PXGrid ID="PXGridAnswers" runat="server" Caption="Attributes" DataSourceID="ds" Height="150px" MatrixMode="True" Width="420px" SkinID="Attributes">
                        <Levels>
                            <px:PXGridLevel DataKeyNames="AttributeID,RefNoteID" DataMember="Attributes">
                                <RowTemplate>
                                    <px:PXLayoutRule runat="server" ControlSize="XM" LabelsWidth="M" StartColumn="True" />
                                    <px:PXTextEdit ID="edParameterID" runat="server" DataField="AttributeID" Enabled="False" />
                                    <px:PXTextEdit ID="edAnswerValue" runat="server" DataField="Value" />
                                </RowTemplate>
                                <Columns>
                                    <px:PXGridColumn AllowShowHide="False" DataField="AttributeID" TextField="AttributeID_description" TextAlign="Left" Width="135px" />
                                    <px:PXGridColumn DataField="isRequired" TextAlign="Center" Type="CheckBox" Width="80px" />
                                    <px:PXGridColumn DataField="Value" Width="185px" />
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                    </px:PXGrid>
                    <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="S" ControlSize="XM" />
                    <px:PXImageUploader Height="320px" Width="430px" ID="imgUploader" runat="server" 
                                        DataField="ImageUrl" AllowUpload="true" ShowComment="true" DataMember="InventoryLotSerialContainerSettings"/>
                </Template>
            </px:PXTabItem>
        </Items>
        <AutoSize Enabled="True" MinHeight="150" />
    </px:PXTab>
</asp:Content>