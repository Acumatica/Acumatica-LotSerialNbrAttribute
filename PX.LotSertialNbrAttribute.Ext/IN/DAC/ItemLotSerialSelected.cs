using System;
using PX.Data;
using PX.Objects.IN;
using PX.Objects.SO;
using PX.Objects.CR;
using PX.Objects.CS;

namespace PX.LotSertialNbrAttribute.Ext
{
    [Serializable]
    [PXProjection(typeof(Select2<UsrInventoryLotSerialContainer,
                        InnerJoin<INLotSerialStatus, On<UsrInventoryLotSerialContainer.inventoryID, Equal<INLotSerialStatus.inventoryID>,
                                                                    And<UsrInventoryLotSerialContainer.lotSerialNbr, Equal<INLotSerialStatus.lotSerialNbr>>>,
                        InnerJoin<InventoryItem, On<INLotSerialStatus.inventoryID, Equal<InventoryItem.inventoryID>>,
                        InnerJoin<INSiteLotSerial, On<INLotSerialStatus.inventoryID, Equal<INSiteLotSerial.inventoryID>,
                                                        And<INLotSerialStatus.siteID, Equal<INSiteLotSerial.siteID>,
                                                        And<INLotSerialStatus.lotSerialNbr, Equal<INSiteLotSerial.lotSerialNbr>>>>,
                        LeftJoin<INSiteStatus, On<INSiteStatus.inventoryID, Equal<INLotSerialStatus.inventoryID>,
                                                    And<INSiteStatus.siteID, Equal<INLotSerialStatus.siteID>,
                                                    And<INSiteStatus.subItemID, Equal<INLotSerialStatus.subItemID>>>>,
                        LeftJoin<INSubItem, On<INSubItem.subItemID, Equal<INSiteStatus.subItemID>>>>>>>,
                        Where<CurrentValue<SOOrder.customerID>, IsNotNull,
                              And2<CurrentMatch<InventoryItem, AccessInfo.userName>,
                              And2<Where<INSiteStatus.subItemID, IsNull, Or<CurrentMatch<INSubItem, AccessInfo.userName>>>,
                              And2<Where<CurrentValue<ItemLotSerialAdvSearchFilter.onlyAvailable>, Equal<boolFalse>,
                                    Or<INSiteLotSerial.qtyAvail, Greater<PX.Objects.CS.decimal0>>>,
                              And<InventoryItem.itemStatus, NotEqual<InventoryItemStatus.inactive>,
                              And<InventoryItem.itemStatus, NotEqual<InventoryItemStatus.noSales>>>>>>>>), Persistent = false)]
    public partial class ItemLotSerialSelected : UsrInventoryLotSerialContainer
    {
        #region Selected

        public abstract class selected : PX.Data.IBqlField { }
        protected bool? _Selected = false;
        [PXBool]
        [PXDefault(false)]
        [PXUIField(DisplayName = "Selected")]
        public virtual bool? Selected
        {
            get
            {
                return _Selected;
            }
            set
            {
                _Selected = value;
            }
        }

        #endregion

        #region ContainerID

        public new abstract class containerID : PX.Data.IBqlField { }

        [PXDBInt(IsKey = true, BqlField = typeof(UsrInventoryLotSerialContainer.containerID))]
        [PXUIField(DisplayName = "ContainerID")]
        public override int? ContainerID { get; set; }

        #endregion

        #region InventoryID

        public new abstract class inventoryID : PX.Data.IBqlField { }

        [Inventory(IsKey = true, BqlField = typeof(UsrInventoryLotSerialContainer.inventoryID))]
        [PXDefault]
        public override int? InventoryID { get; set; }

        #endregion

        #region Descr

        public abstract class descr : PX.Data.IBqlField { }

        [PXDBString(255, IsUnicode = true, BqlField = typeof(InventoryItem.descr))]
        [PXUIField(DisplayName = "Description")]
        public virtual String Descr { get; set; }

        #endregion

        #region ClassID

        public new abstract class lotSerClassID : PX.Data.IBqlField { }

        [PXDBString(10, IsUnicode = true, BqlField = typeof(UsrInventoryLotSerialContainer.lotSerClassID))]
        [PXUIField(DisplayName = "Lot/Serial Class")]
        public override String LotSerClassID { get; set; }

        #endregion

        #region LotSerialNbr

        public new abstract class lotSerialNbr : PX.Data.IBqlField { }
        [PXDBString(100, IsUnicode = true, BqlField = typeof(UsrInventoryLotSerialContainer.lotSerialNbr))]
        [PXDefault()]
        [PXUIField(DisplayName = "Lot/Serial Nbr.")]
        public override String LotSerialNbr { get; set; }

        #endregion

        #region SiteID

        public abstract class siteID : PX.Data.IBqlField { }

        [Site(IsKey = true, BqlField = typeof(INLotSerialStatus.siteID))]
        [PXDefault()]
        public virtual Int32? SiteID { get; set; }

        #endregion

        #region LocationID
        public abstract class locationID : PX.Data.IBqlField
        {
        }
        [Location(typeof(siteID), IsKey = true, BqlField = typeof(INLotSerialStatus.locationID))]
        [PXDefault()]
        public virtual Int32? LocationID { get; set; }
        #endregion

        #region QtySelected

        public abstract class qtySelected : PX.Data.IBqlField { }
        protected Decimal? _QtySelected;
        [PXQuantity]
        [PXDefault(TypeCode.Decimal, "0.0")]
        [PXUIField(DisplayName = "Qty")]
        public virtual decimal? QtySelected
        {
            get
            {
                return this._QtySelected ?? 0m;
            }
            set
            {
                if (value != null && value != 0m)
                    this._Selected = true;
                this._QtySelected = value;
            }
        }

        #endregion

        #region QtyOnHand

        public abstract class qtyOnHand : PX.Data.IBqlField { }

        [PXDBQuantity(BqlField = typeof(INLotSerialStatus.qtyOnHand))]
        [PXDefault(TypeCode.Decimal, "0.0")]
        [PXUIField(DisplayName = "Qty. On Hand")]
        public virtual Decimal? QtyOnHand { get; set; }

        #endregion

        #region QtyAvail

        public abstract class qtyAvail : PX.Data.IBqlField { }

        [PXDBQuantity(BqlField = typeof(INSiteLotSerial.qtyAvail))]
        [PXDefault(TypeCode.Decimal, "0.0")]
        [PXUIField(DisplayName = "Qty. Available")]
        public virtual Decimal? QtyAvail { get; set; }

        #endregion

        #region ExpireDate
        public abstract class expireDate : PX.Data.IBqlField { }
        [PXDBDate(BqlField = typeof(INLotSerialStatus.expireDate))]
        [PXUIField(DisplayName = "Expiry Date")]
        public virtual DateTime? ExpireDate { get; set; }

        #endregion

        #region SubItemID

        public abstract class subItemID : PX.Data.IBqlField { }

        [PXDBInt(BqlField = typeof(INSubItem.subItemID))]
        [PXUIField(DisplayName = "Subitem ID", Visible = false)]
        public virtual int? SubItemID { get; set; }

        #endregion

        #region SalesUnit

        public abstract class salesUnit : PX.Data.IBqlField { }

        [INUnit(typeof(ItemLotSerialSelected.inventoryID), DisplayName = "Sales Unit",
                BqlField = typeof(InventoryItem.salesUnit))]
        public virtual string SalesUnit { get; set; }

        #endregion
    }
}